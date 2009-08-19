#ifndef _IncomingBuffer_H_
#define _IncomingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/buffer/IBuffer.H"

_jingxian_begin


class IncomingBuffer
{
public:
	IncomingBuffer();

	~IncomingBuffer();

	ICommand* makeCommand();

	void clearBytes(size_t len);

private:
	NOCOPY(IncomingBuffer);

	InternalBuffer freeBuffer_;

	InternalBuffer dataBuffer_;
};

_jingxian_end

#endif //_IncomingBuffer_H_