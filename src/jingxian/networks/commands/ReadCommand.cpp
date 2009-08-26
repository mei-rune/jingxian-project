
# include "pro_config.h"
# include "jingxian/networks/commands/ReadCommand.h"


_jingxian_begin

ReadCommand::ReadCommand(ConnectedSocket* transport)
: transport_(transport)
{
}

ReadCommand::~ReadCommand( )
{
}

std::vector<io_mem_buf>& ReadCommand::iovec()
{
	return iovec_;
}

void ReadCommand::on_complete(size_t bytes_transferred
		, bool success
		, void *completion_key
		, errcode_t error)
{
	if (!success)
	{
		transport_->onError(transport_mode::Receive, error, _T("写数据时发生错误"));
		return;
	}
	else if (0 == bytes_transferred)
	{
		transport_->onError(transport_mode::Receive, error, _T("读0个字节"));
		return;
	}
	else
	{
		transport_->onRead(bytes_transferred);
	}
}

bool ReadCommand::execute()
{
	DWORD bytesTransferred;
	DWORD flags =0;

	if (SOCKET_ERROR != ::WSARecv(transport_->handle()
		,  &(iovec_[0])
		, (DWORD)iovec_.size()
		, &bytesTransferred
		, &flags
		, this
		, NULL))
		return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;

	return false;
}

_jingxian_end
