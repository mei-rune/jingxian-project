

OS_INLINE TCPClient::TCPClient (void)
: socket_( )   
, blocking_( true )
{
	//AF_INET , SOCK_STREAM, IPPROTO_TCP
}

OS_INLINE TCPClient::~TCPClient (void)
{
}

OS_INLINE bool TCPClient::readable()
{
	TIMEVAL time_val;
	time_val.tv_sec = 0;
	time_val.tv_usec = 0;
	return socket_.poll( time_val, select_read );
}

OS_INLINE bool TCPClient::writable()
{
	TIMEVAL time_val;
	time_val.tv_sec = 0;
	time_val.tv_usec = 0;
	return socket_.poll( time_val, select_write );
}

OS_INLINE void TCPClient::blocking(bool val)
{
	if( val )
	{
		socket_.enable( FIONBIO );
		blocking_ = false;
	}
	else
	{
		socket_.disable( FIONBIO );
		blocking_ = true;
	}
}

OS_INLINE bool TCPClient::blocking() const
{
	return blocking_;
}

OS_INLINE const NetAddress& TCPClient::remote_addr () const
{
	return remote_addr_;
}

OS_INLINE const NetAddress& TCPClient::local_addr () const
{
  return local_addr_;
}

OS_INLINE BaseSocket& TCPClient::socket()
{
	return socket_;
}
	
OS_INLINE const BaseSocket& TCPClient::socket() const
{
	return socket_;
}

OS_INLINE bool TCPClient::is_good() const
{
	return socket_.is_good();
}

OS_INLINE ssize_t TCPClient::recv (void *buf,
                   size_t len,
                   int flags )
{

	return ::recv ( socket_.handle (),
                    ( char* )buf,
                    ( int )len,
                    flags );
}

OS_INLINE ssize_t TCPClient::send (const void *buf,
                   size_t len,
                   int flags )
{

    return ::send (socket_.handle(), (const char *) buf, ( int )len, flags);
}

OS_INLINE ssize_t TCPClient::recvv (iovec* iov, size_t n)
{
	DWORD bytes_recvd = 0;
	DWORD Flags = 0;
	int r = WSARecv( socket_.handle()
		, iov
		, ( DWORD ) n
		, &bytes_recvd
		, &Flags
		, 0
		, 0 );
	if( SOCKET_ERROR != r )
		return  bytes_recvd;
	return r;
}

OS_INLINE ssize_t TCPClient::sendv ( const iovec* iov,
                    size_t n )
{
	DWORD bytes_sent = 0;
	int r = ::WSASend ( socket_.handle(),
		(WSABUF *) iov,
		( DWORD )n,
		&bytes_sent,
		0,
		0,
		0);
	if( SOCKET_ERROR != r )
		return bytes_sent;
	return r;
}

OS_INLINE bool TCPClient::send_n( const char* buf, size_t length)
{
	do
	{
 #pragma warning(disable: 4267)
		int n = ::send( socket_.handle(), buf, length, 0 ); 
 #pragma warning(default: 4267)
		if( 0 >= n)
			return false;

		length -= n;
		buf += n;
	}while( 0 < length );

	return true;
}

OS_INLINE bool TCPClient::recv_n( char* buf, size_t length)
{
	do
	{
 #pragma warning(disable: 4267)
		int n = ::recv( socket_.handle(), buf, length, 0 ); 
 #pragma warning(default: 4267)

		if( 0 >= n)
			return false;

		length -= n;
		buf += n;
	}while( 0 < length );

	return true;
}

OS_INLINE bool TCPClient::sendv_n( const iovec* wsaBuf, size_t size)
{
	std::vector<iovec> buf( wsaBuf, wsaBuf + size );
	iovec* p = &buf[0];

	do
	{
		DWORD numberOfBytesSent =0;
 #pragma warning(disable: 4267)
		if( SOCKET_ERROR == ::WSASend( socket_.handle(), p, size, &numberOfBytesSent,0,0 , 0 ) )
 #pragma warning(default: 4267)
			return false;

		do
		{
			if( numberOfBytesSent < p->len)
			{
				p->len -= numberOfBytesSent;
				p->buf = p->buf + numberOfBytesSent;
				break;
			}
			numberOfBytesSent -= p->len;
			++ p;
			-- size;
		}
		while( 0 < numberOfBytesSent );
	}
	while( 0 < size );

	return true;
}

