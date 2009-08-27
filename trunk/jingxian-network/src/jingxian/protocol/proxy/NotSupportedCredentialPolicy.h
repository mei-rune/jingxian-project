
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
	class NotSupportedCredentialPolicy : public ICredentialPolicy
	{
	public:

		virtual ~NotSupportedCredentialPolicy()
		{
		}

		virtual int authenticationType()
		{
			return (int) AuthenticationType::NotSupported;
		}

		virtual  size_t onReceived(ProtocolContext& context)
		{
			ThrowException(RuntimeException, _T( "不支持的授权!" ));
		}

		virtual  bool isComplete()
		{
			return true;
		}
	};
}

_jingxian_end

#endif // _NullCredentialPolicy_H_