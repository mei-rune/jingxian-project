
# include "pro_config.h"
# include "jingxian/networks/commands/ReadCommand.h"


_jingxian_begin

ReadCommand::ReadCommand(ConnectedSocket* transport
						, char* ptr
						, size_t len )
: transport_(transport)
, ptr_(ptr)
, len_(len)
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
		transport_->onError(logging::Receive, error, _T("д����ʱ��������"));
		return;
	}
	else if (0 == bytes_transferred)
	{
		transport_->onError(logging::Receive, error, _T("��0���ֽ�"));
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
	if (::WSARecv(transport_->handle()
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
