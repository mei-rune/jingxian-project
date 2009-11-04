
#ifndef _ICredentialPolicy_H_
#define _ICredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"

_jingxian_begin

namespace AuthenticationType
{
enum Type
{
    NONE = 0x00 //(无需认证)
    ,
    GSSAPI = 0x01
    ,
    BASE = 0x02 //USERNAME/PASSWORD(用户名/口令认证机制)
    ,
    NOTSUPPORTED = 0xFF //(完全不兼容)
};
}

namespace AuthenticationStatus
{
enum Type
{
    Success = 0x00 // 成功
    ,
    Error = 0x01   // 失败
    ,
    Pending = 0xFF // 还没有完成授权操作
};
}

namespace proxy
{
class ICredentialPolicy
{
public:

    virtual ~ICredentialPolicy() {}

    virtual int authenticationType() const = 0;

    virtual size_t onReceived(ProtocolContext& context, InBuffer& inBuffer) = 0;

    /**
    * 如果身份验证过程已完成，则为 true；否则为 false
    */
    virtual bool isComplete() = 0;
};

class ICredentials
{
public:
    virtual ~ICredentials() {}

    virtual ICredentialPolicy* GetCredential(const std::vector<int>& authTypes) = 0;
};
}

_jingxian_end

#endif // _ICredentialPolicy_H_