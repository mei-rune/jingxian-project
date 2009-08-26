
#ifndef _EchoProtocol_H_
#define _EchoProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"

_jingxian_begin

class EchoProtocol : public BaseProtocol
{
public:
	EchoProtocol()
	{
		toString_ = _T("EchoProtocol");
	}

    /**
     * ��ָ����ʱ�����û���յ��κ�����
     * 
     * @param[ in ] context �Ự��������
	 */
    virtual void onTimeout(ProtocolContext& context)
	{
	}

    /**
     * ���Ự�����󣬱����á�
     * 
     * @param[ in ] context �Ự��������
	 */
    virtual void onConnected(ProtocolContext& context)
	{
		INFO(log(), _T("�����ӵ��� - ") << context.transport().peer());
	}

    /**
     * ���Ự�رպ󣬱����á�
     * 
     * @param[ in ] context �Ự��������
     * @param[ in ] errCode �رյ�ԭ��,Ϊ0�Ǳ�ʾ�����ر�
     * @param[ in ] reason �رյ�ԭ������
	 */
    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
	{
		INFO(log(), _T("���ӶϿ� - ") << context.transport().peer());
	}

    /**
     * �����µ���Ϣ����ʱ�������á�
     * 
     * @param[ in ] context �Ự��������
     * @param[ in ] buffer �����µ�����Ϣ�Ļ�����
	 */
    virtual void onReceived(ProtocolContext& context)
	{
		std::string str(context.inBuffer().size(), 'a');
		context.inBuffer().readBlob((void*)str.c_str(), str.size());		
		context.outBuffer().writeBlob(str.c_str(), str.size());
	}
private:
	NOCOPY(EchoProtocol);
};

_jingxian_end

#endif //_EchoProtocol_H_