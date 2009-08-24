
# include "pro_config.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/networks/commands/DisconnectCommand.h"

_jingxian_begin

ConnectedSocket::ConnectedSocket(IOCPServer* core
								 , SOCKET socket
								 , const tstring& host
								 , const tstring& peer)
: core_(core)
, socket_(socket)
, host_(host)
, peer_(peer)
, state_(connection_status::connected)
, timeout_(3*1000)
, protocol_(0)
, context_(core, this)
, isInitialize_(false)
, stopReading_(false)
, reading_(false)
, writing_(false)
, tracer_(0)
{
	tracer_ = logging::makeTracer( _T("ConnectedSocket[ip=") + host_ + _T(",port=") + peer_ + _T("]"));	
	TP_CRITICAL(tracer_, transport_mode::Both, _T("���� ConnectedSocket ����ɹ�"));
	
	incoming_.initialize(this);
	outgoing_.initialize(this);
}

ConnectedSocket::~ConnectedSocket( )
{
	TP_CRITICAL(tracer_, transport_mode::Both, _T("���� ConnectedSocket ����ɹ�"));
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
	if( connection_status::connected != state_)
	{
		TP_TRACE(tracer_, transport_mode::Send, _T("����д����ʱ�����ѶϿ�"));
		return;
	}

	outgoing_.push(buffer);
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
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("���Զ�����ʱ�����ѶϿ�"));
		doDisconnect(transport_mode::Receive, 0, null_ptr);
		return;
	}

	std::auto_ptr<ICommand> command(incoming_.makeCommand());
	if(is_null(command))
	{
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("���Զ�����ʱ����������ʧ��"));
		doDisconnect(transport_mode::Receive, 0, _T("���Զ�����ʱ����������ʧ��"));
		return;
	}
	
	if(!command->execute())
	{
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("���Զ�����ʱ���Ͷ�����ʧ��"));
		doDisconnect(transport_mode::Receive, ::WSAGetLastError(), null_ptr);
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
		TP_CRITICAL(tracer_, transport_mode::Send, _T("����д����ʱ�����ѶϿ�"));
		doDisconnect(transport_mode::Send, 0, null_ptr);
		return;
	}

	std::auto_ptr<ICommand> command(outgoing_.makeCommand());
	if(is_null(command))
	{
		TP_TRACE(tracer_, transport_mode::Send, _T("����д����ʱ����д����ʧ��"));
		doDisconnect(transport_mode::Send, 0, _T("����д����ʱ����д����ʧ��"));
		return;
	}
	
	if(!command->execute())
	{
		TP_CRITICAL(tracer_, transport_mode::Receive, _T("����д����ʱ����д����ʧ��"));
		doDisconnect(transport_mode::Send, ::WSAGetLastError(), null_ptr);
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
		TP_TRACE(tracer_, mode, _T("׼���Ͽ�����ʱ����д��δ����"));
		return;
	}
	if(reading_)
	{
		assert( transport_mode::Receive != mode);

		TP_TRACE(tracer_, mode, _T("׼���Ͽ�����ʱ���ֶ���δ����"));
		return;
	}

	std::auto_ptr<ICommand> command(new DisconnectCommand(this));
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
	incoming_.clearBytes(bytes_transferred);

	protocol_->onConnected( context_ );
	doRead();
}

void ConnectedSocket::onWrite(size_t bytes_transferred)
{
	writing_ = false;
	incoming_.clearBytes(bytes_transferred);
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

_jingxian_end