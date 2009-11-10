// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/directory.h"



# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
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
    StringArray<char, detail::StringOp<char> > sa(split<char, detail::StringOp<char> >("ad,adf,ff,d,,.d.f", ",."));
    StringArray<char, detail::StringOp<char> > sa1 = split<std::string, detail::StringOp<char> >(std::string("ad,adf,ff,d,,.d.f"), ",.");

    StringArray<char > sa2 = split("ad,adf,ff,d,,.d.f", ",.");

    StringArray<char> sa3 = split(std::string("ad,adf,ff,d,,.d.f"), ",.");
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
        std::cout << "LOG_ERROR split!" << std::endl;
    }
    catch (OutOfRangeException& e)
    {
    }



    std::string str1("asdfasdfas");
    std::string str2("as");

    ASSERT_TRUE(begin_with(str1, "asd"));

    ASSERT_FALSE(begin_with(str2, "asd"));

    ASSERT_FALSE(begin_with(str1, "as1d"));

    if (!end_with(str1, "fas"))
        std::cout << "LOG_ERROR end_with!" << std::endl;
    ASSERT_FALSE(end_with(str1, "f1as"));

    std::string str3("       asdkdfasdf");
    std::string str4("asdkdfasdf         ");
    std::string str5("       asdkdfasdf         ");

    ASSERT_FALSE(trim_left(str3) != "asdkdfasdf");

    ASSERT_FALSE(trim_right(str4) != "asdkdfasdf");

    ASSERT_FALSE(trim_all(str5) != "asdkdfasdf");


    std::string str6("asdkdfasdf");
    std::string str7("asdkdfasdf");
    std::string str8("asdkdfasdf");

    ASSERT_FALSE(trim_left(str6, "af") != "sdkdfasdf");

    ASSERT_FALSE(trim_right(str7, "af") != "asdkdfasd");

    ASSERT_FALSE(trim_all(str8, "af") != "sdkdfasd");

    std::string str9("asdkdfasdf");
    std::string str10("asdddkdfasdf");
    std::string str11("asdkdfasdf");

    ASSERT_FALSE(replace_all(str9, "a", "c") != "csdkdfcsdf");

    ASSERT_FALSE(replace_all(str10, "a", "cc") != "ccsdddkdfccsdf");

    ASSERT_FALSE(replace_all(str11, "a", "aaa") != "aaasdkdfaaasdf");

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
#ifdef  _UNICODE
        std::wcerr << e << std::endl;
#else
        std::cerr << e << std::endl;
#endif
    }
}

_jingxian_end