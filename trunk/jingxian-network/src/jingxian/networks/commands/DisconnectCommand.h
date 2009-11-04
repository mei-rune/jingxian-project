
#ifndef _DisconnectCommand_H_
#define _DisconnectCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/connectedsocket.h"

_jingxian_begin

class DisconnectCommand : public ICommand
{
public:

    DisconnectCommand(ConnectedSocket* connectedSocket, tstring reason);

    virtual ~DisconnectCommand();

    virtual void on_complete(size_t bytes_transferred
                             , bool success
                             , void *completion_key
                             , errcode_t error);

    virtual bool execute();

private:

    NOCOPY(DisconnectCommand);

    ConnectedSocket* connectedSocket_;
    tstring reason_;
};

_jingxian_end

#endif //_DisconnectCommand_H_
