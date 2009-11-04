#ifndef _IncomingBuffer_H_
#define _IncomingBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/linklist.h"
# include "jingxian/buffer/buffer.h"
# include "jingxian/buffer/IBuffer.h"
# include "jingxian/networks/commands/ICommand.H"

_jingxian_begin

class ConnectedSocket;

class IncomingBuffer
{
public:
    IncomingBuffer();

    ~IncomingBuffer();

    void initialize(ConnectedSocket* connectedSocket);

    ICommand* makeCommand();

    bool decreaseBytes(size_t len);

    bool increaseBytes(size_t len);

    const linklist<buffer_chain_t>& buffer() const;

    const buffer_chain_t* current() const;

    void dataBuffer(std::vector<io_mem_buf>& buf);

private:
    NOCOPY(IncomingBuffer);

    ConnectedSocket* connectedSocket_;
    linklist<buffer_chain_t> dataBuffer_;
    buffer_chain_t* current_;
};

_jingxian_end

#endif //_IncomingBuffer_H_