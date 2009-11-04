
#ifndef _MY_Exception_H_
#define _MY_Exception_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <stdexcept>
#include "jingxian/string/os_string.h"
#include "jingxian/lastError.h"
#include "jingxian/utilities/StackTracer.h"

_jingxian_begin

#ifndef _RAISE
#define _RAISE(x)  throw (x)
#endif

class Exception : public std::runtime_error
{
public:
    virtual ~Exception() throw()
    {
    }

    const char* getFile() const
    {
        if (!fSrcFile)
            return "";
        return fSrcFile;
    }

    size_t getLine() const
    {
        return fSrcLine;
    }

    void setPosition(const char* const file
                     , size_t line)
    {
        fSrcFile = file;
        fSrcLine = line;
    }

    Exception()
            : std::runtime_error("<未知异常>")
            , fSrcFile(0)
            , fSrcLine(0)
    {
        initStackTrace(3);
    }

    Exception(const tstring& message)
            : std::runtime_error(toNarrowString(message))
            , fSrcFile(0)
            , fSrcLine(0)
    {
        initStackTrace(3);
    }

    Exception(const char* const srcFile
              , size_t srcLine
              , const tstring& message)
            : std::runtime_error(toNarrowString(message))
            , fSrcFile(srcFile)
            , fSrcLine(srcLine)
    {
        initStackTrace(3);
    }

    Exception(const char* const srcFile
              , size_t srcLine
              , const tstring& message
              , const Exception& e)
            : std::runtime_error(toNarrowString(message) + "," + e.what())
            , fSrcFile(srcFile)
            , fSrcLine(srcLine)
    {
        initStackTrace(3);
    }

    template< typename E >
    void Raise(E& e)
    {
        throw e;
    }

    void dump(tostream& target) const
    {
        target << _T("[ file:")
        << toTstring(getFile())
        << _T(" line:")
        << (int) getLine()
        << _T(" ] ")
        << std::endl
        << _stack;
    }

    virtual Exception* clone()
    {
        return new Exception(*this);
    }

    virtual void rethrow()
    {
        Raise(*this);
    }

    virtual void print(tostream& target) const
    {
        target << _T("Exception: ")
        << what();
        dump(target);

    }

#if !_HAS_EXCEPTIONS
protected:
    virtual void _Doraise() const
    {   // perform class-specific exception handling
        _RAISE(*this);
    }
#endif /* _HAS_EXCEPTIONS */

protected :

    Exception(const Exception& ex)
            : std::runtime_error(ex)
            , fSrcFile(ex.fSrcFile)
            , fSrcLine(ex.fSrcLine)
            , _stack(ex._stack)
    {
    }

    void initStackTrace(int skipFrames)
    {
        StackTracer stackWalker(StackTracer::RetrieveLine);
        stackWalker.ShowCallstack(skipFrames);
        _stack = toTstring(stackWalker.GetCallStack());
    }

    const char*     fSrcFile;
    size_t    fSrcLine;
    tstring   _stack;
};

inline tostream& operator<<(tostream& target, const Exception& err)
{
    err.print(target);
    return target;
}


#define MakeException(theType , msg )                       \
class theType : public Exception                            \
{                                                           \
public:                                                     \
                                                            \
    theType(const   char* const         srcFile             \
            , size_t        srcLine                         \
            , const tstring & m                             \
            , const Exception& e                            \
            ) :                                             \
        Exception(srcFile, srcLine, msg + m ,e )            \
    {                                                       \
                                                            \
    }                                                       \
    theType(const   char* const         srcFile             \
            , size_t        srcLine                         \
            , const tstring & m                             \
            ) :                                             \
        Exception(srcFile, srcLine, msg + m )               \
    {                                                       \
                                                            \
    }                                                       \
    theType(const   char* const         srcFile             \
            , size_t        srcLine                         \
            ) :                                             \
        Exception(srcFile, srcLine, msg )                   \
    {                                                       \
                                                            \
    }                                                       \
    theType( ) :                                            \
        Exception( msg )                                    \
    {                                                       \
                                                            \
    }                                                       \
    theType( const tstring & m  ) :                         \
        Exception( msg + m )                                \
    {                                                       \
                                                            \
    }                                                       \
                                                            \
    virtual ~theType() throw() {}                           \
    virtual Exception* clone()                              \
    {                                                       \
        return new theType( *this );                        \
    }                                                       \
    virtual void rethrow()                                  \
    {                                                       \
        Raise( *this );                                     \
    }                                                       \
    virtual void print(tostream& target) const              \
    {                                                       \
            target << MAKE_STRING( theType )                \
      << what();                    \
      dump( target );                 \
    }                                                       \
};


