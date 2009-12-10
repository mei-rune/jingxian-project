
#ifndef _Application_H_
#define _Application_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <map>
#include <vector>
#include <iostream>
#include "jingxian/directory.h"
#include "jingxian/configure.h"
#include "jingxian/networks/IOCPServer.h"
#include "jingxian/utilities/NTService.h"

_jingxian_begin

class Application : public IInstance
{
public:

    virtual ~Application();


    static int main(int argc, tchar** args);

    /**
     * NT ������
     */
    virtual const tstring& name() const;

	/**
	 * ���ô�������
	 */
	virtual bool configure(configure::Context& context, const tstring& txt);

    /**
    * ��������
    */
    virtual int onRun(const std::vector<tstring>& args);

    /**
     * ���յ�һ���û������֪ͨ
     * @param dwEventType �û�������¼�����
     * @param lpEventData �û�������¼�����
     * @remarks ע�⣬�����Է����쳣��
     */
    virtual void onControl(DWORD dwEventType
                           , LPVOID lpEventData);

    /**
     * ���յ�һ������ֹͣ��֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
    virtual void interrupt();

    /**
     * ���������
     */
    virtual const tstring& toString() const;

private:
    NOCOPY(Application);
    Application(const tstring& name, const tstring& descr);

	IProtocolFactory* createProtocolFactory(tchar* name);

    IOCPServer core_;
    tstring name_;
	std::map<tstring, configure::callback_type*> callbacks_;
    tstring toString_;
};

_jingxian_end

#endif //_Application_H_