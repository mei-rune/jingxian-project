
#ifndef _NTSERVICE_H_
#define _NTSERVICE_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <winsvc.h>
#include <vector>
#include "jingxian/logging/logging.h"

_jingxian_begin

#define SERVICE_CONTROL_USER 128

//class IService
//{
//public:
//	
//	/**
//	 * ��������
//	 */
//	virtual void run() = 0;
//
//	/**
//	 * ��ʼ��,������run֮ǰ���á�
//	 * @param[ in ] dwArgc ���������С
//	 * @param[ in ] lpszArgv ��������
//	 * @return �ɹ�����true, ʧ�ܷ���false
//	 * @remarks ע�⣬�����Է����쳣�������ָ���˳����룬����SetLastError
//	 * ����SetLastErrorEx ����Ϊ�����߻���GetLastErrorȡ���˳����롣
//	 */
//	 bool OnInit( DWORD dwArgc, LPTSTR* lpszArgv ) = 0;
//
//	/**
//	 * ���յ�һ������ֹͣ��֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnStop() = 0;
//
//	/**
//	 * ���յ�һ��ѯ�ʷ���״̬��֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnInterrogate() = 0;
//
//	/**
//	 * ���յ�һ��������ͣ��֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnPause() = 0;
//
//	/**
//	 * ���յ�һ������ָ���֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnContinue() = 0;
//	
//	/**
//	 * ���յ�һ��ϵͳ���رյ�֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnShutdown() = 0;
//
//	/**
//	 * ���յ�һ���µ�����������󶨵�֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnNetBindAdd() = 0;
//	
//	/**
//	 * ���յ�һ����������󶨱����õ�֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnNetBindEnable() = 0;
//
//	/**
//	 * ���յ�һ����������󶨱����õ�֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnNetBindDisable() = 0;
//
//	/**
//	 * ���յ�һ����������󶨱�ɾ����֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnNetBindRemove() = 0;
//	
//	/**
//	 * ���յ�һ��ɾ����֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnParamChange() = 0;
//
//	/**
//	 * ���յ�һ���û������֪ͨ
//	 * @param dwOpcode �û������֪ͨ
//	 * @remarks ע�⣬�����Է����쳣��
//	 */
//     void OnControl(DWORD dwOpcode) = 0;
//};
template<typename SERVER>
class NTService
{
public:
	NTService(const std::string& name, SERVER* svr)
		: _name( name )
		, _svr( svr )
		, _logger(logging::makeLogger(_T("NTService")))
		, _majorVersion( 1 )
		, _minorVersion( 0 )
		, _serviceStatus( NULL )
		, _isRunning( false )
	{
	if( _pThis != 0 )
		throw std::runtime_error( "����ʵ��ֻ����һ��" );

	if( _name.empty() )
		throw std::runtime_error( "����������Ϊ��" );

	if( _svr == 0 )
		throw std::runtime_error( "����ص����ܿ�" );

    _pThis = this;
    
    _status.dwServiceType = SERVICE_WIN32_OWN_PROCESS;
    _status.dwCurrentState = SERVICE_STOPPED;
    _status.dwControlsAccepted = SERVICE_ACCEPT_STOP;
    _status.dwWin32ExitCode = 0;
    _status.dwServiceSpecificExitCode = 0;
    _status.dwCheckPoint = 0;
    _status.dwWaitHint = 0;

	
		LOG_TRACE( _logger ,_T("NTService::NTService()") );
	}

    ~NTService()
	{
		LOG_TRACE( _logger ,_T("NTService::~NTService()") );
	}

    bool start( DWORD dwArgc, LPTSTR* lpszArgv )
	{
		SERVICE_TABLE_ENTRY st[] = {
			{ (char*)_name.c_str(), ServiceMain},
			{NULL, NULL}
		};

		if ( ::StartServiceCtrlDispatcher(st) )
		{
			LOG_CRITICAL(_logger ,_T("�������� ")<< _name << _T(" �ɹ�!"));
			return true;
		}
		else
		{
			LOG_ERROR(_logger ,_T("�������� ")<< _name << _T(" ʧ�� - ") << lastError() << _T("!"));
			return false;
		}
	}

	static void WINAPI ServiceMain(DWORD dwArgc, LPTSTR* lpszArgv)
	{
		_pThis->run(dwArgc, lpszArgv);
	}

	static DWORD WINAPI HandlerEx(
                   DWORD dwControl,
                   DWORD dwEventType,
                   LPVOID lpEventData,
                   LPVOID lpContext)
	{
		NTService* pThis = (NTService*)lpContext;
		return _pThis->OnControl(dwControl);
	}

private:


