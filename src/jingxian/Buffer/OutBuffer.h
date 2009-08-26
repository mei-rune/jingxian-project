
#ifndef _OutBuffer_H_
#define _OutBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.H"
# include "jingxian/Buffer/Buffer.H"
# include "jingxian/Buffer/BaseBuffer.H"

_jingxian_begin

class OutBuffer : public BaseBuffer<IOutBuffer>
{
public:
	virtual ~OutBuffer( );

	virtual int beginTranscation();
	virtual void rollbackTranscation(int);
	virtual void commitTranscation(int);

	virtual IOutBuffer& writeBoolean(bool value);
	virtual IOutBuffer& writeInt8(int8_t value);
	virtual IOutBuffer& writeInt16(int16_t value);
	virtual IOutBuffer& writeInt32(int32_t value);
	virtual IOutBuffer& writeInt64(const int64_t& value);
	virtual IOutBuffer& writeBlob(const void* blob, size_t len);

	virtual databuffer_t* allocate(size_t len);

	Buffer<buffer_chain_t>& rawBuffer();
private:
	Buffer<buffer_chain_t> dataBuffer_;
};

_jingxian_end

#endif //_OutBuffer_H_