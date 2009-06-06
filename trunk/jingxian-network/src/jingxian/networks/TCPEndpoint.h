
#ifndef _TCPEndpoint_H_
#define _TCPEndpoint_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/networks/NetAddress.h"
# include "jingxian/IEndpoint.h"

_jingxian_begin

class TCPEndpoint : public IEndpoint
{
public:
	
	explicit TCPEndpoint(const tchar* endpoint);
	
	virtual ~TCPEndpoint();

	/**
	 * @implements protocol
	 */
	virtual const tstring& protocol() const;

	/**
	 * 取得地址的内存地址
	 */
	virtual const void* addr() const;

	/**
	 * @implements size
	 */
	virtual size_t size (void) const;

	/**
	 * @implements compareTo
	 */
	virtual int compareTo(const IEndpoint& endpoint) const;

	/**
	* @implements toString
	*/
	virtual const tstring& toString() const;
	
private:
	NOCOPY(TCPEndpoint);

	const static tstring _protocol;

	NetAddress _address;
	
	tstring toString_;
};

_jingxian_end

#endif //_TCPEndpoint_H_ 