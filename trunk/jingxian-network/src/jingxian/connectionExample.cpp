// connectionExample.cpp : 定义控制台应用程序的入口点。
//

#include "pro_config.h"
#include <iostream>
#include "jingxian/connection_functionals.h"

# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#endif

class ConnectionExample
{
public:

	void OnHandler()
	{
		std::cout << "OnHandler" << std::endl;
	}
	void OnHandler1(int i1)
	{
		std::cout << "OnHandler1=" << i1 << std::endl;
	}
	void OnHandler2(int i1,int i2)
	{
		std::cout << "OnHandler2=" << i1 << "," << i2 << std::endl;
	}
	void OnHandler3(int i1,int i2,int i3)
	{
		std::cout << "OnHandler3=" << i1 << "," << i2 << "," << i3 <<std::endl;
	}
	void OnHandler4(int i1,int i2,int i3,int i4)
	{
		std::cout << "OnHandler4=" << i1 << "," << i2 << "," << i3 <<"," << i4 <<std::endl;
	}
	void OnHandler5(int i1,int i2,int i3,int i4,int i5)
	{
		std::cout << "OnHandler5=" << i1 << "," << i2 << "," << i3 <<"," << i4 <<"," << i5 <<std::endl;
	}
	void OnHandler6(int i1,int i2,int i3,int i4,int i5,int i6)
	{
		std::cout << "OnHandler6=" << i1 << "," << i2 << "," << i3 <<"," << i4 <<"," << i5<<"," << i6 <<std::endl;
	}
	void OnHandler7(int i1,int i2,int i3,int i4,int i5,int i6,int i7)
	{
		std::cout << "OnHandler7=" << i1 << "," << i2 << "," << i3 <<"," << i4 <<"," << i5<<"," << i6<<"," << i7 <<std::endl;
	}
	void OnHandler8(int i1,int i2,int i3,int i4,int i5,int i6,int i7,int i8)
	{
		std::cout << "OnHandler8=" << i1 << "," << i2 << "," << i3 <<"," << i4 <<"," << i5<<"," << i6<<"," << i7<<"," << i8 <<std::endl;
	}
};

TEST(connection, connectionBase)
{
	ConnectionExample example;
	{
		signal<void ()> sig;
		sig.connect(&example, &ConnectionExample::OnHandler);

		_connection<ConnectionExample,void ()> connection(&example, &ConnectionExample::OnHandler);
		sig.connect(connection);
		sig();
		sig.disconnect(connection);
	}
	{
		signal<void (int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler1);

		_connection<ConnectionExample, void (int)> connection(&example, &ConnectionExample::OnHandler1);
		sig.connect(connection);
		sig(1);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler2);

		_connection<ConnectionExample,void (int,int)> connection(&example, &ConnectionExample::OnHandler2);
		sig.connect(connection);
		sig(1,2);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler3);

		_connection<ConnectionExample, void (int,int,int)> connection(&example, &ConnectionExample::OnHandler3);
		sig.connect(connection);
		sig(1,2,3);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler4);

		_connection<ConnectionExample, void (int,int,int,int)> connection(&example, &ConnectionExample::OnHandler4);
		sig.connect(connection);
		sig(1,2,3,4);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler5);

		_connection<ConnectionExample, void (int,int,int,int,int)> connection(&example, &ConnectionExample::OnHandler5);
		sig.connect(connection);
		sig(1,2,3,4,5);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int,int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler6);

		_connection<ConnectionExample, void (int,int,int,int,int,int)> connection(&example, &ConnectionExample::OnHandler6);
		sig.connect(connection);
		sig(1,2,3,4,5,6);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int,int,int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler7);

		_connection<ConnectionExample, void (int,int,int,int,int,int,int)> connection(&example, &ConnectionExample::OnHandler7);
		sig.connect(connection);
		sig(1,2,3,4,5,6,7);
		sig.disconnect(connection);
	}
	{
		signal<void (int,int,int,int,int,int,int,int)> sig;
		sig.connect(&example, &ConnectionExample::OnHandler8);

		_connection<ConnectionExample, void (int,int,int,int,int,int,int,int)> connection(&example, &ConnectionExample::OnHandler8);
		sig.connect(connection);
		sig(1,2,3,4,5,6,7,8);
		sig.disconnect(connection);
	}
}


