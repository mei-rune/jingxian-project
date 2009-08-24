
#ifndef _TCPContext_H_
#define _TCPContext_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/networks/networking.h"
# include "jingxian/buffer/InBuffer.h"
# include "jingxian/buffer/OutBuffer.h"


_jingxian_begin

class TCPContext : public ProtocolContext
{
public:
	TCPContext(IReactorCore* core
		, ITransport* transport)
		: ProtocolContext(core, transport)
	{
		inBuffer(&_inBuffer);
		outBuffer(&_outBuffer);
	}

	void inBuffer(IInBuffer* inBuffer)
	{
		inBuffer_ = inBuffer;
	}
	
	void outBuffer(IOutBuffer* outBuffer)
	{
		outBuffer_ = outBuffer_;
	}

	void inBuffer(io_mem_buf* inBuffer)
	{
	}

private:
	InBuffer _inBuffer;
	OutBuffer _outBuffer;
};

_jingxian_end

#endif //_TCPContext_H_ 