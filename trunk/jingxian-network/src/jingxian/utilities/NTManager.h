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
     * ��װһ�� Win32 ����
	 * @param[ in ] name Win32 ���������
	 * @param[ in ] display Win32 �����������Ϣ
	 * @param[ in ] executable Win32 �����ִ�г�������
	 * @param[ in ] args Win32 ����Ĳ���
	 * @return �ɹ�����0,���򷵻ط�0
     */
	int installService( const tstring& name, const tstring& display, 
		const tstring& executable,const std::vector<tstring>& args);

    /**
     * ж��һ�� Win32 ����
	 * @param[ in ] name Win32 ���������
	 * @return �ɹ�����0,���򷵻ط�0
	 */
    int uninstallService(const tstring& name );

    /**
     * ����һ�� Win32 ����
	 * @param[ in ] name Win32 ���������
	 * @param[ in ] args Win32 ����Ĳ���
	 * @return �ɹ�����0,���򷵻ط�0
     */
	int startService(const tstring& name, const std::vector<tstring>& args);

    /**
     * ֹͣһ�� Win32 ����
	 * @param[ in ] name Win32 ���������
	 * @return �ɹ�����0,���򷵻ط�0
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