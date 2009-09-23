
#ifndef _AbstractCredentialPolicy_H_
#define _AbstractCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/ICredentialPolicy.h"
# include "jingxian/protocol/proxy/config/Credential.h"
# include "jingxian/protocol/proxy/Proxy.h"

_jingxian_begin

namespace proxy
{
	class AbstractCredentialPolicy : public ICredentialPolicy
	{
	public:
		AbstractCredentialPolicy(Proxy* server, const config::Credential& credential)
			: _server(server)
			, _credential(credential)
			, _complete(false)
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
		Proxy* _server;
		config::Credential _credential;
		bool _complete;
	};
}

_jingxian_end

#endif // _AbstractCredentialPolicy_H_
