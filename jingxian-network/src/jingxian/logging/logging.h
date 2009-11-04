
#ifndef _LOGGING_H_
#define _LOGGING_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/logging/ILogger.h"
# include "jingxian/logging/ITracer.h"


_jingxian_begin

namespace logging
{
ILogger* makeLogger( const tstring& nm );

spi::ILogFactory* setLogFactory(spi::ILogFactory* factory);

ITracer* makeTracer( const tstring& nm, const tstring& host, const tstring& peer, const tstring& sessionId);

spi::ITraceFactory* setTraceFactory(spi::ITraceFactory* factory);
}

_jingxian_end

#endif // _LOGGING_H_