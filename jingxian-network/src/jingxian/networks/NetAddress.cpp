
# include "pro_config.h"
# include <vector>
# include "NetAddress.h"

_jingxian_begin

const size_t NetAddressLength = sizeof(struct sockaddr_in)+sizeof(struct sockaddr);

NetAddress::NetAddress (void)
: addr_((char*)::malloc(NetAddressLength))
, len_(NetAddressLength)
{
	this->reset ();
}

NetAddress::NetAddress (const NetAddress &sa)
: addr_((char*)::malloc(NetAddressLength))
, len_(NetAddressLength)
{
	this->reset ();
	*this = sa;
}

NetAddress::NetAddress (const sockaddr_in *addr, int len)
: addr_((char*)::malloc(NetAddressLength))
, len_(NetAddressLength)
{
	this->reset ();
	this->set(addr, len);
}

NetAddress::NetAddress (const void* addr, int len)
: addr_((char*)::malloc(NetAddressLength))
, len_(NetAddressLength)
{
	this->reset ();
	this->set(addr, len);
}

NetAddress::NetAddress ( u_long ip,u_int16_t number )
 : addr_((char*)::malloc(NetAddressLength))
 , len_(NetAddressLength)
{
	this->reset ();
	this->ip( ip );
	this->port( number );
}

NetAddress::NetAddress ( const char* ipstr,u_int16_t number)
 : addr_((char*)::malloc(NetAddressLength))
 , len_(NetAddressLength)
{
	this->reset ();
	this->ip( ipstr );
	this->port( number );
}

NetAddress::NetAddress ( const char* ipstr, const char* number)
 : addr_((char*)::malloc(NetAddressLength))
 , len_(NetAddressLength)
{
	this->reset ();
	this->ip( ipstr );
	this->port( number );
}

NetAddress::NetAddress (const char* address)
 : addr_((char*)::malloc(NetAddressLength))
 , len_(NetAddressLength)
{
	this->reset ();
	this->set( address );
}

NetAddress::~NetAddress (void)
{
	if( null_ptr == addr_)
	{
		::free(addr_);
		addr_ = null_ptr;
	}
}

bool NetAddress::operator != (const NetAddress &sap) const
{
	return !((*this) == sap);
}

bool NetAddress::operator == (const NetAddress &sap) const
{
	return (::memcmp (&this->addr_,
		&sap.addr_,
		this->size ()) == 0);
}

bool NetAddress::operator < (const NetAddress &rhs) const
{
	return (::memcmp (&this->addr_,
		&rhs.addr_,
		this->size ()) < 0 );
}

bool NetAddress::operator > (const NetAddress &rhs) const
{
	return (::memcmp (&this->addr_,
		&rhs.addr_,
		this->size ()) > 0 );
}

NetAddress& NetAddress::operator=( const NetAddress& sa)
{
	if( this != &sa)
		set(sa.addr(), sa.size ());

	return *this;
}

void NetAddress::swap( NetAddress& r)
{
	std::swap(r.addr_, this->addr_);
	std::swap(r.len_, this->len_);
	std::swap(r.ip_string_,this->ip_string_);
	std::swap(r.to_string_,this->to_string_);
}

void NetAddress::reset (void)
{
	memset (&this->addr_ , 0, sizeof (this->addr_ ));
	ip_string_ = _T("0.0.0.0");
	to_string_ = _T("0.0.0.0:0");
	((sockaddr*) &addr_)->sa_family = AF_INET;
}

void NetAddress::port(u_int16_t number,
					  bool encode )
{
	to_string_ = ip_string_ + ::toString(encode?number:htons(number));
	((sockaddr_in*) &addr_)->sin_port = encode?htons (number):number;
}

void NetAddress::port( const char* number )
{
	this->port(atoi( number ), true);
}

u_int16_t NetAddress::port( void ) const 
{
	return htons( ((sockaddr_in*) &addr_)->sin_port );
}

void NetAddress::ip( u_long ip , bool encode )
{
	((sockaddr_in*)&addr_)->sin_addr.s_addr = encode? htonl ( ip ) : ip;
	ip_string_ = inet_ntoa(((sockaddr_in*) &addr_)->sin_addr);
	to_string_ = ip_string_ + ::toString(this->port());
}

void NetAddress::ip( const char* ipstr )
{
	this->ip(::inet_addr( ipstr ),false);
}

u_long NetAddress::ip ( void ) const
{
	return (((sockaddr_in*) &addr_)->sin_addr .s_addr);
}

const tstring& NetAddress::ip_string ( ) const
{
	return ip_string_;
}

size_t NetAddress::size (void) const
{
	return len_;
}


const sockaddr* NetAddress::addr (void) const
{
	return (sockaddr*)&(this->addr_);
}

void NetAddress::set ( const void * address, size_t len)
{
	if( len > size() )
		addr_ = (char*)::malloc(len);

	memcpy( addr_, address, len );
	len_ = len;

	ip_string_ = toTstring(inet_ntoa(((sockaddr_in*) &addr_)->sin_addr));
	to_string_ = ip_string_ + ::toString(this->port());
}

bool NetAddress::set (const char* address)
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
	return to_string_;
}

_jingxian_end