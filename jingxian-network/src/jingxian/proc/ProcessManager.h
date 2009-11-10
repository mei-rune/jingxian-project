
#ifndef _ProcessManager_H_
#define _ProcessManager_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <vector>
#include "jingxian/string/string.h"
#include "jingxian/logging/logging.h"

_jingxian_begin

class Process
{
public:
	Process(const tstring& filePath, int argc, tchar *argv[])
		: FilePath(filePath)
	{
		for (int i = 0; i < argc; i++) 
		{
			CommandLine.push_back(_T('"'));
			CommandLine.insert(CommandLine.end()
				, argv[i]
				, argv[i] + string_traits<tchar>::strlen(argv[i]));
			CommandLine.push_back(_T('"'));
			CommandLine.push_back(_T(' '));
		}

		memset(&StartInfo, 0, sizeof(STARTUPINFO));
		memset(&ProcessInfo, 0, sizeof(PROCESS_INFORMATION));
		ProcessInfo.hProcess =  INVALID_HANDLE_VALUE;
		ProcessInfo.hThread =  INVALID_HANDLE_VALUE;
	}

	virtual ~Process()
	{
		if(INVALID_HANDLE_VALUE != ProcessInfo.hProcess)
		{
			CloseHandle(ProcessInfo.hProcess);
			ProcessInfo.hProcess = INVALID_HANDLE_VALUE;
		}
		if(INVALID_HANDLE_VALUE != ProcessInfo.hThread)
		{
			CloseHandle(ProcessInfo.hThread);
			ProcessInfo.hThread = INVALID_HANDLE_VALUE;
		}
	}

	bool start()
	{
		return TRUE == CreateProcess(FilePath.c_str()
					, &CommandLine[0]
					, NULL
					, NULL
					, 0
					, DETACHED_PROCESS
					, NULL
					, NULL
					, &StartInfo
					, &ProcessInfo);

	}

	bool stop()
	{
		if(INVALID_HANDLE_VALUE == ProcessInfo.hProcess)
			return true;

		return TRUE == TerminateProcess(ProcessInfo.hProcess, 0);
	}

	PROCESS_INFORMATION	ProcessInfo;

private:
	NOCOPY(Process);

	STARTUPINFO StartInfo;
	tstring FilePath;
	std::vector<tchar> CommandLine;
};

typedef std::list<Process*> ProcessList;

class ProcessManager
{
public:

	ProcessManager(void);

	virtual ~ProcessManager(void);

	/**
	 * 启动一个进程
	 */
	HANDLE startProcess(const tstring& filePath, int argc, tchar *argv[]);

	/**
	 * 停止一个进程
	 */
	void stopProcess(HANDLE handle);

	/**
	 * 检测进程列表
	 */
	void poll(DWORD seconds);

	ProcessList::const_iterator begin() const;

	ProcessList::const_iterator end() const;
	
	ProcessList::iterator begin();

	ProcessList::iterator end();

private:
	NOCOPY(ProcessManager);

	ProcessList processes_;
	
	logging::logger logger_;
};

_jingxian_end

#endif //_ProcessManager_H_