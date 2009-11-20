
#ifndef _BindPorts_H_
#define _BindPorts_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/directory.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/protocol/proxy/ICredentialPolicy.h"
# include "jingxian/protocol/proxy/Credentials.h"
# include "jingxian/protocol/proxy/BaseCredentialPolicy.h"
# include "jingxian/protocol/proxy/NullCredentialPolicy.h"
# include "jingxian/protocol/proxy/config/Configuration.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"



_jingxian_begin

namespace proxy
{
struct BindPort
{
    bool IsUsed;
    int Port;

    BindPort()
    {
        IsUsed = false;
        Port = 0;
    }
};

class BindPorts
{
public:
    BindPorts(int begin, int end)
            : _begin(begin)
            , _end(end)
            , _position(-1)
    {
        if (begin > end)
            ThrowException1(IllegalArgumentException, _T("begin 不能比 end 还要大!"));

        //_ports = new BindPort[ Math.Min( 1000,_end - _begin  ) ];

        BindPort bindPort;
        for (int i = _begin; i <= _end; ++ i)
        {
            bindPort.IsUsed = false;
            bindPort.Port = i;

            _ports.push_back(bindPort);
        }
    }

    int GetPort()
    {
        for (size_t i = 0; i < _ports.size(); ++ i)
        {
            if (!_ports[++_position % _ports.size()].IsUsed)
                return _ports[_position % _ports.size()].Port;
        }
        return 0;
    }

    void ReleasePort(int port)
    {
        if (_begin > port || _end < port)
            return;

        _ports[ port - _begin ].IsUsed = false;
    }

private:

    int _begin;
    int _end;

    std::vector<BindPort> _ports;
    int _position;
};

}

_jingxian_end

#endif // _BindPorts_H_