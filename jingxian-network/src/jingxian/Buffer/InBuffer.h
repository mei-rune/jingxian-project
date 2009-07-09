
#ifndef _InBuffer_H_
#define _InBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/BaseBuffer.H"

_jingxian_begin

class InBuffer : public IInBuffer, public BaseBuffer
{
public:
	InBuffer(LPWSABUF ptr, size_t count, size_t totalLength);
	virtual ~InBuffer(void);

	virtual bool    readBoolean();
	virtual int8_t  readInt8();
	virtual int16_t readInt16();
	virtual int32_t readInt32();
	virtual int64_t readInt64();
	virtual void readBlob(void* blob, size_t len);

	virtual size_t size();
private:
	NOCOPY(InBuffer);

	LPWSABUF ptr_;
	size_t size_;

	size_t totalLength_;
	size_t readLength_;

	LPWSABUF current_;
	const char* currentPtr_;
	size_t currentLength_;
};

_jingxian_end

#endif //_InBuffer_H_