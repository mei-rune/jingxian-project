
# include "pro_config.h"
# include "jingxian/Buffer/IncomingBuffer.h"


_jingxian_begin

IncomingBuffer::IncomingBuffer(ConnectedSocket* connectedSocket)
: connectedSocket_(connectedSocket)
{
}

IncomingBuffer::~IncomingBuffer()
{
}

ICommand* IncomingBuffer::makeCommand()
{	
	ReadCommand* command = new ReadCommand(connectedSocket_);

	buffer_chain_t* current = null_ptr;
	while(null_ptr != (current = next(current)))
	{	
	}
}

bool IncomingBuffer::clearBytes(size_t len)
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
