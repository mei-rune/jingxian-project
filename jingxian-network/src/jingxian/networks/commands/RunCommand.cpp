
# include "pro_config.h"
# include "jingxian/networks/commands/RunCommand.h"

_jingxian_begin

RunCommand::RunCommand(HANDLE completion_port, IRunnable* runnbale)
        : completion_port_(completion_port)
        , ptr_(runnbale)
{
    if (is_null(runnbale))
        ThrowException1(ArgumentNullException, _T("runnbale"));
}

RunCommand::~RunCommand()
{
}

void RunCommand::on_complete(size_t bytes_transferred,
                             bool success,
                             void *completion_key,
                             errcode_t error)
{
    ptr_->run();
}


bool RunCommand::execute()
{
    DWORD bytes_transferred = 0;
    ULONG_PTR comp_key = 0;
    return TRUE == ::PostQueuedCompletionStatus(completion_port_,  // completion port
            bytes_transferred ,      // xfer count
            comp_key,               // completion key
            this                  // overlapped
                                               );
}

_jingxian_end
