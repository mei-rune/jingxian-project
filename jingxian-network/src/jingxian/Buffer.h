
#ifndef _buffer_h_
#define _buffer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

namespace ExceptionStyle
{
	enum type
	{
		  THROW
		, NOTHROW
	};
}

class BaseBuffer
{
public:
	virtual ~BaseBuffer(){}

	/**
	 * 当从流中读或写数据时,指针当前位置也向前移动,启动事
	 * 务则会记住指针的当前位置,如果回滚则会将指针移动到
	 * 事务启始位置.
	 *
	 * 启动一个事务
	 */
	virtual int beginTranscation() = 0;
	/**
	 * 回滚一个事务
	 */
	virtual void rollbackTranscation(int) = 0;
	
	/**
	 * 提交一个事务
	 */
	virtual void commitTranscation(int) = 0;
	
	/**
	 * 设置流在发生错误时是否抛出异常.
	 */
	virtual void exceptions(ExceptionStyle::type exceptionStyle) = 0;
	
	/**
	 * 取得流在发生错误时处理方式.
	 */
	virtual ExceptionStyle::type exceptions() const = 0;

	/**
	 * 当前流是否处于错误状态
	 */
	virtual bool fail() const = 0;

	/**
	 * 取得错误号
	 */
	virtual errcode_t lastError() const = 0;
	
	/**
	 * 清除当前流的错误状态
	 */
	virtual void clearError() = 0;
};


/**
 * 当从流中读或写数据时,指针当前位置也向前移动,启动事
 * 务则会记住指针的当前位置,如果回滚则会将指针移动到
 * 事务启始位置.
 */
class IOTranscationScope
{
public:
	IOTranscationScope(BaseBuffer& buffer)
		: buffer_(buffer)
		, transcationId_(buffer.beginTranscation())
	{
	}
	
	~IOTranscationScope()
	{
		if(-1 != transcationId_)
			return;
		buffer_.rollbackTranscation(transcationId_);
	}

	void commit()
	{
		if(-1 != transcationId_)
			return;
		buffer_.commitTranscation(transcationId_);
		transcationId_ = -1;
	}

private:
	BaseBuffer& buffer_;
	int transcationId_;
};

/**
 * InBuffer 从 BaseBuffer 一般来说应该是虚继承,但
 * 我不想搞得太复杂,在实现该类时请注意不要同时继
 * 承 OutBuffer;
 */
class InBuffer : public BaseBuffer
{
public:
	virtual ~InBuffer(void) {}

	virtual int8_t readInt8() = 0;
	virtual int16_t readInt16() = 0;
	virtual int32_t readInt32() = 0;
	virtual int64_t readInt64() = 0;
	virtual void readBlob(char* blob, int32_t* len) = 0;

	virtual size_t size() = 0;
};

/**
 * OutBuffer 从 BaseBuffer 一般来说应该是虚继承,但
 * 我不想搞得太复杂,在实现该类时请注意不要同时继
 * 承 InBuffer;
 */
class OutBuffer : public BaseBuffer
{
public:
	virtual ~OutBuffer(void) {}

	virtual InBuffer& writeInt8(int8_t value);
	virtual InBuffer& writeInt16(int16_t value);
	virtual InBuffer& writeInt32(int32_t value);
	virtual InBuffer& writeInt64(const int64_t& value);
	virtual InBuffer& writeBlob(const char* blob, int32_t len);
};

_jingxian_end

#endif //_buffer_h_