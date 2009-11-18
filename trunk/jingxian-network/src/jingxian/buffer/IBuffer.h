
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
     * �������ڷ�������ʱ�Ƿ��׳��쳣.
     */
    virtual void exceptions(ExceptionStyle::type exceptionStyle) = 0;

    /**
     * ȡ�����ڷ�������ʱ����ʽ.
     */
    virtual ExceptionStyle::type exceptions() const = 0;

    /**
     * ��ǰ���Ƿ��ڴ���״̬
     */
    virtual bool fail() const = 0;

    /**
     * ȡ�ô����
     */
    virtual errcode_t error() const = 0;

    /**
     * �����ǰ���Ĵ���״̬
     */
    virtual void clearError() = 0;
};

_jingxian_end

#endif //_IBuffer_h_