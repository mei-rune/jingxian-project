
#ifndef _TCPFactory_H_
#define _TCPFactory_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/logging/logging.hpp"
# include "jingxian/networks/sockets/basesocket.h"

_jingxian_begin

class TCPFactory
{
public:
	TCPFactory(void);

	~TCPFactory(void);

	/**
	 * createSocket ´´½¨ socket ¾ä±ú
	 */
	SOCKET createSocket();

	/**
	 * releaseSocket ÊÍ·Å socket ¾ä±ú
	 */
	void releaseSocket(SOCKET socket, bool fa);

	const tstring& toString() const;

private:
	
	NOCOPY(TCPFactory);

	tstring toString_;
};

_jingxian_end

#endif //_TCPFactory_H_