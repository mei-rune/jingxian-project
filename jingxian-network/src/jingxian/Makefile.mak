

ifdef JINGXIAN_ROOT
	BASE_PATH=$(JINGXIAN_ROOT)
else
	BASE_PATH="."
endif

include $(BASE_PATH)/include.mak

OBJECTS=

.PHONY : clean
clean :
	del *.obj
    
$(TargetPath)/LogUtils.obj : LogUtils.cpp
	$(CC) -I$(BASE_PATH) $(INCLUDES) $(CFLAGS) -c LogUtils.cpp
    

all : clean
