
#ifndef _SOCKSv5Protocol_H_
#define _SOCKSv5Protocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer/OutBuffer.h"
# include "jingxian/Connector.h"
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/protocol/proxy/SOCKSv5Incoming.h"
# include "jingxian/protocol/proxy/SOCKSv5Outgoing.h"
# include "jingxian/protocol/proxy/ICredentialPolicy.h"

_jingxian_begin

namespace proxy
{
class Proxy;

namespace SocksError
{
enum Type
{
    Success = 0x5A//    允许转发
    ,
    Error = 0x5B//    拒绝转发，一般性失败
    ,
    NoConnect = 0x5C//    拒绝转发，SOCKS 4 Server无法连接到SOCS 4 Client所在主机的
    //              IDENT服务
    , ErrorUser = 0x5D//    拒绝转发，请求报文中的USERID与IDENT服务返回值不相符

};
}

namespace SOCKSv5Error
{
enum Type
{
    Success = 0 //       成功
    ,
    Error = 0x01 //        一般性失败
    ,
    Deny = 0x02 //        规则不允许转发
    ,
    NetworkError = 0x03 //        网络不可达
    ,
    HostError = 0x04 //        主机不可达
    ,
    Timeout = 0x05 //        连接拒绝
    ,
    TTLError = 0x06 //        TTL超时
    ,
    NotSupportCommand = 0x07 //        不支持请求包中的CMD
    ,
    NotSupportAddr = 0x08 //        不支持请求包中的ATYP
};
}

class SOCKSv5Protocol : public BaseProtocol
{
public :
    SOCKSv5Protocol(Proxy* server);
    ~SOCKSv5Protocol();

    virtual void onConnected(ProtocolContext& context);
    virtual size_t onReceived(ProtocolContext& context);
    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason);

    virtual void writeIncoming(const std::vector<io_mem_buf>& buf);
    virtual void writeOutgoing(const std::vector<io_mem_buf>& buf);

    void onError(ProtocolContext& context);
    size_t onHello(ProtocolContext& context, InBuffer& inBuffer);
    size_t onAuthenticating(ProtocolContext& context, InBuffer& inBuffer);
    size_t onCommand(ProtocolContext& context, InBuffer& inBuffer);

    void connectTo(ProtocolContext& context, const tstring& host);
    void onConnectComplete(ITransport* transport, ProtocolContext& context);
    void onConnectError(const ErrorCode&, ProtocolContext& context);

    void sendReply(ProtocolContext& context, int reply, int version, int addressType, const char* addr, size_t len, int port);

    Proxy* internalCore();
private:
    Proxy* server_;
    int status_;
    std::auto_ptr<proxy::ICredentialPolicy> credentialPolicy_;
    typedef ConnectProxy<SOCKSv5Protocol, ProtocolContext&> connectorType;
    connectorType* connectProxy_;

    SOCKSv5Outgoing outgoing_;
    SOCKSv5Incoming incoming_;

#ifdef DEBUG_TRACE
    tstring sessionPath_;
    FILE* sessionId_;
#endif
};
}

_jingxian_end

#endif //_SOCKSv5Protocol_H_