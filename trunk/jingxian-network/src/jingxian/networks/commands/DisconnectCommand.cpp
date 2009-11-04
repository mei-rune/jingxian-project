
# include "pro_config.h"
# include "jingxian/networks/commands/DisconnectCommand.h"
# include "jingxian/networks/networking.h"

_jingxian_begin

DisconnectCommand::DisconnectCommand(ConnectedSocket* connectedSocket, tstring reason)
        : connectedSocket_(connectedSocket)
        , reason_(reason)
{
}

DisconnectCommand::~DisconnectCommand()
{
}

void DisconnectCommand::on_complete(size_t bytes_transferred
                                    , bool success
                                    , void *completion_key
                                    , errcode_t error)
{
    connectedSocket_->onDisconnected(*this, error, reason_);
    delete connectedSocket_;
}

bool DisconnectCommand::execute()
{
    if (networking::disconnectEx(connectedSocket_->handle()
                                 , this
                                 , 0
                                 , 0))
        return true;

    if (WSA_IO_PENDING == ::WSAGetLastError())
        return true;
    return false;
}

_jingxian_end

