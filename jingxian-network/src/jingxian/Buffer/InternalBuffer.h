
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
	 * @params[ in,out ] iovec  WSABUF指针
	 * @params[ in ] WSABUF块大小
	 * @return 返回填充数据后的WSABUF块数
	 */
	LPWSABUF GetBuffer();

	/**
	 * 取得 Buffer 中数据内存块,以便读数据
	 * @params[ in,out ] iovec  WSABUF指针
	 * @params[ in ] WSABUF块大小
	 * @return 返回填充数据后的WSABUF块数
	 */
	LPWSABUF GetBuffer();

	/**
	 * 取得 Buffer 尾部空闲内存块,以便填写数据
	 * @params[ in,out ] iovec  WSABUF指针
	 * @params[ in ] length WSABUF块大小
	 * @params[ in ] capacity 空闲内存块的字节数最小字节数
	 * @return 返回填充数据后的WSABUF块数
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