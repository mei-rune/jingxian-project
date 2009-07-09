
#ifndef _InBuffer_H_
#define _InBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/BaseBuffer.H"

_jingxian_begin

class InBuffer : public IInBuffer, public BaseBuffer
{
public:
	virtual ~InBuffer(void);

	virtual bool    readBoolean();
	virtual int8_t  readInt8();
	virtual int16_t readInt16();
	virtual int32_t readInt32();
	virtual int64_t readInt64();
	virtual void readBlob(void* blob, size_t len);

	void readBuffer(char* blob, size_t len);
	virtual size_t size();
};

_jingxian_end

#endif //_InBuffer_H_