// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/exception.h"
#include "jingxian/string/string.h"
//#include "jingxian/directory.h"



# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#else
#include "jingxian/utilities/unittest.h"
#endif

_jingxian_begin

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

TEST(string, stringOP)
{
	StringArray<char, detail::StringOp<char> > sa(split<char, detail::StringOp<char> >("ad,adf,ff,d,,.d.f", ",.", StringSplitOptions::None));
    StringArray<char, detail::StringOp<char> > sa1 = split<std::string, detail::StringOp<char> >(std::string("ad,adf,ff,d,,.d.f"), ",.", StringSplitOptions::None);

    StringArray<char > sa2 = split("ad,adf,ff,d,,.d.f", ",.", StringSplitOptions::None);

    StringArray<char> sa3 = split(std::string("ad,adf,ff,d,,.d.f"), ",.", StringSplitOptions::None);
    ASSERT_FALSE(sa.size() != 6);
    ASSERT_FALSE(0 != strcmp("ad", sa.ptr(0))
                 && 0 != strcmp("adf", sa.ptr(1))
                 && 0 != strcmp("ff", sa.ptr(2))
                 && 0 != strcmp("d", sa.ptr(3))
                 && 0 != strcmp("d", sa.ptr(4))
                 && 0 != strcmp("f", sa.ptr(5)));

    try
    {
        char* p = sa[ 8 ].ptr;
		ASSERT_FALSE(true);
    }
    catch (OutOfRangeException& e)
    {
        tcerr << e << std::endl;
    }



    std::string str1("asdfasdfas");
    std::string str2("as");

    ASSERT_TRUE(begin_with(str1, "asd"));

    ASSERT_FALSE(begin_with(str2, "asd"));

    ASSERT_FALSE(begin_with(str1, "as1d"));

	ASSERT_TRUE(end_with(str1, "fas"));
    ASSERT_FALSE(end_with(str1, "f1as"));

	{

    std::string str33("       ");
    std::string str34("       ");
    std::string str35("       ");

    std::string str3("       asdkdfasdf");
    std::string str4("asdkdfasdf         ");
    std::string str5("       asdkdfasdf         ");

	trim_left(str33);
    ASSERT_TRUE( str33.empty() );

	trim_right(str34);
    ASSERT_TRUE( str34.empty() );

	trim_all(str35);
    ASSERT_TRUE( str35.empty() );

	trim_left(str3);
    ASSERT_FALSE( str3 != "asdkdfasdf");

	trim_right(str4);
    ASSERT_FALSE( str4 != "asdkdfasdf");

	trim_all(str5);
    ASSERT_FALSE( str5 != "asdkdfasdf");


    std::string str6("asdkdfasdf");
    std::string str7("asdkdfasdf");
    std::string str8("asdkdfasdf");

	trim_left(str6, "af");
    ASSERT_FALSE( str6 != "sdkdfasdf");

	trim_right(str7, "af");
    ASSERT_FALSE( str7 != "asdkdfasd");

	trim_all(str8, "af");
    ASSERT_FALSE( str8 != "sdkdfasd");

    std::string str9("asdkdfasdf");
    std::string str10("asdddkdfasdf");
    std::string str11("asdkdfasdf");
	replace_all(str9, "a", "c");
	replace_all(str10, "a", "cc");
	replace_all(str11, "a", "aaa");

    ASSERT_FALSE( str9 != "csdkdfcsdf");

    ASSERT_FALSE( str10 != "ccsdddkdfccsdf");

    ASSERT_FALSE( str11 != "aaasdkdfaaasdf");
	}

	
	{

    const std::string str33("       ");
    const std::string str34("       ");
    const std::string str35("       ");
    const std::string str3("       asdkdfasdf");
    const std::string str4("asdkdfasdf         ");
    const std::string str5("       asdkdfasdf         ");

	
    ASSERT_TRUE( trim_left(str33).empty() );
    ASSERT_TRUE( trim_right(str34).empty() );
    ASSERT_TRUE( trim_all(str35).empty() );

    ASSERT_FALSE( trim_left(str3) != "asdkdfasdf");
    ASSERT_FALSE( trim_right(str4) != "asdkdfasdf");
    ASSERT_FALSE( trim_all(str5) != "asdkdfasdf");


    const std::string str6("asdkdfasdf");
    const std::string str7("asdkdfasdf");
    const std::string str8("asdkdfasdf");

    ASSERT_FALSE( trim_left(str6, "af") != "sdkdfasdf");
    ASSERT_FALSE( trim_right(str7, "af") != "asdkdfasd");
    ASSERT_FALSE( trim_all(str8, "af") != "sdkdfasd");

    const std::string str9("asdkdfasdf");
    const std::string str10("asdddkdfasdf");
    const std::string str11("asdkdfasdf");

    ASSERT_FALSE( replace_all(str9, "a", "c") != "csdkdfcsdf");
    ASSERT_FALSE( replace_all(str10, "a", "cc") != "ccsdddkdfccsdf");
    ASSERT_FALSE( replace_all(str11, "a", "aaa") != "aaasdkdfaaasdf");
	}

	{	
    ASSERT_TRUE( trim_left<std::string>("       ").empty() );
    ASSERT_TRUE( trim_right<std::string>("       ").empty() );
    //ASSERT_TRUE( trim_all<std::string>("       ").empty() );

	ASSERT_FALSE( trim_left<std::string>("       asdkdfasdf") != "asdkdfasdf");
    ASSERT_FALSE( trim_right<std::string>("asdkdfasdf         ") != "asdkdfasdf");
    //ASSERT_FALSE( trim_all<std::string>("       asdkdfasdf         ") != "asdkdfasdf");

    ASSERT_FALSE( trim_left<std::string>("asdkdfasdf", "af") != "sdkdfasdf");
    ASSERT_FALSE( trim_right<std::string>("asdkdfasdf", "af") != "asdkdfasd");
    //ASSERT_FALSE( trim_all<std::string>("asdkdfasdf", "af") != "sdkdfasd");

	}

    std::string str12("aAsDFddSdkdfasdf");
    std::string str13("asdSkdfaFAsSDdf");

    ASSERT_FALSE(transform_upper(str12) != "AASDFDDSDKDFASDF");

    ASSERT_FALSE(transform_lower(str13) != "asdskdfafassddf");

    try
    {
        testStackTracer1();
    }
    catch (Exception& e)
    {
		tcerr << e << std::endl;
    }
}

_jingxian_end