
# include "pro_config.h"
# include <Winsock2.h>
# include <Ws2tcpip.h>
# include "jingxian/networks/commands/CreateProcessCommand.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/threading/thread.h"


_jingxian_begin

CreateProcessCommand::CreateProcessCommand(IOCPServer* core
                               , const tstring& fileName
                               , const tstring& cmdLine
                               , OnBuildConnectionComplete onComplete
                               , OnBuildConnectionError onError
                               , void* context)
        : core_(core)
        , fileName_(fileName)
        , commandLine_(cmdLine)
        , onComplete_(onComplete)
        , onError_(onError)
        , context_(context)
{
}

CreateProcessCommand::~CreateProcessCommand()
{
}

bool CreateProcessCommand::execute()
{
	//HANDLE hInputRD = NULL;
	//HANDLE hInputWR = NULL;
	//HANDLE hOutputRD = NULL;
	//HANDLE hOutputWR = NULL;

	//PROCESS_INFORMATION	ProcessInfo;
	//STARTUPINFO startInfo;
	//std::vector<tchar> CommandLine(commandLine_.begin(),commandLine_.end());

	//memset(&startInfo, 0, sizeof(STARTUPINFO));
	//memset(&ProcessInfo, 0, sizeof(PROCESS_INFORMATION));
	//ProcessInfo.hProcess =  INVALID_HANDLE_VALUE;
	//ProcessInfo.hThread =  INVALID_HANDLE_VALUE;

	//SECURITY_ATTRIBUTES saAttr; 
	//saAttr.nLength = sizeof(SECURITY_ATTRIBUTES); 
	//saAttr.bInheritHandle = TRUE; 
	//saAttr.lpSecurityDescriptor = NULL; 

	//if (!CreatePipe(&hInputRD, &hInputWR, &saAttr, 0))
	//{
	//	errcode_t code = GetLastError();
 //       ErrorCode err(code, concat<tstring>(_T("创建管道失败 - ")
 //                     , lastError(code)));
 //       onError_(err, context_);
 //       return true;
	//}

	//if (!CreatePipe(&hOutputRD, &hOutputWR, &saAttr, 0))
	//{
	//	errcode_t code = GetLastError();
 //       ErrorCode err(code, concat<tstring>(_T("创建管道失败 - ")
 //                     , lastError(code)));
 //       onError_(err, context_);
 //       return true;
	//}

	//if (!SetHandleInformation(hOutputRD, HANDLE_FLAG_INHERIT, 0))
	//{
	//	errcode_t code = GetLastError();
 //       ErrorCode err(code, concat<tstring>(_T("设置管道继承属性失败 - ")
 //                     , lastError(code)));
 //       onError_(err, context_);
 //       return true;
	//}

	//if (!SetHandleInformation(hInputWR, HANDLE_FLAG_INHERIT, 0))
	//{
	//	errcode_t code = GetLastError();
 //       ErrorCode err(code, concat<tstring>(_T("设置管道继承属性失败 - ")
 //                     , lastError(code)));
 //       onError_(err, context_);
 //       return true;
	//}


	//startInfo.hStdError = hOutputWR;
	//startInfo.hStdOutput = hOutputWR;
	//startInfo.hStdInput = hInputRD;
	//startInfo.wShowWindow = 
	//startInfo.dwFlags |= STARTF_USESTDHANDLES|STARTF_USESHOWWINDOW;


	//bool result = TRUE == CreateProcess(fileName_.c_str()
	//	, &CommandLine[0]
	//    , NULL
	//	, NULL
	//	, 0
	//	, DETACHED_PROCESS
	//	, NULL
	//	, NULL
	//	, &startInfo
	//	, &ProcessInfo);

	//errcode_t error = 0; 
	//if(!result)
	//	error = GetLastError();


	//on_complete();

	//
	//CloseHandle(hOutputWR);
	//CloseHandle(hInputRD);
	return true;
}

void CreateProcessCommand::on_complete(size_t bytes_transferred
                                 , bool success
                                 , void *completion_key
                                 , errcode_t error)
{
    
}

_jingxian_end
