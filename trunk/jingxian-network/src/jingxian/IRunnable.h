
#ifndef _IRunnable_H_
#define _IRunnable_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"

_jingxian_begin

/**
 * 方法接口
 */
class IRunnable
{
public:
    virtual ~IRunnable() {}

    virtual void run() = 0;
};

_jingxian_end

#endif //_IRunnable_H_
