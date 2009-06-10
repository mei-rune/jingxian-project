
# include "pro_config.h"
# include "jingxian/exception.hpp"
# include "jingxian/networks/TCPAcceptor.h"
# include "jingxian/networks/commands/AcceptCommand.h"

_jingxian_begin

TCPAcceptor::TCPAcceptor(IOCPServer* core, IProtocolFactory* protocolFactory, const tchar* endpoint)
: server_(core)
, protocolFactory_(protocolFactory)
, socket_()
, endpoint_(endpoint)
, status_(connection_status::disconnected)
, logger_(null_ptr)
, toString_(_T("TCPAcceptor"))
{
}

TCPAcceptor::~TCPAcceptor()
{
	assert( connection_status::disconnected == status_ );
}

time_t TCPAcceptor::timeout () const
{
	ThrowException( NotImplementedException );
}

const IEndpoint& TCPAcceptor::bindPoint() const
{
	return endpoint_;
}

bool TCPAcceptor::isListening() const
{
	return connection_status::listening == status_;
}

void TCPAcceptor::stopListening()
{
	socket_.close();
	status_ = connection_status::disconnecting;
}

bool TCPAcceptor::doAccept()
{
	std::auto_ptr< ICommand> command(new AcceptCommand( this ));
	if(! command->execute() )
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - '") << lastError()
			<< _T("'" ));
		return false;
	}

	status_ = connection_status::listening;
	command.release();
	return true;
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

	if(!socket_.open(AF_INET , SOCK_STREAM, IPPROTO_TCP))
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 创建 socket失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

#pragma warning(disable: 4267)
	if ( SOCKET_ERROR == ::bind( socket_.handle(),( const sockaddr*) endpoint_.addr(), endpoint_.size() ) )
#pragma warning(default: 4267)
	{		
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 - 绑定端口失败 - '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(SOCKET_ERROR == ::listen( socket_.handle(), SOMAXCONN))
	{
		LOG_ERROR( logger_, _T("启动监听地址 '") << endpoint_ 
			<< _T("' 时发生错误 -  '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(!doAccept())
		return false;

	INFO( logger_, _T("启动监听地址 '") << endpoint_ 
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

TCPAcceptorFactory::TCPAcceptorFactory(IOCPServer* core)
: server_(core)
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

	return new TCPAcceptor(server_, protocolFactory, endPoint );
}

const tstring& TCPAcceptorFactory::toString() const
{
	return toString_;
}

_jingxian_end
