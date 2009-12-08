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

    void copyTo(std::vector<io_mem_buf>& buf);

private:
	NOCOPY(IncomingBuffer);

	template<typename T>
	class linktraits
	{
	public:
		static void setNext(T* rhs, T* lhs)
		{
			rhs->chain._next = &(lhs->chain);
		}

		static T* getNext(T* t)
		{
			return (T*)t->chain._next;
		}

		static const T* getNext(const T* t)
		{
			return (const T*)t->chain._next;
		}
	};

	ConnectedSocket* connectedSocket_;
	linklist<databuffer_t, linktraits<databuffer_t>> dataBuffer_;
	databuffer_t* current_;
};

_jingxian_end

#endif //_IncomingBuffer_H_