
#ifndef _protocolcontext_h_
#define _protocolcontext_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.h"

_jingxian_begin

namespace configure
{

class Context
{
public:

	virtual ~Context(){}

	void push();

	void pop();
};

class ICallback
{
public:
	virtual ~IHandler(){}

	virtual bool configure(ConfigureContext& context, const tstring& txt) = 0;
};

}

_jingxian_end

#endif //_protocolcontext_h_