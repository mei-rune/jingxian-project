
#ifndef _buffer_internal_h_
#define _buffer_internal_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer/buffer.h"
# include "jingxian/networks/commands/ICommand.H"

_jingxian_begin

class IBufferHandler
{
public:
    virtual bool   isMemory(const buffer_chain_t* chain) = 0;

    virtual char*  wd_ptr(buffer_chain_t* chain) = 0;
    virtual void   wd_ptr(buffer_chain_t* chain, size_t len) = 0;
    virtual int    wd_length(const buffer_chain_t* chain) = 0;

    virtual char*  rd_ptr(buffer_chain_t* chain) = 0;
    virtual void   rd_ptr(buffer_chain_t* chain, size_t len) = 0;
    virtual int    rd_length(const buffer_chain_t* chain) = 0;

    virtual ICommand* makeCommand(buffer_chain_t* chain, bool isRead) = 0;
    virtual bool releaseCommand(buffer_chain_t* chain, ICommand* command, size_t len) = 0;
};

namespace bufferOP
{
IBufferHandler* GetBufferHandler(int type);

inline bool   isMemory(const buffer_chain_t* chain)
{
    return GetBufferHandler(chain->type)->isMemory(chain);
}

inline char*  wd_ptr(buffer_chain_t* chain)
{
    return GetBufferHandler(chain->type)->wd_ptr(chain);
}

inline void   wd_ptr(buffer_chain_t* chain, size_t len)
{
    GetBufferHandler(chain->type)->wd_ptr(chain, len);
}

inline int    wd_length(const buffer_chain_t* chain)
{
    return GetBufferHandler(chain->type)->wd_length(chain);
}

inline char*  rd_ptr(buffer_chain_t* chain)
{
    return GetBufferHandler(chain->type)->rd_ptr(chain);
}

inline void   rd_ptr(buffer_chain_t* chain, size_t len)
{
    GetBufferHandler(chain->type)->rd_ptr(chain, len);
}

inline int    rd_length(const buffer_chain_t* chain)
{
    return GetBufferHandler(chain->type)->rd_length(chain);
}

inline ICommand* makeCommand(buffer_chain_t* chain, bool isRead)
{
    return GetBufferHandler(chain->type)->makeCommand(chain, isRead);
}

inline bool releaseCommand(buffer_chain_t* chain, ICommand* command, size_t len)
{
    return GetBufferHandler(chain->type)->releaseCommand(chain, command, len);
}
}

_jingxian_end

#endif //_buffer_internal_h_