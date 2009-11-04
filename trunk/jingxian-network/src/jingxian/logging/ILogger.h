
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
  ,Error
  ,Info
  ,Debug
  ,Warn
  ,Trace
};


typedef long LevelPtr;
}

/**
 * @Brief ILogger ��־�ӿ�
 * ����BT����־�ӿڣ�ÿһ���������־����������̳У�ÿһ����Ҫ����־�Ķ����Եõ�һ�������Ľӣ�
 * �����Ƽ�ֱ��ʹ��������һ��Ԥ����õĺ꣬��ʹ�ú�
 */
class ILogger
{
public:


  /**
   * virtual JINGXIAN_Log_Impl destructor
   */
  virtual ~ILogger() {};

  /**
   * assert ���
   * @param[ in ] assertion �����Ƿ�Ϊ��
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void assertLog(bool assertion, const LogStream& msg, const char* file=0, int line=-1) = 0;

  /**
   * fatal������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isFatalEnabled() const = 0;

  /**
   * ��¼fatal������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void fatal(const LogStream& message, const char* file=0, int line=-1) = 0;

  /**
   * error������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isErrorEnabled() const = 0;

  /**
   * ��¼error������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void error(const LogStream& message, const char* file=0, int line=-1) = 0;

  /**
   * info������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isInfoEnabled() const = 0;

  /**
   * ��¼info������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void info(const LogStream& message, const char* file=NULL, int line=-1) = 0;

  /**
   * debug������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isDebugEnabled() const = 0;

  /**
   * ��¼debug������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void debug(const LogStream& message, const char* file=0, int line=-1) = 0;

  /**
   * warn������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isWarnEnabled() const = 0;

  /**
   * ��¼warn������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void warn(const LogStream& message, const char* file=NULL, int line=-1) = 0;

  /**
   * Trace������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isTraceEnabled() const = 0;

  /**
   * ��¼trace������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void trace(const LogStream& message, const char* file=NULL, int line=-1) = 0;

  /**
   * Trace������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isCritEnabled() const = 0;

  /**
   * ��¼trace������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void crit(const LogStream& message, const char* file=NULL, int line=-1) = 0;

  /**
   * level������־�Ƿ���Լ���־
   * @return ����true,������false
   */
  virtual bool isEnabledFor(const logging::LevelPtr& level) const = 0;

  /**
   * ��¼level������־
   * @param[ in ] message ��־����
   * @param[ in ] file ��־��¼��Դ�ļ���
   * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
   */
  virtual void log(const logging::LevelPtr& level, const LogStream& message,
                   const char* file=0, int line=-1) = 0;

  /**
   * ȡ�õ�ǰ����־�ļ���
   * @return ��־�ļ���
   */
  virtual logging::LevelPtr getLevel() const = 0;

  /**
   * ���ڶ����NDC,������Ļ����̵߳Ĳ�һ��������ֻ��ʹ�������־�������
   * ������־�д�ӡNDC
   * ��һ��NDC
   * @param[ in ] str �����ַ���
   */
  virtual void pushNDC( const tchar* str ) = 0;

  /**
   * ȡ��һ��NDC
   */
  virtual void popNDC( ) = 0;

  /**
   * ���NDC
   */
  virtual void clearNDC() = 0;
};

class ndc
{
public:
  virtual ~ndc() {}
  virtual void pushNDC( const tchar* str ) = 0;
  virtual void popNDC( ) = 0;
};

namespace logging
{
namespace spi
{

class ILogFactory
{
public:

  virtual ~ILogFactory() {};

  virtual ILogger* make(const tchar* nm) = 0;
};
}
}

_jingxian_end

#ifndef _NO_LOG_



#ifndef LOG
#define LOG(logger, level, message) { \
	if ( logger != 0 && logger->isEnabledFor(level)) {\
	LogStream oss; \
	oss << message; \
	logger->fatal(level, oss, __FILE__, __LINE__); }}
#endif // LOG

#ifndef LOG_DEBUG
#define LOG_DEBUG(logger, message) { \
	if ( logger != 0 && logger->isDebugEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->debug( oss, __FILE__, __LINE__); }}
#endif // LOG_DEBUG

#ifndef LOG_INFO
#define LOG_INFO(logger, message) { \
	if ( logger != 0 && logger->isInfoEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->info( oss, __FILE__, __LINE__); }}
#endif // LOG_INFO

#ifndef LOG_WARN
#define LOG_WARN(logger, message ) { \
	if ( logger != 0 && logger->isWarnEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->warn( oss, __FILE__, __LINE__); }}
#endif // LOG_WARN

#ifndef LOG_ERROR
#define LOG_ERROR(logger, message) { \
	if ( logger != 0 && logger->isErrorEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->error( oss, __FILE__, __LINE__); }}
#endif // LOG_ERROR

#ifndef LOG_FATAL
#define LOG_FATAL(logger, message) { \
	if ( logger != 0 && logger->isFatalEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->fatal( oss, __FILE__, __LINE__); }}
#endif // LOG_FATAL

#ifndef LOG_TRACE
#define LOG_TRACE(logger, message) { \
	if ( logger != 0 && logger->isTraceEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->trace( oss, __FILE__, __LINE__); }}
#endif // LOG_TRACE

#ifndef LOG_CRITICAL
#define LOG_CRITICAL(logger, message) { \
	if ( logger != 0 && logger->isCritEnabled()) {\
	LogStream oss; \
	oss << message; \
	logger->crit( oss, __FILE__, __LINE__); }}
#endif // TP_CRITICAL

#ifndef LOG_ASSERT
#define LOG_ASSERT( logger,assertion ) { \
	if ( logger != 0 ) {\
	LogStream oss;											\
	logger->assertLog( assertion ,oss, __FILE__, __LINE__); 	\
	}															\
	else														\
	{															\
	(void)( (!!(assertion)) || (_wassert(_CRT_WIDE(#assertion), _CRT_WIDE(__FILE__), __LINE__), 0) );\
	} }

#endif // LOG_ASSERT

#else	// _NO_LOG_

#ifndef BT_NDC
#define BT_NDC( logger, ndc , msg )				{}
#endif // BT_NDC

#ifndef LOG
#define LOG(logger, level, message)			{}
#endif // LOG

#ifndef LOG_DEBUG
#define LOG_DEBUG(logger, message)			{}
#endif // LOG_DEBUG

#ifndef LOG_INFO
#define LOG_INFO(logger, message)			{}
#endif // LOG_INFO

#ifndef LOG_WARN
#define LOG_WARN(logger, message)			{}
#endif // LOG_WARN

#ifndef LOG_ERROR
#define LOG_ERROR(logger, message)			{}
#endif // LOG_ERROR

#ifndef LOG_FATAL
#define LOG_FATAL(logger, message)			{}
#endif // LOG_FATAL

#ifndef LOG_TRACE
#define LOG_TRACE(logger, message)			{}
#endif // LOG_TRACE

#ifndef LOG_ASSERT
#define LOG_ASSERT(logger,assertion )			{}
#endif // LOG_ASSERT

#endif // _NO_LOG_

#endif // _JINGXIAN_Log_H_