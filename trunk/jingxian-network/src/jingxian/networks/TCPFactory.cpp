
# include "pro_config.h"
# include "jingxian/networks/TCPFactory.h"


#ifdef _MEMORY_DEBUG
#undef THIS_FILE
#define new	   DEBUG_NEW  
#define malloc DEBUG_MALLOC  
static char THIS_FILE[] = __FILE__;  
#endif // _MEMORY_DEBUG

_jingxian_begin

TCPFactory::TCPFactory(void)
: toString_(_T("SocketFactory"))
{
}

TCPFactory::~TCPFactory(void)
{
}

SOCKET TCPFactory::createSocket(void)
{
	return socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
}

void TCPFactory::releaseSocket(SOCKET socket, bool fa)
{
	if (INVALID_SOCKET == socket )
		return;

	for( int i =0 ; i < 5; i ++ )
	{
		if(0 == closesocket (socket))
			break;
	}
}

const tstring& TCPFactory::toString() const
{
	return toString_;
}


_jingxian_end
