
# include "pro_config.h"
# include "jingxian/networks/commands/DisconnectCommand.h"
# include "jingxian/networks/sockets/BaseSocket.h"

_jingxian_begin

DisconnectCommand::DisconnectCommand(IOCPServer* core, ConnectedSocket* connectedSocket)
: core_(core)
, connectedSocket_(connectedSocket)
{
}

void DisconnectCommand::on_complete(size_t bytes_transferred
		, int success
		, void *completion_key
		, u_int32_t error)
{
	_connectedSocket.onDisconnected(0, _T("±»³·Ïû!"));
}

bool DisconnectCommand::execute()
{
	int bytesTransferred;
	if (BaseSocket::__disconnectex(connectedSocket_->handle()
		, this
		, 0
		, 0)
		return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;
	return false;
}

_jingxian_end

