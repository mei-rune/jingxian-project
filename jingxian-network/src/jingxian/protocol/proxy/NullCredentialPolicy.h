
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
		NullCredentialPolicy(const config::Credential& credential, Proxy* server)
			: AbstractCredentialPolicy( credential, server )
		{
			_complete = true;
		}

		virtual ~NullCredentialPolicy()
		{
		}

		virtual size_t onReceived(ProtocolContext context, IOBuffer IOBuffer)
		{
			return 0;
		}
	};
}

_jingxian_end

#endif // _NullCredentialPolicy_H_
