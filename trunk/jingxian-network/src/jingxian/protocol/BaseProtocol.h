
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
		, logger_(null_ptr)
	{
	}

	BaseProtocol(const BaseProtocol& protocol)
	{
		toString_ = protocol.toString_;
		logger_ = null_ptr;
	}

	virtual ~BaseProtocol()
	{
		delete logger_;
		logger_ = null_ptr;
	}
	
	BaseProtocol& operator=(const BaseProtocol& protocol)
	{
		toString_ = protocol.toString_;
		logger_ = null_ptr;
	}

	virtual buffer_chain_t* createBuffer(const ProtocolContext& context, const Buffer<buffer_chain_t>& lastBuffer, const buffer_chain_t* current)
	{
		databuffer_t* result = (databuffer_t*)calloc(1,sizeof(databuffer_t)+100);
		result->capacity = 100;
		result->start = result->end = result->ptr;
		return (buffer_chain_t*)result;
	}

	ILogger* log()
	{
		if(is_null(logger_))
			logger_ = logging::makeLogger(toString_);

		return logger_;
	}

	/**
	 * 取得地址的描述
	 */
	virtual const tstring& toString() const
	{
		return toString_;
	}
protected:
	ILogger* logger_;
	tstring toString_;
};

_jingxian_end

#endif //_BaseProtocol_H_