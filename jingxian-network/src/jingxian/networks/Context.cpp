
# include "pro_config.h"
# include "jingxian/exception.hpp"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/AcceptCommand.h"

_jingxian_begin

TCPAcceptor::TCPAcceptor(IOCPServer* core, const tchar* endpoint)
: core_(core)
, socket_(INVALID_SOCKET)
, endpoint_(endpoint)
, status_(connection_status::disconnected)
, logger_(null_ptr)
, toString_(_T("TCPAcceptor"))
{
	toString_ = _T("TCPAcceptor[address=") + endpoint_ + _T("]");
	logger_ = logging::makeLogger(_T("TCPAcceptor"));
}

TCPAcceptor::~TCPAcceptor()
{
	stopListening();
	assert( connection_status::disconnected == status_ );
}

time_t TCPAcceptor::timeout () const
{
	ThrowException( NotImplementedException );
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
	closesocket(socket_);
	socket_ = INVALID_SOCKET;
	status_ = connection_status::disconnected;
}

void TCPAcceptor::accept(OnBuildConnectionComplete onComplete
                            , OnBuildConnectionError onError
                            , void* context)
{
	std::auto_ptr< ICommand> command(new AcceptCommand(this, onComplete, onError, context));
	if(! command->execute() )
	{
		int code = WSAGetLastError();
		tstring descr = _T("启动监听地址 '") + endpoint_ 
			+ _T("' 时发生错误 - '") + lastError(code)
			+ _T("'" );

		LOG_ERROR( logger_,descr );

		ErrorCode err(false, code, descr);
		onError(err, context);
		return ;
	}

	command.release();
}

bool TCPAcceptor::startListening()
{
	if( connection_status::disconnected != status_ )
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 状态不正确 - '") << status_
			<< _T("'" ));
		return false;
	}
	struct sockaddr addr;
	addr.sa_family = AF_INET;

	tstring endpoint = endpoint_;
	tstring::size_type index = endpoint.find(_T("://"));
	if(tstring::npos != index)
	{
		tstring prefix = endpoint.substr(0, index);
		if(0 == string_traits<tstring::value_type>::stricmp( prefix.c_str(), "tcp"))
		{
			addr.sa_family = AF_INET;
		}
		else if(0 == string_traits<tstring::value_type>::stricmp( prefix.c_str(), "tcp6")
			  || 0 == string_traits<tstring::value_type>::stricmp( prefix.c_str(), "tcpv6"))
		{
			addr.sa_family = AF_INET6;
		}
		else
		{
			LOG_ERROR( logger_, _T("监听地址 '") << endpoint_ 
				<< _T("' 格式不正确 - 不可识别的协议类型"));
		}
		endpoint = endpoint.substr(index + 3);
	}

	int len = sizeof(addr);
	if(SOCKET_ERROR == ::WSAStringToAddress((LPSTR)endpoint.c_str(), addr.sa_family, 0, &addr, &len))
	{
		LOG_ERROR( logger_, _T("监听地址 '") << endpoint_ 
			<< _T("' 格式不正确 - ") << lastError(WSAGetLastError()));
		return false;
	}

	//struct sockaddr addr;
	//addr.sa_family = AF_INET;
	//((sockaddr_in*)&addr)->sin_addr.s_addr = inet_addr(toNarrowString( endpoint.substr(0, index)).c_str());
	//((sockaddr_in*)&addr)->sin_port = htons(atoi(endpoint.substr(index+1).c_str()));


	if(INVALID_SOCKET == (socket_ = socket(addr.sa_family , SOCK_STREAM, IPPROTO_TCP)))
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 创建 socket失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

//#pragma warning(disable: 4267)
	if (SOCKET_ERROR == ::bind(socket_,&addr, len))
//#pragma warning(default: 4267)
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 绑定端口失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(SOCKET_ERROR == ::listen( socket_, SOMAXCONN))
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 -  '") << lastError()
			<< _T("'" ));
		return false;
	}
	
	if(!core_->bind((HANDLE)socket_, this))
	{
		LOG_ERROR( logger_, _T("绑定监听地址 '") << endpoint_ 
			<< _T("' 到完成端口时发生错误 -  '") << lastError()
			<< _T("'" ));
		return false;
	}

	status_ = connection_status::listening;

	INFO( logger_, _T("启动监听地址 '") << endpoint_ 
		<< _T("' 成功!") );
	toString_ = _T("TCPAcceptor[ socket=") + ::toString((int)socket_) + _T(",address=") + endpoint_ + _T("]");

	return true;
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
	if(is_null(endPoint))
		return null_ptr;

	std::auto_ptr<TCPAcceptor> acceptor(new TCPAcceptor(core_, endPoint));
	if( acceptor->startListening())
		return acceptor.release();
	return null_ptr;
}

const tstring& TCPAcceptorFactory::toString() const
{
	return toString_;
}

_jingxian_end
