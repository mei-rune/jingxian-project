
# include "pro_config.h"
# include "jingxian/networks/commands/WriteCommand.h"

_jingxian_begin
WriteCommand::WriteCommand(ConnectedSocket* transport)
: transport_(transport)
{
}

WriteCommand::~WriteCommand()
{
}

std::vector<io_mem_buf>& WriteCommand::iovec()
{
	return iovec_;
}

void WriteCommand::on_complete(size_t bytes_transferred,
							   bool success,
							   void *completion_key,
							   errcode_t error)
{
	if (!success)
	{
		transport_->onError(transport_mode::Send, error, _T("写数据时发生错误"));
		return;
	}
	else if (0 == bytes_transferred)
	{
		transport_->onError(transport_mode::Send, error, _T("对方主动关闭!"));
		return;
	}
	else
	{
		transport_->onWrite(bytes_transferred);
	}
}

bool WriteCommand::execute()
{
	DWORD bytesTransferred;
	if (SOCKET_ERROR  != ::WSASend(transport_->handle()
		, &(iovec_[0])
		, (DWORD)iovec_.size()
		, &bytesTransferred
		, 0
		, this
		, NULL))
		return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;

	return false;
}

_jingxian_end
