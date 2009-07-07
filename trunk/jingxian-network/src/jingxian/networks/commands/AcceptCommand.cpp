
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"
# include "jingxian/networks/ConnectedSocket.h"


_jingxian_begin

AcceptCommand::AcceptCommand(TCPAcceptor* acceptor
							, OnBuildConnectionSuccess onSuccess
                            , OnBuildConnectionError onError
                            , void* context)
: core_(acceptor->nextCore())
, onSuccess_(onSuccess)
, onError_(onError)
, context_(context)
, listener_(acceptor->handle())
, listenAddr_(acceptor->bindPoint())
, socket_(::socket(AF_INET 
						, SOCK_STREAM
						, IPPROTO_TCP))
, ptr_((char*)malloc(sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 100))
, len_(sizeof (sockaddr_in)*2 + sizeof (sockaddr)*2 + 100)
{
}

AcceptCommand::~AcceptCommand()
{
	free( ptr_ );
	ptr_ = null_ptr;

	if( INVALID_SOCKET == socket_ )
	{
		closesocket(socket_);
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
		ErrorCode err(0 == success, error, _T("接受器 '") 
			+ listenAddr_
			+ _T("' 获取连接请求失败 - ")
			+ lastError(error));
		onError_(err, context_);
		return;
	}

	//if (!acceptor_->isListening())
	//{
	//	ErrorCode err(_T("接受器 '") 
	//		+ listenAddr_
	//		+ _T("' 获取连接请求返回,但已经停止监听!"));
	//	onError_(err, context_);
	//	return;
	//}

	if( SOCKET_ERROR == setsockopt(socket_, SOL_SOCKET, SO_UPDATE_ACCEPT_CONTEXT,
		(char *) &listener_, sizeof(listener_)))
	{
		int errCode = ::WSAGetLastError();
		
		ErrorCode err(false, errCode, _T("接受器 '") 
			+ listenAddr_
			+ _T("' 获取连接请求返回,在对 socket 句柄设置 SO_UPDATE_ACCEPT_CONTEXT 选项时发生错误 - ")
			+ lastError(errCode));
		onError_(err, context_);
		return;
	}

	sockaddr *local_addr = 0;
	sockaddr *remote_addr = 0;
	int local_size = 0;
	int remote_size = 0;

	::GetAcceptExSockaddrs (ptr_,
		static_cast < DWORD >( bytes_transferred),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		static_cast < DWORD >( sizeof (sockaddr_in) + sizeof (sockaddr) ),
		&local_addr,
		&local_size,
		&remote_addr,
		&remote_size);

	tchar buf[1024];
	DWORD len = 1024;
	if(SOCKET_ERROR == WSAAddressToString(remote_addr, remote_size, NULL,buf,&len))
	{
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring, char*, tstring,char*,tstring>(_T("接受器 '") 
			, listenAddr_
			, _T("' 获取连接请求返回,获取远程地址失败 -")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	buf[len] = 0;
	tstring peer = concat<tstring>(_T("tcp://") 
				, buf
				, _T(":")
				, htons(((sockaddr_in*)remote_addr)->sin_port));

	
	len = 1024;
	if(SOCKET_ERROR == WSAAddressToString(local_addr, local_size, NULL,buf,&len))
	{
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring>(_T("接受器 '") 
			, listenAddr_
			, _T("' 获取连接请求返回,获取本地地址失败 -")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	tstring host = concat<tstring>(_T("tcp://") 
				, buf
				, _T(":")
				, htons(((sockaddr_in*)local_addr)->sin_port));

	
	std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket(core_, socket_, host, peer));
	socket_ = INVALID_SOCKET;

	if (!core_->bind(socket,connectedSocket.get()))
	{	
		int errCode = ::WSAGetLastError();
		ErrorCode err(false, errCode, concat<tstring>(_T("初始化来自 '") 
			, peer
			, _T("' 的连接时，绑定到iocp发生错误 - ")
			, lastError(errCode)));
		onError_(err, context_);
		return;
	}
	connectedSocket->initialize();
	onSuccess_(connectedSocket.get(), context_);
	connectedSocket.release();
}

bool AcceptCommand::execute()
{
	DWORD bytesTransferred;
	if (BaseSocket::__acceptex(listener_
		, socket_
		, ptr_
		, 0 //_byteBuffer.Space - (HazelAddress.MaxSize + 4) * 2 
		//必须为0,否则会有大量的连接处于accept中，因为客户端只
		//建立连接，没有发送数据。
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
