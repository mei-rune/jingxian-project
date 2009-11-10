# include "pro_config.h"
# include "logging.h"
# include "ConsoleLogger.h"
# include "DefaultTracer.h"
# include "log4cpp.h"

_jingxian_begin

namespace logging
{

namespace spi
{
spi::ITraceFactory* tracefactory_ = null_ptr;
spi::ILogFactory* logFactory_ = null_ptr;

ITracer* makeTracer(const tchar* nm, const tstring& host, const tstring& peer, const tstring& sessionId)
{
  if (null_ptr == tracefactory_)
    return new log4cppAdaptor::Tracer(nm, host, peer, sessionId);

  return tracefactory_->make(nm, host, peer, sessionId);
}

ITracer* makeTracer(const tstring& nm, const tstring& host, const tstring& peer, const tstring& sessionId)
{
  return makeTracer(nm.c_str(), host, peer, sessionId);
}

spi::ITraceFactory* setTraceFactory(spi::ITraceFactory* factory)
{
  spi::ITraceFactory*  old = tracefactory_;
  tracefactory_ = factory;
  return old;
}

ILogger* makeLogger(const tchar* nm)
{
  if (null_ptr == tracefactory_)
    return new log4cppAdaptor::Logger(nm);

  return logFactory_->make(nm);
}

ILogger* makeLogger(const tstring& nm)
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

//logger::logger()
//: logger_(null_ptr)
//{
//}
//
//logger::logger(const tchar* nm)
//: impl_(null_ptr)
//{
//	if(null_ptr == nm)
//		return;
//	
//	impl_ = spi::makeLogger(nm);
//}

logger::logger(const tstring& nm)
: impl_(spi::makeLogger(nm))
{
}

logger::~logger()
{
	if (null_ptr != impl_)
	{
		delete impl_;
		impl_ = null_ptr;
	}
}

void logger::assertLog(bool assertion, const LogStream& msg, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  impl_->assertLog(assertion, msg, file, line);
}

bool logger::isCritEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isCritEnabled();
}

void logger::crit(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->crit(message, file, line);
}

bool logger::isFatalEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isFatalEnabled();
}

void logger::fatal(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->fatal(message, file, line);
}

bool logger::isErrorEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isErrorEnabled();
}

void logger::error(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->error(message, file, line);
}

bool logger::isInfoEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isInfoEnabled();
}

void logger::info(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->info(message, file, line);
}

bool logger::isDebugEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isDebugEnabled();
}

void logger::debug(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->debug(message, file, line);
}

bool logger::isWarnEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isWarnEnabled();
}

void logger::warn(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->warn(message, file, line);
}

bool logger::isTraceEnabled() const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isTraceEnabled();
}

void logger::trace(const LogStream& message, const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->trace(message, file, line);
}

bool logger::isEnabledFor(const LevelPtr& level) const
{
  if (null_ptr == impl_)
    return false;

  return impl_->isEnabledFor(level);
}

void logger::log(const LevelPtr& level, const LogStream& message,
                 const char* file, int line)
{
  if (null_ptr == impl_)
    return ;

  return impl_->log(level, message, file, line);
}

LevelPtr logger::getLevel() const
{
  if (null_ptr == impl_)
    return 0;

  return impl_->getLevel();
}

}

_jingxian_end
