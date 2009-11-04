
#ifndef _EchoServer_H_
#define _EchoServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/directory.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/protocol/EchoProtocol.h"



_jingxian_begin


class EchoProtocolFactory : public IProtocolFactory
{
public:
    EchoProtocolFactory()
    {
    }

    virtual IProtocol* createProtocol(ITransport* transport, IReactorCore* core)
    {
        return &protocol_;
    }

    virtual const tstring& toString() const
    {
        return protocol_.toString();
    }
private:

    EchoProtocol protocol_;
};

_jingxian_end

#endif //_EchoServer_H_