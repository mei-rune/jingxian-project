
# include "pro_config.h"
# include <windows.h>
# include <winsvc.h>
# include "jingxian/lastError.h"
# include "jingxian/logging/logging.h"
# include "jingxian/utilities/NTService.h"

_jingxian_begin

#define SERVICE_NAME_SIZE       1024

static  logging::logger*         serviceLogger;
static  IInstance*              serviceInstance;
static  SERVICE_STATUS          serviceStatus;
static  SERVICE_STATUS_HANDLE   serviceHandle;
static  TCHAR                   serviceName[SERVICE_NAME_SIZE];
static  SERVICE_TABLE_ENTRY     serviceTable[] =
	{
		{ NULL, NULL },
		{ NULL, NULL }
	};


/**
 * 服务控制函数
 */
static DWORD WINAPI serviceCtrlHandler(DWORD dwControl,
                                       DWORD dwEventType,
                                       LPVOID lpEventData,
                                       LPVOID lpContext)
{
    serviceStatus.dwServiceType   = SERVICE_WIN32_OWN_PROCESS;
    serviceStatus.dwCurrentState    = SERVICE_RUNNING;
    serviceStatus.dwControlsAccepted  = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
    serviceStatus.dwWin32ExitCode   = 0;
    serviceStatus.dwServiceSpecificExitCode = 0;
    serviceStatus.dwCheckPoint    = 0;
    serviceStatus.dwWaitHint    = 0;

    switch (dwControl)
    {
    case SERVICE_CONTROL_STOP:
    case SERVICE_CONTROL_SHUTDOWN:
        //LOG_CRITICAL((*serviceLogger), serviceName << _T( "  服务收到停止请求!"));

        serviceStatus.dwCurrentState  = SERVICE_STOP_PENDING;
        serviceStatus.dwWaitHint  = 4000;
        if (!SetServiceStatus(serviceHandle, &serviceStatus))
            LOG_WARN((*serviceLogger), serviceName
                     << _T("  服务设置 'SERVICE_STOP_PENDING' 状态失败 - ")
                     << lastError(GetLastError()));

        serviceInstance->interrupt();
        break;
    default:
        serviceInstance->onControl(dwEventType, lpEventData);
        break;
    }
    return NO_ERROR;
}

/**
 * 服务入口函数
 */
static VOID WINAPI serviceEntry(DWORD argc, LPTSTR *argv)
{
    serviceHandle = RegisterServiceCtrlHandlerEx(serviceName, serviceCtrlHandler, NULL);
    if (0 == serviceHandle)
    {
        LOG_FATAL((*serviceLogger), serviceName << _T("  服务注册控制回调失败 - ") << lastError(GetLastError()));
        return;
    }

    /* 启动服务 */
    serviceStatus.dwServiceType   = SERVICE_WIN32_OWN_PROCESS;
    serviceStatus.dwCurrentState    = SERVICE_START_PENDING;
    serviceStatus.dwControlsAccepted  = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
    serviceStatus.dwWin32ExitCode   = 0;
    serviceStatus.dwServiceSpecificExitCode = 0;
    serviceStatus.dwCheckPoint    = 0;
    serviceStatus.dwWaitHint    = 2000;

    if (!SetServiceStatus(serviceHandle, &serviceStatus))
    {
        LOG_WARN((*serviceLogger), serviceName
                 << _T("  服务设置 'SERVICE_START_PENDING' 状态失败 - ")
                 << lastError(GetLastError()));
    }

    /* 服务启行中 */
    serviceStatus.dwCurrentState  = SERVICE_RUNNING;
    serviceStatus.dwWaitHint  = 0;
    if (!SetServiceStatus(serviceHandle, &serviceStatus))
    {
        LOG_WARN((*serviceLogger), serviceName
                 << _T("  服务设置 'SERVICE_RUNNING' 状态失败 - ")
                 << lastError(GetLastError()));
    }

    std::vector<tstring> arguments;
    for (DWORD i = 0; i < argc; ++ i)
    {
        arguments.push_back(argv[i]);
    }


    //LOG_CRITICAL((*serviceLogger),  serviceName << _T( "  服务启动成功，正在运行中......"));
    serviceInstance->onRun(arguments);
    //LOG_CRITICAL((*serviceLogger),  serviceName << _T( "  服务退出，运行结束!"));

    serviceStatus.dwCurrentState  = SERVICE_STOPPED;
    serviceStatus.dwWin32ExitCode = 0;
    serviceStatus.dwCheckPoint  = 0;
    serviceStatus.dwWaitHint  = 0;
    if (!SetServiceStatus(serviceHandle, &serviceStatus))
        LOG_FATAL((*serviceLogger), serviceName << _T("  服务设置 'SERVICE_STOPPED' 状态失败 - ") << lastError(GetLastError()));
}


/**
 * 启动服务
 */
