

LPFN_TRANSMITFILE BaseSocket::__transmitfile = 0;
LPFN_ACCEPTEX BaseSocket::__acceptex = 0;
LPFN_TRANSMITPACKETS BaseSocket::__transmitpackets = 0;
LPFN_CONNECTEX BaseSocket::__connectex = 0;
LPFN_DISCONNECTEX BaseSocket::__disconnectex = 0;
LPFN_GETACCEPTEXSOCKADDRS BaseSocket::__getacceptexsockaddrs = 0;


OS_INLINE BaseSocket::BaseSocket (void)
: handle_ (INVALID_SOCKET )
, toString_( _T("INVALID_SOCKET" ))

{
}

OS_INLINE BaseSocket::BaseSocket (int protocol_family,
					int type, 
					int protocol,
                    int reuse_addr)
: handle_ (INVALID_SOCKET )
, toString_( _T("INVALID_SOCKET") )
{
  this->open (protocol_family,
				  type, 
                  protocol,
                  reuse_addr);
}

OS_INLINE BaseSocket::BaseSocket (int protocol_family,
					int type, 
					int protocol,
                    LPWSAPROTOCOL_INFO protocolinfo,
                    GROUP g,
                    u_long flags,
                    int reuse_addr)
: handle_ (INVALID_SOCKET )
{
  this->open ( protocol_family,
				  type,
                  protocol,
                  protocolinfo,
                  g,
                  flags,
                  reuse_addr);				  
}

OS_INLINE BaseSocket::~BaseSocket (void)
{
	close();
}

OS_INLINE bool BaseSocket::is_good() const
{
	return INVALID_SOCKET != this->handle ();
}

OS_INLINE bool BaseSocket::open ( int protocol_family,
				int type, 
                int protocol,
                int reuse_addr)
{
  int one = 1;

  this->set_handle (socket (protocol_family,
                                    type,
                                    protocol));

  if (this->handle () == INVALID_SOCKET )
    return false;
  else if (protocol_family != PF_UNIX 
           && reuse_addr 
           && !this->set_option (SOL_SOCKET,
                                SO_REUSEADDR,
                                &one,
                                sizeof one) )
    {
      this->close ();
      return false;
    }
  return true;
}

OS_INLINE bool BaseSocket::open (int protocol_family,
				int type, 
                int protocol,
                LPWSAPROTOCOL_INFO protocolinfo,
                GROUP g,
                u_long flags,
                int reuse_addr)
{
  this->set_handle (::WSASocket (protocol_family,
                                    type,
                                    protocol,
                                    protocolinfo,
                                    g,
                                    flags));
  int one = 1;

  if (this->handle () == INVALID_SOCKET )
    return false;
  else if (reuse_addr 
           && !this->set_option (SOL_SOCKET,
                                SO_REUSEADDR,
                                &one,
                                sizeof one))
    {
      this->close ();
	  return false;
    }
  else
    return true;
}

OS_INLINE SOCKET BaseSocket::handle (void) const
{
  return this->handle_;
}


OS_INLINE void BaseSocket::set_handle (SOCKET handle)
{
  close();
  this->handle_ = handle;
}

OS_INLINE void BaseSocket::swap( BaseSocket& r )
{
	std::swap( this->handle_, r.handle_ );
}

OS_INLINE bool BaseSocket::set_option (int level, 
		      int option, 
		      void *optval, 
		      int optlen) const
{
  return ( SOCKET_ERROR != setsockopt (this->handle (), level, 
			     option, (char *) optval, optlen) );
}

OS_INLINE bool BaseSocket::get_option (int level, 
		      int option, 
		      void *optval, 
		      int *optlen) const
{
  return ( SOCKET_ERROR != getsockopt (this->handle (), level, 
			     option, (char *) optval, optlen) );
}

OS_INLINE bool BaseSocket::enable (int value)
{
	u_long nonblock = 1;
	return ( 0 ==::ioctlsocket (this->handle_,
		value,
		&nonblock));
}

OS_INLINE bool BaseSocket::disable (int value)
{
    u_long nonblock = 0;
    return ( 0 == ioctlsocket (this->handle_,
                              value,
                              &nonblock));
}

