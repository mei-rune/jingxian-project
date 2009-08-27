
#ifndef _SOCKSv5Outgoing_H_
#define _SOCKSv5Outgoing_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/buffer/OutBuffer.h"

_jingxian_begin

    public class SOCKSv5Outgoing : public BaseProtocol
    {
        public SOCKSv5 _socks;

        public SOCKSv5Outgoing(SOCKSv5 peer)
        {
            _socks = peer;
        }

        #region IProtocol ≥…‘±

        public override void OnReceived(ProtocolContext context, IOBuffer data)
        {
            _socks.Write(data);
        }

        public override void OnConnection(ProtocolContext context)
        {
            base.OnConnection(context);
            _socks.OnConnectSuccess(_socks, this, (TCPEndpoint)Transport.Host);
        }

        public override void OnDisconnection(ProtocolContext context, Exception reason)
        {
            _socks.Transport.Disconnection();
        }

        #endregion
    };

_jingxian_end

#endif //_SOCKSv5Outgoing_H_
