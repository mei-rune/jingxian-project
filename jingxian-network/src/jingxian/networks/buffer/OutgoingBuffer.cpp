
# include "pro_config.h"
# include "jingxian/networks/buffer/OutgoingBuffer.h"
# include "jingxian/networks/buffer/buffer-internal.h"
# include "jingxian/networks/commands/WriteCommand.H"
# include "jingxian/networks/ConnectedSocket.h"

_jingxian_begin

OutgoingBuffer::OutgoingBuffer()
        : connectedSocket_(null_ptr)
{
}

OutgoingBuffer::~OutgoingBuffer()
{
}

void OutgoingBuffer::initialize(ConnectedSocket* connectedSocket)
{
    connectedSocket_ = connectedSocket;
}

void OutgoingBuffer::send(buffer_chain_t* buf)
{
    assertBuffer(buf);
    buffer_.push(buf);
}

ICommand* OutgoingBuffer::makeCommand()
{
    buffer_chain_t* current = buffer_.next(null_ptr);
    if (is_null(current))
        return null_ptr;

    if (bufferOP::isMemory(current))
    {
        std::auto_ptr<WriteCommand> command(new WriteCommand(connectedSocket_));
        io_mem_buf iobuf;
        do
        {
            iobuf.buf = bufferOP::rd_ptr(current);
            iobuf.len = bufferOP::rd_length(current);

            assert(0 <= iobuf.len);
            if (0 < iobuf.len)
                command->iovec().push_back(iobuf);
        }
        while (null_ptr != (current = buffer_.next(current))
                && bufferOP::isMemory(current));

        if (command->iovec().empty())
            return null_ptr;

        return command.release();
    }

    return bufferOP::makeCommand(current, false);
}

bool OutgoingBuffer::clearBytes(size_t len)
{
    size_t exceptLen = len;
    buffer_chain_t* current = null_ptr;
    while (null_ptr != (current = buffer_.head()))
    {
        if (!bufferOP::isMemory(current))
        {
            freebuffer(buffer_.pop());
            return true;
        }

        size_t dataLen = bufferOP::rd_length(current);
        if (dataLen >= exceptLen)
        {
            bufferOP::rd_ptr(current, exceptLen);
            if (dataLen == exceptLen)
                freebuffer(buffer_.pop());

            exceptLen = 0;
            return false;
        }

        bufferOP::rd_ptr(current, dataLen);
        exceptLen -= dataLen;

        freebuffer(buffer_.pop());
    }
    return (0 == exceptLen);
}

void assertBuffer(buffer_chain_t* newbuf)
{
    switch (newbuf->type)
    {
    case BUFFER_ELEMENT_MEMORY:
    {
        databuffer_t* data = (databuffer_t*)newbuf;
        assert(data->ptr <= data->start);
        assert(data->start <= data->end);
        assert(data->end <= data->ptr + data->capacity);
        break;
    }
    case BUFFER_ELEMENT_FILE:
    {
        // TODO: 加入对文件的支持
        filebuffer_t* filebuf = (filebuffer_t*)newbuf;
        assert(false);
        break;
    }
    case BUFFER_ELEMENT_PACKET:
    {
        // TODO: 加入对文件的支持
        packetbuffer_t* packetbuf = (packetbuffer_t*)newbuf;
        assert(false);
        break;
    }
    default:
        assert(false);
        break;
    }
}

_jingxian_end