	bool initialize( DWORD dwArgc, LPTSTR* lpszArgv )
	{
		if( !set_status(SERVICE_START_PENDING) ) return false;

		bool bResult = _svr->OnInit( dwArgc, lpszArgv ); 

		// Set final state
		_status.dwWin32ExitCode = GetLastError();
		_status.dwCheckPoint = 0;
		_status.dwWaitHint = 0;
		if (!bResult)
		{
			set_status(SERVICE_STOPPED);
			return false;    
		}

		set_status(SERVICE_RUNNING);
		return true;
	}


	bool set_status(DWORD dwState)
	{
		DWORD oldState = _status.dwCurrentState;
		_status.dwCurrentState = dwState;
		if( ::SetServiceStatus(_serviceStatus, &_status) )

			return true;

		LOG_WARN( _logger , _T( "���ķ����� ")<< _name << _T(" ״̬(") << oldState << _T("," )<< dwState << _T( ")ʧ�� - ") << lastError() << _T("!"));
		_status.dwCurrentState = oldState;
		return false;
	}

	void run(DWORD dwArgc, LPTSTR* lpszArgv)
	{
		LOG_CRITICAL( _logger,_T("��ʼ���з��� ")<< _name << _T("."));
		_status.dwCurrentState = SERVICE_START_PENDING;
		_serviceStatus = RegisterServiceCtrlHandlerEx( (char*) _name.c_str(), HandlerEx, this);
		if ( NULL == _serviceStatus )
		{
			LOG_ERROR( _logger, _T( "���� ") << _name << _T(" ע�������ƾ��ʧ�� - " << lastError(::GetLastError())));
			return;
		}
		
		LOG_TRACE( _logger,_T( "��ʼ������ ")<< _name << _T(".") );
		if (initialize( dwArgc, lpszArgv ))
		{
			_isRunning = true;
			_status.dwWin32ExitCode = 0;
			_status.dwCheckPoint = 0;
			_status.dwWaitHint = 0;

			LOG_TRACE( _logger,_T( "���� ")<< _name << _T(" ������...") );
			_svr->run();
			// ֪ͨNT��������������ѹر�
			set_status(SERVICE_STOPPED);
			LOG_TRACE( _logger,_T( "���� ")<< _name << _T(" �˳�.") );
		}
		else
		{
			LOG_FATAL(_logger ,_T("�������� ")<< _name << _T(" ʧ�� - ��ʼ����������!"));
			set_status(SERVICE_STOPPED);
		}
	}

	DWORD OnControl(DWORD dwOpcode)
	{
		LOG_TRACE( _logger,_T("���� ")<< _name << _T(" ���յ���������[")<< dwOpcode << _T("]"));

		switch (dwOpcode) 
		{
		case SERVICE_CONTROL_STOP: // 1
			set_status(SERVICE_STOP_PENDING);
			_svr->OnStop();
			_isRunning = false;
			break;
		case SERVICE_CONTROL_PAUSE: // 2
			_svr->OnPause();
			break;
		case SERVICE_CONTROL_CONTINUE: // 3
			_svr->OnContinue();
			break;
		case SERVICE_CONTROL_INTERROGATE: // 4
			_svr->OnInterrogate();
			break;
		case SERVICE_CONTROL_SHUTDOWN: // 5
			_svr->OnShutdown();
			break;
		case SERVICE_CONTROL_NETBINDADD: // NT ��֧��
			_svr->OnNetBindAdd();
			break;
		case SERVICE_CONTROL_NETBINDENABLE: // NT ��֧��
			_svr->OnNetBindEnable();
			break;
		case SERVICE_CONTROL_NETBINDDISABLE: // NT ��֧��
			_svr->OnNetBindDisable();
			break;
		case SERVICE_CONTROL_NETBINDREMOVE: // NT ��֧��
			_svr->OnNetBindRemove();
			break;
		case SERVICE_CONTROL_PARAMCHANGE: // NT ��֧��
			_svr->OnParamChange();
			break;
		default:
			if (dwOpcode >= SERVICE_CONTROL_USER)
			{
				_svr->OnControl(dwOpcode);
			}
			else
			{
				LOG_INFO( _logger,_T( "���� ")<< _name << _T(" ���յ�����ķ���֪ͨ")  );
			}
			break;
		}

		// Report current status
		LOG_TRACE( _logger,_T( "���·��� ")<< _name << _T(" ״̬(") 
			<< _serviceStatus << _T( "," )
			<< _status.dwCurrentState << _T(")"));

		::SetServiceStatus(_serviceStatus, &_status);
		return NO_ERROR;
	}

private:
	std::string _name;
	SERVER* _svr;
	ILogger* _logger;
    int _majorVersion;
    int _minorVersion;
    SERVICE_STATUS_HANDLE _serviceStatus;
    SERVICE_STATUS _status;
    bool _isRunning;

    static NTService<SERVER>* _pThis;
};

template<typename SERVER>
NTService<SERVER>* NTService<SERVER>::_pThis = NULL;

_jingxian_end

#endif // _NTSERVICE_H_
