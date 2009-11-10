# include "pro_config.h"
# include "DefaultTracer.h"


_jingxian_begin

namespace logging
{

const tchar* TRANSPORT_MODE[] = { _T(""), _T("Receive"), _T("Send"), _T("Both") };

DefaultTracer::DefaultTracer(spi::ILogger* logger, const tchar* nm)
        : logger_(logger)
        , name_(nm)
{
}

DefaultTracer::~DefaultTracer(void)
{
}

bool DefaultTracer::isDebugEnabled() const
{
    return logger_->isDebugEnabled();
}

void DefaultTracer::debug(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->debug(stream, file, line);
}

bool DefaultTracer::isErrorEnabled() const
{
    return logger_->isErrorEnabled();
}

void DefaultTracer::error(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->error(stream, file, line);
}

bool DefaultTracer::isFatalEnabled() const
{
    return logger_->isFatalEnabled();
}

void DefaultTracer::fatal(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->fatal(stream, file, line);
}

bool DefaultTracer::isInfoEnabled() const
{
    return logger_->isInfoEnabled();
}

void DefaultTracer::info(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->info(stream, file, line);
}

bool DefaultTracer::isWarnEnabled() const
{
    return logger_->isWarnEnabled();
}

void DefaultTracer::warn(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->warn(stream, file, line);
}

bool DefaultTracer::isTraceEnabled() const
{
    return logger_->isTraceEnabled();
}

void DefaultTracer::trace(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->trace(stream, file, line);
}

}

_jingxian_end
