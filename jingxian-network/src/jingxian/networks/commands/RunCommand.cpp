
# include "pro_config.h"
# include "jingxian/networks/commands/RunCommand.h"

_jingxian_begin

RunCommand::RunCommand(IOCPServer* core, IRunnable* runnbale)
: core_(core)
, ptr_(runnbale)
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
	ptr_->run();
}


bool RunCommand::execute()
{
	return core_->post( this );
}

_jingxian_end
