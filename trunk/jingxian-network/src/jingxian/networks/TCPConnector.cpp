
# include "pro_config.h"
# include "jingxian/exception.h"
# include "jingxian/networks/TCPConnector.h"
# include "jingxian/networks/commands/ConnectCommand.h"

_jingxian_begin

TCPConnector::TCPConnector(IOCPServer* core)
        : core_(core)
        , logger_(_T("jingxian.connector.tcpConnector"))
        , toString_(_T("TCPConnector"))
{
    toString_ = _T("TCPConnector");
}

TCPConnector::~TCPConnector()
{
}

void TCPConnector::connect(const tchar* endPoint
                           , OnBuildConnectionComplete onComplete
                           , OnBuildConnectionError onError
                           , void* context)
{
    std::auto_ptr< ICommand> command(new ConnectCommand(core_
                                     , endPoint
                                     , onComplete
                                     , onError
                                     , context));
    if (! command->execute())
    {
        int code = WSAGetLastError();
        tstring descr = concat<tstring>(_T("连接到地址 '")
                                        , endPoint
                                        , _T("' 时发生错误 - ")
                                        , lastError(code));
        LOG_ERROR(logger_, descr);

        ErrorCode err(code, descr);
        onError(err, context);
        return ;
    }

    command.release();
}

const tstring& TCPConnector::toString() const
{
    return toString_;
}

_jingxian_end
