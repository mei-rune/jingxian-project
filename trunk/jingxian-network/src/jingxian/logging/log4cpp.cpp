# include "pro_config.h"
# include "log4cpp.h"


_jingxian_begin


namespace log4cppAdaptor
{

Logger::Logger(const tchar* nm)
: logger_(::log4cpp::Category::getInstance(toNarrowString(nm)))
{
}

Logger::~Logger(void)
{
}

void Logger::assertLog(bool assertion, const LogStream& msg, const char* file, int line)
{

}

bool Logger::isFatalEnabled() const
{
	return logger_.isFatalEnabled();
}

void Logger::fatal(const LogStream& message, const char* file, int line)
{
	logger_.fatal(toNarrowString(message.str()));
}

bool Logger::isErrorEnabled() const
{
	return logger_.isErrorEnabled();
}

void Logger::error(const LogStream& message, const char* file, int line)
{
	logger_.error(toNarrowString(message.str()));
}

bool Logger::isInfoEnabled() const
{
	return logger_.isCritEnabled();
}

void Logger::info(const LogStream& message, const char* file, int line)
{
	logger_.crit(toNarrowString(message.str()));
}

bool Logger::isDebugEnabled() const
{
	return logger_.isDebugEnabled();
}

void Logger::debug(const LogStream& message, const char* file, int line)
{
	logger_.debug(toNarrowString(message.str()));
}

bool Logger::isWarnEnabled() const
{
	return logger_.isWarnEnabled();
}

void Logger::warn(const LogStream& message, const char* file, int line)
{
	logger_.warn(toNarrowString(message.str()));
}

bool Logger::isTraceEnabled() const
{
	return logger_.isPriorityEnabled(750);
}

void Logger::trace(const LogStream& message, const char* file, int line)
{
	logger_.log(750, toNarrowString(message.str()));
}

bool Logger::isEnabledFor(const logging::LevelPtr& level) const
{
	return logger_.isPriorityEnabled( level );
}

void Logger::log(const logging::LevelPtr& level, const LogStream& message,
	const char* file, int line)
{
	logger_.log(level, toNarrowString(message.str()));
}

logging::LevelPtr Logger::getLevel() const
{
	return logger_.getPriority();
}

void Logger::pushNDC( const tchar* str )
{
}

void Logger::popNDC( )
{
}

void Logger::clearNDC()
{
}


const tchar* TRANSPORT_MODE[] = { _T(""), _T("Receive"),_T("Send"),_T("Both") };

Tracer::Tracer(const tchar* nm, const tstring& host, const tstring& peer)
: logger_(::log4cpp::Category::getInstance(toNarrowString(nm)))
, name_(null_ptr)
{
	size_t len = host.size() + peer.size() + 20;
	name_ = (char*)my_malloc( len );
	memset(name_, 0, len);
	name_[0] = '[';
	memcpy(name_ + 1, host.c_str(), host.size());
	memcpy(name_ + 1 +  host.size(), _T(" - "), 3);
	memcpy(name_ + 4 +  host.size(), peer.c_str(), peer.size());
	memcpy(name_ + 4 +  host.size() + peer.size(), _T("]"), 1);
}

Tracer::~Tracer(void)
{
	::my_free(name_);
	name_ = null_ptr;
}

bool Tracer::isDebugEnabled() const
{
	return logger_.isDebugEnabled();
}

void Tracer::debug(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.debug( "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isErrorEnabled() const
{
	return logger_.isErrorEnabled();
}

void Tracer::error(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.error( "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isFatalEnabled() const
{
	return logger_.isFatalEnabled();
}

void Tracer::fatal(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.fatal( "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isInfoEnabled() const
{
	return logger_.isCritEnabled();
}

void Tracer::info(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.crit( "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isWarnEnabled() const
{
	return logger_.isWarnEnabled();
}

void Tracer::warn(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.warn( "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isTraceEnabled() const
{
	return logger_.isPriorityEnabled(750);
}

void Tracer::trace(transport_mode::type way, const LogStream& message, const char* file, int line)
{
	tstring str = message.str();
	logger_.log( 750, "%s %s %s", name_, TRANSPORT_MODE[way], str.c_str(), file, line);
}

}

_jingxian_end