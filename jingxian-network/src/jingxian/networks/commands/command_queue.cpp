
# include "pro_config.h"
# include "jingxian/networks/commands/command_queue.h"
# include "jingxian/networks/commands/RunCommand.h"
_jingxian_begin


ICommand* command_queue::createRunnable(IRunnable* runnable)
{
	return new RunCommand( runnable );
}

ICommand* command_queue::createWriteRequest()
{
	return NULL;
}

void command_queue::release( ICommand* cmd )
{
	delete cmd;
}

_jingxian_end