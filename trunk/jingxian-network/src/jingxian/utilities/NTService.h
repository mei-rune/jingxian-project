
#ifndef _NTSERVICE_H_
#define _NTSERVICE_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <vector>
# include "jingxian/string/os_string.h"

_jingxian_begin

#ifndef NT_LOG_TRACE
#define NT_LOG_TRACE(message) { \
	LogStream oss; \
	oss << message; \
	logTrace(oss, __FILE__, __LINE__); }
#endif // NT_LOG_TRACE

#ifndef NT_LOG_WARN
#define NT_LOG_WARN(message) { \
	LogStream oss; \
	oss << message; \
	logWarn( oss, __FILE__, __LINE__); }
#endif // NT_LOG_WARN

#ifndef NT_LOG_ERROR
#define NT_LOG_ERROR(message) { \
	LogStream oss; \
	oss << message; \
	logError( oss, __FILE__, __LINE__); }
#endif // NT_LOG_ERROR

#ifndef NT_LOG_FATAL
#define NT_LOG_FATAL(message) { \
	LogStream oss; \
	oss << message; \
	logFatal(oss, __FILE__, __LINE__);}
#endif // NT_LOG_FATAL

#ifndef NT_LOG_CRITICAL
#define NT_LOG_CRITICAL(message) { \
	LogStream oss; \
	oss << message; \
	logCritical(oss, __FILE__, __LINE__); }
#endif // NT_LOG_CRITICAL

class IInstance
{
public:
	virtual ~IInstance(){ }

	/**
	 * NT ������
	 */
	virtual const tstring& name() const = 0;
	
	/**
	 * ��������
	 */
	virtual int onRun(const std::vector<tstring>& arguments) = 0;

	 /**
	  * ���յ�һ���û������֪ͨ
	  * @param dwEventType �û�������¼�����
	  * @param lpEventData �û�������¼�����
	  * @remarks ע�⣬�����Է����쳣��
	  */
     virtual void onControl(DWORD dwEventType
					, LPVOID lpEventData) = 0;

	 
	 /**
	  * ���յ�һ������ֹͣ��֪ͨ
	  * @remarks ע�⣬�����Է����쳣��
	  */
     virtual void interrupt() = 0;

	 /**
	  * ���������
	  */
	 virtual const tstring& toString() const = 0;
};


/**
 * ������ں���
 */
int serviceMain(IInstance* instance);//, DWORD argc,LPTSTR *argv);

/**
 * ��װһ�� Win32 ����
 * @param[ in ] name Win32 ���������
 * @param[ in ] display Win32 �����������Ϣ
 * @param[ in ] executable Win32 �����ִ�г�������
 * @param[ in ] args Win32 ����Ĳ���
 * @return �ɹ�����0,���򷵻ط�0
 */
int installService(const tstring& name
	, const tstring& display
	, const tstring& description
	, const tstring& executable
	, const std::vector<tstring>& args);

/**
 * ж��һ�� Win32 ����
 * @param[ in ] name Win32 ���������
 * @return �ɹ�����0,���򷵻ط�0
 */
int uninstallService(const tstring& name );

/**
 * ����һ�� Win32 ����
 * @param[ in ] name Win32 ���������
 * @param[ in ] args Win32 ����Ĳ���
 * @return �ɹ�����0,���򷵻ط�0
 */
int startService(const tstring& name, const std::vector<tstring>& args);

/**
 * ֹͣһ�� Win32 ����
 * @param[ in ] name Win32 ���������
 * @return �ɹ�����0,���򷵻ط�0
 */
int stopService(const tstring& name);

_jingxian_end

#endif // _NTSERVICE_H_
