
# include "pro_config.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/RunCommand.h"
# include "jingxian/networks/commands/command_queue.h"


#ifdef _MEMORY_DEBUG
#undef THIS_FILE
#define new	   DEBUG_NEW  
#define malloc DEBUG_MALLOC  
static char THIS_FILE[] = __FILE__;  
#endif // _MEMORY_DEBUG

_jingxian_begin

IOCPServer::IOCPServer(void)
	: completion_port_( null_ptr )
	, _timeout( 5*1000 )
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

	close();
}


bool IOCPServer::open ( size_t number_of_threads )
{
	if( ! is_null(completion_port_) )
		return false;

	number_of_threads_ = number_of_threads;
	completion_port_ = ::CreateIoCompletionPort (INVALID_HANDLE_VALUE,
		0,
		0,
		number_of_threads_);

	return  is_null(completion_port_);
}

void IOCPServer::close (void)
{
	if (is_null(completion_port_ ))
		return ;

	for (;;)
	{
		OVERLAPPED *overlapped = 0;
		u_long bytes_transferred = 0;
#if defined (JINGXIAN_WIN64)
		ULONG_PTR completion_key = 0;
#else
		ULONG completion_key = 0;
#endif /* JINGXIAN_WIN64 */

		BOOL res = ::GetQueuedCompletionStatus
			(completion_port_,
			&bytes_transferred,
			&completion_key,
			&overlapped,
			0);  // poll

		if ( is_null(overlapped) || FALSE == res )
			break;

		ICommand *asynch_result =
			(ICommand *) overlapped;

		command_queue::release( asynch_result );
	}

	::CloseHandle( completion_port_);
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
int IOCPServer::handle_events (u_int32_t milli_seconds)
{
	OVERLAPPED *overlapped = 0;
	u_long bytes_transferred = 0;

	ULONG_PTR completion_key = 0;

	BOOL result = ::GetQueuedCompletionStatus (completion_port_,
		&bytes_transferred,
		&completion_key,
		&overlapped,
		milli_seconds);
	if ( FALSE == result && is_null(overlapped) )
	{
		switch ( GetLastError() )
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
		u_long error = 0;
		if( !result )
			error = GetLastError();

		this->application_specific_code (asynch_result,
			bytes_transferred,
			(void *) completion_key,
			error );
	}
	return 0;
}

void IOCPServer::application_specific_code (ICommand *asynch_result,
														  size_t bytes_transferred,
														  const void *completion_key,
														  u_long error)
{
	try
	{
		asynch_result->on_complete (bytes_transferred,
			error == 0 ? 0 : 1,
			(void *) completion_key,
			error );
	}
	catch( std::exception& e )
	{
		FATAL( _logger , "error :" << e.what() );
	}
	catch( ... )
	{	
		FATAL( _logger , "unkown error!" );
	}
	
	command_queue::release( asynch_result );
}

bool IOCPServer::post(ICommand *result)
{
	if( is_null( result ) )
		return false;
		
	DWORD bytes_transferred = 0;
	ULONG_PTR comp_key = 0;

	return TRUE == ::PostQueuedCompletionStatus (completion_port_, // completion port
		bytes_transferred ,      // xfer count
		comp_key,               // completion key
		result                  // overlapped
		);
}

//HANDLE IOCPServer::handle()
//{
//	return completion_port_;
//}

bool IOCPServer::bind (HANDLE handle, void *completion_key)
{
	ULONG_PTR comp_key = reinterpret_cast < ULONG_PTR >( completion_key);

	return 0 != ::CreateIoCompletionPort (handle,
		this->completion_port_,
		comp_key,
		this->number_of_threads_);
}

void IOCPServer::connectWith(const tchar* endPoint
                            , OnBuildConnectionSuccess onSuccess
                            , OnBuildConnectionError onError
                            , void* context )
{
	StringArray<tchar> sa = split_with_string( endPoint, _T("://") );
	if( 2 != sa.size() )
	{
		LOG_ERROR( _logger, _T("尝试连接到 '") << endPoint
			<< _T("' 时发生错误 - 地址格式不正确"));
		
		ErrorCode error( _T("地址格式不正确!") );
		onError( error, context);
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
		
		ErrorCode error(err.c_str());
		onError( error, context);
		return ;
	}
    
	     
    it->second->connect( endPoint, onSuccess, onError, context );
}

IAcceptor* IOCPServer::listenWith(const tchar* endPoint)
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
	     
	std::auto_ptr<IAcceptor> acceptor(it->second->createAcceptor(endPoint));
    
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
	std::auto_ptr< ICommand > ptr(new RunCommand(this, runnable));
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
		if( 1 ==  handle_events(_timeout) )
			onIdle();
	}
}
	
void IOCPServer::interrupt()
{
	_isRunning = false;
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
