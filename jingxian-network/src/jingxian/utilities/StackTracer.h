/**********************************************************************
*
* StackTracer.h
*
*
* History:
*  2009-03-23         - Yalon changes:
*           Made this an abstract class (OnOutput is now pure virtual)
*           Added a parameter skipFramesCount to ShowCallstack
*           Added ability to filter out the general info. As part of this, changed the name and meaning of one of the options
*  2005-07-27   v1    - First public release on http://www.codeproject.com/
*  (for additional changes see History in 'StackTracer.cpp'!
*
**********************************************************************/
// #pragma once is supported starting with _MCS_VER 1000,
// so we need not to check the version (because we only support _MSC_VER >= 1100)!

#ifndef _StackTracer_h_
#define _StackTracer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <windows.h>
#include <string>
#include <sstream>

_jingxian_begin

// special defines for VC5/6 (if no actual PSDK is installed):
#if _MSC_VER < 1300
typedef unsigned __int64 DWORD64, *PDWORD64;
#if defined(_WIN64)
typedef unsigned __int64 SIZE_T, *PSIZE_T;
#else
typedef unsigned long SIZE_T, *PSIZE_T;
#endif
#endif  // _MSC_VER < 1300

const int STACKWALK_MAX_NAMELEN = 1024;

typedef struct StackFrame
{
    DWORD64 offset;  // if 0, we have no valid entry
    CHAR name[STACKWALK_MAX_NAMELEN];
    CHAR undName[STACKWALK_MAX_NAMELEN];
    CHAR undFullName[STACKWALK_MAX_NAMELEN];
    DWORD64 offsetFromSmybol;
    DWORD offsetFromLine;
    DWORD lineNumber;
    CHAR lineFileName[STACKWALK_MAX_NAMELEN];
    DWORD symType;
    LPCSTR symTypeString;
    CHAR moduleName[STACKWALK_MAX_NAMELEN];
    DWORD64 baseOfImage;
    CHAR loadedImageName[STACKWALK_MAX_NAMELEN];
} StackFrame;


class StackWalkerInternal;  // forward
class StackTracer
{
public:
    typedef enum StackWalkOptions
    {
        // No addition info will be retrived
        // (only the address is available)
        RetrieveNone = 0,

        // Retrieves general info like: symbol search path, OS-version, symbol search path, DLLs in use
        RetrieveGeneralInfo = 1,

        // Try to get the line for this symbol
        RetrieveLine = 2,

        // Try to retrieve the module-infos
        RetrieveModuleInfo = 4,

        // Also retrieve the version for the DLL/EXE
        RetrieveFileVersion = 8,

        // Contains all the abouve
        RetrieveVerbose = 0xF,

        // Generate a "good" symbol-search-path
        SymBuildPath = 0x10,

        // Also use the public Microsoft-Symbol-Server
        SymUseSymSrv = 0x20,

        // Contains all the abouve "Sym"-options
        SymAll = 0x30,

        // Contains all options (default)
        OptionsAll = 0x3F
    } StackWalkOptions;

    StackTracer(
        int options = OptionsAll, // 'int' is by design, to combine the enum-flags
        LPCSTR szSymPath = NULL,
        DWORD dwProcessId = GetCurrentProcessId(),
        HANDLE hProcess = GetCurrentProcess()
    );
    StackTracer(DWORD dwProcessId, HANDLE hProcess);
    virtual ~StackTracer();

    typedef BOOL (__stdcall *PReadProcessMemoryRoutine)(
        HANDLE      hProcess,
        DWORD64     qwBaseAddress,
        PVOID       lpBuffer,
        DWORD       nSize,
        LPDWORD     lpNumberOfBytesRead,
        LPVOID      pUserData  // optional data, which was passed in "ShowCallstack"
    );

    BOOL LoadModules();

    BOOL ShowCallstack(
        int skipFramesCount = 0,
        HANDLE hThread = GetCurrentThread(),
        const CONTEXT *context = NULL,
        PReadProcessMemoryRoutine readMemoryFunction = NULL,
        LPVOID pUserData = NULL  // optional to identify some data in the 'readMemoryFunction'-callback
    );

    std::string GetCallStack() const
    {
        return m_buffer.str();
    }

#if _MSC_VER >= 1300
// due to some reasons, the "STACKWALK_MAX_NAMELEN" must be declared as "public"
// in older compilers in order to use it... starting with VC7 we can declare it as "protected"
protected:
#endif


protected:
    // Entry for each Callstack-Entry


    typedef enum StackFrameType {firstEntry, nextEntry, lastEntry};

    virtual void OnSymInit(LPCSTR szSearchPath, DWORD symOptions, LPCSTR szUserName);
    virtual void OnLoadModule(LPCSTR img, LPCSTR mod, DWORD64 baseAddr, DWORD size, DWORD result, LPCSTR symType, LPCSTR pdbName, ULONGLONG fileVersion);
    virtual void OnCallstackEntry(StackFrameType eType, StackFrame &entry);
    virtual void OnDbgHelpErr(LPCSTR szFuncName, DWORD gle, DWORD64 addr);
    void OnOutput(LPCSTR szText)
    {
        m_buffer << szText;
    };

    StackWalkerInternal *m_sw;
    HANDLE m_hProcess;
    DWORD m_dwProcessId;
    BOOL m_modulesLoaded;
    LPSTR m_szSymPath;

    int m_options;
    std::stringstream m_buffer;

    static BOOL __stdcall myReadProcMem(HANDLE hProcess, DWORD64 qwBaseAddress, PVOID lpBuffer, DWORD nSize, LPDWORD lpNumberOfBytesRead);

    friend StackWalkerInternal;
};


_jingxian_end

#endif