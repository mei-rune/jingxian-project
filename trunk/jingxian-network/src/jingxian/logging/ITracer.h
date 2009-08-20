
#ifndef _jingxian_tracer_h_
#define _jingxian_tracer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/networks/connection_status.h"

_jingxian_begin

class ITracer 
{
public:
	/**
	 * virtual tracer destructor
	 */
	virtual ~ITracer(){};

	/**
	 * debug������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isDebugEnabled() const = 0;

	/**
	 * ��¼debug������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
	virtual void debug(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);
	
	/**
	 * error������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isErrorEnabled() const = 0;

	/**
	 * ��¼error������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
    virtual void error(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);
	
	/**
	 * fatal������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isFatalEnabled() const = 0;

	/**
	 * ��¼fatal������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
    virtual void fatal(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);

	/**
	 * info������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isInfoEnabled() const = 0;

	/**
	 * ��¼info������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
    virtual void info(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);
	
	/**
	 * warn������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isWarnEnabled() const = 0;

	/**
	 * ��¼warn������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
    virtual void warn(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);

	
	/**
	 * Trace������־�Ƿ���Լ���־
	 * @return ����true,������false
	 */
	virtual bool isTraceEnabled() const = 0;

	/**
	 * ��¼trace������־
	 * @param[ in ] way ������ķ���
	 * @param[ in ] message ��־����
	 * @param[ in ] file ��־��¼��Դ�ļ���
	 * @param[ in ] line ��־��¼��Դ�ļ��ĵ�ǰ��
	 */
    virtual void trace(transport_mode::type way, const StringStream& message, const char* file=0, int line=-1);
};

namespace logging
{
	ITracer* makeTracer( const tchar* nm );

	ITracer* makeTracer( const tstring& nm );
}

_jingxian_end


#ifndef _NO_LOG_

#ifndef TP_CRITICAL
#define TP_CRITICAL(logger, way, message) { \
	if ( logger != 0 && logger->isDebugEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->info(way, oss, __FILE__, __LINE__); }}
#endif // DEBUG


#ifndef TP_DEBUG
#define TP_DEBUG(logger, way, message) { \
	if ( logger != 0 && logger->isDebugEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->debug(way, oss, __FILE__, __LINE__); }}
#endif // DEBUG

#ifndef TP_INFO
#define TP_INFO(logger, way, message) { \
	if ( logger != 0 && logger->isInfoEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->info(way, oss, __FILE__, __LINE__); }}
#endif // INFO

#ifndef TP_WARN
#define TP_WARN(logger, way, message ) { \
	if ( logger != 0 && logger->isWarnEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->warn(way, oss, __FILE__, __LINE__); }}
#endif // WARN

#ifndef TP_ERROR
#define TP_ERROR(logger, way, message) { \
	if ( logger != 0 && logger->isErrorEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->error(way, oss, __FILE__, __LINE__); }}
#endif // ERROR

#ifndef TP_FATAL
#define TP_FATAL(logger, way, message) { \
	if ( logger != 0 && logger->isFatalEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->fatal(way, oss, __FILE__, __LINE__); }}
#endif // FATAL

#ifndef TP_TRACE
#define TP_TRACE(logger, way, message) { \
	if ( logger != 0 && logger->isFatalEnabled()) {\
	StringStream oss; \
	oss << message; \
	logger->trace(way, oss, __FILE__, __LINE__); }}
#endif // TP_TRACE

#else	// _NO_LOG_

#ifndef TP_LOG
#define TP_LOG(logger, way, level, message)			{}
#endif // TP_LOG

#ifndef TP_DEBUG
#define TP_DEBUG(logger, way, message)			{}
#endif // TP_DEBUG

#ifndef TP_INFO
#define TP_INFO(logger, way, message)			{}
#endif // TP_INFO

#ifndef TP_WARN
#define TP_WARN(logger, way, message)			{}
#endif // TP_WARN

#ifndef TP_ERROR
#define TP_ERROR(logger, way, message)			{}
#endif // TP_TP_ERROR

#ifndef TP_FATAL
#define TP_FATAL(logger, way, message)			{}
#endif // FATAL

#ifndef TP_TRACE
#define TP_TRACE(logger, way, message)			{}
#endif // TP_TRACE

#endif // _NO_LOG_

#endif // _JINGXIAN_Log_H_
