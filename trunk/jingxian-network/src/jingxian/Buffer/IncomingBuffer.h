#ifndef _IncomingBuffer_H_
#define _IncomingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/buffer/IBuffer.H"

_jingxian_begin


class IncomingBuffer
{
public:
	IncomingBuffer(size_t capacity = 30);

	~IncomingBuffer();

	/**
	 * ��β�����һ�����е��ڴ��
	 * @remarks �ڴ������ݽ������
	 */
	void Push(databuffer_t* buf);

	/**
	 * ��ͷ��ȡһ���ڴ��
	 */
	databuffer_t* Pop();

	/**
	 * ȡ�� Buffer �������ڴ��,�Ա������
	 * @params[ out ] length ���ص�WSABUF���С,��ѡֵ
	 * @return ����������ݵ�WSABUF��,���һ��WSABUFָ��һ����null
	 */
	LPWSABUF GetReadBuffer(size_t* len = null_ptr);
	
	/**
	 * �� Buffer �ж����ݺ�,��������ʼָ�����
	 */
	size_t ReadBytes(size_t len);

	/**
	 * ���ݵ��ֽ���
	 */
	size_t TotalReadBytes() const;

	/**
	 * ȡ�� Buffer β�������ڴ��,�Ա���д����
	 * @params[ out ] len ���ص�WSABUF���С,��ѡֵ
	 * @return ����β�������ڴ��,���һ��WSABUFָ��һ����null
	 */
	LPWSABUF GetWriteBuffer(size_t* len = null_ptr);

	/**
	 * �� Buffer β�������ڴ��д���ݺ�,�����ݽ���ָ�����
	 */
	size_t WriteBytes(size_t len);

	/**
	 * ���ݵ��ֽ���
	 */
	size_t TotalWriteBytes() const;

private:
	NOCOPY(IncomingBuffer);
	size_t capacity_;

	databuffer_t** ptr_;
	size_t length_;

	LPWSABUF writePtr_;
	size_t writeLength_;
	size_t writeBytes_;

	LPWSABUF readPtr_;
	size_t readLength_;
	size_t readBytes_;
};

_jingxian_end

#endif //_IncomingBuffer_H_