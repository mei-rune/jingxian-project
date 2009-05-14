#pragma once
#include <hash_map>
#include <list>
#include "micro_task.h"


#define _TEXT_STRING(x)  x

inline std::string get_last_error( DWORD code )
{
	LPVOID lpMsgBuf = 0;
	DWORD ret = FormatMessageA(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM |
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		code,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
		(char*) &lpMsgBuf,
		0,
		NULL );
	if ( ret <= 0 )
	{
		return std::string(_TEXT_STRING("<²»ÖªµÀ>") );
	}

	char* s = (char*)lpMsgBuf;
	s[ ret ] = 0;
	if( s[ret - 1 ] == _TEXT_STRING('\r') || s[ ret - 1 ] == _TEXT_STRING('\n'))
		s[ret - 1 ] = 0;
	if( s[ret - 2 ] == _TEXT_STRING('\r') || s[ ret - 2 ] == _TEXT_STRING('\n'))
		s[ret - 2 ] = 0;

	std::string str( _TEXT_STRING("[") );

	char tmp[110] = _TEXT_STRING("");
	_ultoa_s( code, tmp,110, 10 );
	str += ( const char* )tmp;
	str += ( const char* )_TEXT_STRING("],");
	str += ( char*)lpMsgBuf;
	// Free the buffer.
	LocalFree( lpMsgBuf );

	return str;
}

class server : public main_task
{
public:
	server( )
	{ }

	void start( micro_task* c)
	{
		_starting_tasks.push_back( c );
	}

	
	void stop( micro_task* c)
	{
		_terminate_tasks.push_back( c );
	}

	void run()
	{
		while( true )
		{
			
			for( std::list< micro_task*>::iterator it = _starting_tasks.begin()
				; it != _starting_tasks.end(); it ++ )
			
			{
				(*it)->switch_to();
			}
			_starting_tasks.clear();
			
			for( std::list< micro_task*>::iterator it = _terminate_tasks.begin()
				; it != _terminate_tasks.end(); it ++ )
			
			{
				delete (*it);
			}
			_terminate_tasks.clear();

			fd_set read_set,write_set;
			FD_ZERO(&read_set);
			FD_ZERO(&write_set);

			for( stdext::hash_map<SOCKET, micro_task*>::iterator it = _read_tasks.begin()
				; it != _read_tasks.end(); it ++ )
				FD_SET( it->first, &read_set );

			
			for( stdext::hash_map<SOCKET, micro_task*>::iterator it = _write_tasks.begin()
				; it != _write_tasks.end(); it ++ )
				FD_SET( it->first, &write_set );

			timeval timeout;
			timeout.tv_sec = 20;
			timeout.tv_usec = 0;
			int r = ::select( 0, &read_set, &write_set, NULL, &timeout );

			if( 0 >= r)
				continue;
			
			std::list< std::pair<SOCKET, micro_task*> > tasks;

			for( stdext::hash_map<SOCKET, micro_task*>::iterator it = _read_tasks.begin()
				; it != _read_tasks.end();  )
			{
				if( FD_ISSET( it->first, &read_set) )
				{
					tasks.push_back( *it );
					_read_tasks.erase( it ++ );
				}
				else
				{
					it ++;
				}
			}

			
			for( stdext::hash_map<SOCKET, micro_task*>::iterator it = _write_tasks.begin()
				; it != _write_tasks.end(); )
			{
				if( FD_ISSET( it->first, &write_set) )
				{
					tasks.push_back( *it );
					_write_tasks.erase( it ++ );
				}
				else
				{
					it ++;
				}
			}
			
			for( std::list< std::pair<SOCKET, micro_task*> >::iterator it = tasks.begin()
				; it != tasks.end(); it ++ )
			{
				it->second->switch_to();
			}
		}
	}

	void register_read( SOCKET s, micro_task& task)
	{
		_read_tasks[ s ] = &task;
	}

	void register_write( SOCKET s, micro_task& task)
	{
		_write_tasks[ s ] = &task;
	}


private:

	std::list< micro_task* > _starting_tasks;
	std::list< micro_task* > _terminate_tasks;

	stdext::hash_map<SOCKET, micro_task*> _read_tasks;
	stdext::hash_map<SOCKET, micro_task*> _write_tasks;
};