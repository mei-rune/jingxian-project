
#ifndef _protocolcontext_h_
#define _protocolcontext_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "buffer.h"
# include "Dictionary.h"

_jingxian_begin

class IAcceptor;
class ITransport;

class ProtocolContext
{
public:
	virtual ~ProtocolContext(){}

	virtual ITransport& transport() = 0;
};

_jingxian_end

#endif //_protocolcontext_h_