# include "pro_config.h"
# include "logging.h"
# include "ConsoleLogger.h"
# include "Tracer.h"
# include "log4cpp.h"

_jingxian_begin

namespace logging
{

spi::ITraceFactory* tracefactory_ = null_ptr;
spi::ILogFactory* logFactory_ = null_ptr;

ITracer* makeTracer( const tchar* nm, const tstring& host, const tstring& peer, const tstring& sessionId)
{
    if (null_ptr == tracefactory_)
        return new log4cppAdaptor::Tracer(nm, host, peer, sessionId);

    return tracefactory_->make(nm, host, peer, sessionId);
}

ITracer* makeTracer( const tstring& nm, const tstring& host, const tstring& peer, const tstring& sessionId)
{
    return makeTracer(nm.c_str(), host, peer, sessionId);
}

spi::ITraceFactory* setTraceFactory(spi::ITraceFactory* factory)
{
    spi::ITraceFactory*  old = tracefactory_;
    tracefactory_ = factory;
    return old;
}

ILogger* makeLogger( const tchar* nm )
{
    if (null_ptr == tracefactory_)
        return new log4cppAdaptor::Logger(nm);

    return logFactory_->make(nm);
}

ILogger* makeLogger( const tstring& nm )
{
    return makeLogger(nm.c_str());
}

spi::ILogFactory* setLogFactory(spi::ILogFactory* factory)
{
    spi::ILogFactory*  old = logFactory_;
    logFactory_ = factory;
    return old;
}
}

_jingxian_end
