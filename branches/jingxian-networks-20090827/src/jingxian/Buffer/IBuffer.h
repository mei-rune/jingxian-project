
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
	// �ڴ���С����ѡֵ��Ϊ0ʱΪ��Чֵ��
	size_t capacity;
	// ������ buf ����ʼλ��
	char* start;
	// ������ buf �Ľ���λ��
    char* end;
    // �����ڴ�ָ��
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
	 * �������ж���д����ʱ,ָ�뵱ǰλ��Ҳ��ǰ�ƶ�,������
	 * ������סָ��ĵ�ǰλ��,����ع���Ὣָ���ƶ���
	 * ������ʼλ��.
	 *
	 * ����һ������
	 */
	virtual int beginTranscation() = 0;
	/**
	 * �ع�һ������
	 */
	virtual void rollbackTranscation(int) = 0;
	
	/**
	 * �ύһ������
	 */
	virtual void commitTranscation(int) = 0;
	
	/**
	 * �������ڷ�������ʱ�Ƿ��׳��쳣.
	 */
	virtual void exceptions(ExceptionStyle::type exceptionStyle) = 0;
	
	/**
	 * ȡ�����ڷ�������ʱ����ʽ.
	 */
	virtual ExceptionStyle::type exceptions() const = 0;

	/**
	 * ��ǰ���Ƿ��ڴ���״̬
	 */
	virtual bool fail() const = 0;

	/**
	 * ȡ�ô����
	 */
	virtual errcode_t error() const = 0;
	
	/**
	 * �����ǰ���Ĵ���״̬
	 */
	virtual void clearError() = 0;
};


/**
 * �������ж���д����ʱ,ָ�뵱ǰλ��Ҳ��ǰ�ƶ�,������
 * ������סָ��ĵ�ǰλ��,����ع���Ὣָ���ƶ���
 * ������ʼλ��.
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
 * InBuffer �� IBuffer һ����˵Ӧ������̳�,��
 * �Ҳ�����̫����,��ʵ�ָ���ʱ��ע�ⲻҪͬʱ��
 * �� OutBuffer;
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
	 * ��ǰ��offest���ֽ�
	 * @params[ int ] λ���ƶ����ֽ���
	 * @remarks  offest > 0 ʱ����ǰ�ƶ�,offest < 0 ʱ������ƶ�, ��offest�ƶ���λ�ó�����Χʱ���ƶ���ʼ�����.
	 */
	virtual void seek(int offest) = 0;

	/**
	 * �� Buffer �е����ݵĳ���
	 */
	virtual size_t size() const = 0;

	/**
	 * ����ָ�������ݵĵ�һ�γ���λ��
	 */
	virtual size_t search(const void* context,size_t len) const = 0;

	/**
	 * ����ָ�������ݵĵ�һ�γ���λ��
	 */
	virtual size_t search(char ch) const = 0;
	
	/**
	 * ����ָ�������ݵĵ�һ�γ���λ��
	 */
	virtual size_t search(wchar_t ch) const = 0;
	
	/**
	 * ����ָ�����ַ����������ַ���һ�γ���λ��
	 */
	virtual size_t searchAny(const char* charset) const = 0;

	/**
	 * ����ָ�����ַ����������ַ���һ�γ���λ��
	 */
	virtual size_t searchAny(const wchar_t* charset) const = 0;

	/**
	 * ȡ�� Buffer �е��������ݿ�
	 */
	virtual const std::vector<io_mem_buf>& rawBuffer() const = 0;

	/**
	 * ȡ�� Buffer �е��������ݿ���ܳ���
	 */
	virtual size_t rawLength() const = 0;
};

/**
 * OutBuffer �� IBuffer һ����˵Ӧ������̳�,��
 * �Ҳ�����̫����,��ʵ�ָ���ʱ��ע�ⲻҪͬʱ��
 * �� InBuffer;
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