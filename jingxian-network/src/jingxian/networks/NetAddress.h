
#ifndef _INET_ADDRESS_H_
#define _INET_ADDRESS_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/string/string.hpp"

_jingxian_begin

class NetAddress
{
public:

	NetAddress (void);

	NetAddress (const NetAddress & );
	NetAddress (const sockaddr_in *, int len);
	NetAddress (const void*, int len);
	NetAddress (u_long ip, u_int16_t port_number);
	NetAddress (const char* name, u_int16_t number);
	NetAddress (const char* name, const char* number);
	explicit NetAddress (const char* address);
	~NetAddress (void);

	NetAddress& operator=( const NetAddress& r);

	bool operator < (const NetAddress &rhs) const;
	bool operator > (const NetAddress &rhs) const;
	bool operator == (const NetAddress &SAP) const;
	bool operator != (const NetAddress &SAP) const;

	
	void swap( NetAddress& r);

	void port(u_int16_t number , bool encode = true );
	u_int16_t port() const;

	void ip( u_long addr, bool encode = false);
	void ip( const char* addr);
	u_long ip() const;
	const tstring& ip_string() const;

	size_t size (void) const;
	const sockaddr *addr (void) const ;

	void reset(void);
	void set(const void *, size_t len);
	bool set(const char* txt);

	const tstring& toString( ) const;
private:

	char* addr_;
	size_t len_;

	tstring to_string_;
	tstring ip_string_;
};

inline tostream& operator<<( tostream& target, const NetAddress& addr )
{
	target << addr.toString();
	return target;
}

//#if defined (OS_HAS_INLINED)
//#include "jingxian/networks/NetAddress.inl"
//#endif

_jingxian_end

#endif /* _INET_ADDRESS_H_ */