OS_INLINE void BaseSocket::close (void)
{
	if (INVALID_SOCKET == this->handle () )
		return;

	for( int i =0 ; i < 5; i ++ )
	{
		if( 0 == closesocket (this->handle ()) )
		{
			this->set_handle (INVALID_SOCKET );
			return ;
		}
	}
}

OS_INLINE bool BaseSocket::poll( const TIMEVAL& time_val, int mode)
{
	fd_set socket_set;
	FD_ZERO( &socket_set );
	FD_SET(handle(), &socket_set );

	return ( 1 == ::select( 0, (mode == select_read)?&socket_set:NULL
		, (mode == select_write)?&socket_set:NULL
		, (mode == select_error)?&socket_set:NULL
		, &time_val ) );
}

OS_INLINE const tstring& BaseSocket::toString() const
{
  if( INVALID_SOCKET == handle_)
  {
	  toString_ = _T("INVALID_SOCKET" );
  }
  else
  {
	  tstring::value_type tmp[1024];
 #pragma warning(disable: 4244)
	  string_traits< tstring::value_type >::itoa( handle_, tmp, 1024, 10 );
 #pragma warning(default: 4244)

	  toString_ = tmp;
  }
  return toString_;
}

OS_INLINE bool  BaseSocket::initializeScket()
{
	WSADATA wsaData;
	if ( 0 != WSAStartup( MAKEWORD( 2, 2 ), &wsaData ) )
		return false;
	
	if ( LOBYTE( wsaData.wVersion ) != 2 ||
		HIBYTE( wsaData.wVersion ) != 2 )
	{
			WSACleanup( );
			return false; 
	}

	SOCKET cliSock = ::socket( AF_INET , SOCK_STREAM, IPPROTO_TCP);

	GUID GuidConnectEx = WSAID_CONNECTEX;
	GUID GuidDisconnectEx = WSAID_DISCONNECTEX;
	GUID GuidGetAcceptExSockAddrs = WSAID_GETACCEPTEXSOCKADDRS;
	GUID GuidAcceptEx = WSAID_ACCEPTEX;
	GUID GuidTransmitFile = WSAID_TRANSMITFILE;
	GUID GuidTransmitPackets = WSAID_TRANSMITPACKETS;

	DWORD dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidConnectEx,
		sizeof(GuidConnectEx),
		&__connectex,
		sizeof(__connectex),
		&dwBytes,
		NULL,
		NULL))
	{
		__connectex = NULL;
	}

	
	dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidDisconnectEx,
		sizeof(GuidDisconnectEx),
		&__disconnectex,
		sizeof(__disconnectex),
		&dwBytes,
		NULL,
		NULL))
	{
		__disconnectex = NULL;
	}

	dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidTransmitFile,
		sizeof(GuidTransmitFile),
		&__transmitfile,
		sizeof(__transmitfile),
		&dwBytes,
		NULL,
		NULL))
	{
		__transmitfile = NULL;
	}
	
	dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidAcceptEx,
		sizeof(GuidAcceptEx),
		&__acceptex,
		sizeof(__acceptex),
		&dwBytes,
		NULL,
		NULL))
	{
		__acceptex = NULL;
	}

	dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidTransmitPackets,
		sizeof(GuidTransmitPackets),
		&__transmitpackets,
		sizeof(__transmitpackets),
		&dwBytes,
		NULL,
		NULL))
	{
		__transmitpackets = NULL;
	}

	dwBytes = 0;
	if( SOCKET_ERROR == WSAIoctl(cliSock,
		SIO_GET_EXTENSION_FUNCTION_POINTER,
		&GuidGetAcceptExSockAddrs,
		sizeof(GuidGetAcceptExSockAddrs),
		&__getacceptexsockaddrs,
		sizeof(__getacceptexsockaddrs),
		&dwBytes,
		NULL,
		NULL))
	{
		__getacceptexsockaddrs = NULL;
	}

	closesocket(  cliSock );

	return true;
}

OS_INLINE void BaseSocket::shutdownSocket()
{
	WSACleanup( );
}