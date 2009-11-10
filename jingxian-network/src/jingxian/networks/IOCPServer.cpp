
# include "pro_config.h"
# include "jingxian/exception.h"
# include "jingxian/directory.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/TCPConnector.h"
# include "jingxian/networks/commands/RunCommand.h"
# include "jingxian/networks/commands/command_queue.h"

_jingxian_begin

IOCPServer::IOCPServer(void)
        : completion_port_(null_ptr)
        , isRunning_(false)
        , logger_(_T("jingxian.system"))
        , toString_(_T("IOCPServer"))
{

    resolver_.initialize(this);
    acceptorFactories_[_T("tcp")] = new TCPAcceptorFactory(this);
    connectionBuilders_[_T("tcp")] = new TCPConnector(this);

    path_ = simplify(getApplicationDirectory());

    tstring logPath = simplify(combinePath(path_, _T("log")));
    if (!existDirectory(logPath))
        createDirectory(logPath);
}

IOCPServer::~IOCPServer(void)
{
    for (stdext::hash_map<tstring, IAcceptorFactory* >::iterator it = acceptorFactories_.begin()
            ; it != acceptorFactories_.end()
            ; ++ it)
    {
        delete(it->second);
    }

    for (stdext::hash_map<tstring, IConnectionBuilder* >::iterator it = connectionBuilders_.begin()
            ; it != connectionBuilders_.end()
            ; ++ it)
    {
        delete(it->second);
    }

    close();

    for (stdext::hash_map<tstring, ListenPort*>::iterator it = listenPorts_.begin()
            ; it != listenPorts_.end(); ++it)
    {
        delete(it->second);
    }
}


bool IOCPServer::initialize(size_t number_of_threads)
{
    if (!is_null(completion_port_))
        return false;

    number_of_threads_ = number_of_threads;
    completion_port_ = ::CreateIoCompletionPort(INVALID_HANDLE_VALUE,
                       0,
                       0,
                       number_of_threads_);

    return  !is_null(completion_port_);
}

bool IOCPServer::isPending()
{
    for (stdext::hash_map<tstring, ListenPort*>::iterator it = listenPorts_.begin()
            ; it != listenPorts_.end(); ++it)
    {
        if (it->second->isPending())
            return true;
    }
    return false;
}

void IOCPServer::wait(time_t seconds)
{
    time_t old = time(NULL);

    while ((time(NULL) - old) < 3*60)
    {
        if (sessions_.empty()  // 没有连接了
                && !isPending()) // 没有未完成的请求了
            break;

        if (-1 == handle_events(1000))
            break;
    }
}

void IOCPServer::close(void)
{
    interrupt();

    if (is_null(completion_port_))
        return ;

    wait(3*60);

    ::CloseHandle(completion_port_);
    completion_port_ = null_ptr;

}

