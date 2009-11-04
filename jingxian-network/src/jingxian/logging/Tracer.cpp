# include "pro_config.h"
# include "Tracer.h"


_jingxian_begin

const tchar* TRANSPORT_MODE[] = { _T(""), _T("Receive"),_T("Send"),_T("Both") };

Tracer::Tracer(ILogger* logger, const tchar* nm)
        : logger_(logger)
        , name_(nm)
{
}

Tracer::~Tracer(void)
{
}

bool Tracer::isDebugEnabled() const
{
    return logger_->isDebugEnabled();
}

void Tracer::debug(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->debug(stream, file, line);
}

bool Tracer::isErrorEnabled() const
{
    return logger_->isErrorEnabled();
}

void Tracer::error(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->error(stream, file, line);
}

bool Tracer::isFatalEnabled() const
{
    return logger_->isFatalEnabled();
}

void Tracer::fatal(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->fatal(stream, file, line);
}

bool Tracer::isInfoEnabled() const
{
    return logger_->isInfoEnabled();
}

void Tracer::info(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->info(stream, file, line);
}

bool Tracer::isWarnEnabled() const
{
    return logger_->isWarnEnabled();
}

void Tracer::warn(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->warn(stream, file, line);
}

bool Tracer::isTraceEnabled() const
{
    return logger_->isTraceEnabled();
}

void Tracer::trace(transport_mode::type way, const LogStream& message, const char* file, int line)
{
    LogStream stream;
    stream << name_;
    stream << " ";
    stream << TRANSPORT_MODE[way];
    stream << message;
    logger_->trace(stream, file, line);
}

_jingxian_end
