
#ifndef _ConsoleLogger_H_
#define _ConsoleLogger_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <iostream>
# include "jingxian/string/string.h"
# include "jingxian/logging/ILogger.h"

_jingxian_begin

#ifdef  _UNICODE
#define tcout std::wcout
#else
#define tcout std::cout
#endif

namespace logging
{

	class ConsoleLogger : public spi::ILogger
{
public:

    ConsoleLogger()
    {
        _levelPtr = logging::Trace;
    }

    virtual ~ConsoleLogger()
    {
    }

    virtual void assertLog(bool assertion, const LogStream& message, const char* file = 0, int line = -1)
    {
        tcout << message.str() << std::endl;
        assert(assertion);
    }

    virtual bool isFatalEnabled() const
    {
        return true;
    }

    virtual void fatal(const LogStream& message, const char* file = 0, int line = -1)
    {
        tcout << message.str() << std::endl;
    }


    virtual bool isErrorEnabled() const
    {
        return true;
    }

    virtual void error(const LogStream& message, const char* file = 0, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual bool isInfoEnabled() const
    {
        return true;
    }

    virtual void info(const LogStream& message, const char* file = NULL, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual bool isDebugEnabled() const
    {
        return true;
    }

    virtual void debug(const LogStream& message, const char* file = 0, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual bool isWarnEnabled() const
    {
        return true;
    }

    virtual void warn(const LogStream& message, const char* file = NULL, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual bool isTraceEnabled() const
    {
        return true;
    }

    virtual void trace(const LogStream& message, const char* file = NULL, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual bool isEnabledFor(const logging::LevelPtr& level) const
    {
        return true;
    }

    virtual void log(const logging::LevelPtr& level, const LogStream& message,
                     const char* file = 0, int line = -1)
    {
        tcout << message.str() << std::endl;
    }

    virtual logging::LevelPtr getLevel() const
    {
        return _levelPtr;
    }

    virtual void pushNDC(const tchar* str)
    {
    }

    virtual void popNDC()
    {
    }

    virtual void clearNDC()
    {
    }

private:
    logging::LevelPtr _levelPtr;
};

}

_jingxian_end

#endif // _ITracer_H_
