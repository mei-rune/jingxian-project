

# include "pro_config.h"
#include <vector>
#include "jingxian/utilities/unittest.h"

RawFD RawOpenForWriting(const char* filename) {
  RawFD fd = CreateFileA(filename, GENERIC_WRITE, 0, NULL,
                         CREATE_ALWAYS, 0, NULL);
  if (fd != kIllegalRawFD && GetLastError() == ERROR_ALREADY_EXISTS)
    SetEndOfFile(fd);
  return fd;
}

void RawWrite(RawFD handle, const char* buf, size_t len) {
  while (len > 0) {
    DWORD wrote;
    BOOL ok = WriteFile(handle, buf, len, &wrote, NULL);
    if (!ok) break;
    buf += wrote;
    len -= wrote;
  }
}

void RawClose(RawFD handle) {
  CloseHandle(handle);
}



std::vector<void (*)()>* g_unittestlist = NULL;

void ADD_RUN_TEST(void (*func)())
{
	if(NULL == g_unittestlist)
		g_unittestlist = new std::vector<void (*)()>();

	g_unittestlist->push_back(func);
}

int RUN_ALL_TESTS() 
{
  for (std::vector<void (*)()>::const_iterator it = g_unittestlist->begin();
	  it != g_unittestlist->end(); ++it) 
  {
    (*it)();
  }
	if(NULL == g_unittestlist)
	{
		delete g_unittestlist;
		g_unittestlist = NULL;
	}
  return 0;
}