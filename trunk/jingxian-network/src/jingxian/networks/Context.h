
#ifndef _TCPContext_H_
#define _TCPContext_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/networking.h"
# include "jingxian/networks/IOCPServer.h"


_jingxian_begin

class TCPContext : public ProtocolContext
{
public:
	
};

_jingxian_end

#endif //_TCPContext_H_ 