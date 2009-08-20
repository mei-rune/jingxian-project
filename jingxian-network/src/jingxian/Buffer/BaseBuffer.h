
#ifndef _BaseBuffer_H_
#define _BaseBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/Buffer/IBuffer.H"

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

	virtual errcode_t error() const;

	void error(errcode_t err);

	virtual void clearError();

protected:
	NOCOPY(BaseBuffer);

	ExceptionStyle::type exceptionStyle_;
	errcode_t errno_;
};

_jingxian_end

#endif //_BaseBuffer_H_