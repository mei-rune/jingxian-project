
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

class IBuffer
{
public:
	virtual ~IBuffer(){}

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
	IOTranscationScope(IBuffer& buffer)
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
	IBuffer& buffer_;
	int transcationId_;
};

/**
 * InBuffer 从 IBuffer 一般来说应该是虚继承,但
 * 我不想搞得太复杂,在实现该类时请注意不要同时继
 * 承 OutBuffer;
 */
class IInBuffer : public IBuffer
{
public:
	virtual ~IInBuffer(void) {}

	virtual bool    readBoolean() = 0;
	virtual int8_t  readInt8() = 0;
	virtual int16_t readInt16() = 0;
	virtual int32_t readInt32() = 0;
	virtual int64_t readInt64() = 0;
	virtual void readBlob(void* blob, size_t len) = 0;

	/**
	 * 在 Buffer 中的数据的长度
	 */
	virtual size_t size() = 0;

	/**
	 * 查找指定的数据的第一次出现位置
	 */
	virtual size_t search(const void* context,size_t len) = 0;
	
	/**
	 * 查找指定的字符串中任意字符第一次出现位置
	 */
	virtual size_t searchAny(const char* charset) = 0;

	/**
	 * 查找指定的字符串中任意字符第一次出现位置
	 */
	virtual size_t searchAny(const wchar_t* charset) = 0;

	/**
	 * 取出 Buffer 中的所有数据块
	 * @params[ out ] len Buffer 中的数据块的数量
	 * @return 返回 Buffer 中的所有数据块的指针
	 */
	virtual const LPWSABUF GetBuffer(size_t* len) const = 0;
};

/**
 * OutBuffer 从 IBuffer 一般来说应该是虚继承,但
 * 我不想搞得太复杂,在实现该类时请注意不要同时继
 * 承 InBuffer;
 */
class IOutBuffer : public IBuffer
{
public:
	virtual ~IOutBuffer(void) {}

	virtual IOutBuffer& writeBoolean(bool value) = 0;
	virtual IOutBuffer& writeInt8(int8_t value) = 0;
	virtual IOutBuffer& writeInt16(int16_t value) = 0;
	virtual IOutBuffer& writeInt32(int32_t value) = 0;
	virtual IOutBuffer& writeInt64(const int64_t& value) = 0;
	virtual IOutBuffer& writeBlob(const void* blob, size_t len) = 0;
};

_jingxian_end

#endif //_buffer_h_