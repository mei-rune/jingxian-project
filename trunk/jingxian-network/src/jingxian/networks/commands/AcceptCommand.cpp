
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"

_jingxian_begin

const int addrbufsize = sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 100;

AcceptCommand::AcceptCommand(TCPAcceptor* acceptor)
: acceptor_(acceptor)
, socket_(acceptor_->createSocket())
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
		acceptor_->releaseSocket(socket_, false);
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
		onException(0, error);
		goto end;
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
		goto end;
	}

	ILogger* logger = acceptor_->logger();

	if (!acceptor_->isListening())
	{
		INFO( logger, _T("������ '")<< endpoint_ <<_T("' ��ȡ�������󷵻�,���Ѿ�ֹͣ����!"));
		goto end;
	}

	initializeConnection(bytes_transferred, completion_key);

	if( acceptor_->doAccept())
		return;

	INFO(logger, _T("������ '")<< endpoint_ <<_T("' ��ȡ���ӷ���,���·��ͻ�ȡ��������ʱ��������!"));

end:
	decrementAccepting();
}

void TCPAcceptor::initializeConnection(  int bytesTransferred
									   , void *completion_key)
{
	sockaddr *local_addr = 0;
	sockaddr *remote_addr = 0;
	int local_size = 0;
	int remote_size = 0;

	::GetAcceptExSockaddrs ( ptr,
		static_cast < DWORD >( bytes_transferred),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		&local_addr,
		&local_size,
		&remote_addr,
		&remote_size);

	std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket( socket_ ));
	socket_ = INVALID_HANDLE_VALUE;


	_acceptor.Logger.InfoFormat("��ȡ������[{0}]������,��ʼ��ʼ��!", _socket.RemoteEndPoint);

	IReactorCore* core = _acceptor.GetNextCore();
	if (!core.bind(socket))
	{
		Win32Exception err = new Win32Exception();
		_acceptor.OnError(new InitializeError(_acceptor.BindPoint, string.Format("��ʼ������[{0}]������ʱ���󶨵�iocp�������� - {0}", _socket.RemoteEndPoint, err.Message), err));
		return;
	}

	InitializeError initializeError = null;
	ConnectedSocket connectedSocket = null;
	try
	{
		IDictionary<string, object> misc = (null == _acceptor.ProtocolFactory) ? null : _acceptor.ProtocolFactory.Misc;
		connectedSocket = new ConnectedSocket(core, _socket, new ProtocolContext(null, misc));
		IProtocol protocol = _acceptor.CreateProtocol(connectedSocket);
		core.InitializeConnection(connectedSocket, protocol);
		_acceptor.Logger.InfoFormat("��ʼ������[{0}]�����ӳɹ�!", _socket.RemoteEndPoint);
		_socket = null;
	}
	catch (InitializeError e)
	{
		initializeError = e;
	}
	catch (Exception e)
	{
		initializeError = new InitializeError(_socket.RemoteEndPoint, "�ڴ���������ʱ�����쳣!", e);
	}

	if (null != initializeError)
	{
		if (null != connectedSocket)
			connectedSocket.ReleaseSocket();

		_acceptor.OnError(initializeError);
	}
}

bool AcceptCommand::execute()
{
	int bytesTransferred;
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