/// If the function dequeues a completion packet for a successful I/O operation
/// from the completion port, the return value is nonzero. The function stores
/// information in the variables pointed to by the lpNumberOfBytesTransferred,
/// lpCompletionKey, and lpOverlapped parameters
///
/// 如果函数从端口取出一个完成包，且完成操作是成功的，则返回非0值。上下文数据
/// 保存在lpNumberOfBytesTransferred，lpCompletionKey，lpOverlapped中
///
/// If *lpOverlapped is NULL and the function does not dequeue a completion packet
/// from the completion port, the return value is zero. The function does not
/// store information in the variables pointed to by the lpNumberOfBytes and
/// lpCompletionKey parameters. To get extended error information, call GetLastError.
/// If the function did not dequeue a completion packet because the wait timed out,
/// GetLastError returns WAIT_TIMEOUT.
///
/// 如lpOverlapped 是NULL，没有从端口取出一个完成包，则返回0值。lpNumberOfBytesTransferred
/// ，lpCompletionKey，lpOverlapped也没有保存上下文数据，可以用GetLastError取
/// 得详细错误。如果没有从端口取出一个完成包，可能是超时，GetLastError返回WAIT_TIMEOUT
///
/// If *lpOverlapped is not NULL and the function dequeues a completion packet for
/// a failed I/O operation from the completion port, the return value is zero.
/// The function stores information in the variables pointed to by lpNumberOfBytes,
/// lpCompletionKey, and lpOverlapped. To get extended error information, call GetLastError.
///
/// 如果 lpOverlapped 不是NULL，但完成操作是失败的，则返回0值。上下文数据保存在
/// lpNumberOfBytesTransferred，lpCompletionKey，lpOverlapped中，可以用GetLastError
/// 取得详细错误。
///
/// If a socket handle associated with a completion port is closed, GetQueuedCompletionStatus
/// returns ERROR_SUCCESS, with *lpOverlapped non-NULL and lpNumberOfBytes equal zero.
///
/// 如一个socket句柄被关闭了，GetQueuedCompletionStatus返回ERROR_SUCCESS， lpOverlapped
/// 不是NULL,lpNumberOfBytes等于0。
///
/// </summary>
int IOCPServer::handle_events(uint32_t milli_seconds)
{
    OVERLAPPED *overlapped = 0;
    u_long bytes_transferred = 0;

    ULONG_PTR completion_key = 0;

    BOOL result = ::GetQueuedCompletionStatus(completion_port_,
                  &bytes_transferred,
                  &completion_key,
                  &overlapped,
                  milli_seconds);
    if (FALSE == result && is_null(overlapped))
    {
        switch (GetLastError())
        {
        case WAIT_TIMEOUT:
            return 1;

        case ERROR_SUCCESS:
            return 0;

        default:
            return -1;
        }
    }
    else
    {
        ICommand *asynch_result = (ICommand *) overlapped;
        errcode_t error = 0;
        if (!result)
            error = GetLastError();

        this->application_specific_code(asynch_result,
                                        bytes_transferred,
                                        (void *) completion_key,
                                        error);
    }
    return 0;
}

void IOCPServer::application_specific_code(ICommand *asynch_result,
        size_t bytes_transferred,
        const void *completion_key,
        errcode_t error)
{
    try
    {
        asynch_result->on_complete(bytes_transferred,
                                   error == 0,
                                   (void *) completion_key,
                                   error);
    }
    catch (std::exception& e)
    {
        LOG_FATAL(logger_ , "error :" << e.what());
    }
    catch (...)
    {
        LOG_FATAL(logger_ , "unkown error!");
    }

    command_queue::release(asynch_result);
}

bool IOCPServer::post(ICommand *result)
{
    if (is_null(result))
        return false;

    DWORD bytes_transferred = 0;
    ULONG_PTR comp_key = 0;

    return TRUE == ::PostQueuedCompletionStatus(completion_port_,  // completion port
            bytes_transferred ,      // xfer count
            comp_key,               // completion key
            result                  // overlapped
                                               );
}

//HANDLE IOCPServer::handle()
//{
//  return completion_port_;
//}

bool IOCPServer::bind(HANDLE handle, void *completion_key)
{
    ULONG_PTR comp_key = reinterpret_cast < ULONG_PTR >(completion_key);

    return 0 != ::CreateIoCompletionPort(handle,
                                         this->completion_port_,
                                         comp_key,
                                         this->number_of_threads_);
}

void IOCPServer::connectWith(const tchar* endPoint
                             , OnBuildConnectionComplete onComplete
                             , OnBuildConnectionError onError
                             , void* context)
{
    StringArray<tchar> sa = split_with_string(endPoint, _T("://"));
    if (2 != sa.size())
    {
        LOG_ERROR(logger_, _T("尝试连接到 '") << endPoint
                  << _T("' 时发生错误 - 地址格式不正确"));

        ErrorCode error(_T("地址格式不正确!"));
        onError(error, context);
        return ;
    }

    stdext::hash_map<tstring, IConnectionBuilder*>::iterator it =
        connectionBuilders_.find(to_lower<tstring>(sa.ptr(0)));
    if (it == connectionBuilders_.end())
    {
        LOG_ERROR(logger_, _T("尝试连接到 '") << endPoint
                  << _T("' 时发生错误 - 不能识别的协议‘") << sa.ptr(0)
                  << _T("’"));

        tstring err = _T("不能识别的协议 - ");
        err += sa.ptr(0);
        err += _T("!");

        ErrorCode error(err.c_str());
        onError(error, context);
        return ;
    }


    it->second->connect(endPoint, onComplete, onError, context);
}

