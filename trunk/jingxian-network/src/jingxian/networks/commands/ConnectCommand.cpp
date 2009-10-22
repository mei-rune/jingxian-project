
# include "pro_config.h"
# include <Ws2tcpip.h>
# include "jingxian/networks/commands/ConnectCommand.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/threading/thread.h"


_jingxian_begin



void onResolveComplete(const tstring& name, const IPHostEntry& hostEntry, void* context)
{
	ConnectCommand* cmd = (ConnectCommand*)context;
	cmd->onResolveComplete(name, hostEntry);
}

void onResolveError(const tstring& name, errcode_t err, void* context)
{
	ConnectCommand* cmd = (ConnectCommand*)context;
	cmd->onResolveError(name, err);
}

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


void ConnectCommand::onResolveComplete(const tstring& name, const IPHostEntry& hostEntry)
{
	short port = networking::fetchPort(host_.c_str());
	port = htons(port);
	for(std::vector<HostAddress>::const_iterator it = hostEntry.AddressList.begin()
		; it != hostEntry.AddressList.end(); ++ it)
	{
		HostAddress addr(*it);

		switch(addr.ptr()->sa_family)
		{
		case AF_INET:
			{
				struct sockaddr_in* in = (struct sockaddr_in*)addr.ptr();
				in->sin_port = port;
				break;
			}
		case AF_INET6:
			{
				struct sockaddr_in6* in6 = (struct sockaddr_in6*)addr.ptr();
				in6->sin6_port = port;
				break;
			}
		}
		if(execute(addr.ptr(), addr.len()))
			return;
	}

	int error = WSAGetLastError();
	ErrorCode err(false, error, concat<tstring>(_T("连接到地址 '") 
		, name
		, _T(":")
		, ::toString(port)
		, _T("' 时发生错误 - ")
		, lastError(error)));
	onError_(err, context_);

	delete this;
}

void ConnectCommand::onResolveError(const tstring& name, errcode_t error)
{
	ErrorCode err(false, error, concat<tstring>(_T("解析主机名 '") 
		, name
		, _T("' 失败 - ")
		, lastError(error)));
	onError_(err, context_);

	delete this;
}

//void GetName(const tstring& name, short port, ConnectCommand* command)
//{
//	std::string host = toNarrowString(name);
//	struct hostent* ent = gethostbyname(host.c_str());
//	if(null_ptr == ent)
//		command->onErrorByDnsQuery(name, port);
//	else
//		command->onCompleteByDnsQuery(name, port, ent); 
//}
//
//void ConnectCommand::onErrorByDnsQuery(const tstring& name, short port)
//{
//	int error = WSAGetLastError();
//	ErrorCode err(false, error, concat<tstring>(_T("解析主机名 '") 
//		, name
//		, _T(":")
//		, ::toString(port)
//		, _T("' 失败 - ")
//		, lastError(error)));
//	onError_(err, context_);
//
//	delete this;
//}
//
//class ConnectTask : public IRunnable
//{
//public:
//	ConnectTask(ConnectCommand* cmd)
//		: command(cmd)
//		, len(sizeof(struct sockaddr_in))
//	{
//		memset(&addr, 0, len);
//	}
//
//	virtual ~ConnectTask()
//	{
//	}
//
//	virtual void run()
//	{
//		command->onRun(name, port, *(struct sockaddr*)&addr, len);
//	}
//
//	ConnectCommand* command;
//	tstring name;
//	short port;
//	struct sockaddr_in addr;
//	int len;
//};
//
//void ConnectCommand::onCompleteByDnsQuery(const tstring& name,short port, struct hostent* ent)
//{
//	std::auto_ptr<ConnectTask> runnable(new ConnectTask(this));
//
//	runnable->name = name;
//	runnable->port = port;
//
//	runnable->addr.sin_family = AF_INET;
//	runnable->addr.sin_addr.s_addr = *(ULONG*)ent->h_addr_list[0];
//	runnable->addr.sin_port = htons(port);
//
//	//Note   All I/O initiated by a given thread is canceled when that thread 
//	//exits. For overlapped sockets, pending asynchronous operations can fail
//	//if the thread is closed before the operations complete. See ExitThread 
//	//for more information.
//
//	if(this->core_->send(runnable.get()))
//	{
//		runnable.release();
//		return;
//	}
//
//	int error = WSAGetLastError();
//	ErrorCode err(false, error, concat<tstring>(_T("投递一个 dns 完成任务 '") 
//		, name
//		, _T(":")
//		, ::toString(port)
//		, _T("' 时发生错误 - ")
//		, lastError(error)));
//	onError_(err, context_);
//
//	delete this;
//}
//
//
//void ConnectCommand::onRun(tstring& name, short port, struct sockaddr& addr, int len)
//{
//	if(execute(&addr, len))
//		return;
//
//	int error = WSAGetLastError();
//	ErrorCode err(false, error, concat<tstring>(_T("连接到地址 '") 
//		, name
//		, _T(":")
//		, ::toString(port)
//		, _T("' 时发生错误 - ")
//		, lastError(error)));
//	onError_(err, context_);
//
//	delete this;
//}

void ConnectCommand::dnsQuery(const tstring& name)
{
	core_->resolver().ResolveHostByName(name, this, &::onResolveComplete, &::onResolveError, 10000);
	//create_thread(&GetName, name, networking::fetchPort(host_.c_str()), this);
}

bool ConnectCommand::execute()
{
	struct sockaddr addr;
	int len = sizeof(addr);

	if(! networking::stringToAddress((LPTSTR)host_.c_str(), &addr, &len))
	{
		dnsQuery(networking::fetchAddr(host_.c_str()));
		return true;
	}
	return execute(&addr, len);
}

bool ConnectCommand::execute(struct sockaddr* addr, int len)
{
	if (INVALID_SOCKET == socket_)
		socket_ = WSASocket(addr->sa_family,SOCK_STREAM,IPPROTO_TCP,0,0,WSA_FLAG_OVERLAPPED);

	SOCKADDR_IN bindAddr;
	bindAddr.sin_family = AF_INET;
	bindAddr.sin_port = 0; // htons(0);
	bindAddr.sin_addr.s_addr = ADDR_ANY; //htonl(ADDR_ANY);
	
	// NOTICE: 超级奇怪必须绑定一下, MS 说的
	if(SOCKET_ERROR == ::bind(socket_,(struct sockaddr*)&bindAddr, sizeof(bindAddr)))
		return false;

	if (!core_->bind((HANDLE)socket_, null_ptr))
		return false;

	//DWORD bytesTransferred = 0;
	if ( networking::connectEx(socket_,addr, len, null_ptr, 0,null_ptr, this))
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
		if(!networking::addressToString(&name, namelen, _T("tcp"), local))
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
