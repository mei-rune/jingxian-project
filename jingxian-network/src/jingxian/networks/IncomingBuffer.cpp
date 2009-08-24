
# include "pro_config.h"
# include "jingxian/networks/IncomingBuffer.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/networks/commands/ReadCommand.H"

_jingxian_begin

IncomingBuffer::IncomingBuffer()
: connectedSocket_(null_ptr)
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
	ReadCommand* command = new ReadCommand(connectedSocket_);

	//buffer_chain_t* current = null_ptr;
	//while(null_ptr != (current = this->next(current)))
	//{	
	//}
	return null_ptr;
}

bool IncomingBuffer::clearBytes(size_t len)
{
	size_t exceptLen = len;
	buffer_chain_t* current = null_ptr;
	while(null_ptr != (current = freeBuffer_.next(current)))
	{	
		switch( current->type)
		{
		case BUFFER_ELEMENT_MEMORY:
			{
			databuffer_t* data = databuffer_cast(current);
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
			}
		case BUFFER_ELEMENT_FILE:
			{
				// TODO: 加入对文件的支持
			filebuffer_t* filebuf = filebuffer_cast(current);
			assert( false );
			return false;
			}
		case BUFFER_ELEMENT_PACKET:
			{
				// TODO: 加入对文件的支持
			packetbuffer_t* packetbuf = packetbuffer_cast(current);
			assert( false );
			return false;
			}
		default:
			assert( false );
			return false;
		}
	}

	return (0 == exceptLen);
}

_jingxian_end
