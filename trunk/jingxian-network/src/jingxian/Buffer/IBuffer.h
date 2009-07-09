
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
	virtual errcode_t lastError() const = 0;
	
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
	 * �� Buffer �е����ݵĳ���
	 */
	virtual size_t size() = 0;

	/**
	 * ����ָ�������ݵĵ�һ�γ���λ��
	 */
	virtual size_t search(const void* context,size_t len) = 0;
	
	/**
	 * ����ָ�����ַ����������ַ���һ�γ���λ��
	 */
	virtual size_t searchAny(const char* charset) = 0;

	/**
	 * ����ָ�����ַ����������ַ���һ�γ���λ��
	 */
	virtual size_t searchAny(const wchar_t* charset) = 0;

	/**
	 * ȡ�� Buffer �е��������ݿ�
	 * @params[ out ] len Buffer �е����ݿ������
	 * @return ���� Buffer �е��������ݿ��ָ��
	 */
	virtual const LPWSABUF GetBuffer(size_t* len) const = 0;
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