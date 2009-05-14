#pragma once
#include "server.h"

class client : public micro_task
{
public:
	client(server& task, SOCKET handle )
		: micro_task( task)
		, _svr( &task )
		, _socket( handle )
	{
		std::cout << "client[" <<  _socket << "]连接到来!" << std::endl;
	}

	~client()
	{
		::closesocket( _socket );
		std::cout << "client[" <<  _socket << "]连接断开!" << std::endl;
	}

	void run()
	{
		char buffer[1024];
		while( true )
		{
			_svr->register_read( _socket, *this );
			yield();
			int l = ::recv( _socket, buffer, 1024, 0 );
			if( 0 >= l )
				return;

			_svr->register_write( _socket, *this );
			yield();
			if( 0 >= ::send( _socket, buffer, l, 0 ) )
				return;
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
