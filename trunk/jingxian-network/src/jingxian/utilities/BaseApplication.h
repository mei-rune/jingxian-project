#ifndef _BaseApplication_H
#define _BaseApplication_H

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

class BaseApplication
{
public:
	BaseApplication(const tstring& name);

	virtual ~BaseApplication();

	static int main(BaseApplication* app, int argc, tchar** args);
	static void usage(int argc, tchar** args);

	/**
     * 服务运行
     * @remarks 注意，不可以发生异常。如果想指定退出代码，请用SetLastError
     */
	virtual int run(const std::vector<tstring>& arguments) = 0;


	/**
     * 接收到一个服务将停止的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onStop()
	{
	}

	/**
     * 接收到一个询问服务状态的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onInterrogate()
	{
	}

	/**
     * 接收到一个服务暂停的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onPause()
	{
	}

	/**
     * 接收到一个服务恢复的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onContinue()
	{
	}

	/**
     * 接收到一个系统将关闭的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onShutdown()
	{
	}

	/**
     * 接收到一个新的网络组件被绑定的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onNetBindAdd()
	{
	}

	/**
     * 接收到一个网络组件绑定被启用的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onNetBindEnable()
	{
	}

	/**
     * 接收到一个网络组件绑定被禁用的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onNetBindDisable()
	{
	}

	/**
     * 接收到一个网络组件绑定被删除的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onNetBindRemove()
	{
	}

	/**
     * 接收到一个删除的通知
     * @remarks 注意，不可以发生异常。
     */
	virtual void onParamChange()
	{
	}

	/**
     * 接收到一个用户定义的通知
     * @param dwEventType 用户定义的事件类型
     * @param lpEventData 用户定义的事件数据
     * @remarks 注意，不可以发生异常。
     */
	virtual void onControl(DWORD dwEventType
		, LPVOID lpEventData)
	{
	}

	ILogger* logger()
	{
		return logger_;
	}

	const tstring& name() const
	{
		return name_;
	}

	const tstring& toString() const
	{
		return name_;
	}
private:
	NOCOPY(BaseApplication);
	ILogger* logger_;
	tstring name_;
};

_jingxian_end

#endif // _BaseApplication_H