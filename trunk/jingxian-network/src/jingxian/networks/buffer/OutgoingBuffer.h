
#ifndef _OutgoingBuffer_H_
#define _OutgoingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <queue>
# include "jingxian/linklist.h"
# include "jingxian/buffer/buffer.h"
# include "jingxian/buffer/IBuffer.H"
# include "jingxian/networks/commands/ICommand.H"


_jingxian_begin

class ConnectedSocket;

class OutgoingBuffer
{
public:
    OutgoingBuffer();

    ~OutgoingBuffer();

    void initialize(ConnectedSocket* connectedSocket);

    void send(buffer_chain_t* buf);

    ICommand* makeCommand();

    bool clearBytes(size_t len);

private:
    NOCOPY(OutgoingBuffer);
    ConnectedSocket* connectedSocket_;
    linklist<buffer_chain_t> buffer_;
};


void assertBuffer(buffer_chain_t* newbuf);

_jingxian_end

#endif //_OutgoingBuffer_H_
