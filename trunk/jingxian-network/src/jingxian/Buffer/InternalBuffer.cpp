
# include "pro_config.h"
# include "jingxian/Buffer/BaseBuffer.h"


_jingxian_begin

InternalBuffer::InternalBuffer(size_t bufLength, size_t capacity)
: bufLength_(bufLength)
, ptr_(0)
, capacity_(0)
, length_(0)
{
	bufLength capacity
}

InternalBuffer::~InternalBuffer()
{
}

InternalBuffer::Length()
{
	return length_;
}

void InternalBuffer::Crunch()
{
}

size_t InternalBuffer::GetBuffer(LPWSABUF iovec, size_t len)
{
}

void InternalBuffer::DecreaseBuffer(size_t len)
{
}

size_t InternalBuffer::GetFreeBuffer(LPWSABUF iovec, size_t length, size_t capacity)
{
}

void InternalBuffer::IncreaseBuffer(size_t len)
{
}

_jingxian_end
