
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"
# include "jingxian/networks/ConnectedSocket.h"


_jingxian_begin

AcceptCommand::AcceptCommand(IOCPServer* core
                             , int family
                             , SOCKET listenHandle
                             , const tstring& listenAddr
                             , OnBuildConnectionComplete onComplete
                             , OnBuildConnectionError onError
                             , void* context)
        : core_(core)
        , onComplete_(onComplete)
        , onError_(onError)
        , context_(context)
        , listener_(listenHandle)
        , listenAddr_(listenAddr)
        , socket_(WSASocket(family, SOCK_STREAM, IPPROTO_TCP, 0, 0, WSA_FLAG_OVERLAPPED))
        , ptr_((char*)my_malloc(sizeof(SOCKADDR_STORAGE)*2 + sizeof(SOCKADDR_STORAGE)*2 + 100))
        , len_(sizeof(SOCKADDR_STORAGE)*2 + sizeof(SOCKADDR_STORAGE)*2 + 100)
{
    memset(ptr_, 0, len_);
}

AcceptCommand::~AcceptCommand()
{
    my_free(ptr_);
    ptr_ = null_ptr;

    if (INVALID_SOCKET != socket_)
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
        ErrorCode err(error, concat<tstring>(_T("接受器 '")
                      , listenAddr_
                      , _T("' 获取连接请求失败 - ")
                      , lastError(error)));
        onError_(err, context_);
        return;
    }

    //if (!acceptor_->isListening())
    //{
    //  ErrorCode err(_T("接受器 '")
    //    + listenAddr_
    //    + _T("' 获取连接请求返回,但已经停止监听!"));
    //  onError_(err, context_);
    //  return;
    //}

    sockaddr *local_addr = 0;
    sockaddr *remote_addr = 0;
    int local_size = 0;
    int remote_size = 0;

    /// 超级奇怪!如果直接用 GetAcceptExSockaddrs 会失败,通过调
    /// 用 GetAcceptExSockaddrs 的函数指针就没有问题.
    networking::getAcceptExSockaddrs(ptr_,
                                     0,
                                     sizeof(SOCKADDR_STORAGE) + sizeof(SOCKADDR_STORAGE),
                                     sizeof(SOCKADDR_STORAGE) + sizeof(SOCKADDR_STORAGE),
                                     &local_addr,
                                     &local_size,
                                     &remote_addr,
                                     &remote_size);

    tstring peer;
    if (!networking::addressToString(remote_addr, remote_size, _T("tcp"), peer))
    {
        int errCode = ::WSAGetLastError();
        ErrorCode err(errCode, concat<tstring, tchar*, tstring, tchar*, tstring>(_T("接受器 '")
                      , listenAddr_
                      , _T("' 获取连接请求返回,获取远程地址失败 -")
                      , lastError(errCode)));
        onError_(err, context_);
        return;
    }

    tstring host;
    if (!networking::addressToString(local_addr, local_size, _T("tcp"), host))
    {
        int errCode = ::WSAGetLastError();
        ErrorCode err(errCode, concat<tstring>(_T("接受器 '")
                      , listenAddr_
                      , _T("' 获取连接请求返回,获取本地地址失败 -")
                      , lastError(errCode)));
        onError_(err, context_);
        return;
    }


    if (SOCKET_ERROR == setsockopt(socket_, SOL_SOCKET, SO_UPDATE_ACCEPT_CONTEXT,
                                   (char *) &listener_, sizeof(listener_)))
    {
        int errCode = ::WSAGetLastError();

        ErrorCode err(errCode, _T("接受器 '")
                      + listenAddr_
                      + _T("' 获取连接请求返回,在对 socket 句柄设置 SO_UPDATE_ACCEPT_CONTEXT 选项时发生错误 - ")
                      + lastError(errCode));
        onError_(err, context_);
        return;
    }

    std::auto_ptr<ConnectedSocket> connectedSocket(new ConnectedSocket(core_, socket_, host, peer));
    socket_ = INVALID_SOCKET;

    if (!core_->bind((HANDLE)(connectedSocket->handle()), connectedSocket.get()))
    {
        int errCode = ::WSAGetLastError();
        ErrorCode err(errCode, concat<tstring>(_T("初始化来自 '")
                      , peer
                      , _T("' 的连接时，绑定到iocp发生错误 - ")
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
                             , 0 //必须为0,否则会有大量的连接处于accept中，因为客户端只
                             //建立连接，没有发送数据。
                             , sizeof(SOCKADDR_STORAGE) + sizeof(SOCKADDR_STORAGE)
                             , sizeof(SOCKADDR_STORAGE) + sizeof(SOCKADDR_STORAGE)
                             , &bytesTransferred
                             , this))
        return true;

    if (WSA_IO_PENDING == ::WSAGetLastError())
        return true;

    return false;
}

_jingxian_end
