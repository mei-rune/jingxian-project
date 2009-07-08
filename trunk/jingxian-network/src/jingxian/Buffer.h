
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
 * InBuffer �� BaseBuffer һ����˵Ӧ������̳�,��
 * �Ҳ�����̫����,��ʵ�ָ���ʱ��ע�ⲻҪͬʱ��
 * �� OutBuffer;
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
 * OutBuffer �� BaseBuffer һ����˵Ӧ������̳�,��
 * �Ҳ�����̫����,��ʵ�ָ���ʱ��ע�ⲻҪͬʱ��
 * �� InBuffer;
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