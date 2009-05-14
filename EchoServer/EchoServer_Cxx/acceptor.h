#pragma once
#include "server.h"
#include "client.h"

class acceptor : public micro_task
{
public:
	acceptor(server& svr, const char* ip, int port)
		: micro_task( svr )
		, _svr( &svr )
	{
		_socket = ::socket (AF_INET,
                                    SOCK_STREAM,
                                    IPPROTO_TCP);

		sockaddr addr;
		((sockaddr_in*) &addr)->sin_family = AF_INET;
		((sockaddr_in*) &addr)->sin_addr.s_addr = ::inet_addr( ip );
		((sockaddr_in*) &addr)->sin_port =  htons( port );

		if( SOCKET_ERROR == bind(_socket, &addr, sizeof( addr ) )
			|| SOCKET_ERROR == ::listen(_socket, SOMAXCONN ) )
			throw std::runtime_error(  get_last_error( WSAGetLastError() ) );
	}

	~acceptor()
	{
		::closesocket( _socket );
	}

	void run()
	{
		while( true )
		{
			_svr->register_read( _socket, *this );
			yield();
			
			sockaddr addr;
			int len = sizeof( addr );
			_svr->start( new client( *_svr, ::accept( _socket, &addr, &len ) ) );
		}
	}

	
	virtual void on_exit()
	{
		_svr->stop( this );
	}

private:
	server* _svr;
	SOCKET _socket;
};
