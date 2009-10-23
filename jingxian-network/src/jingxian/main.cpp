// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/AbstractServer.h"
#include "jingxian/directory.h"
#include "jingxian/networks/IOCPServer.h"
#include "jingxian/protocol/EchoProtocol.h"
#include "jingxian/protocol/Proxy/Proxy.h"
#include "jingxian/protocol/EchoServer.h"
#include "jingxian/utilities/NTService.h"



# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#endif

# include "log4cpp/PropertyConfigurator.hh"
# include "log4cpp/Category.hh"
# include "log4cpp/Appender.hh"
# include "log4cpp/NTEventLogAppender.hh"
# include "log4cpp/Priority.hh"

_jingxian_begin


class Handler
{
public:

	/**
	* 服务运行
	*/
	virtual void run()
	{
		_jingxian proxy::Proxy proxy(server, _T("TCP://0.0.0.0:6544"));
		_jingxian EchoServer echo(server);

		server.runForever();
	}

	/**
	* 初始化,它将在run之前调用。
	* @param[ in ] dwArgc 参数数组大小
	* @param[ in ] lpszArgv 参数数组
	* @return 成功返回true, 失败返回false
	* @remarks 注意，不可以发生异常。如果想指定退出代码，请用SetLastError
	* 或者SetLastErrorEx ，因为调用者会用GetLastError取得退出代码。
	*/
	bool OnInit( DWORD dwArgc, LPTSTR* lpszArgv )
	{
		return server.initialize(1);
	}

	/**
	* 接收到一个服务将停止的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnStop()
	{
		server.interrupt();
	}

	/**
	* 接收到一个询问服务状态的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnInterrogate()
	{
	}

	/**
	* 接收到一个服务暂停的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnPause()
	{
	}

	/**
	* 接收到一个服务恢复的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnContinue()
	{
	}

	/**
	* 接收到一个系统将关闭的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnShutdown()
	{
	}

	/**
	* 接收到一个新的网络组件被绑定的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnNetBindAdd()
	{
	}

	/**
	* 接收到一个网络组件绑定被启用的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnNetBindEnable()
	{
	}

	/**
	* 接收到一个网络组件绑定被禁用的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnNetBindDisable()
	{
	}

	/**
	* 接收到一个网络组件绑定被删除的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnNetBindRemove()
	{
	}

	/**
	* 接收到一个删除的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnParamChange()
	{
	}

	/**
	* 接收到一个用户定义的通知
	* @param dwOpcode 用户定义的通知
	* @remarks 注意，不可以发生异常。
	*/
	void OnControl(DWORD dwOpcode)
	{
	}

private:
	_jingxian IOCPServer server;
};

void testStackTracer3()
{
	ThrowException1(Exception, _T("test")); 
}


void testStackTracer2()
{
	testStackTracer3();
}

void testStackTracer1()
{
	testStackTracer2();
}