OS_INLINE bool TCPClient::recvv_n( iovec* wsaBuf, size_t size)
{
	iovec* p = wsaBuf;

	do
	{
		DWORD numberOfBytesRecvd =0;
 #pragma warning(disable: 4267)
		if( SOCKET_ERROR == ::WSARecv( socket_.handle(), p, size, &numberOfBytesRecvd,0,0 , 0 ) )
 #pragma warning(default: 4267)
			return false;

		do
		{
			if( numberOfBytesRecvd < p->len)
			{
				p->len -= numberOfBytesRecvd;
				p->buf = p->buf + numberOfBytesRecvd;
				break;
			}
			numberOfBytesRecvd -= p->len;
			++ p;
			-- size;
		}
		while( 0 < numberOfBytesRecvd );
	}
	while( 0 < size );

	return true;
}

OS_INLINE bool TCPClient::send (const void *buf, size_t n,
				   OVERLAPPED& overlapped)
{
	DWORD bytes_written;
	DWORD short_nbyte = static_cast< DWORD >( n);
	return ::WriteFile ( ( HANDLE ) socket_.handle(), buf, short_nbyte, &bytes_written, &overlapped) ? true : false;
}

OS_INLINE bool TCPClient::recvv (iovec* iov, size_t n,
					OVERLAPPED& overlapped)
{
	DWORD NumberOfBytesRecvd = 0;
	DWORD Flags = 0;
	return ( TRUE == WSARecv( socket_.handle()
		, iov
		, ( DWORD ) n
		, &NumberOfBytesRecvd
		, &Flags
		, &overlapped
		, 0 ));
}

OS_INLINE bool TCPClient::recv (void *buf, size_t n,
				   OVERLAPPED& overlapped)
{
	DWORD bytes_read = 0;
	return TRUE == ::ReadFile( ( HANDLE )socket_.handle ()
		, buf
		, static_cast < DWORD>( n)
		, &bytes_read
		, &overlapped );
}

OS_INLINE   bool TCPClient::transmit (const iopack* iov, size_t n )
{
	return ( TRUE == BaseSocket::__transmitpackets( socket_.handle(),
		( iopack* )iov,
		( DWORD )n,
		0,
		0,
		0) );
}

OS_INLINE   bool TCPClient::transmit (const iopack* iov, size_t n,
                 OVERLAPPED& overlapped)
{
	return ( TRUE == BaseSocket::__transmitpackets( socket_.handle(),
		( iopack* )iov,
		( DWORD )n,
		0,
		&overlapped,
		0) );
}

OS_INLINE bool TCPClient::transmit ( HANDLE hFile
				, size_t nNumberOfBytesToWrite
				, size_t nNumberOfBytesPerSend
				, io_file_buf* transmitBuffers )
{
	return (TRUE == BaseSocket::__transmitfile( socket_.handle(),
		hFile,
        (DWORD) nNumberOfBytesToWrite,
        (DWORD) nNumberOfBytesPerSend,
        0,
		transmitBuffers,
        0 ) );
}

OS_INLINE bool TCPClient::transmit ( HANDLE hFile
				, size_t nNumberOfBytesToWrite
				, size_t nNumberOfBytesPerSend
				, io_file_buf* transmitBuffers
                , OVERLAPPED& overlapped)
{
	return ( TRUE == BaseSocket::__transmitfile( socket_.handle(),
		hFile,
        (DWORD) nNumberOfBytesToWrite,
        (DWORD) nNumberOfBytesPerSend,
        &overlapped,
		transmitBuffers,
        0 ) );
}

OS_INLINE bool TCPClient::connect( const NetAddress& addr )
{
	if( !socket_.open( AF_INET , SOCK_STREAM, IPPROTO_TCP ) )
		return false;

	if( 0 != ::connect( this->socket_.handle(), addr.addr(), (int)addr.size() ) )
		return false;
	
	this->remote_addr_ = addr;
	int len = (int)this->local_addr_.size();
	::getsockname( this->socket_.handle(), this->local_addr_.addr(), &len );
	return true;
}

OS_INLINE bool TCPClient::connect( const NetAddress& addr
									  ,OVERLAPPED& overlapped )
{
	
	if( !socket_.open( AF_INET , SOCK_STREAM, IPPROTO_TCP ) )
		return false;

	DWORD sendbytes = 0;
#pragma warning(disable: 4267)
	return (TRUE == BaseSocket::__connectex( socket_.handle(), addr.addr(), addr.size(), NULL, 0, &sendbytes, &overlapped));
 #pragma warning(default: 4267)
}

OS_INLINE bool TCPClient::connect( const NetAddress& addr
									  , const void* send_buffer
									  , size_t send_data_len
									  , OVERLAPPED& overlapped )
{
	if( !socket_.open( AF_INET , SOCK_STREAM, IPPROTO_TCP ) )
		return false;

	 DWORD sendbytes = 0;
 #pragma warning(disable: 4267)
	 return (TRUE == BaseSocket::__connectex( socket_.handle(), addr.addr(), addr.size(), (void*)send_buffer, send_data_len, &sendbytes, &overlapped));
 #pragma warning(default: 4267)

}
