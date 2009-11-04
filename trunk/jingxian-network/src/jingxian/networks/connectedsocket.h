

#ifndef _CONNECTED_SOCKET_H_
#define _CONNECTED_SOCKET_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <fstream>
# include "jingxian/IProtocol.h"
# include "jingxian/ISession.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/networks/connection_status.h"
# include "jingxian/logging/logging.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/TCPContext.h"
# include "jingxian/networks/buffer/IncomingBuffer.h"
# include "jingxian/networks/buffer/OutgoingBuffer.h"

_jingxian_begin

/**
 * On开头的函数在用户直接调用的方法中不可以使用。
 * 给用户调用的方法为ITransport接口中的方法
 */
class ConnectedSocket : public ITransport, public ISession
{
public:

    ConnectedSocket(IOCPServer* core
                    , SOCKET sock
                    , const tstring& host
                    , const tstring& peer);

    virtual ~ConnectedSocket();

    /**
     * @implements initialize
     */
    virtual void initialize();

    /**
     * @implements bindProtocol
     */
    virtual IProtocol* bindProtocol(IProtocol* protocol);

    /**
     * @implements startReading
     */
    virtual void startReading();

    /**
     * @implements stopReading
     */
    virtual void stopReading();

    /**
     * @implements write
     */
    virtual void write(buffer_chain_t* buffer);
    virtual void writeBatch(buffer_chain_t** buffers, size_t len);

    /**
     * @implements disconnection
     */
    virtual void disconnection();

    /**
     * @implements disconnection
     */
    virtual void disconnection(const tstring& error);

    /**
     * @implements host
     */
    virtual const tstring& host() const;

    /**
     * @implements peer
     */
    virtual const tstring& peer() const;

    /**
     * @implements timeout
     */
    virtual time_t timeout() const;

    /**
     * @implements transport
     */
    virtual ITransport* transport()
    {
        return this;
    }

    /**
     * @implements protocol
     */
    virtual IProtocol*  protocol()
    {
        return protocol_;
    }

    /**
     * @implements toString
     */
    virtual const tstring& toString() const;


    SOCKET handle()
    {
        return socket_;
    }
    ITracer* tracer()
    {
        return tracer_;
    }


    void onWrite(const ICommand& command, size_t bytes_transferred);
    void onRead(const ICommand& command, size_t bytes_transferred);
    void onError(const ICommand& command, transport_mode::type mode, errcode_t error, const tstring& description);
    void onDisconnected(const ICommand& command, errcode_t error, const tstring& description);

    buffer_chain_t* allocateProtocolBuffer();

private:
    NOCOPY(ConnectedSocket);

    void doRead();
    void doWrite();
    void doDisconnect(transport_mode::type mode, errcode_t error, const tstring& description);


    /// iocp对象的引用
    IOCPServer* core_;
    /// socket 对象
    SOCKET socket_;
    /// 本地地址
    tstring host_;
    /// 远程地址
    tstring peer_;
    /// 本对象当前所处的状态
    connection_status::type state_;
    /// 超时时间
    time_t timeout_;
    /// 协议处理器
    IProtocol* protocol_;
    /// 对象的上下文
    TCPContext context_;
    /// 是否已初始化
    bool isInitialize_;
    ///暂停数据时读来的数据临时存放位置

    bool stopReading_;
    /// 表示发出一个读请求,但还没有返回
    bool reading_;
    IncomingBuffer incoming_;

    /// 表示发出一个写请求,但还没有返回
    bool writing_;
    OutgoingBuffer outgoing_;

    /// 已申请断开,为真后不能再发出读和写请求了.
    bool shutdowning_;
    /// 保存被停止的原因
    tstring disconnectReason_;

    ///是不添加到core的sessions容器中
    bool isPosition_;
    ///core的sessions容器中的位置
    SessionList::iterator sessionPosition_;

    /// 日志对象
    ITracer* tracer_;
    tstring toString_;

#ifdef DUMPFILE
    std::auto_ptr<std::ofstream> os;
    std::auto_ptr<std::ofstream> is;
#endif
};

_jingxian_end

#endif // _CONNECTED_SOCKET_H_