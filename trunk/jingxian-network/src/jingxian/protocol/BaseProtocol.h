
#ifndef _BaseProtocol_H_
#define _BaseProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/linklist.h"
# include "jingxian/IProtocol.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/buffer/OutBuffer.h"
# include "jingxian/buffer/InBuffer.h"
# include "jingxian/logging/logging.h"

_jingxian_begin

class BaseProtocol : public IProtocol
{
public:
    BaseProtocol(const tchar* descr = _T("BaseProtocol"))
            : logger_(descr)
			, toString_(descr)
    {
    }

    BaseProtocol(const BaseProtocol& protocol)
	: logger_(protocol.toString_)
	, toString_(protocol.toString_)
    {
    }

    virtual ~BaseProtocol()
    {
    }

    BaseProtocol& operator=(const BaseProtocol& protocol)
    {
        toString_ = protocol.toString_;
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
    }

    /**
     * 当有新的信息到来时，被调用。
     *
     * @param[ in ] context 会话的上下文
     * @param[ in ] buffer 包含新到来信息的缓冲区
    */
    virtual size_t onReceived(ProtocolContext& context)
    {
        return context.inBytes();
    }

    static void freeBuffer(buffer_chain_t* chain, void* context)
    {
        my_free(context);
    }

    virtual buffer_chain_t* createBuffer(const ProtocolContext& context)
    {
        databuffer_t* result = (databuffer_t*)my_calloc(1, sizeof(databuffer_t) + 100);
        result->chain.context = result;
        result->chain.freebuffer = &freeBuffer;
        result->chain.type = BUFFER_ELEMENT_MEMORY;

        result->capacity = 100;
        result->start = result->end = result->ptr;
        return (buffer_chain_t*)result;
    }

    /**
     * 取得地址的描述
     */
    virtual const tstring& toString() const
    {
        return toString_;
    }
protected:
	logging::logger logger_;
private:
    tstring toString_;
};

_jingxian_end

#endif //_BaseProtocol_H_