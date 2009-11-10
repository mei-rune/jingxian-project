// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/Application.h"
# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#endif

void* my_calloc(__in size_t _NumOfElements, __in size_t _SizeOfElements)
{
	void* ptr = my_malloc(_NumOfElements*_SizeOfElements);
	memset(ptr, 0, _NumOfElements*_SizeOfElements);
	return ptr;
}

void  my_free(__inout_opt void * _Memory)
{
	return free(((char*)_Memory) - 4);
}

void* my_malloc(__in size_t _Size)
{
	return ((char*)malloc(_Size + 4)) + 4;
}

void* my_realloc(__in_opt void * _Memory, __in size_t _NewSize)
{
	return ((char*)realloc(((char*)_Memory) - 4, _NewSize + 4)) + 4;
}

char*  my_strdup(__in_z_opt const char * _Src)
{
	size_t len = strlen(_Src) + 1;
	char* ptr =(char*)my_calloc(len, sizeof(char));
	strcpy(ptr, _Src);
	return ptr;
}

wchar_t* my_wcsdup(__in_z const wchar_t * _Str)
{
	size_t len = wcslen(_Str) + 1;
	wchar_t* ptr =(wchar_t*)my_calloc(len, sizeof(wchar_t));
	wcscpy(ptr, _Str);
	return ptr;
}

int _tmain(int argc, tchar* argv[])
{
    int tmpFlag = _CrtSetDbgFlag(_CRTDBG_REPORT_FLAG);
    tmpFlag |= _CRTDBG_LEAK_CHECK_DF;
    tmpFlag &= ~_CRTDBG_CHECK_CRT_DF;
    _CrtSetDbgFlag(tmpFlag);

# ifdef _GOOGLETEST_
    testing::InitGoogleTest(&argc, argv);
    RUN_ALL_TESTS();
#endif

    return Application::main(argc, argv);
}

