
# include "pro_config.h"
# include <windows.h>
# include <winsvc.h>
# include "jingxian/lastError.h"
# include "jingxian/utilities/NTService.h"

_jingxian_begin

#define SERVICE_NAME_SIZE       1024
static  IInstance*              serviceInstance;
static	SERVICE_STATUS		    serviceStatus;
static	SERVICE_STATUS_HANDLE	serviceHandle;
static  TCHAR                   serviceName[SERVICE_NAME_SIZE];
static  SERVICE_TABLE_ENTRY     serviceTable[] = {
		{ NULL, NULL },
		{ NULL, NULL } 
		};

/**
 * ������ƺ���
 */
static DWORD WINAPI serviceCtrlHandler(DWORD dwControl,
                   DWORD dwEventType,
                   LPVOID lpEventData,
                   LPVOID lpContext)
{
	serviceStatus.dwServiceType		= SERVICE_WIN32_OWN_PROCESS;
	serviceStatus.dwCurrentState		= SERVICE_RUNNING;
	serviceStatus.dwControlsAccepted	= SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
	serviceStatus.dwWin32ExitCode		= 0;
	serviceStatus.dwServiceSpecificExitCode	= 0;
	serviceStatus.dwCheckPoint		= 0;
	serviceStatus.dwWaitHint		= 0;

	switch(dwControl)
	{
		case SERVICE_CONTROL_STOP:
		case SERVICE_CONTROL_SHUTDOWN:			
			NT_LOG_CRITICAL(serviceName << _T( "  �����յ�ֹͣ����!"));

			serviceStatus.dwCurrentState	= SERVICE_STOP_PENDING;
			serviceStatus.dwWaitHint	= 4000;
			if(!SetServiceStatus(serviceHandle,&serviceStatus))
				NT_LOG_WARN(serviceName 
					<< _T( "  �������� 'SERVICE_STOP_PENDING' ״̬ʧ�� - ")
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
 * ������ں���
 */
static VOID WINAPI serviceEntry(DWORD argc,LPTSTR *argv)
{
	serviceHandle = RegisterServiceCtrlHandlerEx(serviceName, serviceCtrlHandler, NULL);
	if(0 == serviceHandle)
	{
		NT_LOG_FATAL(serviceName << _T( "  ����ע����ƻص�ʧ�� - ") << lastError(GetLastError()));
		return;
	}
		
	/* �������� */
	serviceStatus.dwServiceType		= SERVICE_WIN32_OWN_PROCESS;
	serviceStatus.dwCurrentState		= SERVICE_START_PENDING;
	serviceStatus.dwControlsAccepted	= SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
	serviceStatus.dwWin32ExitCode		= 0;
	serviceStatus.dwServiceSpecificExitCode	= 0;
	serviceStatus.dwCheckPoint		= 0;
	serviceStatus.dwWaitHint		= 2000;

	if(!SetServiceStatus(serviceHandle, &serviceStatus))
	{
		NT_LOG_WARN(serviceName 
			<< _T( "  �������� 'SERVICE_START_PENDING' ״̬ʧ�� - ")
			<< lastError(GetLastError()));
	}

	/* ���������� */
	serviceStatus.dwCurrentState	= SERVICE_RUNNING;
	serviceStatus.dwWaitHint	= 0;
	if(!SetServiceStatus(serviceHandle, &serviceStatus))
	{
		NT_LOG_WARN(serviceName 
			<< _T( "  �������� 'SERVICE_RUNNING' ״̬ʧ�� - ")
			<< lastError(GetLastError()));
	}

	std::vector<tstring> arguments;
	for(DWORD i = 0; i< argc; ++ i)
	{
		arguments.push_back(argv[i]);
	}
	

	NT_LOG_CRITICAL( serviceName << _T( "  ���������ɹ�������������......"));
	serviceInstance->onRun(arguments);
	NT_LOG_CRITICAL( serviceName << _T( "  �����˳������н���!"));

	serviceStatus.dwCurrentState	= SERVICE_STOPPED;
	serviceStatus.dwWin32ExitCode	= 0;
	serviceStatus.dwCheckPoint	= 0; 
	serviceStatus.dwWaitHint	= 0;
	if(!SetServiceStatus(serviceHandle, &serviceStatus))
		NT_LOG_FATAL(serviceName << _T( "  �������� 'SERVICE_STOPPED' ״̬ʧ�� - ") << lastError(GetLastError()));
}


/**
 * ��������
 */
int serviceMain(IInstance* instance)
{
	if(0 != _tcscpy_s(serviceName, SERVICE_NAME_SIZE, instance->name().c_str()))
	{
		NT_LOG_WARN(_T("��������ʱ��������, ������ '") << instance->name() << _T( "' ̫����"));
		SetLastError(ERROR_INVALID_NAME);
		return -1;
	}

	serviceTable[0].lpServiceName = serviceName;
	serviceTable[0].lpServiceProc = serviceEntry;
	serviceInstance = instance;
	if (StartServiceCtrlDispatcher(serviceTable))
		return 0;

	//if(ERROR_FAILED_SERVICE_CONTROLLER_CONNECT == GetLastError())
	//{
	//	std::vector<tstring> arguments;
	//	for(DWORD i = 0; i< argc; ++ i)
	//	{
	//		arguments.push_back(argv[i]);
	//	}
	//	instance->onRun(arguments);
	//	return 0;
	//}

	return -1;
}

bool  waitForServiceState(SC_HANDLE hService, DWORD pendingState, SERVICE_STATUS& status)
{
	if(!QueryServiceStatus(hService, &status))
		return false;

	// ������ʼtick�����ݺͳ�ʼcheckpoint.
	DWORD startTickCount = GetTickCount();
	DWORD oldCheckPoint = status.dwCheckPoint;
	int tries = 60;

	// ��ѯ����״̬
	while(status.dwCurrentState != pendingState)
	{
		// ����ȴ�ʱ��( 1�� �� 10��)
		status.dwWaitHint = 1000;
		Sleep(1000);

		// �ټ�����״̬
		if(!QueryServiceStatus(hService, &status))
			return false;

		if(status.dwCheckPoint > oldCheckPoint)
		{
			// ����ǰ��һ����
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

void  showServiceStatus(const tstring& msg, SERVICE_STATUS& status)
{
	tstring state;
	switch(status.dwCurrentState)
	{
	case SERVICE_STOPPED:
		state = _T( "��ֹͣ");
		break;
	case SERVICE_START_PENDING:
		state = _T( "������");
		break;
	case SERVICE_STOP_PENDING:
		state = _T( "ֹͣ��");
		break;
	case SERVICE_RUNNING:
		state = _T( "������");
		break;
	case SERVICE_CONTINUE_PENDING:
		state = _T( "�ָ���");
		break;
	case SERVICE_PAUSE_PENDING:
		state = _T( "��ͣ��");
		break;
	case SERVICE_PAUSED:
		state = _T( "����ͣ");
		break;
	default:
		state = _T("δ֪");
		break;
	}

	NT_LOG_CRITICAL(msg << _T( ",")
		<< _T("  ��ǰ״̬: " ) << state << _T( ",")
		<< _T("  �˳�����: " ) << status.dwWin32ExitCode << _T( ",")
		<< _T("  �����˳�����: " ) << status.dwServiceSpecificExitCode << _T( ",")
		<< _T("  ����: " ) << status.dwCheckPoint << _T( ",")
		<< _T("  �ȴ�������: " ) << status.dwWaitHint );
}

int  installService(const tstring& name
								, const tstring& display
								, const tstring& description
								, const tstring& executable
								, const std::vector<tstring>& args)
{
	if(name.size()>=254)
	{
		NT_LOG_ERROR(_T("��װ���� '") << name <<_T("' ʧ�� - ������̫��!") );
		return -1;
	}

	tstring disp = display;
    if(disp.empty())
    {
        disp = name;
    }

    tstring exec = executable;
    if(exec.empty())
    {
        // ʹ�ñ�ִ���ļ�
        tchar buf[_MAX_PATH];
        if(GetModuleFileName(NULL, buf, _MAX_PATH) == 0)
        {
            NT_LOG_ERROR(_T("��װ���� '") << name <<_T("' ʧ�� - û��ִ���ļ���!") );
            return -1;
        }
        exec = buf;
    }

    // ����пո�Ļ���������
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

	// ƴ��ѡ���ַ���
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
		NT_LOG_ERROR(_T("��װ���� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
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
        NT_LOG_ERROR(_T("��װ���� '") << name <<_T("' ʧ��,���ܴ�����ʵ�� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

	if(!description.empty())
	{
		SERVICE_DESCRIPTION descr;
		descr.lpDescription = (tchar*)description.c_str();
		if(!ChangeServiceConfig2(hService,SERVICE_CONFIG_DESCRIPTION,&descr))
		{
			NT_LOG_WARN(_T("��װ���� '") << name <<_T("' ʱ,�������ʧ�� - ") << ::lastError(GetLastError()));
		}
	}

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

    NT_LOG_CRITICAL(_T("��װ���� '") << name <<_T("' �ɹ�"));
    return 0;
}

int  uninstallService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
        NT_LOG_ERROR(_T("ж�ط��� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        NT_LOG_ERROR(_T("ж�ط��� '") << name <<_T("' ʧ��,���ܴ򿪷��� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    BOOL b = DeleteService(hService);

    if(!b)
    {
        NT_LOG_ERROR(_T("ж�ط��� '") << name <<_T("' ʧ��,����ɾ������ - ") << ::lastError(GetLastError()));
    }
	else
	{
		NT_LOG_CRITICAL(_T("ж�ط��� '") << name <<_T("' �ɹ�"));
	}

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

	return ( b?0:-1);
}

int  startService(const tstring& name, const std::vector<tstring>& args)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
        NT_LOG_ERROR(_T("�������� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        NT_LOG_ERROR(_T("�������� '") << name <<_T("' ʧ��,���ܴ򿪷��� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

	// ���ַ���ƴ�� char* []��ʽ
    const size_t argc = args.size();
    const tchar** argv = new const tchar*[argc];
    size_t i = 0;
	for(std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
		argv[i++] = string_traits<tchar>::strdup(p->c_str());
    }

    // ��������
    BOOL b = StartService(hService, ( DWORD )argc, argv);

    // �ͷ��ڴ�
    for(i = 0; i < argc; ++i)
    {
        string_traits<tchar>::free((tchar*)argv[i]);
    }
    delete[] argv;

    if(!b)
    {
        NT_LOG_ERROR(_T("�������� '") << name <<_T("' ʧ�� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

	NT_LOG_TRACE(_T("��������������,���Ե�...") );

	SERVICE_STATUS status;
	if(!waitForServiceState(hService, SERVICE_RUNNING, status))
	{
		NT_LOG_ERROR(_T("�������� '") << name <<_T("' ʧ��, ������״̬�������� - ") << ::lastError(GetLastError()));
		CloseServiceHandle(hService);
		CloseServiceHandle(hSCM);
		return -1;
	}

	CloseServiceHandle(hService);
	CloseServiceHandle(hSCM);

	if(status.dwCurrentState == SERVICE_RUNNING)
	{
		NT_LOG_CRITICAL(_T("�������� '") << name <<_T("' �ɹ�, ����������."));
	}
	else
	{
		showServiceStatus(_T( "������������������"), status);
		return -1;
	}

	return 0;
}

int  stopService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
		NT_LOG_ERROR(_T("ֹͣ���� '") << name <<_T("' ʧ��, ���ܴ� SCM - ") << lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
		NT_LOG_ERROR(_T("ֹͣ���� '") << name <<_T("' ʧ��, ���ܴ򿪷��� - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    SERVICE_STATUS status;
    BOOL b = ControlService(hService, SERVICE_CONTROL_STOP, &status);

    if(!b)
    {
		NT_LOG_ERROR(_T("ֹͣ���� '") << name <<_T("' ʧ�� - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        CloseServiceHandle(hService);
        return -1;
    }

    NT_LOG_TRACE(_T("����ֹͣ��,���Ե�..."));

    ////
    //// �ȴ�����ֹͣ����һ������
    ////
    //if(!waitForServiceState(hService, SERVICE_STOP_PENDING, status))
    //{
    //    LOG_ERROR(logger_, _T("ֹͣ���� '") << name <<_T("' ʧ��,������״̬�������� - ") << lastError(GetLastError()));
    //    CloseServiceHandle(hService);
    //    CloseServiceHandle(hSCM);
    //    return -1;
    //}

	if(!waitForServiceState(hService, SERVICE_STOPPED, status))
    {
        NT_LOG_ERROR(_T("ֹͣ���� '") << name <<_T("' ʧ��,������״̬�������� - ") << lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    CloseServiceHandle(hService);
    CloseServiceHandle(hSCM);

    if(status.dwCurrentState == SERVICE_STOPPED)
    {
		NT_LOG_CRITICAL(_T("ֹͣ���� '") << name <<_T("' �ɹ�."));
    }
    else
    {
		showServiceStatus(_T( "������ֹͣ��������"), status);
		return -1;
    }

	return 0;
}

_jingxian_end