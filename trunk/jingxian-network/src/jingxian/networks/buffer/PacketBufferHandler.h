#ifndef _PacketBufferHandler_H_
#define _PacketBufferHandler_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/buffer/buffer-internal.h"

_jingxian_begin

class PacketBufferHandler : public IBufferHandler
{
public:
    virtual bool   isMemory(const buffer_chain_t* chain)
    {
        return false;
    }

    virtual char*  wd_ptr(buffer_chain_t* chain)
    {
        ThrowException(NotImplementedException);
    }

    virtual void   wd_ptr(buffer_chain_t* chain, size_t len)
    {
        ThrowException(NotImplementedException);
    }

    virtual int    wd_length(const buffer_chain_t* chain)
    {
        ThrowException(NotImplementedException);
    }

    virtual char*  rd_ptr(buffer_chain_t* chain)
    {
        ThrowException(NotImplementedException);
    }

    virtual void   rd_ptr(buffer_chain_t* chain, size_t len)
    {
        ThrowException(NotImplementedException);
    }

    virtual int    rd_length(const buffer_chain_t* chain)
    {
        ThrowException(NotImplementedException);
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

#endif //_PacketBufferHandler_H_