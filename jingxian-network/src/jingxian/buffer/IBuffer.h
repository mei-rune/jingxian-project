
#ifndef _IBuffer_h_
#define _IBuffer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <vector>
# include "jingxian/buffer/buffer.h"

# include "jingxian/exception.h"
# include "jingxian/buffer/buffer.h"

_jingxian_begin

namespace ExceptionStyle
{
enum type
{
    THROW
    , NOTHROW
};
}

namespace buffer_type
{
enum
{
    npos = -1
};
}

class IBuffer
{
public:
    virtual ~IBuffer() {}

    /**
     * 当从流中读或写数据时,指针当前位置也向前移动,启动事
     * 务则会记住指针的当前位置,如果回滚则会将指针移动到
     * 事务启始位置.
     *
     * 启动一个事务
     */
    virtual int beginTranscation() = 0;
    /**
     * 回滚一个事务
     */
    virtual void rollbackTranscation(int) = 0;

    /**
     * 提交一个事务
     */
    virtual void commitTranscation(int) = 0;

    /**
     * 设置流在发生错误时是否抛出异常.
     */
    virtual void exceptions(ExceptionStyle::type exceptionStyle) = 0;

    /**
     * 取得流在发生错误时处理方式.
     */
    virtual ExceptionStyle::type exceptions() const = 0;

    /**
     * 当前流是否处于错误状态
     */
    virtual bool fail() const = 0;

    /**
     * 取得错误号
     */
    virtual errcode_t error() const = 0;

    /**
     * 清除当前流的错误状态
     */
    virtual void clearError() = 0;
};


/**
 * 当从流中读或写数据时,指针当前位置也向前移动,启动事
 * 务则会记住指针的当前位置,如果回滚则会将指针移动到
 * 事务启始位置.
 */
template<typename T, typename I>
class TranscationScope
{
public:
    TranscationScope(T& buffer)
            : buffer_(buffer)
            , transcationId_(buffer.beginTranscation())
    {
    }

    ~TranscationScope()
    {
        if (-1 != transcationId_)
            return;
        buffer_.rollbackTranscation(transcationId_);
    }

    void commit()
    {
        if (-1 != transcationId_)
            return;
        buffer_.commitTranscation(transcationId_);
        transcationId_ = -1;
    }

private:
    NOCOPY(TranscationScope);
    T& buffer_;
    I transcationId_;
};

_jingxian_end

#endif //_IBuffer_h_