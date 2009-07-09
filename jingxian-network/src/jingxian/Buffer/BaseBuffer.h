
#ifndef _BaseBuffer_H_
#define _BaseBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.H"
# include "jingxian/Buffer/InternalBuffer.H"

_jingxian_begin


// 
// ERROR_HANDLE_EOF
class BaseBuffer
{
public:
	struct point
	{
		int beginIndex;
		size_t beginOffest;

		int endIndex;
		size_t endOffest;
	};

	BaseBuffer();

	virtual ~BaseBuffer( );

	virtual int beginTranscation();

	virtual void rollbackTranscation(int);

	virtual void commitTranscation(int);

	virtual void exceptions(ExceptionStyle::type exceptionStyle);

	virtual ExceptionStyle::type exceptions() const;

	virtual bool fail() const;

	virtual errcode_t lastError() const;

	virtual void clearError();

	void crunch();

	void writeBuffer(const char* blob, size_t len);

	void readBuffer(char* blob, size_t len);

protected:
	NOCOPY(BaseBuffer);

	ExceptionStyle::type exceptionStyle_;
	errcode_t errno_;

	LPWSABUF ptr_;
	size_t len_;

	size_t totalLength_;
	size_t readLength_;

	LPWSABUF current_;
};

_jingxian_end

#endif //_BaseBuffer_H_