
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
		return *core_;
	}

	ITransport& transport()
	{
		return *transport_;
	}

	IInBuffer& inBuffer()
	{
		return *inBuffer_;
	}
	
	IOutBuffer& outBuffer()
	{
		return *outBuffer_;
	}
protected:
	IReactorCore* core_;
	ITransport* transport_;
	IOutBuffer* outBuffer_;
	IInBuffer* inBuffer_;
};

_jingxian_end

#endif //_protocolcontext_h_