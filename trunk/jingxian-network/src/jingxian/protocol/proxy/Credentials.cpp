
# include "pro_config.h"
# include "jingxian/protocol/proxy/Credentials.h"
# include "jingxian/protocol/proxy/NotSupportedCredentialPolicy.h"

_jingxian_begin

namespace proxy
{
Credentials::~Credentials()
{
}

ICredentialPolicy* Credentials::GetCredential(const std::vector<int>& authTypes)
{
    for (std::list<ICredentialPolicyFactory*>::iterator it = _policies.begin()
            ; it != _policies.end(); ++it)
    {
        for (std::vector<int>::const_iterator it2 = authTypes.begin()
                ; it2 != authTypes.end(); ++ it2)
        {
            if (*it2 == (*it)->authenticationType())
                return (*it)->make();
        }
    }
    return new NotSupportedCredentialPolicy();
}

std::list<ICredentialPolicyFactory*>& Credentials::policies()
{
    return _policies;
}
}

_jingxian_end