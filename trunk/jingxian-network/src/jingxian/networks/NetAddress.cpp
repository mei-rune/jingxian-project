
# include "pro_config.h"
# include <vector>
# include "NetAddress.h"

_jingxian_begin

 NetAddress::NetAddress (void)
{
	this->reset ();
}

 NetAddress::NetAddress (const NetAddress &sa)
{
	this->reset ();
	*this = sa;
}

 NetAddress::NetAddress (const sockaddr_in *addr, int len)
{
	this->reset ();
	this->addr(addr, len);
}

 NetAddress::NetAddress (const void* addr, int len)
{
	this->reset ();
	this->addr(addr, len);
}

 NetAddress::NetAddress ( u_long ip,u_int16_t number )
{
	this->reset ();
	this->ip( ip );
	this->port( number );
}

 NetAddress::NetAddress ( const char* ipstr,u_int16_t number)
{
	this->reset ();
	this->ip( ipstr );
	this->port( number );
}

 NetAddress::NetAddress ( const char* ipstr, const char* number)
{
	this->reset ();
	this->ip( ipstr );
	this->port( number );
}

 NetAddress::NetAddress (const char* address)
{
	this->reset ();
	this->parse( address );
}

 NetAddress::~NetAddress (void)
{
}

 bool NetAddress::operator != (const NetAddress &sap) const
{
	return !((*this) == sap);
}

bool NetAddress::operator == (const NetAddress &sap) const
{
	return (::memcmp (&this->m_addr_,
		&sap.m_addr_,
		this->size ()) == 0);
}

bool NetAddress::operator < (const NetAddress &rhs) const
{
  return (::memcmp (&this->m_addr_,
		&rhs.m_addr_,
		this->size ()) < 0 );
}

bool NetAddress::operator > (const NetAddress &rhs) const
{
  return (::memcmp (&this->m_addr_,
		&rhs.m_addr_,
		this->size ()) > 0 );
}

NetAddress& NetAddress::operator=( const NetAddress& sa)
{
	if( this != &sa)
		::memcpy( &this->m_addr_, &sa.m_addr_, sa.size () );

	return *this;
}

void NetAddress::swap( NetAddress& r)
{
	sockaddr address;
	memcpy( &address, &this->m_addr_, sizeof( sockaddr ) );
	memcpy( &this->m_addr_, &r.m_addr_, sizeof( sockaddr ) );
	memcpy( &r.m_addr_, &address, sizeof( sockaddr ) );
}

void NetAddress::reset (void)
{
  memset (&this->m_addr_ , 0, sizeof (this->m_addr_ ));
  this->m_addr_.sa_family = AF_INET;
}

void NetAddress::port(u_int16_t number,
							   bool encode )
{
	((sockaddr_in*) &m_addr_)->sin_port = encode?htons (number):number;
}

void NetAddress::port( const char* number )
{
	((sockaddr_in*) &m_addr_)->sin_port = htons( ::atoi( number ) );
}

u_int16_t NetAddress::port( void ) const 
{
	return htons( ((sockaddr_in*) &m_addr_)->sin_port );
}

void NetAddress::ip( u_long ip , bool encode )
{
	((sockaddr_in*) &m_addr_)->sin_addr.s_addr = encode? htonl ( ip ) : ip;
}

void NetAddress::ip( const char* ipstr )
{
	((sockaddr_in*) &m_addr_)->sin_addr.s_addr = ::inet_addr( ipstr );
}

u_long NetAddress::ip ( void ) const
{
	return (((sockaddr_in*) &m_addr_)->sin_addr .s_addr);
}

const tstring& NetAddress::ip_string ( ) const
{
	ip_string_ = toTstring( ::inet_ntoa(((sockaddr_in*) &m_addr_)->sin_addr ) );
	return ip_string_;
}

size_t NetAddress::size (void) const
{
	return sizeof( this->m_addr_ );
}

void NetAddress::size (size_t size)
{
}

const sockaddr* NetAddress::addr (void) const
{
	return &(this->m_addr_);
}

sockaddr* NetAddress::addr (void)
{
	return &(this->m_addr_);
}

void NetAddress::addr ( const void * address, size_t len)
{
	if( len > size() )
		memcpy( addr(), address, size() );
	else
		memcpy( addr(), address, len );
}

bool NetAddress::parse (const char* address)
{
	if( string_traits<char>::strnicmp( address, "TCP://", 6 ) == 0 )
		address += 6;
	else if( string_traits<char>::strstr( address, "://" ) != 0 )
		return false;

	std::vector< char > ip_addr( strlen( address ) + 10 , 0 );
	string_traits<char>::strcpy( &ip_addr[ 0 ], ip_addr.size(), address );
	char *port_p = string_traits<char>::strrchr (&ip_addr[0], ':');

	if (port_p == 0) 
	{
		u_long ip = inet_addr( &ip_addr[0] );
		if( ip == INADDR_NONE )
			return false;
		this->ip( ip );
		return true;
	}
	else
	{
		*port_p = '\0'; ++port_p;
		u_long ip = inet_addr( &ip_addr[0] );
		if( ip == INADDR_NONE )
			return false;
		this->ip( ip );
		this->port( port_p );
		return true;
	}
}

const tstring& NetAddress::toString( ) const
{
	char* ipstr = inet_ntoa( ((sockaddr_in*) &m_addr_)->sin_addr );
	if( is_null( ipstr ) )
	{
		to_string_.clear();
		return to_string_;
	}

	to_string_ = toTstring(ipstr);

	tchar tmp[ 100 ] = _T("");
	string_traits<tstring::value_type>::itoa( port(), tmp, 100, 10);

	to_string_ +=_T( ':');
	to_string_ += tmp;
	return to_string_;
}

_jingxian_end