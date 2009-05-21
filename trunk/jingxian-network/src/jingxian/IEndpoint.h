
#ifndef _IEndpoint_H
#define _IEndpoint_H

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"

_jingxian_begin


class IEndpoint
{
public:

	virtual ~IEndpoint() {}

	/**
	 * 地址的协议
	 */
	virtual const tstring& protocol() const = 0;

	/**
	 * 取得地址的内存地址
	 */
	virtual const void* addr() const = 0;

	/**
	 * 取得地址的内存块大小
	 */
	virtual size_t size (void) const = 0;

	/**
	 * 比较大小
	 *
	 * @return 等于返回0，小于返回负数，在于返回正数
	 */
	virtual int compareTo(const IEndpoint& endpoint) const = 0;

	/**
	 * 取得地址的描述
	 */
    virtual const tstring& toString() const = 0;
};

inline tostream& operator<<( tostream& target, const IEndpoint& addr )
{
	target << addr.toString();
	return target;
}

inline bool operator < (const IEndpoint &a,const IEndpoint &b)
{
	return 0 > a.compareTo( b );
}

inline bool operator > (const IEndpoint &a,const IEndpoint &b)
{
	return 0 < a.compareTo( b );
}

inline bool operator == (const IEndpoint &a,const IEndpoint &b)
{
	return 0 == a.compareTo( b );
}

inline bool operator != (const IEndpoint &a,const IEndpoint &b)
{
	return 0 != a.compareTo( b );
}

_jingxian_end

#endif //_IEndpoint_H
