
# include "pro_config.h"
# include "jingxian/networks/buffer/IncomingBuffer.h"
# include "jingxian/networks/buffer/buffer-internal.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/networks/commands/ReadCommand.H"

_jingxian_begin

IncomingBuffer::IncomingBuffer()
        : connectedSocket_(null_ptr)
        , current_(null_ptr)
{
}

IncomingBuffer::~IncomingBuffer()
{
}

void IncomingBuffer::initialize(ConnectedSocket* connectedSocket)
{
    connectedSocket_ = connectedSocket;
}

ICommand* IncomingBuffer::makeCommand()
{
    std::auto_ptr<ReadCommand> command;

    buffer_chain_t* current = dataBuffer_.next(current_);
    if (!is_null(current))
    {
        if (!bufferOP::isMemory(current))
            return bufferOP::makeCommand(current, true);

        command.reset(new ReadCommand(connectedSocket_));
        io_mem_buf tmp;

        do
        {
            tmp.buf = bufferOP::wd_ptr(current);
            tmp.len = bufferOP::wd_length(current);

            assert(tmp.len >= 0);
            if (tmp.len > 0)
                command->iovec().push_back(tmp);
        }
        while (null_ptr != (current = dataBuffer_.next(current))
                && bufferOP::isMemory(current));
    }
    else
    {
        command.reset(new ReadCommand(connectedSocket_));
    }

    if (command->iovec().empty())
    {
        if (null_ptr != current)
            return bufferOP::makeCommand(current, true);

        buffer_chain_t* ptr = connectedSocket_->allocateProtocolBuffer();
        dataBuffer_.push(ptr);

        if (!bufferOP::isMemory(ptr))
            return bufferOP::makeCommand(ptr, true);

        io_mem_buf tmp;
        tmp.buf = bufferOP::wd_ptr(ptr);
        tmp.len = bufferOP::wd_length(ptr);
        assert(tmp.len >= 0);
        command->iovec().push_back(tmp);

    }

    return command.release();
}

bool IncomingBuffer::increaseBytes(size_t len)
{
    size_t exceptLen = len;

    buffer_chain_t* current = current_;
    buffer_chain_t* last = current_;

    while (null_ptr != (current = dataBuffer_.next(current)))
    {
        size_t bytes = bufferOP::wd_length(current);

        if (bytes >= exceptLen)
        {
            bufferOP::wd_ptr(current, exceptLen);
            current_ = last;
            return true;
        }

        bufferOP::wd_ptr(current, bytes);
        exceptLen -= bytes;
        last = current;
    }
    return false;
}

bool IncomingBuffer::decreaseBytes(size_t len)
{
    size_t exceptLen = len;
    buffer_chain_t* current = null_ptr;

    while (null_ptr != (current = dataBuffer_.head()))
    {
        size_t dataLen = bufferOP::rd_length(current);
        if (current_ == current)
            current_ = null_ptr;

        if (dataLen >= exceptLen)
        {
            bufferOP::rd_ptr(current, exceptLen);
            dataLen -= exceptLen;
            exceptLen = 0;

            size_t capacity = bufferOP::wd_length(current);//(data->ptr + data->capacity) - data->end;

            if (0 == dataLen && 0 == capacity)
                freebuffer(dataBuffer_.pop());
            return true;
        }

        bufferOP::rd_ptr(current, dataLen);
        exceptLen -= dataLen;

        if (is_null(current_))
            break;

        freebuffer(dataBuffer_.pop());
    }

    return (0 == exceptLen);
}


void IncomingBuffer::dataBuffer(std::vector<io_mem_buf>& buf)
{
    io_mem_buf tmp;

    buffer_chain_t* current = null_ptr;
    while (null_ptr != (current = dataBuffer_.next(current)))
    {
        tmp.buf = bufferOP::rd_ptr(current);
        tmp.len = bufferOP::rd_length(current);

        buf.push_back(tmp);

        if (current_ == current)
            break;
    }
}

const linklist<buffer_chain_t>& IncomingBuffer::buffer() const
{
    return dataBuffer_;
}

const buffer_chain_t* IncomingBuffer::current() const
{
    return current_;
}

_jingxian_end
