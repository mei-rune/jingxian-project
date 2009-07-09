
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

	// 内存块大小（可选值，为0时为无效值）
	size_t capacity;
	//数据起始位置
	char* begin;
	//数据结束位置
	char* end;
    // 数据内存指针
    char ptr[1];
} databuffer_t;

class InternalBuffer
{
public:
	InternalBuffer(size_t bufLength, size_t capacity);

	~InternalBuffer();

	
	/**
	 * 从 Buffer 中读数据后,将数据起始指针后移
	 */
	void DecreaseBytes(size_t len);

	/**
	 * 向 Buffer 尾部空闲内存块写数据后,将数据结束指针后移
	 */
	void IncreaseBytes(size_t len);

	/**
	 * 数据的字节数
	 */
	size_t TotalBytes() const;

	/**
	 * 清除头部空数据块
	 */
	void Crunch();

	/**
	 * 取得 Buffer 中数据内存块,以便读数据
	 * @params[ out ] length 返回的WSABUF块大小,可选值
	 * @return 返回填充数据的WSABUF块,最后一个WSABUF指针一定是null
	 */
	LPWSABUF GetBuffer(size_t* len = null_ptr);

	/**
	 * 取得 Buffer 尾部空闲内存块,以便填写数据
	 * @params[ out ] length 返回的WSABUF块大小,可选值
	 * @params[ in ] capacity 空闲内存块的字节数最小字节数
	 * @return 返回尾部空闲内存块,最后一个WSABUF指针一定是null
	 */
	LPWSABUF GetFreeBuffer(size_t capacity, size_t* length = null_ptr);

	/**
	 * 取得指定内存块的下一块,如果没有下一块则分配一块并返回
	 * @params[ in ] 指定的内存块指针,可以为null,如果为null则返回第一块空闲块
	 */
	LPWSABUF GetNextFreeBuffer( LPWSABUF wsaBuf);

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