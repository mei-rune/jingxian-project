
#ifndef _Application_H_
#define _Application_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <vector>
#include <iostream>
#include "jingxian/directory.h"
#include "jingxian/networks/IOCPServer.h"
#include "jingxian/utilities/NTService.h"

_jingxian_begin

class Application : public IInstance
{
public:

    virtual ~Application();


    static int main(int argc, tchar** args);

    /**
     * NT 服务名
     */
    virtual const tstring& name() const;

    /**
    * 服务运行
    */
    virtual int onRun(const std::vector<tstring>& args);

    /**
     * 接收到一个用户定义的通知
     * @param dwEventType 用户定义的事件类型
     * @param lpEventData 用户定义的事件数据
     * @remarks 注意，不可以发生异常。
     */
    virtual void onControl(DWORD dwEventType
                           , LPVOID lpEventData);

    /**
     * 接收到一个服务将停止的通知
     * @remarks 注意，不可以发生异常。
     */
    virtual void interrupt();

    /**
     * 服务的描述
     */
    virtual const tstring& toString() const;

private:
    NOCOPY(Application);
    Application(const tstring& name, const tstring& descr);

    tstring name_;
    tstring toString_;
    IOCPServer core_;
};

_jingxian_end

#endif //_Application_H_