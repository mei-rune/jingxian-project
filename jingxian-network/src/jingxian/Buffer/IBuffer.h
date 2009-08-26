
#ifndef _buffer_h_
#define _buffer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <Mswsock.h>
# include <vector>
# include "jingxian/exception.hpp"

_jingxian_begin

# ifndef _io_mem_buf_buf_
# define _io_mem_buf_buf_
  typedef WSABUF io_mem_buf;
# endif //_io_mem_buf_buf_

# ifndef _io_packect_buf_
# define _io_packect_buf_
  typedef TRANSMIT_PACKETS_ELEMENT io_packect_buf;
# endif // ___iopack___

# ifndef _io_file_buf_
# define _io_file_buf_
  typedef TRANSMIT_FILE_BUFFERS io_file_buf;
# endif // _io_file_buf_

const int BUFFER_ELEMENT_MEMORY = 0;
const int BUFFER_ELEMENT_FILE = 1;
const int BUFFER_ELEMENT_PACKET = 2;

typedef void (*freebuffer_callback)(void* ptr, void* context);

typedef struct buffer_chain
{
	void* context;
	freebuffer_callback freebuffer;
	int type;
	buffer_chain* _next;
} buffer_chain_t;

typedef struct databuffer
{
	buffer_chain_t chain;
	// 内存块大小（可选值，为0时为无效值）
	size_t capacity;
	// 数据在 buf 的起始位置
	char* start;
	// 数据在 buf 的结束位置
    char* end;
    // 数据内存指针
    char ptr[1];
} databuffer_t;


typedef struct filebuffer
{
	buffer_chain_t chain;
	HANDLE file;
	DWORD  write_bytes;
	DWORD  bytes_per_send;
    TRANSMIT_FILE_BUFFERS buf;
} filebuffer_t;

typedef struct packetbuffer
{
	buffer_chain_t chain;
	
	DWORD element_count;
	DWORD send_size;

	TRANSMIT_PACKETS_ELEMENT packetArray[1];

} packetbuffer_t;

inline bool is_null(const freebuffer_callback cb)
{
	return NULL == cb;
}

inline void freebuffer(buffer_chain_t* buf)
{
	if(is_null(buf))
		return;

	if(is_null(buf->freebuffer))
		my_free(buf);
	else
		buf->freebuffer(buf, buf->context);
}

inline void freebuffer( databuffer_t* buf)
{
	if(is_null(buf))
		return;

	if(is_null(buf->chain.freebuffer))
		my_free(buf);
	else
		buf->chain.freebuffer(buf, buf->chain.context);
}

inline databuffer_t* databuffer_cast(buffer_chain_t* ptr)
{
	return (databuffer_t*)ptr;
}

inline filebuffer_t* filebuffer_cast(buffer_chain_t* ptr)
{
	return (filebuffer_t*)ptr;
}

inline packetbuffer_t* packetbuffer_cast(buffer_chain_t* ptr)
{
	return (packetbuffer_t*)ptr;
}

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

	enum 
	{
		npos = -1
	};

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
	virtual errcode_t error() const = 0;
	
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
	 * 向前读offest个字节
	 * @params[ int ] 位置移动的字节数
	 * @remarks  offest > 0 时则向前移动,offest < 0 时则向后移动, 当offest移动的位置超出范围时则移动开始或结束.
	 */
	virtual void seek(int offest) = 0;

	/**
	 * 在 Buffer 中的数据的长度
	 */
	virtual size_t size() const = 0;

	/**
	 * 查找指定的数据的第一次出现位置
	 */
	virtual size_t search(const void* context,size_t len) const = 0;

	/**
	 * 查找指定的数据的第一次出现位置
	 */
	virtual size_t search(char ch) const = 0;
	
	/**
	 * 查找指定的数据的第一次出现位置
	 */
	virtual size_t search(wchar_t ch) const = 0;
	
	/**
	 * 查找指定的字符串中任意字符第一次出现位置
	 */
	virtual size_t searchAny(const char* charset) const = 0;

	/**
	 * 查找指定的字符串中任意字符第一次出现位置
	 */
	virtual size_t searchAny(const wchar_t* charset) const = 0;

	/**
	 * 取出 Buffer 中的所有数据块
	 */
	virtual const std::vector<io_mem_buf>& rawBuffer() const = 0;

	/**
	 * 取出 Buffer 中的所有数据块的总长度
	 */
	virtual size_t rawLength() const = 0;
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