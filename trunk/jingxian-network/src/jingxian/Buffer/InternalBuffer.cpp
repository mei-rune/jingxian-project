
# include "pro_config.h"
# include "jingxian/Buffer/BaseBuffer.h"


_jingxian_begin

InternalBuffer::InternalBuffer(size_t bufLength, size_t capacity)
: bufLength_(bufLength)
, ptr_(0)
, capacity_(0)
, length_(0)
{
}

InternalBuffer::~InternalBuffer()
{
}

_jingxian_end
