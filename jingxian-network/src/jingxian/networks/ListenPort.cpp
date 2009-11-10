# include "pro_config.h"
# include "jingxian/networks/ListenPort.h"

_jingxian_begin

ListenPort::ListenPort(IReactorCore* core
                       , IProtocolFactory* protocolFactory
                       , IAcceptor* acceptor)
        : reactor_(core)
        , protocolFactory_(protocolFactory)
        , acceptor_(acceptor)
        , isPending_(false)
        , errorCount_(0)
        , logger_(_T("jingxian.system.listenPort"))
{
    toString_ = _T("ListenPort[address=")
                + acceptor_.bindPoint()
                + _T("]");
}

ListenPort::~ListenPort()
{
}

bool ListenPort::start()
{
    if (!acceptor_.initialize())
    {
        LOG_TRACE(logger_, toString()
                  << _T(" 尝试监听地址 '")
                  << acceptor_.bindPoint()
                  << _T("' 时发生错误‘")
                  << lastError()
                  << _T("’"));
        return false;
    }

    LOG_TRACE(logger_, toString()
              << _T(" 尝试监听地址 '")
              << acceptor_.bindPoint()
              << _T("' 成功!"));

    isPending_ = true;
    acceptor_.accept(this
                     , &ListenPort::onComplete
                     , &ListenPort::onError
                     , reactor_);
    return true;
}

void ListenPort::stop()
{
    acceptor_.close();
}

void ListenPort::onComplete(ITransport* transport
                            , IReactorCore* core)
{
    errorCount_ = 0;
    isPending_ = false;
    if (!reactor_->isRunning())
    {
        LOG_TRACE(logger_, toString()
                  << _T(" 系统已退出!"));
        return;
    }

    transport->bindProtocol(protocolFactory_->
                            createProtocol(transport, reactor_));
    transport->initialize();

    isPending_ = true;
    acceptor_.accept(this
                     , &ListenPort::onComplete
                     , &ListenPort::onError
                     , reactor_);
}

void ListenPort::onError(const ErrorCode& err
                         , IReactorCore* core)
{
    isPending_ = false;
    if (!reactor_->isRunning())
    {
        LOG_TRACE(logger_, toString()
                  << _T(" 系统已退出!"));
        return;
    }

    if (errorCount_ > 20)
    {
        LOG_FATAL(logger_, toString()
                  << _T(" 尝试接收请求失败超过 '")
                  << errorCount_
                  << _T("' 次,退出服务"));
        reactor_->interrupt();
        return;
    }

    ++ errorCount_;
    isPending_ = true;
    acceptor_.accept(this
                     , &ListenPort::onComplete
                     , &ListenPort::onError
                     , reactor_);
}

bool ListenPort::isPending() const
{
    return isPending_;
}

const tstring& ListenPort::toString() const
{
    return toString_;
}

_jingxian_end
