
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
	 * 向尾部添加一个数据块
	 */
	void Send(databuffer_t* buf, size_t len);

	/**
	 * 取得 Buffer 中数据内存块,以便读数据
	 * @params[ out ] length 返回的WSABUF块大小,可选值
	 * @return 返回填充数据的WSABUF块,最后一个WSABUF指针一定是null
	 */
	LPWSABUF GetBuffer(size_t* len = null_ptr);
	
	/**
	 * 从 Buffer 中读数据后,将数据起始指针后移
	 */
	size_t bytes(size_t len);

	/**
	 * 数据的字节数
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
