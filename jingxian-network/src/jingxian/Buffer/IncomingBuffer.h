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
	 * 向尾部添加一个空闲的内存块
	 * @remarks 内存块的数据将被清空
	 */
	void Push(databuffer_t* buf);

	/**
	 * 向头部取一个内存块
	 */
	databuffer_t* Pop();

	/**
	 * 取得 Buffer 中数据内存块,以便读数据
	 * @params[ out ] length 返回的WSABUF块大小,可选值
	 * @return 返回填充数据的WSABUF块,最后一个WSABUF指针一定是null
	 */
	LPWSABUF GetReadBuffer(size_t* len = null_ptr);
	
	/**
	 * 从 Buffer 中读数据后,将数据起始指针后移
	 */
	size_t ReadBytes(size_t len);

	/**
	 * 数据的字节数
	 */
	size_t TotalReadBytes() const;

	/**
	 * 取得 Buffer 尾部空闲内存块,以便填写数据
	 * @params[ out ] len 返回的WSABUF块大小,可选值
	 * @return 返回尾部空闲内存块,最后一个WSABUF指针一定是null
	 */
	LPWSABUF GetWriteBuffer(size_t* len = null_ptr);

	/**
	 * 向 Buffer 尾部空闲内存块写数据后,将数据结束指针后移
	 */
	size_t WriteBytes(size_t len);

	/**
	 * 数据的字节数
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