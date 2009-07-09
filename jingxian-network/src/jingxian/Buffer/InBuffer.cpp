# include "pro_config.h"
# include "jingxian/Buffer/InBuffer.h"


_jingxian_begin

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
	readBuffer((char*)blob, len);
}

size_t InBuffer::size()
{
	return size_;
}

_jingxian_end
