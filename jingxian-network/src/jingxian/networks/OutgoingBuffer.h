
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
# include "jingxian/networks/commands/ICommand.H"
# include "jingxian/networks/InternalBuffer.H"

_jingxian_begin

class ConnectedSocket;

class OutgoingBuffer : public InternalBuffer
{
public:
	OutgoingBuffer();

	~OutgoingBuffer();

	void initialize(ConnectedSocket* connectedSocket);

	ICommand* makeCommand();

	bool clearBytes(size_t len);

private:
	NOCOPY(OutgoingBuffer);
	ConnectedSocket* connectedSocket_;
};

_jingxian_end

#endif //_OutgoingBuffer_H_
