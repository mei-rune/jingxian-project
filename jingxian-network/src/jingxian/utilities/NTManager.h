#ifndef SERVICEMANAGER_H
#define SERVICEMANAGER_H

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <winsvc.h>
#include <string>
#include <vector>
#include "jingxian/logging/logging.h"

_jingxian_begin

class NTManager
{
public:

	NTManager();

	virtual ~NTManager();

    /**
     * 安装一个 Win32 服务
	 * @param[ in ] name Win32 服务的名称
	 * @param[ in ] display Win32 服务的描述信息
	 * @param[ in ] executable Win32 服务的执行程序名称
	 * @param[ in ] args Win32 服务的参数
	 * @return 成功返回0,否则返回非0
     */
	int installService( const tstring& name, const tstring& display, 
		const tstring& executable,const std::vector<tstring>& args);

    /**
     * 卸载一个 Win32 服务
	 * @param[ in ] name Win32 服务的名称
	 * @return 成功返回0,否则返回非0
	 */
    int uninstallService(const tstring& name );

    /**
     * 启动一个 Win32 服务
	 * @param[ in ] name Win32 服务的名称
	 * @param[ in ] args Win32 服务的参数
	 * @return 成功返回0,否则返回非0
     */
	int startService(const tstring& name, const std::vector<tstring>& args);

    /**
     * 停止一个 Win32 服务
	 * @param[ in ] name Win32 服务的名称
	 * @return 成功返回0,否则返回非0
     */
    int stopService(const tstring& name);

private:
	NOCOPY(NTManager);

	bool waitForServiceState(SC_HANDLE hService, DWORD pendingState, SERVICE_STATUS& status);

	void showServiceStatus(const tstring& msg, SERVICE_STATUS& status);

	ILogger* logger_;
};

_jingxian_end

#endif // SERVICEMANAGER_H