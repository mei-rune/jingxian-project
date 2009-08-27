
#ifndef _NullProtocol_H_
#define _NullProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"

_jingxian_begin

class NullProtocol : public BaseProtocol
{
public:
	NullProtocol()
	{
		toString_ = _T("NullProtocol");
	}

    virtual void onConnected(ProtocolContext& context)
	{
		context.transport().disconnection();
	}

private:
	NOCOPY(NullProtocol);
};

_jingxian_end

#endif //_NullProtocol_H_