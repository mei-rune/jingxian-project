
#ifndef _NullCredentialPolicy_H_
#define _NullCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/ICredentialPolicy.h"

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
			return -1;
		}

		virtual  size_t onReceived(ProtocolContext& context)
		{
			ThrowException1(RuntimeException, _T( "��֧�ֵ���Ȩ!" ));
		}

		virtual  bool isComplete()
		{
			return true;
		}
	};
}

_jingxian_end

#endif // _NullCredentialPolicy_H_