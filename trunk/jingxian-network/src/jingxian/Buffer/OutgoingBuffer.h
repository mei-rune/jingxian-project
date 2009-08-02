
#ifndef _OutgoingBuffer_H_
#define _OutgoingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/buffer/IBuffer.H"

_jingxian_begin


class OutgoingBuffer
{
public:
	OutgoingBuffer(size_t capacity = 30);

	~OutgoingBuffer();

	/**
	 * ��β�����һ�����ݿ�
	 */
	void Send(databuffer_t* buf, size_t len);

	/**
	 * ȡ�� Buffer �������ڴ��,�Ա������
	 * @params[ out ] length ���ص�WSABUF���С,��ѡֵ
	 * @return ����������ݵ�WSABUF��,���һ��WSABUFָ��һ����null
	 */
	LPWSABUF GetBuffer(size_t* len = null_ptr);
	
	/**
	 * �� Buffer �ж����ݺ�,��������ʼָ�����
	 */
	size_t bytes(size_t len);

	/**
	 * ���ݵ��ֽ���
	 */
	size_t bytes() const;

private:
	NOCOPY(OutgoingBuffer);
	size_t capacity_;

	databuffer_t** ptr_;
	LPWSABUF readPtr_;

	size_t length_;
	size_t readBytes_;
};

_jingxian_end

#endif //_OutgoingBuffer_H_