bool IOCPServer::listenWith(const tchar* endPoint, IProtocolFactory* protocolFactory)
{
    // NOTICE: 用字符串地址直接查找是不好的，转换成 IEndpoint 对象进行比较才更准确
    tstring addr = endPoint;
    stdext::hash_map<tstring, ListenPort*>::iterator acceptorIt = listenPorts_.find(to_lower<tstring>(addr));
    if (listenPorts_.end() != acceptorIt)
    {
        LOG_TRACE(logger_, _T("已经创建过监听器 '") << endPoint
                  << _T("' 了!"));
        return false;
    }

    StringArray<tchar> sa = split_with_string(endPoint, _T("://"));
    if (2 != sa.size())
    {
        LOG_ERROR(logger_, _T("尝试监听地址 '") << endPoint
                  << _T("' 时发生错误 - 地址格式不正确!"));
        return false;
    }

    stdext::hash_map<tstring, IAcceptorFactory*>::iterator it =
        acceptorFactories_.find(to_lower<tstring>(sa.ptr(0)));
    if (it == acceptorFactories_.end())
    {
        LOG_ERROR(logger_, _T("尝试监听地址 '") << endPoint
                  << _T("' 时发生错误 - 不能识别的协议‘") << sa.ptr(0)
                  << _T("’"));
        return false;
    }

    listenPorts_[endPoint] = new ListenPort(this, protocolFactory
                                            , it->second->createAcceptor(sa.ptr(1)));
    return false;
}

bool IOCPServer::send(IRunnable* runnable)
{
    std::auto_ptr< ICommand > ptr(new RunCommand(completion_port_, runnable));
    if (ptr->execute())
    {
        ptr.release();
        return true;
    }
    return false;
}

void IOCPServer::runForever()
{
    LOG_CRITICAL(logger_, _T("服务开始运行!"));

    isRunning_ = true;

    std::list<ListenPort*> instances;
    for (stdext::hash_map<tstring, ListenPort*>::iterator it = listenPorts_.begin()
            ; it != listenPorts_.end();)
    {
        stdext::hash_map<tstring, ListenPort* >::iterator current =  it++;
        if (!current->second->start())
        {
            isRunning_ = false;

            LOG_CRITICAL(logger_, _T("启动 '") << current->first << _T("' 组件失败!"));
            break;
        }
        instances.push_back(current->second);
    }

    if (!isRunning_)
    {
        /// 启动服务失败,将已启动成功的停止
        for (std::list<ListenPort*>::iterator it = instances.begin()
                ; it != instances.end(); ++it)
        {
            (*it)->stop();
        }

        LOG_CRITICAL(logger_, _T("服务启动失败,退出!"));
        return;
    }

    while (isRunning_)
    {
        if (1 ==  handle_events(5*1000))
            onIdle();
    }

    LOG_CRITICAL(logger_, _T("服务停止,开始清理工作!"));

    for (stdext::hash_map<tstring, ListenPort*>::iterator it = listenPorts_.begin()
            ; it != listenPorts_.end();)
    {
        stdext::hash_map<tstring, ListenPort* >::iterator current =  it++;
        current->second->stop();
    }

    tstring reason = _T("系统停止");
    for (SessionList::iterator it = sessions_.begin()
                                    ; it != sessions_.end();)
    {
        SessionList::iterator current =  it++;
        (*current)->transport()->disconnection(reason);
    }

    wait(3*60);

    LOG_CRITICAL(logger_, _T("清理工作完成,退出服务!"));
}

void IOCPServer::interrupt()
{
    LOG_CRITICAL(logger_, _T("服务收到停止请求!"));
    isRunning_ = false;
}

bool IOCPServer::isRunning() const
{
    return isRunning_;
}

IDNSResolver& IOCPServer::resolver()
{
    return resolver_;
}

const tstring& IOCPServer::basePath() const
{
    return path_;
}

void IOCPServer::onIdle()
{

}

SessionList::iterator IOCPServer::addSession(ISession* session)
{
    return sessions_.insert(sessions_.end(), session);
}

void IOCPServer::removeSession(SessionList::iterator& it)
{
    sessions_.erase(it);
}

void IOCPServer::onExeception(int errCode, const tstring& description)
{
    LOG_ERROR(logger_, _T("发生错误 - '") << errCode << _T("' ")
              << description);
}

const tstring& IOCPServer::toString() const
{
    return toString_;
}


_jingxian_end
