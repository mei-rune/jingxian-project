
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

bool TCPAcceptor::bindPoint() const
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

bool TCPAcceptor::startListening()
{
	if( connection_status::disconnected != status_ )
	{
		LOG_ERROR( _logger, _T)"����������ַ '") << endpoint_ 
			<< _T("' ʱ�������� - ״̬����ȷ - '") << status_
			<< _T("'" ));
		return false;
	}

	if(!socket_.open(AF_INET , SOCK_STREAM, IPPROTO_TCP))
	{
		LOG_ERROR( _logger, _T)"����������ַ '") << endpoint_ 
			<< _T("' ʱ�������� - ���� socketʧ�� - '") << lastError()
			<< _T("'" ));
		return false;
	}

#pragma warning(disable: 4267)
	if ( SOCKET_ERROR == ::bind( socket_.get_handle(), endpoint_.addr(), endpoint_.size() ) )
#pragma warning(default: 4267)
	{		
		LOG_ERROR( _logger, _T)"����������ַ '") << endpoint_ 
			<< _T("' ʱ�������� - �󶨶˿�ʧ�� - '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(SOCKET_ERROR == ::listen( socket_.get_handle(), SOMAXCONN))
	{
		LOG_ERROR( _logger, _T)"����������ַ '") << endpoint_ 
			<< _T("' ʱ�������� -  '") << lastError()
			<< _T("'" ));
		return false;
	}

	if(!proactor_.send(command_queue::createAcceptCommand( socket.get_handle() )))
	{
		LOG_ERROR( _logger, _T)"����������ַ '") << endpoint_ 
			<< _T("' ʱ�������� - '") << lastError()
			<< _T("'" ));
		return false;
	}
	INFO( _logger, _T("����������ַ '") << endpoint_ 
		<< _T("' �ɹ�!") );

	status_ = connection_status::listening;
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


void TCPAcceptor::on_complete(size_t bytes_transferred
								, int success
								, void *completion_key
								, u_int32_t error)
{
	if (!isListening())
	{
		INFO(logger_, _T("������ '")<< endpoint_ <<_T("' ��ȡ���ӷ���,���Ѿ�ֹͣ����!"));
		decrementAccepting();
		return;
	}

	if (!success)
	{
		Exception err = new SocketException(error);
		_acceptor.OnError(new AcceptError(_acceptor.BindPoint, string.Format("��ȡ�˿�[{0}]������ʧ�� - {1}!", _acceptor.BindPoint, err.Message), err));
	}
	else
	{
		InitializeConnection(bytes_transferred, completion_key);
	}

	try
	{
		doAccept();
	}
	catch (AcceptError e)
	{
		_acceptor.OnError(e);
	}
	catch (Exception e)
	{
		_acceptor.OnError(new AcceptError(_acceptor.BindPoint, e));
	}

	decrementAccepting();
}

void TCPAcceptor::InitializeConnection(int bytes_transferred, object context)
{
	IntPtr bytePointer = Marshal.UnsafeAddrOfPinnedArrayElement(
		_byteBuffer.Array, _byteBuffer.End);
	HazelAddress localAddr = null;
	HazelAddress remoteAddr = null;

	HazelSocket.GetAcceptExSockaddrs(bytePointer
		, 0
		, HazelAddress.MaxSize + 4
		, HazelAddress.MaxSize + 4
		, out localAddr
		, out remoteAddr);

	_byteBuffer.End += bytes_transferred;
	_byteBuffer.Begin += (2 * (HazelAddress.MaxSize + 4));

	_socket.SetLocalAddress(localAddr);
	_socket.SetRemoteAddress(remoteAddr);

	_socket.SetSockOpt(SocketOptionLevel.Socket, SocketOptionName.UpdateAcceptContext, _socket.Handle);


	_acceptor.Logger.InfoFormat("��ȡ������[{0}]������,��ʼ��ʼ��!", _socket.RemoteEndPoint);

	IOCPCore core = _acceptor.Core.GetNextCore();
	if (!core.Bind(_socket))
	{
		Win32Exception err = new Win32Exception();
		_acceptor.OnError(new InitializeError(_acceptor.BindPoint, string.Format("��ʼ������[{0}]������ʱ���󶨵�iocp�������� - {0}", _socket.RemoteEndPoint, err.Message), err));
		return;
	}

	InitializeError initializeError = null;
	ConnectedSocket connectedSocket = null;
	try
	{
		IDictionary<string, object> misc = (null == _acceptor.ProtocolFactory) ? null : _acceptor.ProtocolFactory.Misc;
		connectedSocket = new ConnectedSocket(core, _socket, new ProtocolContext(null, misc));
		IProtocol protocol = _acceptor.CreateProtocol(connectedSocket);
		core.InitializeConnection(connectedSocket, protocol);
		_acceptor.Logger.InfoFormat("��ʼ������[{0}]�����ӳɹ�!", _socket.RemoteEndPoint);
		_socket = null;
	}
	catch (InitializeError e)
	{
		initializeError = e;
	}
	catch (Exception e)
	{
		initializeError = new InitializeError(_socket.RemoteEndPoint, "�ڴ���������ʱ�����쳣!", e);
	}

	if (null != initializeError)
	{
		if (null != connectedSocket)
			connectedSocket.ReleaseSocket();

		_acceptor.OnError(initializeError);
	}
}

void TCPAcceptor::decrementAccepting()
{
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
