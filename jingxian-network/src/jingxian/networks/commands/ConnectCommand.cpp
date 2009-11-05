
# include "pro_config.h"
# include <Winsock2.h>
# include <Ws2tcpip.h>
# include "jingxian/networks/commands/ConnectCommand.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/threading/thread.h"


_jingxian_begin



void OnResolveComplete(const tstring& name, const tstring& port, const IPHostEntry& hostEntry, void* context)
{
    ConnectCommand* cmd = (ConnectCommand*)context;
    cmd->onResolveComplete(name, port, hostEntry);
}

void OnResolveError(const tstring& name, const tstring& port, errcode_t err, void* context)
{
    ConnectCommand* cmd = (ConnectCommand*)context;
    cmd->onResolveError(name, port, err);
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
        , socket_(INVALID_SOCKET)
{
}

ConnectCommand::~ConnectCommand()
{
    if (INVALID_SOCKET != socket_)
    {
        closesocket(socket_);
        socket_ = INVALID_SOCKET;
    }
}


void ConnectCommand::onResolveComplete(const tstring& name, const tstring& port, const IPHostEntry& hostEntry)
{
    for (std::vector<HostAddress>::const_iterator it = hostEntry.AddressList.begin()
            ; it != hostEntry.AddressList.end(); ++ it)
    {
        if (execute(it->ptr(), it->len()))
            return;
    }

    int error = WSAGetLastError();
    ErrorCode err(error, concat<tstring>(_T("连接到地址 '")
                  , name
                  , _T(":")
                  , ::toString(port)
                  , _T("' 时发生错误 - ")
                  , lastError(error)));
    onError_(err, context_);

    delete this;
}

void ConnectCommand::onResolveError(const tstring& name, const tstring& port, errcode_t error)
{
    ErrorCode err(error, concat<tstring>(_T("解析主机名 '")
                  , name
                  , _T("' 失败 - ")
                  , lastError(error)));
    onError_(err, context_);

    delete this;
}

void ConnectCommand::dnsQuery(const tchar* name, const tchar* port)
{
    core_->resolver().ResolveHostByName(name, port, this, &OnResolveComplete, &OnResolveError, 10000);
    //create_thread(&GetName, name, networking::fetchPort(host_.c_str()), this);
}

bool ConnectCommand::execute()
{
    SOCKADDR_STORAGE addr;
    int len = sizeof(addr);

    if (! networking::stringToAddress((LPTSTR)host_.c_str(), (struct sockaddr*)&addr, &len))
    {
        dnsQuery(networking::fetchAddr(host_.c_str()).c_str(), networking::fetchPort(host_.c_str()));
        return true;
    }
    return execute((struct sockaddr*)&addr, len);
}

bool ConnectCommand::execute(const struct sockaddr* addr, int len)
{
    if (INVALID_SOCKET == socket_)
        socket_ = WSASocket(addr->sa_family, SOCK_STREAM, IPPROTO_TCP, 0, 0, WSA_FLAG_OVERLAPPED);

    SOCKADDR_STORAGE bindAddr;
    memset(&bindAddr, 0, sizeof(SOCKADDR_STORAGE));
    bindAddr.ss_family = addr->sa_family;
    //((struct sockaddr_in6*)&bindAddr)->sin6_port = 0; // htons(0);
    //((struct sockaddr_in6*)&bindAddr)->sin6_addr.s_addr = ADDR_ANY; //htonl(ADDR_ANY);

    // NOTICE: 超级奇怪必须绑定一下, MS 说的
    if (SOCKET_ERROR == ::bind(socket_, (struct sockaddr*)&bindAddr, sizeof(bindAddr)))
        return false;

    if (!core_->bind((HANDLE)socket_, null_ptr))
        return false;

    //DWORD bytesTransferred = 0;
    if (networking::connectEx(socket_, addr, len, null_ptr, 0, null_ptr, this))
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
        ErrorCode err(error, concat<tstring>(_T("连接到 '")
                      , host_
                      , _T("' 失败 - ")
                      , lastError(error)));
        onError_(err, context_);
        return;
    }

    try
    {
        setsockopt(socket_,
                   SOL_SOCKET,
                   SO_UPDATE_CONNECT_CONTEXT,
                   NULL,
                   0);

        struct sockaddr name;
        int namelen = sizeof(name);

        if (SOCKET_ERROR == getsockname(socket_, & name, &namelen))
        {
            ErrorCode err(error, concat<tstring>(_T("连接到 '")
                          , host_
                          , _T("' 成功,取本地地址时失败 - ")
                          , lastError(error)));
            onError_(err, context_);
            return;
        }

        tstring local;
        if (!networking::addressToString(&name, namelen, _T("tcp"), local))
        {
            ErrorCode err(error, concat<tstring>(_T("连接到 '")
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
        ErrorCode err(error, concat<tstring>(_T("连接到 '")
                      , host_
                      , _T("' 成功,初始化时失败 - ")
                      , toTstring(e.what())));
        onError_(err, context_);
    }
}

_jingxian_end
