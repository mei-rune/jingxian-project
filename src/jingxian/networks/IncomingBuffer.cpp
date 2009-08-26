
# include "pro_config.h"
# include "jingxian/networks/IncomingBuffer.h"
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
	io_mem_buf tmp;
	std::auto_ptr<ReadCommand> command(new ReadCommand(connectedSocket_));
	
	buffer_chain_t* current = current_;
	while(null_ptr != (current = dataBuffer_.next(current)))
	{
		databuffer_t* buf = databuffer_cast(current);

		tmp.buf = buf->end;
		tmp.len = (buf->ptr + buf->capacity) - buf->end;
		
		assert( tmp.len >= 0);
		if(tmp.len > 0)
			command->iovec().push_back(tmp);
	}

	if(command->iovec().empty())
	{
		databuffer_t* ptr = databuffer_cast(connectedSocket_->allocateProtocolBuffer());
		dataBuffer_.push((buffer_chain_t*)ptr);
		
		tmp.buf = ptr->end;
		tmp.len = (ptr->ptr + ptr->capacity) - ptr->end;
		command->iovec().push_back(tmp);
	}

	return command.release();
}

bool IncomingBuffer::increaseBytes(size_t len)
{
	size_t exceptLen = len;

	buffer_chain_t* current = current_;
	buffer_chain_t* last = current_;

	while(null_ptr != (current = dataBuffer_.next(current)))
	{
		databuffer_t* buf = databuffer_cast(current);
		size_t bytes = (buf->ptr + buf->capacity) - buf->end;

		if( bytes >= exceptLen )
		{
			buf->end += exceptLen;
			current_ = last;
			return true;
		}
		buf->end += bytes;
		exceptLen -= bytes;
		last = current;
	}
	return false;
}

bool IncomingBuffer::decreaseBytes(size_t len)
{
	size_t exceptLen = len;
	buffer_chain_t* current = null_ptr;
	
	while(null_ptr != (current = dataBuffer_.head()))
	{
		databuffer_t* data = databuffer_cast(current);
		size_t dataLen = data->end - data->start;
		
		if( current_ == current)
			current_ = null_ptr;


		if( dataLen >= exceptLen)
		{
			data->start += exceptLen;
			dataLen -= exceptLen;
			exceptLen = 0;
			
			size_t capacity = (data->ptr + data->capacity) - data->end;
			
			if( current_ == current)
				current_ = null_ptr;

			if(0 == dataLen && 0 == capacity)
				freebuffer(dataBuffer_.pop());
			return true;
		}

		data->start += dataLen;
		exceptLen -= dataLen;

		if( current_ == current)
			current_ = null_ptr;
		else if(is_null(current_))
			break;

		freebuffer(dataBuffer_.pop());
	}

	return (0 == exceptLen);
}


void IncomingBuffer::dataBuffer(std::vector<io_mem_buf>& buf)
{	
	io_mem_buf tmp;

	buffer_chain_t* current = null_ptr;
	while(null_ptr != (current = dataBuffer_.next(current)))
	{
		databuffer_t* data = databuffer_cast(current);
		tmp.buf = data->start;
		tmp.len = data->end - data->start;

		buf.push_back(tmp);

		if( current_ == current)
			break;
	}
}

const Buffer<buffer_chain_t>& IncomingBuffer::buffer() const
{
	return dataBuffer_;
}

const buffer_chain_t* IncomingBuffer::current() const
{
	return current_;
}

_jingxian_end
