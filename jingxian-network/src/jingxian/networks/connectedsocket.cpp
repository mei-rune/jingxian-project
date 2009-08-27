
# include "pro_config.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/networks/commands/DisconnectCommand.h"

_jingxian_begin

ConnectedSocket::ConnectedSocket(IOCPServer* core
								 , SOCKET sock
								 , const tstring& host
								 , const tstring& peer)
: core_(core)
, socket_(sock)
, host_(host)
, peer_(peer)
, state_(connection_status::connected)
, timeout_(3*1000)
, protocol_(0)
, isInitialize_(false)
, stopReading_(false)
, reading_(false)
, writing_(false)
, tracer_(0)
{
	toString_ = concat<tstring>(_T("ConnectedSocket["),host_, _T(" - "), peer_);

	tracer_ = logging::makeTracer(_T("ConnectedSocket"), host_, peer_);	
	TP_CRITICAL(tracer_, transport_mode::Both, _T("创建 ConnectedSocket 对象成功"));
	
	context_.initialize(core, this);
	incoming_.initialize(this);
	outgoing_.initialize(this);
}

ConnectedSocket::~ConnectedSocket( )
{
	TP_CRITICAL(tracer_, transport_mode::Both, _T("销毁 ConnectedSocket 对象成功"));
	delete tracer_;
	tracer_ = null_ptr;
}

void ConnectedSocket::bindProtocol(IProtocol* protocol)
{
	protocol_ = protocol;
}

void ConnectedSocket::initialize()
{
	if( isInitialize_ )
		return;

	protocol_->onConnected( context_ );
	isInitialize_ = true;
	startReading();
}

void ConnectedSocket::startReading()
{
	if( connection_status::connected != state_)
	{
		TP_TRACE(tracer_, transport_mode::Receive, _T("尝试读数据时连接已断开"));
		return;
	}

	stopReading_ = false;
	doRead();
}

void ConnectedSocket::stopReading()
{
	stopReading_ = true;
}

void ConnectedSocket::write(buffer_chain_t* buffer)
{
	if(is_null(buffer))
		ThrowException1(ArgumentNullException, "buffer");

	outgoing_.send(buffer);
	doWrite();
}


void ConnectedSocket::writeBatch(buffer_chain_t** buffers, size_t len)
{
	if(is_null(buffers))
		ThrowException1(ArgumentNullException, "buffers");

	for(size_t i = 0; i < len; ++i)
	{
		outgoing_.send(buffers[i]);
	}

	if(0 != len)
		doWrite();
}

void ConnectedSocket::disconnection()
{
	disconnection(_T("用户主动关闭连接"));
}

void ConnectedSocket::disconnection(const tstring& error)
{
	doDisconnect(transport_mode::Both, 0, error);
}

void ConnectedSocket::doRead()
{
	if(reading_)
	{
		TP_TRACE(tracer_, transport_mode::Receive, _T("尝试读数据时发现正在读取中"));
		return;
	}

	if( stopReading_)
	{
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("尝试读数据时发现用户主动停止读数据"));
		return;
	}

	if( connection_status::connected != state_)
	{
		tstring err = concat<tstring>(_T("尝试读数据时连接已断开 - "), disconnectReason_);
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}

	std::auto_ptr<ICommand> command(incoming_.makeCommand());
	if(is_null(command))
	{
		tstring err = _T("尝试读数据时创建读请求失败");
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}
	
	if(!command->execute())
	{
		DWORD errCode = ::WSAGetLastError();
		tstring err = ::concat<tstring>(_T("尝试读数据时发送读请求失败 - "),lastError(errCode));
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, errCode, err);
		return;
	}

	reading_ = true;
	command.release();
}

void ConnectedSocket::doWrite()
{
	if(writing_)
	{
		TP_TRACE(tracer_, transport_mode::Send, _T("尝试写数据时发现正在发送中"));
		return;
	}

	if( connection_status::connected != state_)
	{
		tstring err = concat<tstring>(_T("尝试写数据时连接已断开 - "), disconnectReason_);
		TP_CRITICAL(tracer_, transport_mode::Send, err);
		doDisconnect(transport_mode::Send, 0, err);
		return;
	}

	std::auto_ptr<ICommand> command(outgoing_.makeCommand());
	if(is_null(command))
	{
		tstring err = _T("数据发送完毕! ");
		TP_TRACE(tracer_, transport_mode::Send, err);
		return;
	}
	
	if(!command->execute())
	{
		DWORD errCode = ::WSAGetLastError();
		tstring err = ::concat<tstring>(_T("尝试写数据时发送写请求失败 - "),lastError(errCode));
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Send, errCode, err);
		return;
	}

	writing_ = true;
	command.release();
}

