
#ifndef _InternalBuffer_H_
#define _InternalBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/buffer/IBuffer.H"

_jingxian_begin

typedef struct databuffer
{
	struct databuffer* _next;

	// �ڴ���С����ѡֵ��Ϊ0ʱΪ��Чֵ��
	size_t capacity;
	//������ʼλ��
	char* begin;
	//���ݽ���λ��
	char* end;
    // �����ڴ�ָ��
    char ptr[1];
} databuffer_t;

class InternalBuffer
{
public:
	InternalBuffer(size_t bufLength, size_t capacity);

	~InternalBuffer();

	
	/**
	 * �� Buffer �ж����ݺ�,��������ʼָ�����
	 */
	void DecreaseBytes(size_t len);

	/**
	 * �� Buffer β�������ڴ��д���ݺ�,�����ݽ���ָ�����
	 */
	void IncreaseBytes(size_t len);

	/**
	 * ���ݵ��ֽ���
	 */
	size_t TotalBytes() const;

	/**
	 * ���ͷ�������ݿ�
	 */
	void Crunch();

	/**
	 * ȡ�� Buffer �������ڴ��,�Ա������
	 * @params[ in,out ] iovec  WSABUFָ��
	 * @params[ in ] WSABUF���С
	 * @return ����������ݺ��WSABUF����
	 */
	LPWSABUF GetBuffer();

	/**
	 * ȡ�� Buffer �������ڴ��,�Ա������
	 * @params[ in,out ] iovec  WSABUFָ��
	 * @params[ in ] WSABUF���С
	 * @return ����������ݺ��WSABUF����
	 */
	LPWSABUF GetBuffer();

	/**
	 * ȡ�� Buffer β�������ڴ��,�Ա���д����
	 * @params[ in,out ] iovec  WSABUFָ��
	 * @params[ in ] length WSABUF���С
	 * @params[ in ] capacity �����ڴ����ֽ�����С�ֽ���
	 * @return ����������ݺ��WSABUF����
	 */
	LPWSABUF GetFreeBuffer(LPWSABUF iovec);


	size_t GetFreeBufferSize() const;
private:
	NOCOPY(InternalBuffer);
	size_t bufLength_;

	databuffer_t* ptr_;
	size_t capacity_;
	size_t length_;

	size_t bufBytes_;
};

_jingxian_end

#endif //_InternalBuffer_H_