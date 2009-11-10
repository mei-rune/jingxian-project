
#ifndef _jingxian_log_h_
#define _jingxian_log_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <assert.h>
# include "jingxian/string/string.h"
# include "jingxian/jingxian_export.h"

_jingxian_begin


namespace logging
{
enum
{
    Fatal
    , Error
    , Info
    , Debug
    , Warn
    , Trace
};


typedef long LevelPtr;

namespace spi
{

/**
 * @Brief ILogger 日志接口
 * 这是BT的日志接口，每一个具体的日志都必须从它继承，每一个需要记日志的都可以得到一个这样的接，
 * 但不推荐直接使用它。有一组预定义好的宏，请使用宏
 */
class ILogger
{
public:


    /**
     * destructor
     */
    virtual ~ILogger() {};

    /**
     * assert 语句
     * @param[ in ] assertion 条件是否为真
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void assertLog(bool assertion, const LogStream& msg, const char* file = 0, int line = -1) = 0;

    /**
     * fatal级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isFatalEnabled() const = 0;

    /**
     * 记录fatal级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void fatal(const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * error级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isErrorEnabled() const = 0;

    /**
     * 记录error级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void error(const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * info级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isInfoEnabled() const = 0;

    /**
     * 记录info级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void info(const LogStream& message, const char* file = NULL, int line = -1) = 0;

    /**
     * debug级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isDebugEnabled() const = 0;

    /**
     * 记录debug级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void debug(const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * warn级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isWarnEnabled() const = 0;

    /**
     * 记录warn级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void warn(const LogStream& message, const char* file = NULL, int line = -1) = 0;

    /**
     * Trace级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isTraceEnabled() const = 0;

    /**
     * 记录trace级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void trace(const LogStream& message, const char* file = NULL, int line = -1) = 0;

    /**
     * Trace级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isCritEnabled() const = 0;

    /**
     * 记录trace级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void crit(const LogStream& message, const char* file = NULL, int line = -1) = 0;

    /**
     * level级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isEnabledFor(const logging::LevelPtr& level) const = 0;

    /**
     * 记录level级的日志
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void log(const logging::LevelPtr& level, const LogStream& message,
                     const char* file = 0, int line = -1) = 0;

    /**
     * 取得当前的日志的级别
     * @return 日志的级别
     */
    virtual logging::LevelPtr getLevel() const = 0;
};

class ILogFactory
{
public:

    virtual ~ILogFactory() {};

    virtual ILogger* make(const tchar* nm) = 0;
};

}

}

_jingxian_end


#endif // _JINGXIAN_Log_H_