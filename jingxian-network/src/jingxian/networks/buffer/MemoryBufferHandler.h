#ifndef _MemoryBufferHandler_H_
#define _MemoryBufferHandler_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/exception.h"
# include "jingxian/networks/buffer/buffer-internal.h"

_jingxian_begin

class MemoryBufferHandler : public IBufferHandler
{
public:

    inline databuffer_t* cast(buffer_chain_t* chain)
    {
        return (databuffer_t*)chain;
    }

    inline const databuffer_t* cast(const buffer_chain_t* chain)
    {
        return (const databuffer_t*)chain;
    }

    virtual bool   isMemory(const buffer_chain_t* chain)
    {
        return true;
    }

    virtual char*  wd_ptr(buffer_chain_t* chain)
    {
        return cast(chain)->end;
    }

    virtual void   wd_ptr(buffer_chain_t* chain, size_t len)
    {
        cast(chain)->end += len;
    }

    virtual int    wd_length(const buffer_chain_t* chain)
    {
        const databuffer_t* data = cast(chain);
        return (data->ptr + data->capacity) - data->end;
    }

    virtual char*  rd_ptr(buffer_chain_t* chain)
    {
        return cast(chain)->start;
    }

    virtual void   rd_ptr(buffer_chain_t* chain, size_t len)
    {
        cast(chain)->start += len;
    }

    virtual int    rd_length(const buffer_chain_t* chain)
    {
        const databuffer_t* data = cast(chain);
        return data->end - data->start;
    }

    virtual ICommand* makeCommand(buffer_chain_t* chain, bool isRead)
    {
        ThrowException(NotImplementedException);
    }

    virtual bool releaseCommand(buffer_chain_t* chain, ICommand* command, size_t len)
    {
        ThrowException(NotImplementedException);
    }
};

_jingxian_end

#endif //_MemoryBufferHandler_H_