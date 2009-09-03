

OS_INLINE TCPServer::TCPServer (void)
: socket_(AF_INET , SOCK_STREAM, IPPROTO_TCP )
{
	toString_ = _T("TCPServer - ") + socket_.toString();
}

OS_INLINE TCPServer::~TCPServer (void)
{
}

OS_INLINE BaseSocket& TCPServer::socket()
{
	return socket_;
}

OS_INLINE  const BaseSocket& TCPServer::socket() const
{
	return socket_;
}

OS_INLINE  const NetAddress& TCPServer::bind_addr() const
{
	return bind_addr_;
}

OS_INLINE bool TCPServer::is_good() const
{
	return socket_.is_good();
}

OS_INLINE bool TCPServer::bind( const NetAddress& addr)
{
#pragma warning(disable: 4267)
	if ( SOCKET_ERROR != ::bind( socket_.handle(), addr.addr(), addr.size() ) )
#pragma warning(default: 4267)
	{
		bind_addr_ = addr;
		toString_ = _T("TCPServer ") + socket_.toString() + _T(" ���� ") + bind_addr_.toString();
		return true;
	}
	return false;
}

OS_INLINE bool TCPServer::listen( int backlog )
{
	if( SOCKET_ERROR != ::listen( socket_.handle(), backlog ) )
	{
		toString_ = _T("TCPServer ") + socket_.toString() + _T(" ������ ") + bind_addr_.toString();
		return true;
	}
	return false;
}

OS_INLINE bool TCPServer::accept( TCPClient& accepted)
{
#pragma warning(disable: 4267)
	int len = accepted.remote_addr().size();
#pragma warning(default: 4267)
	accepted.socket().set_handle( ::accept( socket_.handle(),( sockaddr* ) accepted.remote_addr().addr(),&len ) );
	return accepted.is_good();
}

OS_INLINE bool TCPServer::accept(TCPClient& accepted
						, void* data_buffer
						, size_t data_len
						, size_t local_addr_len
						, size_t remote_addr_len
						, OVERLAPPED& overlapped )
{
	DWORD bytesReceived = 0;
#pragma warning(disable: 4267)
	return ( TRUE == BaseSocket::__acceptex( socket_.handle(), accepted.socket().handle(),data_buffer, data_len, local_addr_len, remote_addr_len, &bytesReceived, &overlapped ));
#pragma warning(default: 4267)
}

OS_INLINE void TCPServer::swap( TCPServer& r)
{
	bind_addr_.swap( r.bind_addr_ );
	socket_.swap( r.socket_ );
	toString_.swap( r.toString_ );
}

OS_INLINE const tstring& TCPServer::toString( ) const
{
	return toString_;
}