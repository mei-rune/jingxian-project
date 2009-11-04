
#ifndef _NullCredentialPolicy_H_
#define _NullCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/AbstractCredentialPolicy.h"

_jingxian_begin

namespace proxy
{
class NullCredentialPolicy : public AbstractCredentialPolicy
{
public:
    NullCredentialPolicy(Proxy* server, const config::Credential& credential)
            : AbstractCredentialPolicy(server, credential)
    {
        _complete = true;
    }

    virtual ~NullCredentialPolicy()
    {
    }

    virtual size_t onReceived(ProtocolContext& context, InBuffer& inBuffer)
    {
        _complete = true;
        return 0;
    }
};

class NullCredentialPolicyFactory : public ICredentialPolicyFactory
{
public:

    NullCredentialPolicyFactory(Proxy* server)
            : server_(server)
    {
        credential_.AuthenticationType = AuthenticationType::NONE;
        credential_.Name = _T("NullCredentialPolicy");
        credential_.Description = _T("无需认证!");
    }

    virtual ~NullCredentialPolicyFactory()
    {
    }

    virtual int authenticationType() const
    {
        return credential_.AuthenticationType;
    }

    virtual ICredentialPolicy* make()
    {
        return new NullCredentialPolicy(server_, credential_);
    }

private:
    config::Credential credential_;
    Proxy* server_;
};

}

_jingxian_end

#endif // _NullCredentialPolicy_H_
