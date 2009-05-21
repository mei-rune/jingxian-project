
# include "pro_config.h"
# include "jingxian/exception.hpp"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/command_queue.h"

_jingxian_begin

TCPAcceptor::TCPAcceptor(proactor* core, IProtocolFactory* protocolFactory, const tchar* endpoint)
: proactor_(core)
, protocolFactory_(protocolFactory)
, socket_()
, endpoint_(endpoint)
, toString_(_T("TCPAcceptor"))
{
}
	
TCPAcceptor::~TCPAcceptor()
{
}

time_t TCPAcceptor::timeout () const
{
	ThrowException( NotImplementedException );
}

const IEndpoint& TCPAcceptor::bindPoint() const
{
	return endpoint_;
}

void TCPAcceptor::stopListening()
{
	socket_.close();
}

bool TCPAcceptor::startListening()
{
	if(!socket_.open(AF_INET , SOCK_STREAM, IPPROTO_TCP))
	{
		LOG_ERROR( _logger, _T)"启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 创建 socket失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

#pragma warning(disable: 4267)
	if ( SOCKET_ERROR == ::bind( socket_.get_handle(), endpoint_.addr(), endpoint_.size() ) )
#pragma warning(default: 4267)
	{		
		LOG_ERROR( _logger, _T)"启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 绑定端口失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(SOCKET_ERROR == ::listen( socket_.get_handle(), SOMAXCONN))
	{
		LOG_ERROR( _logger, _T)"启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 -  '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(!proactor_.send(command_queue::createAcceptCommand( socket.get_handle() )))
	{
		LOG_ERROR( _logger, _T)"启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - '") << lastError()
			<< _T("'" ));
		return false;
	}
	TRACE( _logger, _T("启动监听地址 '") << endpoint_ 
		<< _T("' 成功!") );

	toString_ = _T("TCPAcceptor[ socket=") + socket_.toString() + _T(",address=") + endpoint_.toString() + _T("]");
	return true;
}

IProtocolFactory& TCPAcceptor::protocolFactory()
{
	return *protocolFactory_;
}

IDictionary& TCPAcceptor::misc()
{
	ThrowException( NotImplementedException );
}

const IDictionary& TCPAcceptor::misc() const
{
	ThrowException( NotImplementedException );
}

const tstring& TCPAcceptor::toString() const
{
	return toString_;
}

TCPAcceptorFactory::TCPAcceptorFactory(proactor* core)
: proactor_(core)
, toString_(_T("TCPAcceptorFactory"))
{
}

TCPAcceptorFactory::~TCPAcceptorFactory()
{
}

IAcceptor* TCPAcceptorFactory::createAcceptor(const tchar* endPoint, IProtocolFactory* protocolFactory)
{
	if(is_null(endPoint))
		return null_ptr;

	return new TCPAcceptor(proactor_, protocolFactory, endPoint );
}

const tstring& TCPAcceptorFactory::toString() const
{
	return toString_;
}

_jingxian_end
