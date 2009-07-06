
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"
# include "jingxian/networks/ConnectedSocket.h"


_jingxian_begin

const int addrbufsize = sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 100;

AcceptCommand::AcceptCommand(TCPAcceptor* acceptor
							, OnBuildConnectionSuccess onSuccess
                            , OnBuildConnectionError onError
                            , void* context)
: onSuccess_(onSuccess)
, onError_(onError)
, context_(context)
, socket_(createSocket())
, ptr_((char*)malloc(addrbufsize))
, len_(addrbufsize)
{
}

AcceptCommand::~AcceptCommand()
{
	free( ptr_ );
	ptr_ = null_ptr;

	if( INVALID_SOCKET == socket_ )
	{
		acceptor_->tcpFactory().releaseSocket(socket_, false);
		socket_ = INVALID_SOCKET;
	}
}

void AcceptCommand::on_complete(size_t bytes_transferred
								, int success
								, void *completion_key
								, u_int32_t error)
{
	if (!success)
	{
		acceptor_->onException(error, _T("������ '") 
			+ acceptor_->endpoint_.toString() 
			+ _T("' ��ȡ��������ʧ�� - ")
			+ lastError(error));
		return;
	}


	SOCKET listener = acceptor_->handle();
	if( SOCKET_ERROR == setsockopt(socket_, SOL_SOCKET, SO_UPDATE_ACCEPT_CONTEXT,
		(char *) &listener, sizeof(listener)))
	{
		int errCode = ::WSAGetLastError();
		acceptor_->onException(errCode, _T("������ '") 
			+ acceptor_->endpoint_.toString() 
			+ _T("' ��ȡ�������󷵻�,�ڶ� socket ������� SO_UPDATE_ACCEPT_CONTEXT ѡ��ʱ�������� - ")
			+ lastError(errCode));
		return;
	}

	if (!acceptor_->isListening())
	{
		acceptor_->onException(0, _T("������ '") 
			+ acceptor_->endpoint_.toString() 
			+ _T("' ��ȡ�������󷵻�,���Ѿ�ֹͣ����!"));
		return;
	}

	initializeConnection(bytes_transferred, completion_key);

	if( acceptor_->doAccept())
		return;

	int errCode = ::WSAGetLastError();
	acceptor_->onException(errCode, _T("������ '") 
		+ acceptor_->endpoint_.toString() 
		+ _T("' ��ȡ�������󷵻�,���·��ͻ�ȡ��������ʱ�������� - ")
		+ lastError(errCode));
	return;
}

void AcceptCommand::initializeConnection(  int bytesTransferred
									   , void *completion_key)
{
	sockaddr *local_addr = 0;
	sockaddr *remote_addr = 0;
	int local_size = 0;
	int remote_size = 0;

	::GetAcceptExSockaddrs (ptr_,
		static_cast < DWORD >( bytesTransferred),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		&local_addr,
		&local_size,
		&remote_addr,
		&remote_size);

	tstring peer = _T("tcp://") 
				+ toTstring(inet_ntoa(((sockaddr_in*)remote_addr)->sin_addr)) 
				+ _T(":")
				+ toString(htons(((sockaddr_in*)remote_addr)->sin_port));

	tstring host = _T("tcp://") 
				+ toTstring(inet_ntoa(((sockaddr_in*)local_addr)->sin_addr)) 
				+ _T(":")
				+ toString(htons(((sockaddr_in*)local_addr)->sin_port));

	
	IOCPServer* core = acceptor_->nextCore();
	std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket(core, socket_, host, peer));
	socket_ = INVALID_SOCKET;


	INFO( acceptor_->logger(), _T("��ȡ������ '") << peer
							<< _T("' ������,��ʼ��ʼ��!"));

	if (!core->bind(socket,connectedSocket.get()))
	{	
		int errCode = ::WSAGetLastError();
		LOG_ERROR( acceptor_->logger(), _T("��ʼ������ '") << peer
							<< _T("' ������ʱ���󶨵�iocp�������� - ")
							<< lastError(errCode));
		return;
	}

	connectedSocket->bindProtocol( acceptor_->protocolFactory().createProtocol() );
	INFO( acceptor_->logger(), _T("��ʼ������ '") << peer
							<< _T("' �����ӳɹ�!"));

	connectedSocket->initialize();
	connectedSocket.release();
}

bool AcceptCommand::execute()
{
	DWORD bytesTransferred;
	if (BaseSocket::__acceptex(acceptor_->handle()
		, socket_
		, ptr_
		, 0 //_byteBuffer.Space - (HazelAddress.MaxSize + 4) * 2 
		//����Ϊ0,������д��������Ӵ���accept�У���Ϊ�ͻ���ֻ
		//�������ӣ�û�з������ݡ�
		, sizeof (sockaddr_in) + sizeof (sockaddr)
		, sizeof (sockaddr_in) + sizeof (sockaddr)
		, &bytesTransferred
		, this ))
		return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;

	return false;
}

_jingxian_end
