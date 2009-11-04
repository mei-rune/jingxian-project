
#ifndef _IInBuffer_h_
#define _IInBuffer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer/IBuffer.h"

_jingxian_begin

/**
 * InBuffer 从 IBuffer 一般来说应该是虚继承,但
 * 我不想搞得太复杂,在实现该类时请注意不要同时继
 * 承 OutBuffer;
 */
class IInBuffer : public IBuffer
{
public:
    virtual ~IInBuffer(void) {}

    virtual bool    readBoolean() = 0;
    virtual int8_t  readInt8() = 0;
    virtual int16_t readInt16() = 0;
    virtual int32_t readInt32() = 0;
    virtual int64_t readInt64() = 0;
    virtual void readBlob(void* blob, size_t len) = 0;

    /**
     * 向前读offest个字节
     * @params[ int ] 位置移动的字节数
     * @remarks  offest > 0 时则向前移动,offest < 0 时则向后移动, 当offest移动的位置超出范围时则移动开始或结束.
     */
    virtual void seek(int offest) = 0;

    /**
     * 在 Buffer 中的数据的长度
     */
    virtual size_t size() const = 0;

    /**
     * 查找指定的数据的第一次出现位置
     */
    virtual size_t search(const void* context, size_t len) const = 0;

    /**
     * 查找指定的数据的第一次出现位置
     */
    virtual size_t search(char ch) const = 0;

    /**
     * 查找指定的数据的第一次出现位置
     */
    virtual size_t search(wchar_t ch) const = 0;

    /**
     * 查找指定的字符串中任意字符第一次出现位置
     */
    virtual size_t searchAny(const char* charset) const = 0;

    /**
     * 查找指定的字符串中任意字符第一次出现位置
     */
    virtual size_t searchAny(const wchar_t* charset) const = 0;

    /**
     * 取出 Buffer 中的所有数据块
     */
    virtual const std::vector<io_mem_buf>& rawBuffer() const = 0;

    /**
     * 取出 Buffer 中的所有数据块的总长度
     */
    virtual size_t rawLength() const = 0;
};

_jingxian_end

#endif //_IInBuffer_h_