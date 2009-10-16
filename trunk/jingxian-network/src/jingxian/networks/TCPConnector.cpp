
# include "pro_config.h"
# include "jingxian/exception.h"
# include "jingxian/networks/TCPConnector.h"
# include "jingxian/networks/commands/ConnectCommand.h"

_jingxian_begin

TCPConnector::TCPConnector(IOCPServer* core)
: core_(core)
, logger_(null_ptr)
, toString_(_T("TCPConnector"))
{
	toString_ = _T("TCPConnector");
	logger_ = logging::makeLogger(_T("TCPConnector"));
}

TCPConnector::~TCPConnector()
{
	delete logger_;
	logger_ = null_ptr;
}



void TCPConnector::connect(const tchar* endPoint
                       , OnBuildConnectionComplete onComplete
                       , OnBuildConnectionError onError
                       , void* context )
{
	std::auto_ptr< ICommand> command(new ConnectCommand(core_, endPoint, onComplete, onError, context));
	if(! command->execute() )
	{
		int code = WSAGetLastError();
		tstring descr = concat<tstring>(_T("���ӵ���ַ '")
			, endPoint 
			, _T("' ʱ�������� - '")
			, lastError(code)
			, _T("'" ));
		LOG_ERROR(logger_, descr);

		ErrorCode err(false, code, descr);
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