
#ifndef _InBuffer_H_
#define _InBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/BaseBuffer.H"
# include "jingxian/Buffer/IInBuffer.H"

_jingxian_begin


class InBuffer : public BaseBuffer<IInBuffer>
{
public:

    class TranscationData
    {
    public:

        // 已读的字节数
        size_t readLength_;
        // 当前正在读的数据块
        size_t current_;
        // 当前正在读的数据指针
        const char* currentPtr_;
        // 当前正在读的数据块的剩余字节数
        size_t currentLength_;
        // 发性错误时的处理方式
        ExceptionStyle::type exceptionStyle_;
        // 当前是否处于错误中
        errcode_t errno_;
    };

    InBuffer(const std::vector<io_mem_buf>* buf = null_ptr, size_t totalLength = 0);
    virtual ~InBuffer(void);

    virtual void reset(const std::vector<io_mem_buf>* buf, size_t totalLength);

    virtual int beginTranscation();
    virtual void rollbackTranscation(int);
    virtual void commitTranscation(int);

    virtual bool    readBoolean();
    virtual int8_t  readInt8();
    virtual int16_t readInt16();
    virtual int32_t readInt32();
    virtual int64_t readInt64();
    virtual void readBlob(void* blob, size_t len);

    virtual void seek(int offest);

    virtual size_t size()const;

    virtual size_t search(char ch) const;
    virtual size_t search(wchar_t ch) const;
    virtual size_t search(const void* context, size_t len) const;
    virtual size_t searchAny(const char* charset) const;
    virtual size_t searchAny(const wchar_t* charset) const;

    virtual size_t readLength() const;
    virtual const std::vector<io_mem_buf>& rawBuffer() const;
    virtual size_t rawLength() const;

private:
    NOCOPY(InBuffer);

    // 所有的数据内存块
    const std::vector<io_mem_buf>* memory_;
    // 数据的总字节数,此值不会被更改
    size_t totalLength_;

    // 已读的字节数
    size_t readLength_;
    // 当前正在读的数据块
    size_t current_;
    // 当前正在读的数据指针
    const char* currentPtr_;
    // 当前正在读的数据块的剩余字节数
    size_t currentLength_;

    std::vector<TranscationData> transcationDatas_;
};

_jingxian_end

#endif //_InBuffer_H_