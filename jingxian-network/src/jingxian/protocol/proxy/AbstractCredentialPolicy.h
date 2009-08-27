
#ifndef _AbstractCredentialPolicy_H_
#define _AbstractCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/AbstractCredentialPolicy.h"
# include "jingxian/protocol/proxy/config/Credential.h"

_jingxian_begin

namespace proxy
{
	class AbstractCredentialPolicy : public ICredentialPolicy
	{
	public:
		AbstractCredentialPolicy(const config::Credential& credential, Proxy* server)
			: _credential(credential)
			, _server(server)
		{
		}

		virtual ~AbstractCredentialPolicy()
		{
		}

		virtual int authenticationType()
		{
			return _credential.authenticationType();
		}

		virtual bool isComplete()
		{
			return _complete;
		}

	protected:
		bool _complete = false;
		config::Credential _credential;
		Proxy* _server;
	};
}

_jingxian_end

#endif // _AbstractCredentialPolicy_H_
