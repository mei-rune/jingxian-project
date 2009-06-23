// ht-test.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <stdlib.h>
#include <memory.h>

#include "ht-internal.h"

struct BTMessage
{
   HT_ENTRY(BTMessage) key;
	
   int x;
   int y;
   const char* message;
   const char* description;

   BTMessage()
   {
	   x = 0;
	   y=0;
	   message =0;
	   description = 0;
   }

   BTMessage(int a, int b)
   {
	   x = a;
	   y=b;
	   message =0;
	   description = 0;
   }

   
   BTMessage(int a, int b,const char* m)
   {
	   x = a;
	   y=b;
	   message =m;
	   description = 0;
   }
};

unsigned hashkey(struct BTMessage *e)
{
    // 非常不高效,但作为例子够了
	return e->x * 10 + e->y;
}

int eqkey(struct BTMessage *e1, struct BTMessage *e2)
{
	return e1->x == e2->x && e1->y == e2->y;
}

HT_HEAD(BTMessageMap, BTMessage);
HT_PROTOTYPE(BTMessageMap, BTMessage, key, hashkey, eqkey);
HT_GENERATE(BTMessageMap, BTMessage, key, hashkey, eqkey,
			0.5, malloc, realloc, free);


int _tmain(int argc, _TCHAR* argv[])
{
	BTMessageMap map;

	HT_INIT(BTMessageMap, &map);
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,2,"a"));
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,3,"b"));
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,4,"c"));
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,5,"d"));
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,6,"e"));
	HT_INSERT( BTMessageMap, &map, new BTMessage(1,7,"f"));

    BTMessage* m = HT_FIND(BTMessageMap,&map, new BTMessage(1,2) );
    BTMessage* m2 = HT_FIND(BTMessageMap,&map, new BTMessage(1,4) );

	return 0;
}

