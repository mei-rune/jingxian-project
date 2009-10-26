
# include "pro_config.h"
# include <windows.h>
# include "jingxian/lastError.h"
# include "jingxian/utilities/NTManager.h"

_jingxian_begin

NTManager::NTManager( )
: logger_(logging::makeLogger(_T("NTManager")))
{
}


NTManager::~NTManager()
{
	if(null_ptr == logger_)
		return;

	delete logger_;
	logger_ = null_ptr;
}

int NTManager::installService(const tstring& name
								, const tstring& display
								, const tstring& description
								, const tstring& executable
								, const std::vector<tstring>& args)
{
	tstring disp, exec;

    disp = display;
    if(disp.empty())
    {
        disp = name;
    }

    exec = executable;
    if(exec.empty())
    {
        //
        // 使用本执行文件
        //
        tchar buf[_MAX_PATH];
        if(GetModuleFileName(NULL, buf, _MAX_PATH) == 0)
        {
            LOG_ERROR(logger_, _T("安装服务 '") << name <<_T("' 失败 - 没有执行文件名!") );
            return -1;
        }
        exec = buf;
    }

    //
    // 如果有空格的话加上引号
    //
	tstring command;
	if(executable.find( _T(' ')) != tstring::npos)
    {
        command.push_back( _T('"'));
        command.append(exec);
        command.push_back( _T('"'));
    }
    else
    {
        command = exec;
    }

	//
	// 拼上选项字符串
	//
	for(std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
        command.push_back( _T(' '));

		if(p->find_first_of( _T(" \t\n\r") ) != tstring::npos)
        {
            command.push_back( _T('"'));
            command.append(*p);
            command.push_back( _T('"'));
        }
        else
        {
            command.append(*p);
        }
    }

    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
		LOG_ERROR(logger_,  _T("安装服务 '") << name <<_T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()));
        return -1;
    }
    SC_HANDLE hService = CreateService(
        hSCM,
        name.c_str(),
        disp.c_str(),
        SC_MANAGER_ALL_ACCESS,
        SERVICE_WIN32_OWN_PROCESS,
        SERVICE_AUTO_START,
        SERVICE_ERROR_NORMAL,
        command.c_str(),
        NULL,
        NULL,
        NULL,
        NULL,
        NULL);

    if(hService == NULL)
    {
        LOG_ERROR(logger_, _T("安装服务 '") << name <<_T("' 失败,不能创服务实例 - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

	if(!description.empty())
	{
		SERVICE_DESCRIPTION descr;
		descr.lpDescription = (tchar*)description.c_str();
		if(!ChangeServiceConfig2(hService,SERVICE_CONFIG_DESCRIPTION,&descr))
		{
			LOG_WARN(logger_, _T("安装服务 '") << name <<_T("' 时,添加描述失败 - ") << ::lastError(GetLastError()));
		}
	}

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

    LOG_DEBUG(logger_, _T("安装服务 '") << name <<_T("' 成功"));
    return 0;
}

int NTManager::uninstallService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
        LOG_ERROR(logger_, _T("卸载服务 '") << name <<_T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        LOG_ERROR(logger_, _T("卸载服务 '") << name <<_T("' 失败,不能打开服务 - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    BOOL b = DeleteService(hService);

    if(!b)
    {
        LOG_ERROR(logger_, _T("卸载服务 '") << name <<_T("' 失败,不能删除服务 - ") << ::lastError(GetLastError()));
    }
	else
	{
		LOG_DEBUG(logger_, _T("卸载服务 '") << name <<_T("' 成功"));
	}

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

	return ( b?0:-1);
}

int NTManager::startService(const tstring& name, const std::vector<tstring>& args)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
        LOG_ERROR(logger_, _T("启动服务 '") << name <<_T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        LOG_ERROR(logger_, _T("启动服务 '") << name <<_T("' 失败,不能打开服务 - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    //
	// 将字符串拼成 char* []形式
    //
    const size_t argc = args.size();
    const char** argv = new const char*[argc];
    size_t i = 0;
	for(std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
		argv[i++] = string_traits<char>::strdup( toNarrowString(*p).c_str());
    }

    //
    // 启动服务
    //
    BOOL b = StartServiceA(hService, ( DWORD )argc, argv);

    //
    // 释放内存
    //
    for(i = 0; i < argc; ++i)
    {
        string_traits<char>::free((char*)(argv[i]));
    }
    delete[] argv;

    if(!b)
    {
        LOG_ERROR(logger_, _T("启动服务 '") << name <<_T("' 失败 - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

	LOG_TRACE( logger_, _T("服务正在启动中,请稍等...") );

	SERVICE_STATUS status;
	if(!waitForServiceState(hService, SERVICE_RUNNING, status))
	{
		LOG_ERROR(logger_, _T("启动服务 '") << name <<_T("' 失败, 检测服务状态发生错误 - ") << ::lastError(GetLastError()));
		CloseServiceHandle(hService);
		CloseServiceHandle(hSCM);
		return -1;
	}

	CloseServiceHandle(hService);
	CloseServiceHandle(hSCM);

	if(status.dwCurrentState == SERVICE_RUNNING)
	{
		LOG_CRITICAL( logger_, _T("启动服务 '") << name <<_T("' 成功, 服务运行中."));
	}
	else
	{
		showServiceStatus( _T( "服务器启动发生错误"), status);
		return -1;
	}

	return 0;
}

int NTManager::stopService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
		LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败, 不能打开 SCM - ") << lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
		LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败, 不能打开服务 - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    SERVICE_STATUS status;
    BOOL b = ControlService(hService, SERVICE_CONTROL_STOP, &status);

    if(!b)
    {
		LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败 - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        CloseServiceHandle(hService);
        return -1;
    }

    LOG_TRACE( logger_, _T("服务停止中,请稍等..."));

    ////
    //// 等待服务停止或发生一个错误
    ////
    //if(!waitForServiceState(hService, SERVICE_STOP_PENDING, status))
    //{
    //    LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败,检测服务状态发生错误 - ") << lastError(GetLastError()));
    //    CloseServiceHandle(hService);
    //    CloseServiceHandle(hSCM);
    //    return -1;
    //}

	if(!waitForServiceState(hService, SERVICE_STOPPED, status))
    {
        LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败,检测服务状态发生错误 - ") << lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    CloseServiceHandle(hService);
    CloseServiceHandle(hSCM);

    if(status.dwCurrentState == SERVICE_STOPPED)
    {
		LOG_CRITICAL(logger_, _T("停止服务 '") << name <<_T("' 成功."));
    }
    else
    {
		showServiceStatus(_T( "服务器停止发生错误"), status);
		return -1;
    }

    return 0;
}

bool NTManager::waitForServiceState(SC_HANDLE hService, DWORD pendingState, SERVICE_STATUS& status)
{
    if(!QueryServiceStatus(hService, &status))
    {
	return false;
    }

    //
    // 保存起始tick计数据和初始checkpoint.
    //
    DWORD startTickCount = GetTickCount();
    DWORD oldCheckPoint = status.dwCheckPoint;
	int tries = 60;

    //
    // 轮询服务状态
    //
    while(status.dwCurrentState != pendingState)
    {
        //
        // 计算等待时间( 1秒 到 10秒)
        //
		status.dwWaitHint = 1000;
        Sleep(1000);

        //
        // 再检测服务状态
        //
        if(!QueryServiceStatus(hService, &status))
        {
            return false;
        }

        if(status.dwCheckPoint > oldCheckPoint)
        {
            //
            // 服务前进一步了
            //
            startTickCount = GetTickCount();
            oldCheckPoint = status.dwCheckPoint;
			tries = 60;
        }
        else if( --tries < 0)
		{
			break;
		}
    }

    return true;
}

void NTManager::showServiceStatus(const tstring& msg, SERVICE_STATUS& status)
{
	tstring state;
    switch(status.dwCurrentState)
    {
    case SERVICE_STOPPED:
	state = _T( "已停止");
	break;
    case SERVICE_START_PENDING:
	state = _T( "启动中");
	break;
    case SERVICE_STOP_PENDING:
	state = _T( "停止中");
	break;
    case SERVICE_RUNNING:
	state = _T( "运行中");
	break;
    case SERVICE_CONTINUE_PENDING:
	state = _T( "恢复中");
	break;
    case SERVICE_PAUSE_PENDING:
	state = _T( "暂停中");
	break;
    case SERVICE_PAUSED:
	state = _T( "已暂停");
	break;
    default:
	state = _T("未知");
	break;
    }

   
   LOG_CRITICAL(logger_, msg << _T( ",")
	 << _T("  当前状态: " ) << state << _T( ",")
	 << _T("  退出代码: " ) << status.dwWin32ExitCode << _T( ",")
	 << _T("  服务退出代码: " ) << status.dwServiceSpecificExitCode << _T( ",")
	 << _T("  检测点: " ) << status.dwCheckPoint << _T( ",")
	 << _T("  等待次数据: " ) << status.dwWaitHint );
}

_jingxian_end