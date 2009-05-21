
#ifndef _DisconnectCommand_H_
#define _DisconnectCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"

_jingxian_begin

class DisconnectCommand : public ICommand
    {
        ConnectedSocket _connectedSocket;
        Exception _exception;

        public DisconnectRequest(ConnectedSocket connectedSocket,Exception error)
            : base( null )
        {
            _connectedSocket = connectedSocket;
            _exception = error;
        }

        public void Complete(int bytes_transferred, bool success, int error, object context)
        {
            _connectedSocket.OnDisconnection(_exception);
        }

        public void Disconnect()
        {
            if (_connectedSocket.Socket.DisconnectEx(this.NativeOverlapped
                , IoctlSocketConstants.TF_REUSE_SOCKET, 0))
                return;

            int errCode = Marshal.GetLastWin32Error();
            if ((int)SocketError.IOPending == errCode)
            {
                return;
            }

            throw new ReadError(errCode);
        }
    }

_jingxian_end

#endif //_DisconnectCommand_H_
