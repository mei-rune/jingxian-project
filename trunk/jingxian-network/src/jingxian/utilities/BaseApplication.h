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
     * ��������
     * @remarks ע�⣬�����Է����쳣�������ָ���˳����룬����SetLastError
     */
	virtual int run(const std::vector<tstring>& arguments) = 0;


	/**
     * ���յ�һ������ֹͣ��֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onStop()
	{
	}

	/**
     * ���յ�һ��ѯ�ʷ���״̬��֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onInterrogate()
	{
	}

	/**
     * ���յ�һ��������ͣ��֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onPause()
	{
	}

	/**
     * ���յ�һ������ָ���֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onContinue()
	{
	}

	/**
     * ���յ�һ��ϵͳ���رյ�֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onShutdown()
	{
	}

	/**
     * ���յ�һ���µ�����������󶨵�֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onNetBindAdd()
	{
	}

	/**
     * ���յ�һ����������󶨱����õ�֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onNetBindEnable()
	{
	}

	/**
     * ���յ�һ����������󶨱����õ�֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onNetBindDisable()
	{
	}

	/**
     * ���յ�һ����������󶨱�ɾ����֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onNetBindRemove()
	{
	}

	/**
     * ���յ�һ��ɾ����֪ͨ
     * @remarks ע�⣬�����Է����쳣��
     */
	virtual void onParamChange()
	{
	}

	/**
     * ���յ�һ���û������֪ͨ
     * @param dwEventType �û�������¼�����
     * @param lpEventData �û�������¼�����
     * @remarks ע�⣬�����Է����쳣��
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