
#ifndef _protocolcontext_h_
#define _protocolcontext_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.h"

_jingxian_begin

class IReactorCore;
class ITransport;

class ProtocolContext
{
public:
	ProtocolContext(IReactorCore* core
		, ITransport* transport)
		: core_(core)
		, transport_(transport)
		, outBuffer_(0)
		, inBuffer_(0)
	{
	}

	virtual ~ProtocolContext()
	{
	}

	IReactorCore& core()
	{
		if(is_null(core_))
			ThrowException( NullException );
		return *core_;
	}

	ITransport& transport()
	{
		if(is_null(transport_))
			ThrowException( NullException );
		return *transport_;
	}

	const std::vector<io_mem_buf>& inMemory() const
	{
		if(is_null(inMemory_))
			ThrowException( NullException );
		return *inMemory_;
	}

	IInBuffer& inBuffer()
	{
		if(is_null(inBuffer_))
			ThrowException( NullException );
		return *inBuffer_;
	}
	
	IOutBuffer& outBuffer()
	{
		if(is_null(outBuffer_))
			ThrowException( NullException );
		return *outBuffer_;
	}
protected:
	IReactorCore* core_;
	ITransport* transport_;
	IOutBuffer* outBuffer_;
	IInBuffer* inBuffer_;
	//InternalBuffer* internalBuffer_in_free_;
	//InternalBuffer* internalBuffer_in_data_;
	//InternalBuffer* internalBuffer_out_;
	std::vector<io_mem_buf>* inMemory_;
};

_jingxian_end

#endif //_protocolcontext_h_