// 错误描述
#define IllegalArgumentError    _T("无效参数")
#define ArgumentNullError       _T("参数为空")
#define RuntimeError            _T("运行时错误")
#define OutOfRangeError         _T("超出范围")
#define LockError               _T("锁出错")
#define CtrlCHandlerError       _T("设置Ctrl+C出错")
#define NetError                _T("网络错误")
#define InvalidPointerError     _T("无效指针")
#define LengthError             _T("无效长度")
#define EOFError                _T("已经到结尾了")
#define PluginError             _T("插件错误")
#define URLError                _T("URL分析出错")
#define CastError               _T("转换失败")
#define NullError               _T("空指针")
#define NotFindError            _T("没有找到")
#define _ExistError             _T("已经存在")
#define SystemError             _T("系统错误")
#define TimeSyntaxError         _T("时间格式错误")
#define NotImplementedError     _T("没有实现")


// 错误代码
#define  ERR_BAD_BUF         -2
#define  ERR_BAD_BUF_LEN     -1
#define  ERR_SYS             -200 //  
#define  ERR_ARG             -201
#define  ERR_LEN             -202
#define  ERR_POINT           -203
#define  ERR_UNKOWN          -204
#define  ERR_MAXMSGLEN       -205
#define  ERR_HEADERLENGTH    -206
#define  ERR_HEADERTYPE      -207
#define  ERR_MEMORY          -208
#define  ERR_PARAMETER       -400
#define  ERR_OBJECT_NOEXIST  -501    //  对象不存在
#define  ERR_INTERNAL        -502    //  内部错误
#define  ERR_UNKNOWN_COMMAND -503    //  不可识别的命令
#define  ERR_AUTH            -504    //  没有权限
#define  ERR_TYPE            -505    //  类型错误，它是一个目录
#define  ERR_SEEKFILE        -506    //  移动文件读位置出错
#define  ERR_READFILE        -507    //  移动文件读位置出错
#define  ERR_LENGTH          -508    //  内存太小


// 定义异常类
MakeException(NullException , NullError);
MakeException(RuntimeException , RuntimeError);
MakeException(IllegalArgumentException, IllegalArgumentError);
MakeException(ArgumentNullException, ArgumentNullError);
MakeException(OutOfRangeException, OutOfRangeError);
MakeException(InvalidPointerException, InvalidPointerError);
MakeException(LengthException, LengthError);
MakeException(CastException, CastError);
MakeException(EOFException , EOFError);
MakeException(URLException, URLError);
MakeException(NotFindException, NotFindError);
MakeException(SystemException, SystemError);
MakeException(LockException , LockError);
MakeException(TimeSyntaxException , TimeSyntaxError);
MakeException(NotImplementedException, NotImplementedError);

// 异常宏
#define ThrowException( type ) throw type(__FILE__, __LINE__ )
#define ThrowException1( type ,m1 ) throw type(__FILE__, __LINE__ , m1 )
#define ThrowException2( type ,m1 ,m2 ) throw type(__FILE__, __LINE__ , m1, m2 )
#define ThrowException3( type ,m1 ,m2 ,m3 ) throw type(__FILE__, __LINE__ , m1, m2, m3 )
#define ThrowException4( type ,m1 ,m2 ,m3 ,m4) throw type(__FILE__, __LINE__ , m1, m2, m3, m4)

_jingxian_end

#endif //_MY_Exception_H_
