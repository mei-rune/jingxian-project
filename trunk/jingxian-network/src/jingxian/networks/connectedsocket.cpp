
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
	TP_CRITICAL(tracer_, transport_mode::Both, _T("���� ConnectedSocket ����ɹ�"));
	
	context_.initialize(core, this);
	incoming_.initialize(this);
	outgoing_.initialize(this);
}

ConnectedSocket::~ConnectedSocket( )
{
	TP_CRITICAL(tracer_, transport_mode::Both, _T("���� ConnectedSocket ����ɹ�"));
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
		TP_TRACE(tracer_, transport_mode::Receive, _T("���Զ�����ʱ�����ѶϿ�"));
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
	disconnection(_T("�û������ر�����"));
}

void ConnectedSocket::disconnection(const tstring& error)
{
	doDisconnect(transport_mode::Both, 0, error);
}

void ConnectedSocket::doRead()
{
	if(reading_)
	{
		TP_TRACE(tracer_, transport_mode::Receive, _T("���Զ�����ʱ�������ڶ�ȡ��"));
		return;
	}

	if( stopReading_)
	{
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("���Զ�����ʱ�����û�����ֹͣ������"));
		return;
	}

	if( connection_status::connected != state_)
	{
		tstring err = concat<tstring>(_T("���Զ�����ʱ�����ѶϿ� - "), disconnectReason_);
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}

	std::auto_ptr<ICommand> command(incoming_.makeCommand());
	if(is_null(command))
	{
		tstring err = _T("���Զ�����ʱ����������ʧ��");
		TP_CRITICAL(tracer_, transport_mode::Receive, err);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}
	
	if(!command->execute())
	{
		DWORD errCode = ::WSAGetLastError();
		tstring err = ::concat<tstring>(_T("���Զ�����ʱ���Ͷ�����ʧ�� - "),lastError(errCode));
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
		TP_TRACE(tracer_, transport_mode::Send, _T("����д����ʱ�������ڷ�����"));
		return;
	}

	if( connection_status::connected != state_)
	{
		tstring err = concat<tstring>(_T("����д����ʱ�����ѶϿ� - "), disconnectReason_);
		TP_CRITICAL(tracer_, transport_mode::Send, err);
		doDisconnect(transport_mode::Send, 0, err);
		return;
	}

	std::auto_ptr<ICommand> command(outgoing_.makeCommand());
	if(is_null(command))
	{
		tstring err = _T("���ݷ������! ");
		TP_TRACE(tracer_, transport_mode::Send, err);
		return;
	}
	
	if(!command->execute())
	{
		DWORD errCode = ::WSAGetLastError();
		tstring err = ::concat<tstring>(_T("����д����ʱ����д����ʧ�� - "),lastError(errCode));
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
		TP_TRACE(tracer_, mode, _T("���ԶϿ�ʱ�����ѷ����Ͽ�����"));
		return;
	}

	state_ = connection_status::disconnecting;

	if(writing_)
	{
		assert( transport_mode::Send != mode);
		disconnectReason_ = description;
		TP_TRACE(tracer_, mode, _T("׼���Ͽ�����ʱ����д��δ����"));
		return;
	}
	if(reading_)
	{
		assert( transport_mode::Receive != mode);
		disconnectReason_ = description;
		TP_TRACE(tracer_, mode, _T("׼���Ͽ�����ʱ���ֶ���δ����"));
		return;
	}

	std::auto_ptr<ICommand> command(new DisconnectCommand(this, description));
	if(!command->execute())
	{
		TP_FATAL(tracer_, mode, _T("׼���Ͽ�����ʱ������������ʧ��"));
		return;
	}
	command.release();
}

void ConnectedSocket::onRead(size_t bytes_transferred)
{
	reading_ = false;

	if(!incoming_.increaseBytes(bytes_transferred))
	{
		tstring err = _T("��������ֽ���ʱ��������");
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
			tstring err = _T("�����û����ֽ���ʱ��������");
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
		tstring err = ::concat<tstring>(_T("�����û����ֽ���ʱ�����쳣 - "), ex.what());
		TP_FATAL(tracer_, transport_mode::Receive, _T("�����û����ֽ���ʱ�����쳣 ") << ex);
		doDisconnect(transport_mode::Receive, 0, err);
		return;
	}
	catch(const std::exception& e)
	{
		tstring err = ::concat<tstring>(_T("�����û����ֽ���ʱ�����쳣 - "), e.what());
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