int serviceMain(IInstance* instance)
{
    int result = -1;
    if (NULL == serviceLogger)
        serviceLogger = new logging::logger(_T("jingxian.system"));

    if (0 != _tcscpy_s(serviceName, SERVICE_NAME_SIZE, instance->name().c_str()))
    {
        LOG_WARN((*serviceLogger), _T("启动服务时发生错误, 服务名 '") << instance->name() << _T("' 太长。"));
        SetLastError(ERROR_INVALID_NAME);

        result = -1;
        goto end;
    }

    serviceTable[0].lpServiceName = serviceName;
    serviceTable[0].lpServiceProc = serviceEntry;
    serviceInstance = instance;
    if (StartServiceCtrlDispatcher(serviceTable))
    {
        result = 0;
        goto end;
    }

    //if(ERROR_FAILED_SERVICE_CONTROLLER_CONNECT == GetLastError())
    //{
    //  std::vector<tstring> arguments;
    //  for(DWORD i = 0; i< argc; ++ i)
    //  {
    //    arguments.push_back(argv[i]);
    //  }
    //  instance->onRun(arguments);
    //
    //  result = 0;
    //  goto end;
    //}

end:
    if (NULL != serviceLogger)
        delete serviceLogger;
    serviceLogger = NULL;
    return result;
}

bool  waitForServiceState(SC_HANDLE hService, DWORD pendingState, SERVICE_STATUS& status)
{
    if (!QueryServiceStatus(hService, &status))
        return false;

    // 保存起始tick计数据和初始checkpoint.
    DWORD startTickCount = GetTickCount();
    DWORD oldCheckPoint = status.dwCheckPoint;
    int tries = 60;

    // 轮询服务状态
    while (status.dwCurrentState != pendingState)
    {
        // 计算等待时间( 1秒 到 10秒)
        status.dwWaitHint = 1000;
        Sleep(1000);

        // 再检测服务状态
        if (!QueryServiceStatus(hService, &status))
            return false;

        if (status.dwCheckPoint > oldCheckPoint)
        {
            // 服务前进一步了
            startTickCount = GetTickCount();
            oldCheckPoint = status.dwCheckPoint;
            tries = 60;
        }
        else if (--tries < 0)
        {
            break;
        }
    }

    return true;
}

void  showServiceStatus(const tstring& msg, SERVICE_STATUS& status, tostream& out)
{
    out << _T("CRIT ") <<  msg << _T(",")
    << _T("  当前状态: ");

    switch (status.dwCurrentState)
    {
    case SERVICE_STOPPED:
        out << _T("已停止");
        break;
    case SERVICE_START_PENDING:
        out << _T("启动中");
        break;
    case SERVICE_STOP_PENDING:
        out << _T("停止中");
        break;
    case SERVICE_RUNNING:
        out << _T("运行中");
        break;
    case SERVICE_CONTINUE_PENDING:
        out << _T("恢复中");
        break;
    case SERVICE_PAUSE_PENDING:
        out << _T("暂停中");
        break;
    case SERVICE_PAUSED:
        out << _T("已暂停");
        break;
    default:
        out << _T("未知");
        break;
    }
    out << _T(",")
    << _T("  退出代码: ") << status.dwWin32ExitCode << _T(",")
    << _T("  服务退出代码: ") << status.dwServiceSpecificExitCode << _T(",")
    << _T("  检测点: ") << status.dwCheckPoint << _T(",")
    << _T("  等待次数据: ") << status.dwWaitHint << std::endl;
}

int  installService(const tstring& name
                    , const tstring& display
                    , const tstring& description
                    , const tstring& executable
                    , const std::vector<tstring>& args
                    , tostream& out)
{
    if (name.size() >= 254)
    {
        out << _T("安装服务 '") << name << _T("' 失败 - 服务名太长!") << std::endl;
        return -1;
    }

    tstring disp = display;
    if (disp.empty())
    {
        disp = name;
    }

    tstring exec = executable;
    if (exec.empty())
    {
        // 使用本执行文件
        tchar buf[_MAX_PATH];
        if (GetModuleFileName(NULL, buf, _MAX_PATH) == 0)
        {
            out << _T("ERROR 安装服务 '") << name << _T("' 失败 - 没有执行文件名!") << std::endl;
            return -1;
        }
        exec = buf;
    }

    // 如果有空格的话加上引号
    tstring command;
    if (executable.find(_T(' ')) != tstring::npos)
    {
        command.push_back(_T('"'));
        command.append(exec);
        command.push_back(_T('"'));
    }
    else
    {
        command = exec;
    }

    // 拼上选项字符串
    for (std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
        command.push_back(_T(' '));

        if (p->find_first_of(_T(" \t\n\r")) != tstring::npos)
        {
            command.push_back(_T('"'));
            command.append(*p);
            command.push_back(_T('"'));
        }
        else
        {
            command.append(*p);
        }
    }

    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if (hSCM == NULL)
    {
        out << _T("ERROR 安装服务 '") << name << _T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()) << std::endl;
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

    if (hService == NULL)
    {
        out << _T("ERROR 安装服务 '") << name << _T("' 失败,不能创服务实例 - ") << ::lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hSCM);
        return -1;
    }

    if (!description.empty())
    {
        SERVICE_DESCRIPTION descr;
        descr.lpDescription = (tchar*)description.c_str();
        if (!ChangeServiceConfig2(hService, SERVICE_CONFIG_DESCRIPTION, &descr))
        {
            out << _T("WARN 安装服务 '") << name << _T("' 时,添加描述失败 - ") << ::lastError(GetLastError()) << std::endl;
        }
    }

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

    out << _T("CRIT 安装服务 '") << name << _T("' 成功") << std::endl;
    return 0;
}

