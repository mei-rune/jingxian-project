
#ifndef _BaseCredentialPolicy_H_
#define _BaseCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/AbstractCredentialPolicy.h"

_jingxian_begin

namespace proxy
{
class BaseCredentialPolicy : public AbstractCredentialPolicy
{
public:
    BaseCredentialPolicy(Proxy* server, const config::Credential& credential)
            : AbstractCredentialPolicy(server, credential)
    {
    }

    virtual ~BaseCredentialPolicy()
    {
    }

    virtual size_t onReceived(ProtocolContext& context, InBuffer& inBuffer)
    {
        ///+----+------+----------+------+----------+
        ///|VER | ULEN |  UNAME   | PLEN |  PASSWD  |
        ///+----+------+----------+------+----------+
        ///| 1  |  1   | 1 to 255 |  1   | 1 to 255 |
        ///+----+------+----------+------+----------+
        ///VER     子协商的当前版本，目前是0x01
        ///ULEN    UNAME字段的长度
        ///UNAME   用户名
        ///PLEN    PASSWD字段的长度
        ///PASSWD  口令，注意是明文传输的
        ///

        size_t totalLength = inBuffer.size();
        if (3 > totalLength)
            return 0;

        int8_t version = inBuffer.readInt8();
        size_t len = inBuffer.readInt8();

        // 两个字节是密码的最小字节数
        if ((len + 2) > totalLength)
            return 0;

        std::string name(len, ' ');
        inBuffer.readBlob((char*)name.c_str(), len);
        len = inBuffer.readInt8();
        std::string password(len, ' ');
        inBuffer.readBlob((char*)password.c_str(), len);

        if (inBuffer.fail())
            return 0;

        if (name.empty() || password.empty())
        {
            sendReply(context, AuthenticationStatus::Error);
        }
        else
        {
            sendReply(context, AuthenticationStatus::Success);
        }
        return 3 + name.size() + password.size();
    }

    virtual void sendReply(ProtocolContext& context, int status)
    {
        ///+----+--------+
        ///|VER | STATUS |
        ///+----+--------+
        ///| 1  |   1    |
        ///+----+--------+

        OutBuffer out(&context.transport());
        out.writeInt8(5);
        out.writeInt8(status);
        _complete = true;
    }
};

class BaseCredentialPolicyFactory : public ICredentialPolicyFactory
{
public:

    BaseCredentialPolicyFactory(Proxy* server
                                , int authenticationType
                                , const tstring& name
                                , const tstring& description)
            : server_(server)
    {
        credential_.AuthenticationType = authenticationType;
        credential_.Name = name;
        credential_.Description = description;
    }

    BaseCredentialPolicyFactory(Proxy* server, const config::Credential& credential)
            : server_(server)
            , credential_(credential)
    {
    }

    virtual ~BaseCredentialPolicyFactory()
    {
    }

    virtual int authenticationType() const
    {
        return credential_.AuthenticationType;
    }

    virtual ICredentialPolicy* make()
    {
        return new BaseCredentialPolicy(server_, credential_);
    }

private:
    Proxy* server_;
    config::Credential credential_;
};
}

_jingxian_end

#endif // _BaseCredentialPolicy_H_
