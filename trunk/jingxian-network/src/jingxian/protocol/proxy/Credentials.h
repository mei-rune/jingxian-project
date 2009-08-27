
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

		virtual int authenticationType() = 0;

		virtual ICredentialPolicy* make() = 0;
	};

	class Credentials : public ICredentials
	{
	public:

		virtual ~Credentials()
		{
		}

		virtual ICredentialPolicy GetCredential(const std::vector<int>& authTypes)
		{
			for(std::list<ICredentialPolicyFactory*>::iterator it = _policies.begin()
				; it != _policies.end(); ++it)
			{
				for(std::vector<int>::const_iterator it2 = authTypes.begin()
					; it2 != authTypes.end(); ++ it2)
				{
					if (*it2 == it->authenticationType())
						return it->make();
				}
			}
			return new NotSupportedCredentialPolicy();
		}

		std::list<ICredentialPolicyFactory*>& policies()
		{
			return _policies;
		}
	private:
		std::list<ICredentialPolicyFactory*> _policies = new List<ICredentialPolicyFactory*>();
	};
}

_jingxian_end

#endif // _Credentials_H_