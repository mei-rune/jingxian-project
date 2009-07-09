# include "pro_config.h"
# include "jingxian/Buffer/InBuffer.h"


_jingxian_begin

InBuffer::InBuffer(LPWSABUF ptr, size_t count, size_t totalLength)
: totalLength_(totalLength)
, readLength_(0)
, ptr_(ptr)
, size_(count)
, current_(ptr)
, currentPtr_((null_ptr==ptr)?null_ptr:ptr->buf)
, currentLength_((null_ptr==ptr)?0:ptr->len)
{
}

InBuffer::~InBuffer(void)
{
}

bool InBuffer::readBoolean()
{
	return 0 != readInt8();
}

int8_t InBuffer::readInt8()
{
	int8_t value = 0;
	readBlob((char*)&value, sizeof(value));
	return value;
}

int16_t InBuffer::readInt16()
{
	int16_t value = 0;
	readBlob((char*)&value, sizeof(value));
	return value;
}

int32_t InBuffer::readInt32()
{
	int32_t value = 0;
	readBlob((char*)&value, sizeof(value));
	return value;
}

int64_t InBuffer::readInt64()
{
	int64_t value = 0;
	readBlob((char*)&value, sizeof(value));
	return value;
}

void InBuffer::readBlob(void* blob, size_t len)
{
	if(currentLength_>=len)
	{
		memcpy(blob, currentPtr_, len);
		currentPtr_ += len;
		currentLength_ -= len;
		readLength_ += len;
		return ;
	}

	if((totalLength_ - readLength_)<len)
	{
		BaseBuffer::error(ERROR_HANDLE_EOF);
		return;
	}

	char* ptr = (char*)blob;
	size_t count = len;
	do
	{
		if(currentLength_>count)
		{
			memcpy(ptr, currentPtr_, count);
			currentPtr_ += count;
			currentLength_ -= count;
			break;
		}

		memcpy(ptr, currentPtr_, currentLength_);
		count -= currentLength_;
		ptr += currentLength_;

		if((ptr_ + size_) <= ++current_)
		{
			current_ = null_ptr;
			currentPtr_ = null_ptr;
			currentLength_ = 0;
			break;
		}
		else
		{
			currentPtr_ = current_->buf;
			currentLength_ = current_->len;
		}
	}
	while(0<count);
	readLength_ += len;
}

size_t InBuffer::size()
{
	return totalLength_ - readLength_;
}

_jingxian_end
