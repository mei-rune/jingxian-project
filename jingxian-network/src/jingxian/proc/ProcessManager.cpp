
# include "pro_config.h"
# include "jingxian/proc/ProcessManager.h"

_jingxian_begin
	
ProcessManager::ProcessManager(void)
: logger_(_T("jingxian.system"))
{
}

ProcessManager::~ProcessManager(void)
{
}

HANDLE ProcessManager::startProcess(const tstring& filePath
									, int argc
									, tchar *argv[])
{
	std::auto_ptr<Process> ptr(new Process(filePath, argc, argv));
	if(!ptr->start())
	{
		LOG_ERROR(logger_, _T("启动进程 '") 
			<< filePath
			<< _T("'失败 - ")
			<< lastError(GetLastError()));
		return INVALID_HANDLE_VALUE;
	}

	Process* p = ptr.release();
	processes_.push_back(p);

	
	LOG_ERROR(logger_, _T("启动进程 '") 
			<< filePath
			<< _T("'成功 - PID=")
			<< p->ProcessInfo.dwProcessId
			<< _T("!"));

	return p->ProcessInfo.hProcess;
}

void ProcessManager::stopProcess(HANDLE handle)
{
	for(ProcessList::iterator it=processes_.begin()
		; it != processes_.end(); ++it)
	{
		if(handle != (*it)->ProcessInfo.hProcess)
			continue;
		
		if((*it)->stop())
		{
			LOG_ERROR(logger_, _T("停止进程 'PID=") 
				<< (*it)->ProcessInfo.dwProcessId
				<< _T("' 成功!"));
		}
		else
		{
			LOG_ERROR(logger_, _T("停止进程 'PID=") 
				<< (*it)->ProcessInfo.dwProcessId
				<< _T("' 失败 - ")
				<< lastError(GetLastError()));
		}
		return;
	}
}

void ProcessManager::poll(DWORD seconds)
{
	std::vector<HANDLE> handles;
	for(ProcessList::iterator it=processes_.begin()
		; it != processes_.end(); ++ it)
	{
		handles.push_back((*it)->ProcessInfo.hProcess);
	}

	DWORD ret = WaitForMultipleObjects(handles.size(),&handles[0], FALSE, seconds*1000);
	if (ret == WAIT_OBJECT_0) 
	{
		//proctl_msg_main();
	}
	else if (ret == WAIT_FAILED) 
	{
			//LOG_ERROR(logger_, _T("轮询进程 'PID=") 
			//	<< (*it)->ProcessInfo.dwProcessId
			//	<< _T("' 失败 - ")
			//	<< lastError(GetLastError()));
	}
}

ProcessList::const_iterator ProcessManager::begin() const
{
	return processes_.begin();
}

ProcessList::const_iterator ProcessManager::end() const
{
	return processes_.end();
}
	
ProcessList::iterator ProcessManager::begin()
{
	return processes_.begin();
}

ProcessList::iterator ProcessManager::end()
{
	return processes_.end();
}

_jingxian_end