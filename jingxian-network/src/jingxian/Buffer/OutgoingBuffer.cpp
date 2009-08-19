
# include "pro_config.h"
# include "jingxian/Buffer/OutgoingBuffer.h"
# include "jingxian/Link.h"

_jingxian_begin

OutgoingBuffer::OutgoingBuffer(ConnectedSocket* connectedSocket)
: connectedSocket_(connectedSocket)
{
}

OutgoingBuffer::~OutgoingBuffer()
{
}

ICommand* OutgoingBuffer::makeCommand()
{
	buffer_chain_t* current = this->next(null_ptr);
	if(is_null(current))
		return null_ptr;

	switch( current->type)
	{
	case BUFFER_ELEMENT_MEMORY:
		WriteCommand* command = new WriteCommand(connectedSocket_);
		do
		{
			databuffer_t* data = (databuffer_cast(current);
			io_mem_buf iobuf;

			iobuf.buf = data->start;
			iobuf.len = data->end - data->start;
			command->iovec().push_back(iobuf);
		}
		while(null_ptr != (current = next(current))
			&& 0 == current->type);
		return command;
	case BUFFER_ELEMENT_FILE:
		// TODO: 加入对文件的支持
		filebuffer_t* filebuf = filebuffer_cast(newbuf);
		assert( false );
		return null_ptr;
	case BUFFER_ELEMENT_PACKET:
		// TODO: 加入对文件的支持
		packetbuffer_t* packetbuf = packetbuffer_cast(newbuf);
		assert( false );
		return null_ptr;
	default:
		assert( false );
		return null_ptr;
	}
}

bool OutgoingBuffer::clearBytes(size_t len)
{
	size_t exceptLen = len;
	buffer_chain_t* current = null_ptr;
	while(null_ptr != (current = next(current)))
	{	
		switch( current->type)
		{
		case BUFFER_ELEMENT_MEMORY:

			databuffer_t* data = (databuffer_cast(current);
			size_t dataLen = data->end - data->start;

			if( dataLen >= exceptLen)
			{
				data->start += exceptLen;
				exceptLen = 0;
				return false;
			}

			data->start += dataLen;
			exceptLen -= dataLen;
			break;
		case BUFFER_ELEMENT_FILE:
			// TODO: 加入对文件的支持
			filebuffer_t* filebuf = filebuffer_cast(newbuf);
			assert( false );
			return false;
		case BUFFER_ELEMENT_PACKET:
			// TODO: 加入对文件的支持
			packetbuffer_t* packetbuf = packetbuffer_cast(newbuf);
			assert( false );
			return false;
		default:
			assert( false );
			return false;
		}
	}

	return (0 == exceptLen)
}

_jingxian_end
