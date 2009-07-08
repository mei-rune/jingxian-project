# include "pro_config.h"
#include <iostream>
#include "jingxian/logging/ILogger.h"
#include "jingxian/logging/ITracer.h"

_jingxian_begin


#ifdef  _UNICODE
#define tcout std::wcout
#else
#define tcout std::cout
#endif


class ConsoleLogger : public ILogger
{
public:

	ConsoleLogger()
	{
		_levelPtr = logging::Trace;
	}

	virtual void assertLog(bool assertion, const StringStream& message, const char* file=0, int line=-1)
	{
		tcout << message.str() << std::endl;
		assert( assertion );
	}

	virtual bool isFatalEnabled() const
	{
		return true;
	}

	virtual void fatal(const StringStream& message, const char* file=0, int line=-1)
	{
		tcout << message.str() << std::endl;
	}


	virtual bool isErrorEnabled() const
	{
		return true;
	}

	virtual void error(const StringStream& message, const char* file=0, int line=-1)
	{
		tcout << message.str() << std::endl;
	}


	virtual bool isInfoEnabled() const
	{
		return true;
	}

	virtual void info(const StringStream& message, const char* file=NULL, int line=-1)
	{
		tcout << message.str() << std::endl;
	}

	virtual bool isDebugEnabled() const
	{
		return true;
	}

	virtual void debug(const StringStream& message, const char* file=0, int line=-1)
	{
		tcout << message.str() << std::endl;
	}

	virtual bool isWarnEnabled() const
	{
		return true;
	}

	virtual void warn(const StringStream& message, const char* file=NULL, int line=-1)
	{
		tcout << message.str() << std::endl;
	}

	virtual bool isTraceEnabled() const
	{
		return true;
	}

	virtual void trace(const StringStream& message, const char* file=NULL, int line=-1)
	{
		tcout << message.str() << std::endl;
	}

	virtual bool isEnabledFor(const LevelPtr& level) const
	{
		return true;
	}

	virtual void log(const LevelPtr& level, const StringStream& message,
		const char* file=0, int line=-1)
	{
		tcout << message.str() << std::endl;
	}

	virtual const LevelPtr& getLevel() const
	{
		return _levelPtr;
	}

	virtual void pushNDC( const tchar* str )
	{
	}

	virtual void popNDC( )
	{
	}

	virtual void clearNDC()
	{
	}

private:
	LevelPtr _levelPtr;
};


namespace logging
{

	ConsoleLogger _console;

	ILogger* makeLogger( const tchar* nm )
	{
		return &_console;
	}

	ILogger* makeLogger( const tstring& nm )
	{
		return &_console;
	}
	
	ITracer* makeTracer( const tchar* nm )
	{
		return 0;
	}

	ITracer* makeTracer( const tstring& nm )
	{
		return 0;
	}
}

_jingxian_end
