
#ifndef _Tracer_H_
#define _Tracer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"
# include "jingxian/logging/ITracer.h"
# include "jingxian/logging/ILogger.h"

_jingxian_begin

namespace logging
{

class DefaultTracer : public ITracer
{
public:
	DefaultTracer(spi::ILogger* logger, const tchar* nm);

    virtual ~DefaultTracer(void);

    virtual bool isDebugEnabled() const;

    virtual void debug(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

    virtual bool isErrorEnabled() const;

    virtual void error(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

    virtual bool isFatalEnabled() const;

    virtual void fatal(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

    virtual bool isInfoEnabled() const;

    virtual void info(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

    virtual bool isWarnEnabled() const ;

    virtual void warn(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

    virtual bool isTraceEnabled() const;

    virtual void trace(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1);

private:

	NOCOPY(DefaultTracer);

    spi::ILogger* logger_;
    tstring name_;
};

}

_jingxian_end

#endif // _Tracer_H_
