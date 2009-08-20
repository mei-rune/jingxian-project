
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

class InternalBuffer
{
public:
	InternalBuffer();

	/**
	 * ��������
	 */
	~InternalBuffer();

	/**
	 * ��β�����һ�����е��ڴ��
	 * @remarks �ڴ������ݽ������
	 */
	void push(buffer_chain_t* buf);

	/**
	 * ��ͷ��ȡһ���ڴ��
	 */
	buffer_chain_t* pop();

	/**
	 * ���ڱ���buf,
	 */
	buffer_chain_t* next(buffer_chain_t* current);

	/**
	 * ���ڱ���buf,
	 */
	const buffer_chain_t* next(const buffer_chain_t* current)const;

	/**
	 * �Ƿ�Ϊ��
	 */
	bool empty() const;

protected:
	buffer_chain_t* head_;
	buffer_chain_t* tail_;
private:
	NOCOPY(InternalBuffer);
};

_jingxian_end

#endif //_InternalBuffer_H_