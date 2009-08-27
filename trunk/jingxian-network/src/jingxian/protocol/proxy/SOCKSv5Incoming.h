
#ifndef _SOCKSv5Incoming_H_
#define _SOCKSv5Incoming_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/buffer/OutBuffer.h"

_jingxian_begin

    public class SOCKSv5Incoming : public BaseProtocol
    {
        SOCKSv5 _socks;

        public SOCKSv5Incoming(SOCKSv5 socks)
        {
            _socks = socks;
            _socks.Peer = this;
        }

        #region IProtocol ≥…‘±

        public override void OnConnection(ProtocolContext context)
        {
            ITCPTransport transport = context.Transport as ITCPTransport;
            transport.NoDelay = true;

            base.OnConnection(context);

            _socks.OnBindSuccess(this);
        }

        public override void OnReceived(ProtocolContext context, IOBuffer data)
        {
            _socks.Write(data);
        }

        public override void OnDisconnection(ProtocolContext context, Exception reason)
        {
            _socks.Transport.Disconnection();
        }

        #endregion
    };

_jingxian_end

#endif //_SOCKSv5Incoming_H_