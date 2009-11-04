
#ifndef _OutBuffer_H_
#define _OutBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer/IOutBuffer.H"
# include "jingxian/buffer/BaseBuffer.H"
# include "jingxian/ITransport.H"

_jingxian_begin

class OutBuffer : public BaseBuffer<IOutBuffer>
{
public:
    OutBuffer(ITransport* transport);
    virtual ~OutBuffer();

    virtual int beginTranscation();
    virtual void rollbackTranscation(int);
    virtual void commitTranscation(int);

    virtual IOutBuffer& writeBoolean(bool value);
    virtual IOutBuffer& writeInt8(int8_t value);
    virtual IOutBuffer& writeInt16(int16_t value);
    virtual IOutBuffer& writeInt32(int32_t value);
    virtual IOutBuffer& writeInt64(const int64_t& value);
    virtual IOutBuffer& writeBlob(const void* blob, size_t len);


    virtual size_t size() const;

private:
    std::vector<buffer_chain_t*> dataBuffer_;
    size_t bytes_;
    ITransport* transport_;

    virtual databuffer_t* allocate(size_t len);
    //std::vector<buffer_chain_t*>& dataBuffer();
    //void releaseBuffer();
    void freeBuffer();
};

_jingxian_end

#endif //_OutBuffer_H_