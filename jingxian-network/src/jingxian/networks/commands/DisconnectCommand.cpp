
# include "pro_config.h"
# include "jingxian/networks/commands/DisconnectCommand.h"

_jingxian_begin

DisconnectCommand::DisconnectRequest(ConnectedSocket connectedSocket,Exception error)
: base( null )
{
	_connectedSocket = connectedSocket;
	_exception = error;
}

void DisconnectCommand::Complete(int bytes_transferred, bool success, int error, object context)
{
	_connectedSocket.OnDisconnection(_exception);
}

void DisconnectCommand::Disconnect()
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

_jingxian_end

