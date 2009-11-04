
#ifndef _Credentials_H_
#define _Credentials_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/protocol/proxy/ICredentialPolicy.h"


_jingxian_begin

namespace proxy
{
class ICredentialPolicyFactory
{
public:
    virtual ~ICredentialPolicyFactory() {}

    virtual int authenticationType() const = 0;

    virtual ICredentialPolicy* make() = 0;
};

class Credentials : public ICredentials
{
public:

    virtual ~Credentials();

    virtual ICredentialPolicy* GetCredential(const std::vector<int>& authTypes);

    std::list<ICredentialPolicyFactory*>& policies();

private:
    std::list<ICredentialPolicyFactory*> _policies;
};
}

_jingxian_end

#endif // _Credentials_H_