
# include "pro_config.h"
# include "jingxian/Buffer/BaseBuffer.h"

_jingxian_begin

BaseBuffer::BaseBuffer()
: exceptionStyle_(ExceptionStyle::THROW)
, errno_(ERROR_SUCCESS)
, ptr_(0)
, len_(0)
{
	errno_ = ERROR_SUCCESS;
}

BaseBuffer:: ~BaseBuffer( )
{
}

int BaseBuffer::beginTranscation()
{
}

void BaseBuffer::rollbackTranscation(int)
{
}

void BaseBuffer::commitTranscation(int)
{
}

void BaseBuffer::exceptions(ExceptionStyle::type exceptionStyle)
{
	exceptionStyle_ = exceptionStyle;
}

ExceptionStyle::type BaseBuffer::exceptions() const
{
	return exceptionStyle_;
}

bool BaseBuffer::fail() const
{
	return ERROR_SUCCESS != errno_;
}

errcode_t BaseBuffer::lastError() const
{
	return errno_;
}

void BaseBuffer::clearError()
{
	errno_ = ERROR_SUCCESS;
}

_jingxian_end
