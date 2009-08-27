
#ifndef _Buffer_Internal_H_
#define _Buffer_Internal_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

const int BUFFER_ELEMENT_MEMORY = 0;
const int BUFFER_ELEMENT_FILE = 1;
const int BUFFER_ELEMENT_PACKET = 2;


template<typename T>
class Buffer
{
public:
	Buffer()
		: head_(0)
		, tail_(0)
		, length_(0)
	{
	}

	/**
	 * ��������
	 */
	~Buffer()
	{
		while(null_ptr != head_)
		{
			T* current = head_;
			head_= (head_)->_next;

			freebuffer(current);
		}
	}

	T* head()
	{
		return head_;
	}

	T* tail()
	{
		return is_null(head_)? null_ptr : tail_;
	}

	/**
	 * ��β�����һ�����е��ڴ��
	 */
	void push(T* newbuf)
	{
		newbuf->_next = NULL;
		if( is_null(head_))
			head_ = newbuf;
		else
			tail_->_next = newbuf;
		tail_ = newbuf;
		++ length_;
	}

	/**
	 * ��ͷ��ȡһ���ڴ��
	 */
	T* pop()
	{
		T* current = head_;

		if(null_ptr != head_)
		{
			-- length_;
			head_ = head_->_next;
		}
		else
		{
			length_ = 0;
			tail_ = NULL;
		}		
		return current;
	}

	/**
	 * ���ڱ���buf,
	 */
	T* next(T* current)
	{
		return is_null(current)?head_:current->_next;
	}

	/**
	 * ���ڱ���buf,v
	 */
	const T* next(const T* current) const
	{
		return is_null(current)?head_:current->_next;
	}

	/**
	 * �Ƿ�Ϊ��
	 */
	bool empty() const
	{
		return is_null(head_);
	}

	size_t size()
	{
		return length_;
	}

protected:
	T* head_;
	T* tail_;
	size_t length_;
private:
	NOCOPY(Buffer);
};

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

_jingxian_end

#endif //_Buffer_Internal_H_