#!/usr/bin/env python
#coding=utf-8
import os;

ignoreList=[".svn",".suo",".user","Thumbs.db",".scc",   \
            ".obj",".eprj",".idb",".pch",".res", \
            ".manifest",".lib",".dll",".pdb",".ilk","obj"];


def copyfile(src,dstDir,file):
    copy_command='''copy "%s\%s" "%s\%s"'''%(src,file,dstDir,file)
    print copy_command;
    return os.system(copy_command)


def isCopy(file):
    for ignore in ignoreList:
        if ignore==file or -1!=file.find(ignore):
            return False;
    return True;


def xcopy(src, dst):
    print u"开始处理目录 '".encode("gbk"),src,"' ......";
    if not os.path.exists(dst):
        print u"目录 '".encode("gbk"),dst,u"不存在,创建它".encode("gbk");
        os.makedirs(dst);
    dirList=[];
    for file in os.listdir(src):
        if not isCopy(file):
            print u"不处理 '".encode("gbk"),file,"' !";
            continue;
        if(os.path.isfile(os.path.join(src,file))):
            if(0 != copyfile(src,dst,file)):
                print u"拷贝文件 '".encode("gbk"),  \
                file,u"' 失败!".encode("gbk")
                return -1;
        else:
            print "append dir '",file,"' !";
            dirList.append(file);

    for sub_dir in dirList:
        if 0 != xcopy(os.path.join(src,sub_dir),os.path.join(dst,sub_dir)):
            return -1;
    return 0

if __name__ == '__main__':
    if(3 != len(os.sys.argv)):
        print u"参数不正确：xcopy 源目录 目标目录".encode("gbk")
    else:
        xcopy( os.sys.argv[1], os.sys.argv[2] );