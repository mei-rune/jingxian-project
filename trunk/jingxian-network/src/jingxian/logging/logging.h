
#ifndef _LOGGING_H_
#define _LOGGING_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/logging/ILogger.h"
# include "jingxian/logging/ITracer.h"


_jingxian_begin

namespace logging
{
namespace spi
{
ILogger* makeLogger(const tstring& nm);

ILogFactory* setLogFactory(ILogFactory* factory);

ITracer* makeTracer(const tstring& nm, const tstring& host, const tstring& peer, const tstring& sessionId);

ITraceFactory* setTraceFactory(ITraceFactory* factory);
}

/**
 * @Brief ILogger 日志接口
 * 这是BT的日志接口，每一个具体的日志都必须从它继承，每一个需要记日志的都可以得到一个这样的接，
 * 但不推荐直接使用它。有一组预定义好的宏，请使用宏
 */
class logger
{
public:
  //logger(const tchar* nm);

  logger(const tstring& nm);

  virtual ~logger();

  /**
   * assert 语句
   * @param[ in ] assertion 条件是否为真
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void assertLog(bool assertion, const LogStream& msg, const char* file, int line);

  /**
   * CRIT 级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isCritEnabled() const;

  /**
   * 记录 CRIT 级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void crit(const LogStream& message, const char* file, int line);

  /**
   * fatal级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isFatalEnabled() const;

  /**
   * 记录fatal级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void fatal(const LogStream& message, const char* file, int line);

  /**
   * error级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isErrorEnabled() const;

  /**
   * 记录error级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void error(const LogStream& message, const char* file, int line);

  /**
   * info级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isInfoEnabled() const;

  /**
   * 记录info级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void info(const LogStream& message, const char* file, int line);

  /**
   * debug级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isDebugEnabled() const;

  /**
   * 记录debug级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void debug(const LogStream& message, const char* file, int line);

  /**
   * warn级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isWarnEnabled() const;

  /**
   * 记录warn级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void warn(const LogStream& message, const char* file, int line);

  /**
   * Trace级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isTraceEnabled() const;

  /**
   * 记录trace级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void trace(const LogStream& message, const char* file, int line);

  /**
   * level级的日志是否可以记日志
   * @return 可以true,不可以false
   */
  virtual bool isEnabledFor(const logging::LevelPtr& level) const;

  /**
   * 记录level级的日志
   * @param[ in ] message 日志内容
   * @param[ in ] file 日志记录的源文件名
   * @param[ in ] line 日志记录的源文件的当前行
   */
  virtual void log(const LevelPtr& level, const LogStream& message,
                   const char* file, int line);

  /**
   * 取得当前的日志的级别
   * @return 日志的级别
   */
  virtual LevelPtr getLevel() const;

private:
  NOCOPY(logger);

  spi::ILogger* impl_;
};

}



#ifndef _NO_LOG_

#ifndef LOG
#define LOG(logger, level, message) { \
  if(logger.isEnabledFor(level)) {\
  LogStream oss; \
  oss << message; \
  logger.fatal(level, oss, __FILE__, __LINE__); }}
#endif // LOG

#ifndef LOG_DEBUG
#define LOG_DEBUG(logger, message) { \
  if(logger.isDebugEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.debug( oss, __FILE__, __LINE__); }}
#endif // LOG_DEBUG

#ifndef LOG_INFO
#define LOG_INFO(logger, message) { \
  if(logger.isInfoEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.info( oss, __FILE__, __LINE__); }}
#endif // LOG_INFO

#ifndef LOG_WARN
#define LOG_WARN(logger, message ) { \
  if(logger.isWarnEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.warn( oss, __FILE__, __LINE__); }}
#endif // LOG_WARN

#ifndef LOG_ERROR
#define LOG_ERROR(logger, message) { \
  if(logger.isErrorEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.error( oss, __FILE__, __LINE__); }}
#endif // LOG_ERROR

#ifndef LOG_FATAL
#define LOG_FATAL(logger, message) { \
  if(logger.isFatalEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.fatal( oss, __FILE__, __LINE__); }}
#endif // LOG_FATAL

#ifndef LOG_TRACE
#define LOG_TRACE(logger, message) { \
  if(logger.isTraceEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.trace( oss, __FILE__, __LINE__); }}
#endif // LOG_TRACE

#ifndef LOG_CRITICAL
#define LOG_CRITICAL(logger, message) { \
  if(logger.isCritEnabled()) {\
  LogStream oss; \
  oss << message; \
  logger.crit( oss, __FILE__, __LINE__); }}
#endif // TP_CRITICAL

#ifndef LOG_ASSERT
#define LOG_ASSERT(logger,assertion) { \
  LogStream oss;                      \
  logger.assertLog( assertion ,oss, __FILE__, __LINE__);   \
  (void)( (!!(assertion)) || (_wassert(_CRT_WIDE(#assertion), _CRT_WIDE(__FILE__), __LINE__), 0) );	\
  }

#endif // LOG_ASSERT

#else // _NO_LOG_

#ifndef BT_NDC
#define BT_NDC( logger, ndc , msg )       {}
#endif // BT_NDC

#ifndef LOG
#define LOG(logger, level, message)     {}
#endif // LOG

#ifndef LOG_DEBUG
#define LOG_DEBUG(logger, message)      {}
#endif // LOG_DEBUG

#ifndef LOG_INFO
#define LOG_INFO(logger, message)     {}
#endif // LOG_INFO

#ifndef LOG_WARN
#define LOG_WARN(logger, message)     {}
#endif // LOG_WARN

#ifndef LOG_ERROR
#define LOG_ERROR(logger, message)      {}
#endif // LOG_ERROR

#ifndef LOG_FATAL
#define LOG_FATAL(logger, message)      {}
#endif // LOG_FATAL

#ifndef LOG_TRACE
#define LOG_TRACE(logger, message)      {}
#endif // LOG_TRACE

#ifndef LOG_ASSERT
#define LOG_ASSERT(logger,assertion )     (void)( (!!(assertion)) || (_wassert(_CRT_WIDE(#assertion), _CRT_WIDE(__FILE__), __LINE__), 0) );
#endif // LOG_ASSERT

#endif // _NO_LOG_

_jingxian_end

#endif // _LOGGING_H_