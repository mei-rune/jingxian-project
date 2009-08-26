
#ifndef _BaseProtocol_H_
#define _BaseProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/IProtocol.h"


_jingxian_begin


class BaseProtocol : public IProtocol
{
public:
	BaseProtocol()
		: toString_(_T("BaseProtocol"))
	{
	}


	virtual buffer_chain_t* createBuffer(const ProtocolContext& context, const Buffer<buffer_chain_t>& lastBuffer, const buffer_chain_t* current)
	{
		databuffer_t* result = (databuffer_t*)calloc(1,sizeof(databuffer_t)+100);
		result->capacity = 100;
		result->start = result->end = result->ptr;
		return (buffer_chain_t*)result;
	}

	/**
	 * 取得地址的描述
	 */
	virtual const tstring& toString() const
	{
		return toString_;
	}
protected:
	tstring toString_;
};

_jingxian_end

#endif //_BaseProtocol_H_