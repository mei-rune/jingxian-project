
# include "pro_config.h"
# include "jingxian/networks/commands/ReadCommand.h"


_jingxian_begin

ReadCommand::ReadCommand(ConnectedSocket* transport)
        : transport_(transport)
{
}

ReadCommand::~ReadCommand()
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
        tstring err = ::concat<tstring>(_T("读数据时发生错误 - "), lastError(error));
        transport_->onError(*this, transport_mode::Receive, error, err);
        return;
    }
    else if (0 == bytes_transferred)
    {
        transport_->onError(*this, transport_mode::Receive, error, _T("对方主动关闭!"));
        return;
    }
    else
    {
        transport_->onRead(*this, bytes_transferred);
    }
}

bool ReadCommand::execute()
{
    DWORD bytesTransferred;
    DWORD flags = 0;

    assert(iovec_.size() > 0);
    assert(iovec_[0].len > 0);

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