# ifdef _GOOGLETEST_
TEST(string, stringOP)
{
	StringArray<char, detail::StringOp<char> > sa( split<char, detail::StringOp<char> >( "ad,adf,ff,d,,.d.f",",." ) );
	StringArray<char, detail::StringOp<char> > sa1 = split<std::string, detail::StringOp<char> >( std::string("ad,adf,ff,d,,.d.f"),",." );

	StringArray<char > sa2 = split( "ad,adf,ff,d,,.d.f",",." );

	StringArray<char> sa3 = split(std::string( "ad,adf,ff,d,,.d.f" ),",." );
	ASSERT_FALSE( sa.size() != 6);
	ASSERT_FALSE(    0 != strcmp( "ad", sa.ptr( 0 ) )
		&& 0 != strcmp( "adf", sa.ptr( 1 ) )
		&& 0 != strcmp( "ff", sa.ptr( 2 ) )
		&& 0 != strcmp( "d", sa.ptr( 3 ) )
		&& 0 != strcmp( "d", sa.ptr( 4 ) )
		&& 0 != strcmp( "f", sa.ptr( 5 ) ) );

	try
	{
		char* p = sa[ 8 ].ptr;
		std::cout << "LOG_ERROR split!" << std::endl;
	}
	catch( OutOfRangeException& e)
	{
	}



	std::string str1( "asdfasdfas" );
	std::string str2( "as" );

	ASSERT_TRUE( begin_with( str1, "asd" ) );

	ASSERT_FALSE( begin_with( str2, "asd" ) );

	ASSERT_FALSE( begin_with( str1, "as1d" ) );

	if( !end_with( str1, "fas" ) )
		std::cout << "LOG_ERROR end_with!" << std::endl;
	ASSERT_FALSE(end_with( str1, "f1as" ) );

	std::string str3( "       asdkdfasdf");
	std::string str4( "asdkdfasdf         ");
	std::string str5( "       asdkdfasdf         ");

	ASSERT_FALSE( trim_left( str3 ) != "asdkdfasdf" );

	ASSERT_FALSE( trim_right( str4 ) != "asdkdfasdf" );

	ASSERT_FALSE( trim_all( str5 ) != "asdkdfasdf" );


	std::string str6( "asdkdfasdf");
	std::string str7( "asdkdfasdf");
	std::string str8( "asdkdfasdf");

	ASSERT_FALSE( trim_left( str6, "af" ) != "sdkdfasdf" );

	ASSERT_FALSE( trim_right( str7, "af" ) != "asdkdfasd" );

	ASSERT_FALSE( trim_all( str8, "af" ) != "sdkdfasd" );

	std::string str9( "asdkdfasdf");
	std::string str10( "asdddkdfasdf");
	std::string str11( "asdkdfasdf");

	ASSERT_FALSE( replace_all( str9, "a", "c" ) != "csdkdfcsdf" );

	ASSERT_FALSE( replace_all( str10, "a", "cc" ) != "ccsdddkdfccsdf" );

	ASSERT_FALSE( replace_all( str11, "a", "aaa" ) != "aaasdkdfaaasdf" );

	std::string str12( "aAsDFddSdkdfasdf");
	std::string str13( "asdSkdfaFAsSDdf");

	ASSERT_FALSE( transform_upper( str12 ) != "AASDFDDSDKDFASDF" );

	ASSERT_FALSE( transform_lower( str13 ) != "asdskdfafassddf" );

	try
	{
		testStackTracer1();
	}
	catch(Exception& e)
	{
#ifdef  _UNICODE
		std::wcerr << e << std::endl;
#else
		std::cerr << e << std::endl;
#endif
	}
}
#endif

_jingxian_end

_jingxian Handler* server_;

BOOL WINAPI handlerRoutine( DWORD ctrlType )
{
	switch(ctrlType)
	{
	case CTRL_C_EVENT:
		server_->OnStop();
		return TRUE;
	}
	return FALSE;
}

int main(int argc, char* argv[])
{
	int tmpFlag = _CrtSetDbgFlag( _CRTDBG_REPORT_FLAG );
	tmpFlag |= _CRTDBG_LEAK_CHECK_DF;
	tmpFlag &= ~_CRTDBG_CHECK_CRT_DF;
	_CrtSetDbgFlag( tmpFlag );


	_jingxian networking::initializeScket();


	try
	{
		log4cpp::PropertyConfigurator::configure(toNarrowString(::simplify (::combinePath(getApplicationDirectory(), _T("log4cpp.config")))));
	}
	catch (const log4cpp::ConfigureFailure& e)
	{
		log4cpp::Category::getRoot().warn(e.what());
	}
	catch (const std::exception& e)
	{
		std::cerr << e.what() << std::endl;
	}

# ifdef _GOOGLETEST_
	testing::InitGoogleTest(&argc, argv);
	RUN_ALL_TESTS();
#endif

	Handler handler;
	server_ = &handler;

	//NTService<Handler> ntService(_T("jingxian-service"),&handler);
	//ntService.start(argc, argv);

	SetConsoleCtrlHandler(&handlerRoutine, TRUE);

	handler.run();

	_jingxian networking::shutdownSocket();
	return 0;
}

