
# include "pro_config.h"
# include "jingxian/Buffer/OutBuffer.h"


_jingxian_begin

OutBuffer::OutBuffer()
{
}

OutBuffer::~OutBuffer()
{
	for(std::vector<buffer_chain_t*>::iterator it = dataBuffer_.begin()
		; it != dataBuffer_.end(); ++it)
		freebuffer(*it);
	dataBuffer_.clear();
}

IOutBuffer& OutBuffer::writeBoolean(bool value)
{
	int8_t tmp = value?1:0;
	writeBlob(&tmp,sizeof(tmp));
	return *this;
}
	
IOutBuffer& OutBuffer::writeInt8(int8_t value)
{
	writeBlob(&value,sizeof(value));
	return *this;
}

IOutBuffer& OutBuffer::writeInt16(int16_t value)
{
	writeBlob(&value,sizeof(value));
	return *this;
}

IOutBuffer& OutBuffer::writeInt32(int32_t value)
{
	writeBlob(&value,sizeof(value));
	return *this;
}

IOutBuffer& OutBuffer::writeInt64(const int64_t& value)
{
	writeBlob(&value,sizeof(value));
	return *this;
}

databuffer_t* OutBuffer::allocate(size_t len)
{
	databuffer_t* result = (databuffer_t*)my_calloc(1,sizeof(databuffer_t)+len);
	result->capacity = len;
	result->start = result->end = result->ptr;
	return result;
}

IOutBuffer& OutBuffer::writeBlob(const void* blob, size_t len)
{
	size_t exceptLen = len;
	const char* ptr = (const char*)blob;

	if(!dataBuffer_.empty())
	{
		databuffer_t* data = databuffer_cast(dataBuffer_[dataBuffer_.size() - 1]);
		if(!is_null(data))
		{
			size_t capacity = data->ptr + data->capacity - data->end;
			::memcpy(data->end, ptr, capacity);
			data->end += capacity;
			ptr += capacity;
			exceptLen -= capacity;
		}
	}

	size_t bytes = max(exceptLen, 100 );
	databuffer_t* data = allocate(bytes);
	dataBuffer_.push_back((buffer_chain_t*)data);

	memcpy(data->end, ptr, exceptLen);
	data->end += exceptLen;
	exceptLen = 0;

	return *this;
}

std::vector<buffer_chain_t*>& OutBuffer::dataBuffer()
{
	return dataBuffer_;
}

void OutBuffer::releaseBuffer()
{
	dataBuffer_.clear();
}

int OutBuffer::beginTranscation()
{
	ThrowException(NotImplementedException);
}

void OutBuffer::rollbackTranscation(int id)
{
	ThrowException(NotImplementedException);
}

void OutBuffer::commitTranscation(int id)
{
	ThrowException(NotImplementedException);
}

_jingxian_end
