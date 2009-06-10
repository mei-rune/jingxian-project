
# include "pro_config.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/RunCommand.h"


#ifdef _MEMORY_DEBUG
#undef THIS_FILE
#define new	   DEBUG_NEW  
#define malloc DEBUG_MALLOC  
static char THIS_FILE[] = __FILE__;  
#endif // _MEMORY_DEBUG

_jingxian_begin

IOCPServer::IOCPServer(void)
	: _timeout( 5*1000 )
	, _isRunning( false )
	, _logger( null_ptr )
	, toString_( _T("IOCPServer") )
{
	_logger = logging::makeLogger("IOCPServer");
	_acceptorFactories[_T("tcp")] = new TCPAcceptorFactory( this );
}

IOCPServer::~IOCPServer(void)
{
	for( stdext::hash_map<tstring, IAcceptor* >::iterator it = _acceptors.begin()
		; it != _acceptors.end()
		; ++ it)
	{
		delete (it->second);
	}

	for( stdext::hash_map<tstring, IAcceptorFactory* >::iterator it = _acceptorFactories.begin()
		; it != _acceptorFactories.end()
		; ++ it)
	{
		delete (it->second);
	}

	for( stdext::hash_map<tstring, IConnectionBuilder* >::iterator it = _connectionBuilders.begin()
		; it != _connectionBuilders.end()
		; ++ it)
	{
		delete (it->second);
	}
}

void IOCPServer::connectWith(const tchar* endPoint
                            , BuildProtocol buildProtocol
                            , OnConnectError onConnectError
                            , void* context )
{
	StringArray<tchar> sa = split_with_string( endPoint, _T("://") );
	if( 2 != sa.size() )
	{
		LOG_ERROR( _logger, _T("尝试连接到 '") << endPoint
			<< _T("' 时发生错误 - 地址格式不正确"));
		
		IllegalArgumentException error( _T("地址格式不正确!") );
		onConnectError( error, context);
		return ;
	}
	
    stdext::hash_map<tstring, IConnectionBuilder*>::iterator it = 
                                         _connectionBuilders.find( to_lower<tstring>( sa.ptr( 0 ) ));
    if( it == _connectionBuilders.end() )
	{
		LOG_ERROR( _logger, _T("尝试连接到 '") << endPoint 
			<< _T("' 时发生错误 - 不能识别的协议‘") << sa.ptr(0) 
			<< _T("’") );
		
		tstring err = _T("不能识别的协议 - ");
		err += sa.ptr(0);
		err += _T("!");
		
		IllegalArgumentException error(err);
		onConnectError( error, context);
		return ;
	}
    
	     
    it->second->connect( endPoint, buildProtocol, onConnectError, context );
}

IAcceptor* IOCPServer::listenWith(const tchar* endPoint
			, IProtocolFactory* protocolFactory)
{
	// NOTICE: 用字符串地址直接查找是不好的，转换成 IEndpoint 对象进行比较才更准确
	tstring addr = endPoint;
	stdext::hash_map<tstring,IAcceptor*>::iterator acceptorIt = _acceptors.find( to_lower<tstring>( addr ));
	if( _acceptors.end() != acceptorIt )
	{
		TRACE( _logger, _T("已经创建过监听器 '") << endPoint 
			<< _T("' 了!") );
		return null_ptr;
	}
	
	StringArray<tchar> sa = split_with_string( endPoint, "://" );
	if( 2 != sa.size() )
	{
		LOG_ERROR( _logger, _T("尝试监听地址 '") << endPoint 
			<< _T("' 时发生错误 - 地址格式不正确!") );
		 return null_ptr;
	}

    stdext::hash_map<tstring, IAcceptorFactory*>::iterator it =
                                      _acceptorFactories.find(to_lower<tstring>(sa.ptr( 0 )));
    if( it == _acceptorFactories.end() )
	{
		LOG_ERROR( _logger, _T("尝试监听地址 '") << endPoint 
			<< _T("' 时发生错误 - 不能识别的协议‘") << sa.ptr(0) 
			<< _T("’") );
	     return null_ptr;
	}
	     
	std::auto_ptr< IAcceptor> acceptor = it->second->createAcceptor(endPoint, protocolFactory);
    
 //   if( acceptor->startListening() )
	//{
	//	TRACE( _logger, "尝试监听地址 '" << endPoint 
	//		<< "' 时发生错误‘" << lastError()
	//		<< "’" );
	//	return null_ptr;
 //   }
	//
 //   TRACE( _logger, _T("尝试监听地址 '") << endPoint 
	//	<< _T("' 成功‘") << sa.ptr(0) 
	//	<< _T("’") );
	
    _acceptors[ endPoint ] = acceptor.get();
    
    return acceptor.release();
}
	
bool IOCPServer::send( IRunnable* runnable )
{
	std::auto_ptr< ICommand > ptr(new RunCommand(&_proactor, runnable));
	if(ptr->execute())
	{
		ptr.release();
		return true;
	}
	return false;
}
	
void IOCPServer::runForever()
{
	_isRunning = true;
	while( _isRunning )
	{
		if( 1 ==  _proactor.handle_events(_timeout) )
			onIdle();
	}
}
	
void IOCPServer::interrupt()
{
	_isRunning = false;
}
	
bool IOCPServer::bind(HANDLE systemHandler, void* completion_key)
{
	return _proactor.bind(systemHandler,completion_key);
}

TCPFactory& IOCPServer::tcpFactory()
{
	return tcpFactory_;
}

void IOCPServer::onIdle()
{
	
}

void IOCPServer::onExeception(int errCode, const tstring& description)
{
	LOG_ERROR( _logger, _T("发生错误 - '") << errCode << _T("' ") 
			<< description );
}
	
const tstring& IOCPServer::toString() const
{
		return toString_;
}


_jingxian_end
