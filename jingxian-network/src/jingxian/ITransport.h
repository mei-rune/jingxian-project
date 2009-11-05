
#ifndef _itransport_h_
#define _itransport_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/exception.h"
# include "jingxian/buffer/buffer.h"

_jingxian_begin

class IProtocol;

class ITransport
{
public:
    virtual ~ITransport() {}

    /**
     * 初始化 Transport 实例
     */
    virtual void initialize() = 0;

    /**
         * 指定用 @see{IProtocol} 接口来接收读到的数据
     * @return 返回旧的 @see{IProtocol} 接口
         */
    virtual IProtocol* bindProtocol(IProtocol* protocol) = 0;

    /**
     * 开始读数据
     */
    virtual void startReading() = 0;

    /**
     * 停止读数据
     */
    virtual void stopReading() = 0;

    /**
     * 发送数据（注意它是异步的  )
     * @param[ in ] buffer 待发送的数据块
     */
    virtual void write(buffer_chain_t* buffer) = 0;

    /**
         * 发送多块数据（注意它是异步的  )
         * @param[ in ] buffers 待发送的数据块数组指针
         * @param[ in ] len 数据块的个数
         */
    virtual void writeBatch(buffer_chain_t** buffers, size_t len) = 0;

    /**
     * 关闭连接
     */
    virtual void disconnection() = 0;

    /**
         * 关闭连接
         */
    virtual void disconnection(const tstring& error) = 0;

    /**
     * 源地址
     */
    virtual const tstring& host() const = 0;

    /**
     * 目标地址
     */
    virtual const tstring& peer() const = 0;

    /**
     * 引发 @see{protocol} 的onTimeout事件的超时时间
     */
    virtual time_t timeout() const = 0;

    /**
     * 取得地址的描述
     */
    virtual const tstring& toString() const = 0;
};


class ErrorCode
{
public:
    ErrorCode(const tchar* err)
            : code_(0)
            , err_(err)
    {}

    ErrorCode(int code)
            : code_(code)
            , err_(lastError(code))
    {
    }

    ErrorCode(int code, const tstring& err)
            : code_(code)
            , err_(err)
    {
    }

    ErrorCode(int code, const tchar* err)
            : code_(code)
            , err_(err)
    {
    }

    virtual ~ErrorCode()
	{
	}


    int errorCode() const
    {
        return code_;
    }

    const tstring& toString() const
    {
        return err_;
    }

private:
    int code_;
    tstring err_;
};

inline tostream& operator<<(tostream& target, const ITransport& transport)
{
    target << transport.toString();
    return target;
}

inline tostream& operator<<(tostream& target, const ErrorCode& err)
{
    target << err.toString();
    return target;
}

typedef void (*OnBuildConnectionComplete)(ITransport* transport, void* context);
typedef void (*OnBuildConnectionError)(const ErrorCode& err,  void* context);

_jingxian_end

#endif //_itransport_h_
