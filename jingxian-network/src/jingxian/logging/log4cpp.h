
#ifndef _log4cpp_Logger_H_
#define _log4cpp_Logger_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <iostream>
# include "jingxian/string/string.h"
# include "jingxian/logging/ILogger.h"
# include "jingxian/logging/ITracer.h"

# include "log4cpp/Export.hh"
# include "log4cpp/Portability.hh"
# include "log4cpp/Category.hh"
# include "log4cpp/Appender.hh"
# include "log4cpp/NTEventLogAppender.hh"
# include "log4cpp/Priority.hh"

_jingxian_begin

namespace log4cppAdaptor
{
	class Logger : public ILogger
	{
	public:
		Logger(const tchar* nm);

		virtual ~Logger(void);

		virtual void assertLog(bool assertion, const LogStream& msg, const char* file=0, int line=-1) ;

		virtual bool isFatalEnabled() const ;

		virtual void fatal(const LogStream& message, const char* file=0, int line=-1) ;

		virtual bool isErrorEnabled() const ;

		virtual void error(const LogStream& message, const char* file=0, int line=-1) ;

		virtual bool isInfoEnabled() const ;

		virtual void info(const LogStream& message, const char* file=NULL, int line=-1) ;

		virtual bool isDebugEnabled() const ;

		virtual void debug(const LogStream& message, const char* file=0, int line=-1) ;

		virtual bool isWarnEnabled() const ;

		virtual void warn(const LogStream& message, const char* file=NULL, int line=-1);

		virtual bool isTraceEnabled() const;

		virtual void trace(const LogStream& message, const char* file=NULL, int line=-1);

		virtual bool isEnabledFor(const logging::LevelPtr& level) const;

		virtual void log(const logging::LevelPtr& level, const LogStream& message,
			const char* file=0, int line=-1);

		virtual logging::LevelPtr getLevel() const;

		virtual void pushNDC( const tchar* str );

		virtual void popNDC( );

		virtual void clearNDC();

	private:
		log4cpp::Category& logger_;
	};

	class Tracer : public ITracer
	{
	public:
		Tracer( const tchar* nm, const tstring& host, const tstring& peer);

		virtual ~Tracer(void);

		virtual bool isDebugEnabled() const;

		virtual void debug(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

		virtual bool isErrorEnabled() const;

		virtual void error(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

		virtual bool isFatalEnabled() const;

		virtual void fatal(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

		virtual bool isInfoEnabled() const;

		virtual void info(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

		virtual bool isWarnEnabled() const ;

		virtual void warn(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

		virtual bool isTraceEnabled() const;

		virtual void trace(transport_mode::type way, const LogStream& message, const char* file=0, int line=-1);

	private:
		log4cpp::Category& logger_;
		char* name_;
	};
}

_jingxian_end

#endif // _log4cpp_Logger_H_