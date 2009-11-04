
#ifndef _ITracer_H_
#define _ITracer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"
# include "jingxian/networks/connection_status.h"

_jingxian_begin

class ITracer
{
public:
    /**
     * virtual tracer destructor
     */
    virtual ~ITracer() {};

    /**
     * debug级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isDebugEnabled() const = 0;

    /**
     * 记录debug级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void debug(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * error级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isErrorEnabled() const = 0;

    /**
     * 记录error级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void error(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * fatal级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isFatalEnabled() const = 0;

    /**
     * 记录fatal级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void fatal(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * info级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isInfoEnabled() const = 0;

    /**
     * 记录info级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void info(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * warn级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isWarnEnabled() const = 0;

    /**
     * 记录warn级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void warn(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;


    /**
     * Trace级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isTraceEnabled() const = 0;

    /**
     * 记录trace级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void trace(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;

    /**
     * Crit 级的日志是否可以记日志
     * @return 可以true,不可以false
     */
    virtual bool isCritEnabled() const = 0;

    /**
     * 记录 Crit 级的日志
     * @param[ in ] way 流传输的方向
     * @param[ in ] message 日志内容
     * @param[ in ] file 日志记录的源文件名
     * @param[ in ] line 日志记录的源文件的当前行
     */
    virtual void crit(transport_mode::type way, const LogStream& message, const char* file = 0, int line = -1) = 0;
};

namespace logging
{
namespace spi
{
class ITraceFactory
{
public:
    virtual ~ITraceFactory() {};

    virtual ITracer* make(const tchar* nm, const tstring& host, const tstring& peer, const tstring& sessionId) = 0;
};
}
}

_jingxian_end


#ifndef _NO_LOG_

#ifndef TP_CRITICAL
#define TP_CRITICAL(logger, way, message) { \
  if ( logger != 0 && logger->isCritEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->crit(way, oss, __FILE__, __LINE__); }}
#endif // TP_CRITICAL

#ifndef TP_DEBUG
#define TP_DEBUG(logger, way, message) { \
  if ( logger != 0 && logger->isDebugEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->debug(way, oss, __FILE__, __LINE__); }}
#endif // LOG_DEBUG

#ifndef TP_INFO
#define TP_INFO(logger, way, message) { \
  if ( logger != 0 && logger->isInfoEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->info(way, oss, __FILE__, __LINE__); }}
#endif // LOG_INFO

#ifndef TP_WARN
#define TP_WARN(logger, way, message ) { \
  if ( logger != 0 && logger->isWarnEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->warn(way, oss, __FILE__, __LINE__); }}
#endif // LOG_WARN

#ifndef TP_ERROR
#define TP_ERROR(logger, way, message) { \
  if ( logger != 0 && logger->isErrorEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->error(way, oss, __FILE__, __LINE__); }}
#endif // LOG_ERROR

#ifndef TP_FATAL
#define TP_FATAL(logger, way, message) { \
  if ( logger != 0 && logger->isFatalEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->fatal(way, oss, __FILE__, __LINE__); }}
#endif // LOG_FATAL

#ifndef TP_TRACE
#define TP_TRACE(logger, way, message) { \
  if ( logger != 0 && logger->isTraceEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger->trace(way, oss, __FILE__, __LINE__); }}
#endif // TP_TRACE

#else // _NO_LOG_

#ifndef TP_LOG
#define TP_LOG(logger, way, level, message)     {}
#endif // TP_LOG

#ifndef TP_DEBUG
#define TP_DEBUG(logger, way, message)      {}
#endif // TP_DEBUG

#ifndef TP_INFO
#define TP_INFO(logger, way, message)     {}
#endif // TP_INFO

#ifndef TP_WARN
#define TP_WARN(logger, way, message)     {}
#endif // TP_WARN

#ifndef TP_ERROR
#define TP_ERROR(logger, way, message)      {}
#endif // TP_TP_ERROR

#ifndef TP_FATAL
#define TP_FATAL(logger, way, message)      {}
#endif // LOG_FATAL

#ifndef TP_TRACE
#define TP_TRACE(logger, way, message)      {}
#endif // TP_TRACE

#endif // _NO_LOG_

#endif // _ITracer_H_
