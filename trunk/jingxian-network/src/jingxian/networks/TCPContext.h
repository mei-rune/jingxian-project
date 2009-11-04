
#ifndef _TCPContext_H_
#define _TCPContext_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"
# include "jingxian/IReactorCore.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/networks/networking.h"


_jingxian_begin

class TCPContext : public ProtocolContext
{
public:
    TCPContext()
    {
        //inBuffer_  = &_inBuffer;
        //outBuffer_ = &_outBuffer;
    }

    void inMemory(const std::vector<io_mem_buf>* buffers, size_t totalLen)
    {
        inMemory_ = buffers;
        if (-1 == totalLen)
        {
            totalLen = 0;
            for (std::vector<io_mem_buf>::const_iterator it = buffers->begin()
                    ; it != buffers->end(); ++ it)
            {
                totalLen += it->len;
            }
        }

        this->inBytes_ = totalLen;
    }

    //InBuffer& GetInBuffer()
    //{
    //  return _inBuffer;
    //}

    //OutBuffer& GetOutBuffer()
    //{
    //  return _outBuffer;
    //}

//private:
//  InBuffer _inBuffer;
//  OutBuffer _outBuffer;
};

_jingxian_end

#endif //_TCPContext_H_ 