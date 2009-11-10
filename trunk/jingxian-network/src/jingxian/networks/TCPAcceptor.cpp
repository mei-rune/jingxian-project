
# include "pro_config.h"
# include "jingxian/exception.h"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/AcceptCommand.h"

_jingxian_begin

TCPAcceptor::TCPAcceptor(IOCPServer* core, const tchar* endpoint)
        : core_(core)
        , socket_(INVALID_SOCKET)
        , endpoint_(endpoint)
        , status_(connection_status::disconnected)
        , logger_(_T("jingxian.acceptor.tcpAcceptor"))
        , toString_(_T("TCPAcceptor"))
{
    toString_ = _T("TCPAcceptor[address=") + endpoint_ + _T("]");
}

TCPAcceptor::~TCPAcceptor()
{
    stopListening();

    assert(connection_status::disconnected == status_);
}

time_t TCPAcceptor::timeout() const
{
    ThrowException(NotImplementedException);
}

const tstring& TCPAcceptor::bindPoint() const
{
    return endpoint_;
}

bool TCPAcceptor::isListening() const
{
    return connection_status::listening == status_;
}

void TCPAcceptor::stopListening()
{
    if (INVALID_SOCKET != socket_)
    {
        closesocket(socket_);
        socket_ = INVALID_SOCKET;
    }
    status_ = connection_status::disconnected;
}

void TCPAcceptor::accept(OnBuildConnectionComplete onComplete
                         , OnBuildConnectionError onError
                         , void* context)
{
    if (!core_->isRunning())
    {
        tstring descr = concat<tstring>(_T("启动监听地址 '")
                                        , endpoint_
                                        , _T("' 时发生错误 - '系统已停止'"));

        LOG_ERROR(logger_, descr);

        ErrorCode err(0, descr);
        onError(err, context);
        return ;
    }

    std::auto_ptr< ICommand> command(new AcceptCommand(this->core_, family_, socket_, endpoint_, onComplete, onError, context));
    if (! command->execute())
    {
        int code = WSAGetLastError();
        tstring descr = concat<tstring>(_T("启动监听地址 '")
                                        , endpoint_
                                        , _T("' 时发生错误 - '")
                                        , lastError(code)
                                        , _T("'"));

        LOG_ERROR(logger_, descr);

        ErrorCode err(code, descr);
        onError(err, context);
        return ;
    }

    command.release();
}

bool TCPAcceptor::startListening()
{
    if (connection_status::disconnected != status_)
    {
        LOG_ERROR(logger_, _T("启动监听地址 '") << endpoint_
                  << _T("' 时发生错误 - 状态不正确 - '") << status_
                  << _T("'"));
        return false;
    }
    SOCKADDR_STORAGE  addr;
    int len = sizeof(SOCKADDR_STORAGE);
    if (!networking::stringToAddress(endpoint_.c_str(), (struct sockaddr*)&addr, &len))
    {
        LOG_ERROR(logger_, _T("监听地址 '") << endpoint_
                  << _T("' 格式不正确 - ") << lastError(WSAGetLastError()));
        return false;
    }

    if (INVALID_SOCKET == (socket_ = socket(addr.ss_family, SOCK_STREAM, IPPROTO_TCP)))
    {
        LOG_ERROR(logger_, _T("启动监听地址 '") << endpoint_
                  << _T("' 时发生错误 - 创建 socket失败 - '") << lastError()
                  << _T("'"));
        return false;
    }

//#pragma warning(disable: 4267)
    if (SOCKET_ERROR == ::bind(socket_, (struct sockaddr*)&addr, len))
//#pragma warning(default: 4267)
    {
        LOG_ERROR(logger_, _T("启动监听地址 '") << endpoint_
                  << _T("' 时发生错误 - 绑定端口失败 - '") << lastError()
                  << _T("'"));
        return false;
    }

    if (SOCKET_ERROR == ::listen(socket_, SOMAXCONN))
    {
        LOG_ERROR(logger_, _T("启动监听地址 '") << endpoint_
                  << _T("' 时发生错误 -  '") << lastError()
                  << _T("'"));
        return false;
    }

    if (!core_->bind((HANDLE)socket_, this))
    {
        LOG_ERROR(logger_, _T("绑定监听地址 '") << endpoint_
                  << _T("' 到完成端口时发生错误 -  '") << lastError()
                  << _T("'"));
        return false;
    }

    status_ = connection_status::listening;
    family_ = addr.ss_family;

    LOG_INFO(logger_, _T("启动监听地址 '") << endpoint_
             << _T("' 成功!"));

    toString_ = concat<tstring>(_T("TCPAcceptor[ socket=")
                                , ::toString((int)socket_)
                                , _T(",address=")
                                , endpoint_
                                , _T("]"));

    return true;
}


bool TCPAcceptor::initialize()
{
    return startListening();
}

void TCPAcceptor::close()
{
    stopListening();
}

const tstring& TCPAcceptor::toString() const
{
    return toString_;
}

TCPAcceptorFactory::TCPAcceptorFactory(IOCPServer* core)
        : core_(core)
        , toString_(_T("TCPAcceptorFactory"))
{
}

TCPAcceptorFactory::~TCPAcceptorFactory()
{
}

IAcceptor* TCPAcceptorFactory::createAcceptor(const tchar* endPoint)
{
    if (is_null(endPoint))
        return null_ptr;

    return new TCPAcceptor(core_, endPoint);

    //std::auto_ptr<TCPAcceptor> acceptor(new TCPAcceptor(core_, endPoint));
    //if( acceptor->startListening())
    //  return acceptor.release();
    //return null_ptr;
}

const tstring& TCPAcceptorFactory::toString() const
{
    return toString_;
}

_jingxian_end
