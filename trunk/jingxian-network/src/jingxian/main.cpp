// Jingxian_Network.cpp : �������̨Ӧ�ó������ڵ㡣

#include "pro_config.h"
#include <iostream>
#include "jingxian/Application.h"

int _tmain(int argc, tchar* argv[])
{
    int tmpFlag = _CrtSetDbgFlag( _CRTDBG_REPORT_FLAG );
    tmpFlag |= _CRTDBG_LEAK_CHECK_DF;
    tmpFlag &= ~_CRTDBG_CHECK_CRT_DF;
    _CrtSetDbgFlag( tmpFlag );

# ifdef _GOOGLETEST_
    testing::InitGoogleTest(&argc, argv);
    RUN_ALL_TESTS();
#endif

    return Application::main(argc, argv);
}

