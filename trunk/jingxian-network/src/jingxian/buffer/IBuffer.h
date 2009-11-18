
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

_jingxian_end

#endif //_IBuffer_h_