void ConnectedSocket::doDisconnect(transport_mode::type mode, errcode_t error, const tstring& description)
{	
	if( connection_status::connected != state_)
	{
		TP_TRACE(tracer_, mode, _T("尝试断开时发现已发出断开请求"));
		return;
	}

	state_ = connection_status::disconnecting;

	if(writing_)
	{
		assert( transport_mode::Send != mode);
		disconnectReason_ = description;
		TP_TRACE(tracer_, mode, _T("准备断开连接时发现写请未返回"));
		return;
	}
	if(reading_)
	{
		assert( transport_mode::Receive != mode);
		disconnectReason_ = description;
		TP_TRACE(tracer_, mode, _T("准备断开连接时发现读请未返回"));
		return;
	}

	std::auto_ptr<ICommand> command(new DisconnectCommand(this, description));
	if(!command->execute())
	{
		TP_FATAL(tracer_, mode, _T("准备断开连接时发送连接请求失败"));
		return;
	}
	command.release();
}

void ConnectedSocket::onRead(size_t bytes_transferred)
{
	reading_ = false;

	if(!incoming_.increaseBytes(bytes_transferred))
	{
		tstring err = _T("计算接收字节数时发生错误");
		TP_FATAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}

	try
	{
		std::vector<io_mem_buf> ioBuf;
		incoming_.dataBuffer(ioBuf);
		context_.inMemory(&ioBuf, -1);

		size_t readLen = protocol_->onReceived( context_ );
		if(!incoming_.decreaseBytes(readLen))
		{
			tstring err = _T("计算用户读字节数时发生错误");
			TP_FATAL(tracer_, transport_mode::Receive, err);
			doDisconnect(transport_mode::Receive, 0, err);
			return;
		}
		
		if( connection_status::connected != state_)
			return;
		
		//int count = 0;
		//Buffer<buffer_chain_t>& dataBuffer = context_.GetOutBuffer().rawBuffer();
		//buffer_chain_t* ptr = null_ptr;
		//while(null_ptr != (ptr = dataBuffer.pop()))
		//{
		//	outgoing_.send(ptr);
		//	++ count;
		//}
		//if( 0!= count)
		//	doWrite();
	}
	catch(const Exception& ex)
	{
		tstring err = ::concat<tstring>(_T("计算用户读字节数时发生异常 - "), ex.what());
		TP_FATAL(tracer_, transport_mode::Receive, _T("计算用户读字节数时发生异常 ") << ex);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}
	catch(const std::exception& e)
	{
		tstring err = ::concat<tstring>(_T("计算用户读字节数时发生异常 - "), e.what());
		TP_FATAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}
	doRead();
}

void ConnectedSocket::onWrite(size_t bytes_transferred)
{
	writing_ = false;
	outgoing_.clearBytes(bytes_transferred);
	doWrite();
}

void ConnectedSocket::onError(transport_mode::type mode, errcode_t error, const tstring& description)
{
	switch( mode )
	{
	case transport_mode::Receive:
		reading_ = false;
		break;
	case transport_mode::Send:
		reading_ = false;
		break;
	default:
		assert(false);
		break;
	}

	doDisconnect(mode, error, description);
}

const tstring& ConnectedSocket::host() const
{
	return host_;
}

const tstring& ConnectedSocket::peer() const
{
	return peer_;
}

time_t ConnectedSocket::timeout() const
{
	return timeout_;
}

const tstring& ConnectedSocket::toString() const
{
	return toString_;
}

void ConnectedSocket::onDisconnected(errcode_t error, const tstring& description)
{
	protocol_->onDisconnected(context_,error, description);
}

buffer_chain_t* ConnectedSocket::allocateProtocolBuffer()
{
	return protocol_->createBuffer(context_, incoming_.buffer(), incoming_.current());
}

_jingxian_end