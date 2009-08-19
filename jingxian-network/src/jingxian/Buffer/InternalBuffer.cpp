
# include "pro_config.h"
# include "jingxian/Buffer/BaseBuffer.h"
# include "jingxian/Link.h"

_jingxian_begin

InternalBuffer::InternalBuffer()
: head_(0)
, tail_(0)
{
}

InternalBuffer::~InternalBuffer()
{
	while(is_null(head_))
	{
		buffer_chain_t* current = head_;
		head_= (_head)->_next;

		freebuffer(current);
	}
}

void InternalBuffer::push(buffer_chain_t* newbuf)
{
	switch( newbuf->type)
	{
	case BUFFER_ELEMENT_MEMORY:
		databuffer_t* data = (databuffer_cast(newbuf);
		assert( data->ptr <= data->start );
		assert( data->start <= data->end);
		assert( data->end <= data->ptr + data->capacity);
		break;
	case BUFFER_ELEMENT_FILE:
		// TODO: 加入对文件的支持
		filebuffer_t* filebuf = filebuffer_cast(newbuf);
		assert( false );
		break;
	case BUFFER_ELEMENT_PACKET:
		// TODO: 加入对文件的支持
		packetbuffer_t* packetbuf = packetbuffer_cast(newbuf);
		assert( false );
		break;
	default:
		assert( false );
		break;
	}
	
	newbuf->_next = NULL;
	if( is_null(head_))
		head_ = newbuf;
	else
		tail_->_next = newbuf;
	tail_ = newbuf;
}

buffer_chain_t* InternalBuffer::pop()
{
	buffer_chain_t* current = head_;

	if(!is_null(head_))
		head_ = head_->_next;
	else
		tail_ = NULL;
	return current;
}

buffer_chain_t* InternalBuffer::next(buffer_chain_t* current)
{
	return is_null(current)?head_:current->_next;
}

const buffer_chain_t* InternalBuffer::next(const buffer_chain_t* current)const 
{
	return is_null(current)?head_:current->_next;
}

bool InternalBuffer::empty() const
{
	return is_null(head_);
}

_jingxian_end
