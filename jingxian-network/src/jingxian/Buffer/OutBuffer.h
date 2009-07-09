
#ifndef _OutBuffer_H_
#define _OutBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/IBuffer.H"

_jingxian_begin

class OutBuffer : public IOutBuffer
{
public:
	virtual ~OutBuffer( );

	virtual IOutBuffer& writeBoolean(bool value);
	virtual IOutBuffer& writeInt8(int8_t value);
	virtual IOutBuffer& writeInt16(int16_t value);
	virtual IOutBuffer& writeInt32(int32_t value);
	virtual IOutBuffer& writeInt64(const int64_t& value);
	virtual IOutBuffer& writeBlob(const void* blob, size_t len);
};

_jingxian_end

#endif //_OutBuffer_H_