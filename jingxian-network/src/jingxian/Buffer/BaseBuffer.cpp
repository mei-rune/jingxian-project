
# include "pro_config.h"
# include "jingxian/Buffer/BaseBuffer.h"

_jingxian_begin

BaseBuffer::BaseBuffer()
: exceptionStyle_(ExceptionStyle::THROW)
, errno_(ERROR_SUCCESS)
{
	errno_ = ERROR_SUCCESS;
}

BaseBuffer:: ~BaseBuffer( )
{
}

int BaseBuffer::beginTranscation()
{
	return 0;
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

errcode_t BaseBuffer::error() const
{
	return errno_;
}

void BaseBuffer::error(errcode_t err)
{
	if( ERROR_SUCCESS == errno_)
		errno_ = err;

	if(ExceptionStyle::THROW == exceptionStyle_)
		ThrowException1(Exception, get_last_error(err));
}

void BaseBuffer::clearError()
{
	errno_ = ERROR_SUCCESS;
}

_jingxian_end
