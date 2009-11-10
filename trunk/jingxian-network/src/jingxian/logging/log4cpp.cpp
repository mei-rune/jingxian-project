# include "pro_config.h"
# include "log4cpp.h"
# include "jingxian/directory.h"

_jingxian_begin

namespace logging
{

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

bool Logger::isCritEnabled() const
{
    return logger_.isCritEnabled();
}

void Logger::crit(const LogStream& message, const char* file, int line)
{
    logger_.crit(toNarrowString(message.str()));
}

bool Logger::isFatalEnabled() const
{
    return logger_.isErrorEnabled();
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
    return logger_.isDebugEnabled();
}

void Logger::trace(const LogStream& message, const char* file, int line)
{
    logger_.debug(toNarrowString(message.str()));
}

bool Logger::isEnabledFor(const logging::LevelPtr& level) const
{
    return logger_.isPriorityEnabled(level);
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

void Logger::pushNDC(const tchar* str)
{
}

void Logger::popNDC()
{
}

void Logger::clearNDC()
{
}


const char* TRANSPORT_MODE[] = { "", "Receive", "Send", "Both" };

Tracer::Tracer(const tchar* nm, const tstring& thost, const tstring& tpeer, const tstring& sessionId)
        : logger_(toNarrowString(nm), "")
        , name_(null_ptr)
{
    std::string host = toNarrowString(thost);
    std::string peer = toNarrowString(tpeer);

    size_t len = host.size() + peer.size() + 20;
    name_ = (char*)my_malloc(len);
    memset(name_, 0, len);
    name_[0] = '[';
    memcpy(name_ + 1, host.c_str(), host.size());
    memcpy(name_ + 1 +  host.size(), " - ", 3);
    memcpy(name_ + 4 +  host.size(), peer.c_str(), peer.size());
    memcpy(name_ + 4 +  host.size() + peer.size(), "]", 1);

    logger_.setContext(name_);

    host = trim_all(host, "tcp://");
    host = trim_all(host, "tcp6://");
    host = trim_all(host, "tcpv6://");

    peer = trim_all(peer, "tcp://");
    peer = trim_all(peer, "tcp6://");
    peer = trim_all(peer, "tcpv6://");

    host = replace_all(host, ":", "[");
    peer = replace_all(peer, ":", "[");

    std::string name = host + "]_" + peer + "]_" + toNarrowString(sessionId);
    tstring dir = simplify(combinePath(getApplicationDirectory(), _T("log")));
    if (!existDirectory(dir))
        createDirectory(dir);

    dir = combinePath(dir, _T("connection"));
    if (!existDirectory(dir))
        createDirectory(dir);

    appender = toNarrowString(combinePath(dir, toTstring(name) + _T(".txt")));
    logger_.addAppender(new log4cpp::FileAppender(name, appender));
}

Tracer::~Tracer(void)
{
    ::my_free(name_);
    name_ = null_ptr;

    logger_.removeAllAppenders();
    DeleteFileA(appender.c_str());
}

bool Tracer::isDebugEnabled() const
{
    return logger_.isDebugEnabled();
}

void Tracer::debug(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.debug("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isErrorEnabled() const
{
    return logger_.isErrorEnabled();
}

void Tracer::error(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.error("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isFatalEnabled() const
{
    return logger_.isFatalEnabled();
}

void Tracer::fatal(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.fatal("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isInfoEnabled() const
{
    return logger_.isCritEnabled();
}

void Tracer::info(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.crit("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isWarnEnabled() const
{
    return logger_.isWarnEnabled();
}

void Tracer::warn(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.warn("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isTraceEnabled() const
{
    return logger_.isDebugEnabled();
}

void Tracer::trace(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.debug("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}

bool Tracer::isCritEnabled() const
{
    return logger_.isCritEnabled();
}

void Tracer::crit(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    std::string str = toNarrowString(message.str());
    logger_.crit("%s %s", TRANSPORT_MODE[way], str.c_str(), file, line);
}



ContextCategory::ContextCategory(const std::string& name
                                 , const std::string& context)
        : log4cpp::Category(name, &log4cpp::Category::getInstance(name), log4cpp::Priority::DEBUG)
        , context_(context)
{
    this->setPriority(this->getParent()->getPriority());
}

void ContextCategory::setContext(const std::string& context)
{
    context_ = context;
}

const std::string& ContextCategory::getContext() const
{
    return context_;
}

void ContextCategory::_logUnconditionally2(log4cpp::Priority::Value priority,
        const std::string& message) throw()
{
    log4cpp::LoggingEvent evt(getName(), message, context_, priority);
    callAppenders(evt);
}

}

}

_jingxian_end