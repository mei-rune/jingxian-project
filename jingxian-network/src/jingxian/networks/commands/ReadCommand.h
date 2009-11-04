
#ifndef _ReadCommand_H_
#define _ReadCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/ConnectedSocket.h"

_jingxian_begin

class ReadCommand : public ICommand
{
public:

    ReadCommand(ConnectedSocket* transport);

    virtual ~ReadCommand();

    virtual void on_complete(size_t bytes_transferred
                             , bool success
                             , void *completion_key
                             , errcode_t error);

    virtual bool execute();

    std::vector<io_mem_buf>& iovec();

private:
    NOCOPY(ReadCommand);

    ConnectedSocket* transport_;
    std::vector<io_mem_buf> iovec_;
};

_jingxian_end

#endif //_ReadCommand_H_
