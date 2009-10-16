
# include "pro_config.h"
# include "jingxian/networks/commands/ConnectCommand.h"
# include "jingxian/networks/ConnectedSocket.h"


_jingxian_begin


ConnectCommand::ConnectCommand(IOCPServer* core
							   , const tchar* host
							   , OnBuildConnectionComplete onComplete
							   , OnBuildConnectionError onError
							   , void* context)
: core_(core)
, host_(host)
, onComplete_(onComplete)
, onError_(onError)
, context_(context)
, socket_(INVALID_SOCKET) //WSASocket(AF_INET,SOCK_STREAM,IPPROTO_TCP,0,0,WSA_FLAG_OVERLAPPED))
{
}

ConnectCommand::~ConnectCommand()
{
	if(INVALID_SOCKET != socket_)
	{
		closesocket(socket_);
		socket_ = INVALID_SOCKET;
	}
}


bool ConnectCommand::execute()
{
	struct sockaddr addr;
	int len = sizeof(addr);

	if(! networking::stringToAddress((LPTSTR)host_.c_str(), &addr, &len))
		return false;
	
	if (INVALID_SOCKET == socket_)
		socket_ = WSASocket(addr.sa_family,SOCK_STREAM,IPPROTO_TCP,0,0,WSA_FLAG_OVERLAPPED);

	SOCKADDR_IN bindAddr;
	bindAddr.sin_family = AF_INET;
	bindAddr.sin_port = 0; // htons(0);
	bindAddr.sin_addr.s_addr = ADDR_ANY; //htonl(ADDR_ANY);
	
	// NOTICE: 超级奇怪必须绑定一下, MS 说的
	if(SOCKET_ERROR == ::bind(socket_,(struct sockaddr*) &bindAddr, sizeof(bindAddr)))
		return false;

	if (!core_->bind((HANDLE)socket_, null_ptr))
		return false;

	//DWORD bytesTransferred = 0;
	if ( networking::connectEx(socket_,&addr, len, null_ptr, 0,null_ptr, this))
			return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;

	return false;
}

void ConnectCommand::on_complete(size_t bytes_transferred
								 , bool success
								 , void *completion_key
								 , errcode_t error)
{
	if (!success)
	{
		ErrorCode err(0 == success, error, concat<tstring>(_T("连接到 '") 
			, host_
			, _T("' 失败 - ")
			, lastError(error)));
		onError_(err, context_);
		return;
	}

	try
	{
		setsockopt( socket_, 
				  SOL_SOCKET, 
				  SO_UPDATE_CONNECT_CONTEXT, 
				  NULL, 
				  0 );

		struct sockaddr name;
		int namelen = sizeof(name);

		if(SOCKET_ERROR == getsockname( socket_,& name,&namelen))
		{
			ErrorCode err(0 == success, error, concat<tstring>(_T("连接到 '") 
				, host_
				, _T("' 成功,取本地地址时失败 - ")
				, lastError(error)));
			onError_(err, context_);
			return;
		}

		tstring local;
		if(!networking::addressToString(&name, namelen, local))
		{
			ErrorCode err(0 == success, error, concat<tstring>(_T("连接到 '") 
				, host_
				, _T("' 成功,转换本地地址时失败 - ")
				, lastError(error)));
			onError_(err, context_);
			return;
		}

		std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket(core_, socket_, local, host_));
		socket_ = INVALID_SOCKET;

		onComplete_(connectedSocket.get(), context_);
		connectedSocket->initialize();
		connectedSocket.release();
		return;
	}
	catch (std::exception& e)
	{
		ErrorCode err(0 == success, error, concat<tstring>(_T("连接到 '") 
			, host_
			, _T("' 成功,初始化时失败 - ")
			, toTstring(e.what())));
		onError_(err, context_);
	}
}

_jingxian_end
