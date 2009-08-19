
#ifndef _OutgoingBuffer_H_
#define _OutgoingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <queue>
# include "jingxian/buffer/IBuffer.H"

_jingxian_begin

class OutgoingBuffer : protected InternalBuffer
{
public:
	OutgoingBuffer(ConnectedSocket* connectedSocket);

	~OutgoingBuffer();

	ICommand* makeCommand();

	void clearBytes(size_t len);

private:
	NOCOPY(OutgoingBuffer);
};

_jingxian_end

#endif //_OutgoingBuffer_H_
