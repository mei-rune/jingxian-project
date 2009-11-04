
#ifndef _protocolcontext_h_
#define _protocolcontext_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.h"

_jingxian_begin

class IReactorCore;
class ITransport;

class ProtocolContext
{
public:
    ProtocolContext()
            : core_(null_ptr)
            , transport_(null_ptr)
            , inMemory_(null_ptr)
            , inBytes_(0)
            //, outBuffer_(null_ptr)
            //, inBuffer_(null_ptr)
    {
    }

    virtual ~ProtocolContext()
    {
    }

    void initialize(IReactorCore* core
                    , ITransport* transport)
    {
        core_ = core;
        transport_ = transport;
    }

    IReactorCore& core()
    {
        if (is_null(core_))
            ThrowException(NullException);
        return *core_;
    }

    ITransport& transport()
    {
        if (is_null(transport_))
            ThrowException(NullException);
        return *transport_;
    }

    const std::vector<io_mem_buf>& inMemory() const
    {
        if (is_null(inMemory_))
            ThrowException(NullException);
        return *inMemory_;
    }

    size_t inBytes() const
    {
        return inBytes_;
    }

    //IInBuffer& inBuffer()
    //{
    //  if(is_null(inBuffer_))
    //    ThrowException( NullException );
    //  return *inBuffer_;
    //}
    //
    //IOutBuffer& outBuffer()
    //{
    //  if(is_null(outBuffer_))
    //    ThrowException( NullException );
    //  return *outBuffer_;
    //}

protected:
    IReactorCore* core_;
    ITransport* transport_;
    const std::vector<io_mem_buf>* inMemory_;
    size_t inBytes_;
    //IOutBuffer* outBuffer_;
    //IInBuffer* inBuffer_;

    //InternalBuffer* internalBuffer_in_free_;
    //InternalBuffer* internalBuffer_in_data_;
    //InternalBuffer* internalBuffer_out_;
    //std::vector<io_mem_buf>* inMemory_;
};

_jingxian_end

#endif //_protocolcontext_h_