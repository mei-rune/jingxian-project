// cxxTester.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
#include "acceptor.h"



int _tmain(int argc, _TCHAR* argv[])
{
	WSADATA wsaData;
	if ( 0 != WSAStartup( MAKEWORD( 2, 2 ), &wsaData ) )
		return false;


	try
	{
	server svr;
	svr.start( new acceptor( svr, "0.0.0.0", 30003 ) );
	svr.runForever();
	}
	catch( std::exception& e)
	{
		std::cout << e.what() << std::endl;
	}
	//{
	//Iterator it( 0,10, 1 );

	//while( it.next() )
	//{
	//	std::cout << it.current() << std::endl;
	//}
	//}

	WSACleanup( );
	return 0;
}

