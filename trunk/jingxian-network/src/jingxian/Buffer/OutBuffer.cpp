
# include "pro_config.h"
# include "jingxian/Buffer/OutBuffer.h"


_jingxian_begin

OutBuffer::~OutBuffer()
{
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

IOutBuffer& OutBuffer::writeBlob(const void* blob, size_t len)
{
	ThrowException(NotImplementedException);
	return *this;
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
