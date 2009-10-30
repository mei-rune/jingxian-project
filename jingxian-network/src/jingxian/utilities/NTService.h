
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
//	 * 服务运行
//	 */
//	virtual void run(const std::vector<tstring>& arguments) = 0;
//
//	/**
//	 * 接收到一个服务将停止的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnStop() = 0;
//
//	/**
//	 * 接收到一个询问服务状态的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnInterrogate() = 0;
//
//	/**
//	 * 接收到一个服务暂停的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnPause() = 0;
//
//	/**
//	 * 接收到一个服务恢复的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnContinue() = 0;
//	
//	/**
//	 * 接收到一个系统将关闭的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnShutdown() = 0;
//
//	/**
//	 * 接收到一个新的网络组件被绑定的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnNetBindAdd() = 0;
//	
//	/**
//	 * 接收到一个网络组件绑定被启用的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnNetBindEnable() = 0;
//
//	/**
//	 * 接收到一个网络组件绑定被禁用的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnNetBindDisable() = 0;
//
//	/**
//	 * 接收到一个网络组件绑定被删除的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnNetBindRemove() = 0;
//	
//	/**
//	 * 接收到一个删除的通知
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnParamChange() = 0;
//
//	/**
//	 * 接收到一个用户定义的通知
//	 * @param dwEventType 用户定义的事件类型
//	 * @param lpEventData 用户定义的事件数据
//	 * @remarks 注意，不可以发生异常。
//	 */
//     void OnControl(DWORD dwEventType
//					, LPVOID lpEventData) = 0;
//};
template<typename SERVER>
class NTService
{
public:

	~NTService()
	{
		LOG_TRACE( _logger ,_T("NTService::~NTService()") );
		_pThis = NULL;
	}

	static int main(const tstring& name, SERVER* svr)
	{
		NTService ntService(name, svr);
		SERVICE_TABLE_ENTRY st[] = {
			{ (tchar*)name.c_str(), ServiceMain},
			{NULL, NULL}
		};

		if (!::StartServiceCtrlDispatcher(st) )
		{
			LOG_ERROR(ntService._logger, _T("启动服务 ")<< name << _T(" 失败 - ") << lastError() << _T("!"));
			return -1;
		}
		return 0;
	}

private:

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
		return _pThis->OnControl(dwControl, dwEventType, lpEventData);
	}

	NTService(const tstring& name, SERVER* svr)
		: _name( name )
		, _svr( svr )
		, _logger(logging::makeLogger(_T("NTService")))
		, _majorVersion( 1 )
		, _minorVersion( 0 )
		, _serviceStatus( NULL )
		, _isRunning( false )
	{
		if( _pThis != 0 )
			throw std::runtime_error( "服务实例只能有一个" );

		if( _name.empty() )
			throw std::runtime_error( "服务名不能为空" );

		if( _svr.get() == 0 )
			throw std::runtime_error( "服务回调不能空" );

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

	bool set_status(DWORD dwState)
	{
		DWORD oldState = _status.dwCurrentState;
		_status.dwCurrentState = dwState;
		if( ::SetServiceStatus(_serviceStatus, &_status) )
			return true;

		LOG_WARN( _logger , _T( "更改服务器 ")<< _name << _T(" 状态(") << oldState << _T("," )<< dwState << _T( ")失败 - ") << lastError() << _T("!"));
		_status.dwCurrentState = oldState;
		return false;
	}

	void run(DWORD dwArgc, LPTSTR* lpszArgv)
	{
		LOG_CRITICAL( _logger,_T("开始运行服务 ")<< _name << _T("."));
		_status.dwCurrentState = SERVICE_START_PENDING;
		_serviceStatus = RegisterServiceCtrlHandlerEx( (tchar*) _name.c_str(), HandlerEx, this);
		if ( NULL == _serviceStatus )
		{
			LOG_ERROR( _logger, _T( "服务 ") << _name << _T(" 注册服务控制句柄失败 - " << lastError(::GetLastError())));
			return;
		}

		if(!set_status(SERVICE_START_PENDING))
		{
			return ;
		}
		std::vector<tstring> arguments;
		for(DWORD i = 0; i < dwArgc; ++ i)
		{
			arguments.push_back(lpszArgv[i]);
		}

		_status.dwWin32ExitCode = GetLastError();
		_status.dwCheckPoint = 0;
		_status.dwWaitHint = 0;

		set_status(SERVICE_RUNNING);

		_isRunning = true;

		_status.dwWin32ExitCode = 0;
		_status.dwCheckPoint = 0;
		_status.dwWaitHint = 0;

		LOG_CRITICAL( _logger,_T( "服务 ")<< _name << _T(" 运行中...") );
		_svr->run(arguments);
		// 通知NT服务管理器自已已关闭
		LOG_CRITICAL( _logger,_T( "服务 ")<< _name << _T(" 退出.") );
		_isRunning = false;

		// 注意在设置 SERVICE_STOPPED 状态之后不能运行任何代码
		set_status(SERVICE_STOPPED);
	}

	DWORD OnControl(DWORD dwOpcode
		, DWORD dwEventType
		, LPVOID lpEventData)
	{
		LOG_TRACE( _logger,_T("服务 ")<< _name << _T(" 接收到操作代码[")<< dwOpcode << _T("]"));

		switch (dwOpcode) 
		{
		case SERVICE_CONTROL_STOP: // 1
			set_status(SERVICE_STOP_PENDING);
			_svr->onStop();
			_isRunning = false;
			break;
		case SERVICE_CONTROL_PAUSE: // 2
			_svr->onPause();
			break;
		case SERVICE_CONTROL_CONTINUE: // 3
			_svr->onContinue();
			break;
		case SERVICE_CONTROL_INTERROGATE: // 4
			_svr->onInterrogate();
			break;
		case SERVICE_CONTROL_SHUTDOWN: // 5
			_svr->onShutdown();
			break;
		case SERVICE_CONTROL_NETBINDADD: // NT 不支持
			_svr->onNetBindAdd();
			break;
		case SERVICE_CONTROL_NETBINDENABLE: // NT 不支持
			_svr->onNetBindEnable();
			break;
		case SERVICE_CONTROL_NETBINDDISABLE: // NT 不支持
			_svr->onNetBindDisable();
			break;
		case SERVICE_CONTROL_NETBINDREMOVE: // NT 不支持
			_svr->onNetBindRemove();
			break;
		case SERVICE_CONTROL_PARAMCHANGE: // NT 不支持
			_svr->onParamChange();
			break;
		default:
			if (dwOpcode >= SERVICE_CONTROL_USER)
			{
				_svr->onControl(dwEventType, lpEventData);
			}
			else
			{
				LOG_INFO( _logger,_T( "服务 ")<< _name << _T(" 接收到错误的服务通知")  );
			}
			break;
		}

		// Report current status
		LOG_TRACE( _logger,_T( "更新服务 ")<< _name << _T(" 状态(") 
			<< _serviceStatus << _T( "," )
			<< _status.dwCurrentState << _T(")"));

		::SetServiceStatus(_serviceStatus, &_status);
		return NO_ERROR;
	}

private:
	tstring _name;
	std::auto_ptr<SERVER> _svr;
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
