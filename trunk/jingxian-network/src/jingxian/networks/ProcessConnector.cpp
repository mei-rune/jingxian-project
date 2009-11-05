
# include "pro_config.h"
# include "jingxian/exception.h"
# include "jingxian/networks/ProcessConnector.h"
# include "jingxian/networks/commands/CreateProcessCommand.h"

_jingxian_begin

ProcessConnector::ProcessConnector(IOCPServer* core)
        : core_(core)
        , logger_(null_ptr)
        , toString_(_T("ProcessConnector"))
{
    toString_ = _T("ProcessConnector");
    logger_ = logging::makeLogger(_T("jingxian.connector.processConnector"));
}

ProcessConnector::~ProcessConnector()
{
    delete logger_;
    logger_ = null_ptr;
}

void ProcessConnector::connect(const tchar* endPoint
                           , OnBuildConnectionComplete onComplete
                           , OnBuildConnectionError onError
                           , void* context)
{
    std::auto_ptr< ICommand> command(new CreateProcessCommand(core_
                                     , endPoint
                                     , onComplete
                                     , onError
                                     , context));
    if (! command->execute())
    {
        int code = GetLastError();
        tstring descr = concat<tstring>(_T("连接到地址 '")
                                        , endPoint
                                        , _T("' 时发生错误 - ")
                                        , lastError(code));
        LOG_ERROR(logger_, descr);

        ErrorCode err(false, code, descr);
        onError(err, context);
        return ;
    }

    command.release();
}

const tstring& ProcessConnector::toString() const
{
    return toString_;
}

_jingxian_end
