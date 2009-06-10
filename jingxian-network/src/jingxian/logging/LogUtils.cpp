
# include "pro_config.h"
# include "jingxian/logging/logutils.h"
# include "jingxian/logging/stdlogger.h"

_jingxian_begin

namespace logging
{

	StdLogger _console;

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