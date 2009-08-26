
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
     * 在指定的时间段内没有收到任何数据
     * 
     * @param[ in ] context 会话的上下文
	 */
    virtual void onTimeout(ProtocolContext& context)
	{
	}

    /**
     * 当会话建立后，被调用。
     * 
     * @param[ in ] context 会话的上下文
	 */
    virtual void onConnected(ProtocolContext& context)
	{
		INFO(log(), _T("新连接到来 - ") << context.transport().peer());
	}

    /**
     * 当会话关闭后，被调用。
     * 
     * @param[ in ] context 会话的上下文
     * @param[ in ] errCode 关闭的原因,为0是表示主动关闭
     * @param[ in ] reason 关闭的原因描述
	 */
    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
	{
		INFO(log(), _T("连接断开 - ") << context.transport().peer());
	}

    /**
     * 当有新的信息到来时，被调用。
     * 
     * @param[ in ] context 会话的上下文
     * @param[ in ] buffer 包含新到来信息的缓冲区
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