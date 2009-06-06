
#ifndef _IOCPServer_H_
#define _IOCPServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <hash_map>
# include "jingxian/string/string.hpp"
# include "jingxian/logging/logging.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/proactor.h"
# include "jingxian/networks/connection_status.h"
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
	
	NOCOPY(SocketFactory);

	tstring toString_;
};

_jingxian_end

#endif //_IOCPServer_H_