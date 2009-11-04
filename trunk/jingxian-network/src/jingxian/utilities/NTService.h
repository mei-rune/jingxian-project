
#ifndef _NTSERVICE_H_
#define _NTSERVICE_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <vector>
# include "jingxian/string/os_string.h"

_jingxian_begin

class IInstance
{
public:
    virtual ~IInstance() { }

    /**
     * NT 服务名
     */
    virtual const tstring& name() const = 0;

    /**
     * 服务运行
     */
    virtual int onRun(const std::vector<tstring>& arguments) = 0;

    /**
     * 接收到一个用户定义的通知
     * @param dwEventType 用户定义的事件类型
     * @param lpEventData 用户定义的事件数据
     * @remarks 注意，不可以发生异常。
     */
    virtual void onControl(DWORD dwEventType
                           , LPVOID lpEventData) = 0;


    /**
     * 接收到一个服务将停止的通知
     * @remarks 注意，不可以发生异常。
     */
    virtual void interrupt() = 0;

    /**
     * 服务的描述
     */
    virtual const tstring& toString() const = 0;
};


/**
 * 服务入口函数
 */
int serviceMain(IInstance* instance);//, DWORD argc,LPTSTR *argv);

/**
 * 安装一个 Win32 服务
 * @param[ in ] name Win32 服务的名称
 * @param[ in ] display Win32 服务的描述信息
 * @param[ in ] executable Win32 服务的执行程序名称
 * @param[ in ] args Win32 服务的参数
 * @return 成功返回0,否则返回非0
 */
int installService(const tstring& name
                   , const tstring& display
                   , const tstring& description
                   , const tstring& executable
                   , const std::vector<tstring>& args
                   , tostream& out);

/**
 * 卸载一个 Win32 服务
 * @param[ in ] name Win32 服务的名称
 * @return 成功返回0,否则返回非0
 */
int uninstallService(const tstring& name, tostream& out);

/**
 * 启动一个 Win32 服务
 * @param[ in ] name Win32 服务的名称
 * @param[ in ] args Win32 服务的参数
 * @return 成功返回0,否则返回非0
 */
int startService(const tstring& name, const std::vector<tstring>& args, tostream& out);

/**
 * 停止一个 Win32 服务
 * @param[ in ] name Win32 服务的名称
 * @return 成功返回0,否则返回非0
 */
int stopService(const tstring& name, tostream& out);

_jingxian_end

#endif // _NTSERVICE_H_
