// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/AbstractServer.h"
#include "jingxian/networks/IOCPServer.h"

# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#endif

_jingxian_begin

class EchoProtocol : public IProtocol
{
public:
	EchoProtocol()
		: toString_(_T("EchoProtocol"))
	{
	}
    /**
     * 在指定的时间段内没有收到任何数据
     * 
     * @param[ in ] context 会话的上下文
	 */
    virtual void onTimeout(ProtocolContext& context)
	{
	}

    /**
     * 当会话建立后，被调用。
     * 
     * @param[ in ] context 会话的上下文
	 */
    virtual void onConnected(ProtocolContext& context)
	{
		std::cout << "新连接到来 - " << context.transport().peer() << std::endl;
	}

    /**
     * 当会话关闭后，被调用。
     * 
     * @param[ in ] context 会话的上下文
     * @param[ in ] errCode 关闭的原因,为0是表示主动关闭
     * @param[ in ] reason 关闭的原因描述
	 */
    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
	{
		std::cout << "连接断开 - " << context.transport().peer() << std::endl;
	}

    /**
     * 当有新的信息到来时，被调用。
     * 
     * @param[ in ] context 会话的上下文
     * @param[ in ] buffer 包含新到来信息的缓冲区
	 */
    virtual void onReceived(ProtocolContext& context)
	{
		size_t len = context.inBuffer().size();
		char* ptr = (char*)my_malloc(len);

		context.inBuffer().readBlob(ptr, len);
		context.outBuffer().writeBlob(ptr, len);
	}

	virtual buffer_chain_t* createBuffer(const ProtocolContext& context, buffer_chain_t* lastBuffer, size_t len)
	{
		databuffer_t* result = (databuffer_t*)calloc(1,sizeof(databuffer_t)+100);
		result->capacity = 100;
		result->start = result->end = result->ptr;
		return (buffer_chain_t*)result;
	}

	/**
	* 取得地址的描述
	*/
	virtual const tstring& toString() const
	{
		return toString_;
	}
private:
	tstring toString_;
};

class EchoServer : public AbstractServer
{
public:
	EchoServer(IOCPServer& core)
		: AbstractServer( &core )
	{
		if(!this->initialize("tcp://0.0.0.0:6543"))
		{
			std::cout << "初始失败" << std::endl;
			return;
		}
		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, &core);
	}


	void OnComplete(ITransport* transport, IOCPServer* core)
	{		
		transport->bindProtocol(&protocol_);
		transport->initialize();

		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, core);
	}

	void OnError(const ErrorCode& err, IOCPServer* core)
	{
		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, core);
	}
	virtual const tstring& toString() const
	{
		return protocol_.toString();
	}
private:

	EchoProtocol protocol_;
};

_jingxian_end



TEST(sstring, sStringOP)
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
		std::cout << "ERROR split!" << std::endl;
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
		std::cout << "ERROR end_with!" << std::endl;
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
}


int main(int argc, char* argv[])
{
	_jingxian networking::initializeScket();

	_jingxian IOCPServer server;

	if( !server.initialize(1) )
		return -1;

	_jingxian EchoServer echo(server);

	server.runForever();

	_jingxian networking::shutdownSocket();
	int i ; 
	std::cin  >> i;
	return 0;
}

