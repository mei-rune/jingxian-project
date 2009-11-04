

#ifndef _ISession_H_
#define _ISession_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/IProtocol.h"
# include "jingxian/ITransport.h"

_jingxian_begin

/**
 * 代表一个连接
 */
class ISession
{
public:
    virtual ~ISession() {}
    virtual ITransport* transport() = 0;
    virtual IProtocol*  protocol() = 0;
};

_jingxian_end

#endif // _ISession_H_