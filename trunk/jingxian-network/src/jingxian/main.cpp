// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include "jingxian/AbstractServer.h"
#include "jingxian/networks/IOCPServer.h"


#include <iostream>

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
		context.transport().disconnection();
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
    virtual void onReceived(ProtocolContext& context, Buffer& buffer)
	{
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

int main(int argc, char* argv[])
{
#ifdef TEST

	{
	StringArray<char, detail::StringOp<char> > sa( split<char, detail::StringOp<char> >( "ad,adf,ff,d,,.d.f",",." ) );
	StringArray<char, detail::StringOp<char> > sa1 = split<std::string, detail::StringOp<char> >( std::string("ad,adf,ff,d,,.d.f"),",." );

	StringArray<char > sa2 = split( "ad,adf,ff,d,,.d.f",",." );
	
	StringArray<char> sa3 = split(std::string( "ad,adf,ff,d,,.d.f" ),",." );
	if( sa.size() != 6)
		std::cout << "ERROR split!" << std::endl;
	if(    0 != strcmp( "ad", sa.ptr( 0 ) )
		&& 0 != strcmp( "adf", sa.ptr( 1 ) )
		&& 0 != strcmp( "ff", sa.ptr( 2 ) )
		&& 0 != strcmp( "d", sa.ptr( 3 ) )
		&& 0 != strcmp( "d", sa.ptr( 4 ) )
		&& 0 != strcmp( "f", sa.ptr( 5 ) ) )
		std::cout << "ERROR split!" << std::endl;

	try
	{
		char* p = sa[ 8 ].ptr;
		std::cout << "ERROR split!" << std::endl;
	}
	catch( OutOfRangeException& e)
	{
	}
	}



	std::string str1( "asdfasdfas" );
	std::string str2( "as" );

	if( !begin_with( str1, "asd" ) )
		std::cout << "ERROR begin_with!" << std::endl;
	
	if( begin_with( str2, "asd" ) )
		std::cout << "ERROR begin_with!" << std::endl;

	if( begin_with( str1, "as1d" ) )
		std::cout << "ERROR begin_with!" << std::endl;

	if( !end_with( str1, "fas" ) )
		std::cout << "ERROR end_with!" << std::endl;
	if( end_with( str1, "f1as" ) )
		std::cout << "ERROR end_with!" << std::endl;

	std::string str3( "       asdkdfasdf");
	std::string str4( "asdkdfasdf         ");
	std::string str5( "       asdkdfasdf         ");

	if( trim_left( str3 ) != "asdkdfasdf" )
		std::cout << "ERROR trim_left!" << std::endl;

	if( trim_right( str4 ) != "asdkdfasdf" )
		std::cout << "ERROR trim_right!" << std::endl;

	if( trim_all( str5 ) != "asdkdfasdf" )
		std::cout << "ERROR trim_all!" << std::endl;

	
	std::string str6( "asdkdfasdf");
	std::string str7( "asdkdfasdf");
	std::string str8( "asdkdfasdf");
	
	if( trim_left( str6, "af" ) != "sdkdfasdf" )
		std::cout << "ERROR trim_left!" << std::endl;
	
	if( trim_right( str7, "af" ) != "asdkdfasd" )
		std::cout << "ERROR trim_right!" << std::endl;

	if( trim_all( str8, "af" ) != "sdkdfasd" )
		std::cout << "ERROR trim_all!" << std::endl;

	std::string str9( "asdkdfasdf");
	std::string str10( "asdddkdfasdf");
	std::string str11( "asdkdfasdf");

	if( replace_all( str9, "a", "c" ) != "csdkdfcsdf" )
		std::cout << "ERROR replace_all!" << std::endl;
	
	if( replace_all( str10, "a", "cc" ) != "ccsdddkdfccsdf" )
		std::cout << "ERROR replace_all!" << std::endl;

	if( replace_all( str11, "a", "aaa" ) != "aaasdkdfaaasdf" )
		std::cout << "ERROR replace_all!" << std::endl;

	std::string str12( "aAsDFddSdkdfasdf");
	std::string str13( "asdSkdfaFAsSDdf");

	if( transform_upper( str12 ) != "AASDFDDSDKDFASDF" )
		std::cout << "ERROR transform_upper!" << std::endl;

	if( transform_lower( str13 ) != "asdskdfafassddf" )
		std::cout << "ERROR transform_lower!" << std::endl;

#endif //

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

