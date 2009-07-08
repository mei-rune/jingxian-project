
#ifndef _protocolcontext_h_
#define _protocolcontext_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "Buffer.h"

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

	InBuffer& inBuffer()
	{
		return *inBuffer_;
	}
	
	OutBuffer& outBuffer()
	{
		return *outBuffer_;
	}
private:
	IReactorCore* core_;
	ITransport* transport_;
	OutBuffer* outBuffer_;
	InBuffer* inBuffer_;
};

_jingxian_end

#endif //_protocolcontext_h_