
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
	 * ��ַ��Э��
	 */
	virtual const tstring& protocol() const = 0;

	/**
	 * ȡ�õ�ַ���ڴ��ַ
	 */
	virtual const void* addr() const = 0;

	/**
	 * ȡ�õ�ַ���ڴ���С
	 */
	virtual size_t size (void) const = 0;

	/**
	 * �Ƚϴ�С
	 *
	 * @return ���ڷ���0��С�ڷ��ظ��������ڷ�������
	 */
	virtual int compareTo(const IEndpoint& endpoint) const = 0;

	/**
	 * ȡ�õ�ַ������
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
