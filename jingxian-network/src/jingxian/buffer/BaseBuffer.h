
#ifndef _BaseBuffer_H_
#define _BaseBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.H"

_jingxian_begin


//
// ERROR_HANDLE_EOF
template<typename IBUFFER>
class BaseBuffer : public IBUFFER
{
public:

    BaseBuffer()
            : exceptionStyle_(ExceptionStyle::NOTHROW)
            , errno_(ERROR_SUCCESS)
    {
        errno_ = ERROR_SUCCESS;
    }

    virtual ~BaseBuffer()
    {
    }

    virtual void exceptions(ExceptionStyle::type exceptionStyle)
    {
        exceptionStyle_ = exceptionStyle;
    }

    virtual ExceptionStyle::type exceptions() const
    {
        return exceptionStyle_;
    }

    virtual bool fail() const
    {
        return ERROR_SUCCESS != errno_;
    }

    virtual errcode_t error() const
    {
        return errno_;
    }

    void error(errcode_t err)
    {
        if (ERROR_SUCCESS == errno_)
            errno_ = err;

        if (ExceptionStyle::THROW == exceptionStyle_)
            ThrowException1(Exception, get_last_error(err));
    }

    virtual void clearError()
    {
        errno_ = ERROR_SUCCESS;
    }

protected:
    NOCOPY(BaseBuffer);

    // 发性错误时的处理方式
    ExceptionStyle::type exceptionStyle_;

    // 当前是否处于错误中
    errcode_t errno_;
};

_jingxian_end

#endif //_BaseBuffer_H_