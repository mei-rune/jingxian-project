

#ifndef _RunCommand_H_
#define _RunCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/IRunnable.h"
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/IOCPServer.h"

_jingxian_begin

class RunCommand : public ICommand
{

public:
    RunCommand(HANDLE completion_port, IRunnable* runnable);

    virtual ~RunCommand();

    virtual void on_complete(size_t bytes_transferred,
                             bool success,
                             void *completion_key,
                             errcode_t error);

    virtual bool execute();

private:
    NOCOPY(RunCommand);

    HANDLE completion_port_;
    std::auto_ptr< IRunnable> ptr_;
};

_jingxian_end

#endif //_RunCommand_H_
