
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"
# include "jingxian/networks/ConnectedSocket.h"


_jingxian_begin

AcceptCommand::AcceptCommand(TCPAcceptor* acceptor
							, OnBuildConnectionComplete onComplete
                            , OnBuildConnectionError onError
                            , void* context)
: core_(acceptor->nextCore())
, onComplete_(onComplete)
, onError_(onError)
, context_(context)
, listener_(acceptor->handle())
, listenAddr_(acceptor->bindPoint())
, socket_(WSASocket(AF_INET,SOCK_STREAM,IPPROTO_TCP,0,0,WSA_FLAG_OVERLAPPED))
, ptr_((char*)my_malloc(sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 1000))
, len_(sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 1000)
{
	memset( ptr_, 0, len_);
}

AcceptCommand::~AcceptCommand()
{
	my_free( ptr_ );
	ptr_ = null_ptr;

	if( INVALID_SOCKET == socket_ )
	{
		closesocket(socket_);
		socket_ = INVALID_SOCKET;
	}
}

void AcceptCommand::on_complete(size_t bytes_transferred
								, bool success
								, void *completion_key
								, errcode_t error)
{
	if (!success)
	{
		ErrorCode err(0 == success, error, concat<tstring>(_T("������ '") 
			, listenAddr_
			, _T("' ��ȡ��������ʧ�� - ")
			, lastError(error)));
		onError_(err, context_);
		return;
	}

	//if (!acceptor_->isListening())
	//{
	//	ErrorCode err(_T("������ '") 
	//		+ listenAddr_
	//		+ _T("' ��ȡ�������󷵻�,���Ѿ�ֹͣ����!"));
	//	onError_(err, context_);
	//	return;
	//}

	sockaddr *local_addr = 0;
	sockaddr *remote_addr = 0;
	int local_size = 0;
	int remote_size = 0;

	/// �������!���ֱ���� GetAcceptExSockaddrs ��ʧ��,ͨ����
	/// �� GetAcceptExSockaddrs �ĺ���ָ���û������.
	networking::getAcceptExSockaddrs(ptr_,
		0,
		sizeof (sockaddr_in) + sizeof (sockaddr),
		sizeof (sockaddr_in) + sizeof (sockaddr),
		&local_addr,
		&local_size,
		&remote_addr,
		&remote_size);

	tchar buf[1024];
	DWORD len = 1024;
	if(SOCKET_ERROR == WSAAddressToString(remote_addr, remote_size, NULL,buf,&len))
	{
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring,tchar*, tstring,tchar*,tstring>(_T("������ '") 
			, listenAddr_
			, _T("' ��ȡ�������󷵻�,��ȡԶ�̵�ַʧ�� -")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	buf[len] = 0;
	tstring peer = concat<tstring>(_T("tcp://") 
				, buf);

	
	len = 1024;
	if(SOCKET_ERROR == WSAAddressToString(local_addr, local_size, NULL,buf,&len))
	{
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring>(_T("������ '") 
			, listenAddr_
			, _T("' ��ȡ�������󷵻�,��ȡ���ص�ַʧ�� -")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	tstring host = concat<tstring>(_T("tcp://") 
				, buf);

	
	if( SOCKET_ERROR == setsockopt(socket_, SOL_SOCKET, SO_UPDATE_ACCEPT_CONTEXT,
		(char *) &listener_, sizeof(listener_)))
	{
		int errCode = ::WSAGetLastError();
		
		ErrorCode err(false, errCode, _T("������ '") 
			+ listenAddr_
			+ _T("' ��ȡ�������󷵻�,�ڶ� socket ������� SO_UPDATE_ACCEPT_CONTEXT ѡ��ʱ�������� - ")
			+ lastError(errCode));
		onError_(err, context_);
		return;
	}
	
	std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket(core_, socket_, host, peer));
	socket_ = INVALID_SOCKET;

	if (!core_->bind((HANDLE)(connectedSocket->handle()),connectedSocket.get()))
	{	
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring>(_T("��ʼ������ '") 
			, peer
			, _T("' ������ʱ���󶨵�iocp�������� - ")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	onComplete_(connectedSocket.get(), context_);
	connectedSocket->initialize();
	connectedSocket.release();
}

bool AcceptCommand::execute()
{
	DWORD bytesTransferred;
	if (networking::acceptEx(listener_
		, socket_
		, ptr_
		, 0 //����Ϊ0,������д��������Ӵ���accept�У���Ϊ�ͻ���ֻ
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
