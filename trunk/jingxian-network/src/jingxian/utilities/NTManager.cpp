
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
        // ʹ�ñ�ִ���ļ�
        //
        tchar buf[_MAX_PATH];
        if(GetModuleFileName(NULL, buf, _MAX_PATH) == 0)
        {
            LOG_ERROR(logger_, _T("��װ���� '") << name <<_T("' ʧ�� - û��ִ���ļ���!") );
            return -1;
        }
        exec = buf;
    }

    //
    // ����пո�Ļ���������
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
	// ƴ��ѡ���ַ���
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
		LOG_ERROR(logger_,  _T("��װ���� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
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
        LOG_ERROR(logger_, _T("��װ���� '") << name <<_T("' ʧ��,���ܴ�����ʵ�� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

	if(!description.empty())
	{
		SERVICE_DESCRIPTION descr;
		descr.lpDescription = (tchar*)description.c_str();
		if(!ChangeServiceConfig2(hService,SERVICE_CONFIG_DESCRIPTION,&descr))
		{
			LOG_WARN(logger_, _T("��װ���� '") << name <<_T("' ʱ,�������ʧ�� - ") << ::lastError(GetLastError()));
		}
	}

    CloseServiceHandle(hSCM);
    CloseServiceHandle(hService);

    LOG_DEBUG(logger_, _T("��װ���� '") << name <<_T("' �ɹ�"));
    return 0;
}

int NTManager::uninstallService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
        LOG_ERROR(logger_, _T("ж�ط��� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        LOG_ERROR(logger_, _T("ж�ط��� '") << name <<_T("' ʧ��,���ܴ򿪷��� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    BOOL b = DeleteService(hService);

    if(!b)
    {
        LOG_ERROR(logger_, _T("ж�ط��� '") << name <<_T("' ʧ��,����ɾ������ - ") << ::lastError(GetLastError()));
    }
	else
	{
		LOG_DEBUG(logger_, _T("ж�ط��� '") << name <<_T("' �ɹ�"));
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
        LOG_ERROR(logger_, _T("�������� '") << name <<_T("' ʧ��,���ܴ� SCM - ") << ::lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
        LOG_ERROR(logger_, _T("�������� '") << name <<_T("' ʧ��,���ܴ򿪷��� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    //
	// ���ַ���ƴ�� char* []��ʽ
    //
    const size_t argc = args.size();
    const char** argv = new const char*[argc];
    size_t i = 0;
	for(std::vector<tstring>::const_iterator p = args.begin(); p != args.end(); ++p)
    {
		argv[i++] = string_traits<char>::strdup( toNarrowString(*p).c_str());
    }

    //
    // ��������
    //
    BOOL b = StartServiceA(hService, ( DWORD )argc, argv);

    //
    // �ͷ��ڴ�
    //
    for(i = 0; i < argc; ++i)
    {
        string_traits<char>::free((char*)(argv[i]));
    }
    delete[] argv;

    if(!b)
    {
        LOG_ERROR(logger_, _T("�������� '") << name <<_T("' ʧ�� - ") << ::lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

	LOG_TRACE( logger_, _T("��������������,���Ե�...") );

	SERVICE_STATUS status;
	if(!waitForServiceState(hService, SERVICE_RUNNING, status))
	{
		LOG_ERROR(logger_, _T("�������� '") << name <<_T("' ʧ��, ������״̬�������� - ") << ::lastError(GetLastError()));
		CloseServiceHandle(hService);
		CloseServiceHandle(hSCM);
		return -1;
	}

	CloseServiceHandle(hService);
	CloseServiceHandle(hSCM);

	if(status.dwCurrentState == SERVICE_RUNNING)
	{
		LOG_CRITICAL( logger_, _T("�������� '") << name <<_T("' �ɹ�, ����������."));
	}
	else
	{
		showServiceStatus( _T( "������������������"), status);
		return -1;
	}

	return 0;
}

int NTManager::stopService(const tstring& name)
{
    SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
    if(hSCM == NULL)
    {
		LOG_ERROR(logger_, _T("ֹͣ���� '") << name <<_T("' ʧ��, ���ܴ� SCM - ") << lastError(GetLastError()));
        return -1;
    }

    SC_HANDLE hService = OpenService(hSCM, name.c_str(), SC_MANAGER_ALL_ACCESS);
    if(hService == NULL)
    {
		LOG_ERROR(logger_, _T("ֹͣ���� '") << name <<_T("' ʧ��, ���ܴ򿪷��� - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        return -1;
    }

    SERVICE_STATUS status;
    BOOL b = ControlService(hService, SERVICE_CONTROL_STOP, &status);

    if(!b)
    {
		LOG_ERROR(logger_, _T("ֹͣ���� '") << name <<_T("' ʧ�� - ") << lastError(GetLastError()));
        CloseServiceHandle(hSCM);
        CloseServiceHandle(hService);
        return -1;
    }

    LOG_TRACE( logger_, _T("����ֹͣ��,���Ե�..."));

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
        LOG_ERROR(logger_, _T("ֹͣ���� '") << name <<_T("' ʧ��,������״̬�������� - ") << lastError(GetLastError()));
        CloseServiceHandle(hService);
        CloseServiceHandle(hSCM);
        return -1;
    }

    CloseServiceHandle(hService);
    CloseServiceHandle(hSCM);

    if(status.dwCurrentState == SERVICE_STOPPED)
    {
		LOG_CRITICAL(logger_, _T("ֹͣ���� '") << name <<_T("' �ɹ�."));
    }
    else
    {
		showServiceStatus(_T( "������ֹͣ��������"), status);
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
    // ������ʼtick�����ݺͳ�ʼcheckpoint.
    //
    DWORD startTickCount = GetTickCount();
    DWORD oldCheckPoint = status.dwCheckPoint;
	int tries = 60;

    //
    // ��ѯ����״̬
    //
    while(status.dwCurrentState != pendingState)
    {
        //
        // ����ȴ�ʱ��( 1�� �� 10��)
        //
		status.dwWaitHint = 1000;
        Sleep(1000);

        //
        // �ټ�����״̬
        //
        if(!QueryServiceStatus(hService, &status))
        {
            return false;
        }

        if(status.dwCheckPoint > oldCheckPoint)
        {
            //
            // ����ǰ��һ����
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

   
   LOG_CRITICAL(logger_, msg << _T( ",")
	 << _T("  ��ǰ״̬: " ) << state << _T( ",")
	 << _T("  �˳�����: " ) << status.dwWin32ExitCode << _T( ",")
	 << _T("  �����˳�����: " ) << status.dwServiceSpecificExitCode << _T( ",")
	 << _T("  ����: " ) << status.dwCheckPoint << _T( ",")
	 << _T("  �ȴ�������: " ) << status.dwWaitHint );
}

_jingxian_end