int  uninstallService(const tstring& name, tostream& out)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if (hSCM == NULL)
    {
        out << _T("ERROR 卸载服务 '") << name << _T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()) << std::endl;
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if (hService == NULL)
    {
        out << _T("ERROR 卸载服务 '") << name << _T("' 失败,不能打开服务 - ") << ::lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hSCM);
        return -1;
    }

    BOOL b = DeleteService(hService);

    if (!b)
    {
        out << _T("ERROR 卸载服务 '") << name << _T("' 失败,不能删除服务 - ") << ::lastError(GetLastError()) << std::endl;
    }
    else
    {
        out << _T("CRIT 卸载服务 '") << name << _T("' 成功") << std::endl;
    }

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

    return (b ? 0 : -1);
}

int  startService(const tstring& name, const std::vector<tstring>& args, tostream& out)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if (hSCM == NULL)
    {
        out << _T("ERROR 启动服务 '") << name << _T("' 失败,不能打开 SCM - ") << ::lastError(GetLastError()) << std::endl;
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if (hService == NULL)
    {
        out << _T("ERROR 启动服务 '") << name << _T("' 失败,不能打开服务 - ") << ::lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hSCM);
        return -1;
    }

    // 将字符串拼成 char* []形式
    const size_t argc = args.size();
    const tchar** argv = new const tchar*[argc];
    size_t i = 0;
    for (std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
        argv[i++] = string_traits<tchar>::strdup(p->c_str());
    }

    // 启动服务
    BOOL b = StartService(hService, (DWORD)argc, argv);

    // 释放内存
    for (i = 0; i < argc; ++i)
    {
        string_traits<tchar>::free((tchar*)argv[i]);
    }
    delete[] argv;

    if (!b)
    {
        out << _T("ERROR 启动服务 '") << name << _T("' 失败 - ") << ::lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    out << _T("TRACE 服务正在启动中,请稍等...") << std::endl;

    SERVICE_STATUS status;
    if (!waitForServiceState(hService, SERVICE_RUNNING, status))
    {
        out << _T("ERROR 启动服务 '") << name << _T("' 失败, 检测服务状态发生错误 - ") << ::lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    CloseServiceHandle(hService);
    CloseServiceHandle(hSCM);

    if (status.dwCurrentState == SERVICE_RUNNING)
    {
        out << _T("CRIT 启动服务 '") << name << _T("' 成功, 服务运行中.") << std::endl;
    }
    else
    {
        showServiceStatus(_T("服务器启动发生错误"), status, out);
        return -1;
    }

    return 0;
}

int  stopService(const tstring& name, tostream& out)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if (hSCM == NULL)
    {
        out << _T("ERROR 停止服务 '") << name << _T("' 失败, 不能打开 SCM - ") << lastError(GetLastError()) << std::endl;
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if (hService == NULL)
    {
        out << _T("ERROR 停止服务 '") << name << _T("' 失败, 不能打开服务 - ") << lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hSCM);
        return -1;
    }

    SERVICE_STATUS status;
    BOOL b = ControlService(hService, SERVICE_CONTROL_STOP, &status);

    if (!b)
    {
        out << _T("ERROR 停止服务 '") << name << _T("' 失败 - ") << lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hSCM);
        CloseServiceHandle(hService);
        return -1;
    }

    out << _T("TRACE 服务停止中,请稍等...") << std::endl;

    ////
    //// 等待服务停止或发生一个错误
    ////
    //if(!waitForServiceState(hService, SERVICE_STOP_PENDING, status))
    //{
    //    LOG_ERROR(logger_, _T("停止服务 '") << name <<_T("' 失败,检测服务状态发生错误 - ") << lastError(GetLastError()) << std::endl;
    //    CloseServiceHandle(hService);
    //    CloseServiceHandle(hSCM);
    //    return -1;
    //}

    if (!waitForServiceState(hService, SERVICE_STOPPED, status))
    {
        out << _T("ERROR 停止服务 '") << name << _T("' 失败,检测服务状态发生错误 - ") << lastError(GetLastError()) << std::endl;
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    CloseServiceHandle(hService);
    CloseServiceHandle(hSCM);

    if (status.dwCurrentState == SERVICE_STOPPED)
    {
        out << _T("CRIT 停止服务 '") << name << _T("' 成功.") << std::endl;
    }
    else
    {
        showServiceStatus(_T("服务器停止发生错误"), status, out);
        return -1;
    }

    return 0;
}

_jingxian_end