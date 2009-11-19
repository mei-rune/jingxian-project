

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



std::vector<void (*)()> g_testlist;

void ADD_RUN_TEST(void (*func)())
{
	g_testlist.push_back(func);
}

int RUN_ALL_TESTS() 
{
	std::vector<void (*)()>::const_iterator it;
  for (it = g_testlist.begin(); it != g_testlist.end(); ++it) {
    (*it)();
  }
  fprintf(stderr, "\nPassed %d tests\n\nPASS\n", (int)g_testlist.size());
  return 0;
}