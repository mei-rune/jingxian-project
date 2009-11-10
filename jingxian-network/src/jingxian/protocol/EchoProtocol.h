
#ifndef _EchoProtocol_H_
#define _EchoProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/buffer/OutBuffer.h"


_jingxian_begin

class EchoProtocol : public BaseProtocol
{
public:
    EchoProtocol()
		: BaseProtocol(_T("EchoProtocol"))
    {
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
        LOG_INFO(logger_, _T("新连接到来 - ") << context.transport().peer());
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
        LOG_INFO(logger_, _T("连接断开 - ") << context.transport().peer());
    }

    /**
     * 当有新的信息到来时，被调用。
     *
     * @param[ in ] context 会话的上下文
     * @param[ in ] buffer 包含新到来信息的缓冲区
    */
    virtual size_t onReceived(ProtocolContext& context)
    {
        OutBuffer out(&context.transport());
        for (std::vector<io_mem_buf>::const_iterator it = context.inMemory().begin()
                ; it != context.inMemory().end(); ++ it)
            out.writeBlob(it->buf, it->len);

        return out.size();
    }
private:
    NOCOPY(EchoProtocol);
};

_jingxian_end

#endif //_EchoProtocol_H_