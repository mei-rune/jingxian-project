
# include "pro_config.h"
# include "jingxian/networks/commands/RunCommand.h"

_jingxian_begin

RunCommand::RunCommand(IRunnable* runnbale)
: _ptr( runnbale )
{
	if( is_null( runnbale ) )
		ThrowException1(ArgumentNullException, "runnbale");
}

RunCommand::~RunCommand()
{
}

void RunCommand::on_complete (size_t bytes_transferred,
                         int success,
                         void *completion_key,
                         u_int32_t error)
{
	_ptr->run();
}

_jingxian_end
