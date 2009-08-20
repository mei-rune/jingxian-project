
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
	 * 析构函数
	 */
	~InternalBuffer();

	/**
	 * 向尾部添加一个空闲的内存块
	 * @remarks 内存块的数据将被清空
	 */
	void push(buffer_chain_t* buf);

	/**
	 * 向头部取一个内存块
	 */
	buffer_chain_t* pop();

	/**
	 * 用于遍历buf,
	 */
	buffer_chain_t* next(buffer_chain_t* current);

	/**
	 * 用于遍历buf,
	 */
	const buffer_chain_t* next(const buffer_chain_t* current)const;

	/**
	 * 是否为空
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