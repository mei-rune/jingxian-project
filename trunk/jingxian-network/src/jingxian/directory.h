

/**
* -*- C++ -*-
* -------------------------------------------------------------------------------
* - ¤╭⌒╮ ╭⌒╮          System_Directory.H,v 1.0 2005/05/17 16:41:54
*  ╱◥██◣ ╭╭ ⌒╮
* ︱田︱田田| ╰------
* -------------------------------------------------------------------------------
*/

#ifndef _System_Directory_H_
#define _System_Directory_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
#include <Windows.h>
#include <vector>
#include <errno.h>

#ifdef _WIN32
# include <direct.h>
# include <io.h>
# include <sys/stat.h>
# include "windows.h"
# define S_ISDIR(mode) ((mode) & _S_IFDIR)
# define S_ISREG(mode) ((mode) & _S_IFREG)
#else
# include <unistd.h>
# include <dirent.h>
#endif

# include "jingxian/exception.h"
# include "jingxian/string/string.h"

_jingxian_begin

inline DWORD getApplicationDirectory(tchar *szModName, DWORD Size, bool slash = true)
{
    DWORD ps = GetModuleFileName(NULL, szModName, Size);
    while (ps > 0 && szModName[ps-1] != _T('\\') && szModName[ps-1] != _T('/')) ps--;
    szModName[ps] = _T('\0');
    if (!slash && ps > 0)
    {
        ps --;
        szModName[ps] = _T('\0');
    }

    return ps;
}

/**
 * 取得模块所在的目录
 * @param[ in ] slash 是否保留最后的"/"符（true 保留，false 不保留）
 */
inline tstring getApplicationDirectory(bool slash = true)
{
    tchar path[ MAX_PATH ] = _T("");
    getApplicationDirectory(path, MAX_PATH , slash);
    return tstring(path);
}

/**
 * 规格化路径
 * @param[ in ] pa 需要规格化的路径
 */
inline tstring simplify(const tstring& pa)
{
    tstring result = pa;

    tstring::size_type pos;

#ifdef _WIN32
    for (pos = 0; pos < result.size(); ++pos)
    {
        if (result[pos] == _T('\\'))
        {
            result[pos] = _T('/');
        }
    }
#endif

    pos = 0;
    while ((pos = result.find(_T("//"), pos)) != tstring::npos)
    {
        result.erase(pos, 1);
    }

    pos = 0;
    while ((pos = result.find(_T("/./"), pos)) != tstring::npos)
    {
        result.erase(pos, 2);
    }

    if (result.substr(0, 2) == _T("./"))
    {
        result.erase(0, 2);
    }

    if (result == _T("/.") ||
            result.size() == 4 && isalpha(result[0]) && result[1] == _T(':') && result[2] == _T('/') && result[3] == _T('.'))
    {
        return result.substr(0, result.size() - 1);
    }

    if (result.size() >= 2 && result.substr(result.size() - 2, 2) == _T("/."))
    {
        result.erase(result.size() - 2, 2);
    }

    if (result == _T("/") || result.size() == 3 && isalpha(result[0]) && result[1] == _T(':') && result[2] == _T('/'))
    {
        return result;
    }

    if (result.size() >= 1 && result[result.size() - 1] == _T('/'))
    {
        result.erase(result.size() - 1);
    }

    return result;
}

/**
 * 判它是不是绝对路径
 * @return 是绝对路径返回true,否则返回false
 */
inline bool isAbsolute(const tstring& pa)
{
    unsigned i = 0;
    while (isspace(pa[i]))
    {
        ++i;
    }
#ifdef _WIN32
    return pa[i] == _T('\\') || pa[i] == _T('/') || pa.size() > i + 1 && isalpha(pa[i]) && pa[i + 1] == _T(':');
#else
    return pa[i] == _T('/');
#endif
}

/**
 * 判它是不是根目录
 * @return 是根目录返回true,否则返回false
 * @remarks 注意根目录是指 "c:\","x:\"这样的路径
 */
inline bool isRoot(const tstring& pa)
{
    tstring path = simplify(pa);
#ifdef _WIN32
    return path == _T("/") || path.size() == 3 && isalpha(path[0]) && path[1] == _T(':') && path[2] == _T('/');
#else
    return path == _T("/");
#endif
}


/**
 * 判它是不是目录
 */
inline bool existDirectory(const tstring& pa)
{
#ifdef  _UNICODE
    struct _stat buf;
    if (_wstat(pa.c_str(), &buf) == -1)
#else
    struct stat buf;
    if (stat(pa.c_str(), &buf) == -1)
#endif
        return false;

    return S_ISDIR(buf.st_mode) != 0 ;
}


/**
 * 判它是不是目录
 */
inline bool existFile(const tstring& pa)
{
#ifdef  _UNICODE
    struct _stat buf;
    if (_wstat(pa.c_str(), &buf) == -1)
#else
    struct stat buf;
    if (stat(pa.c_str(), &buf) == -1)
#endif
        return false;

    return S_ISREG(buf.st_mode) != 0 ;
}

/**
 * 取得路径中的上一级目录
 * @remarks 注意，不论路径是一个文件还是一个目录都是删除最后一个
 * "\"后面的字符，同时保留"\",如
 * "c:\\aa\a"，返回"c:\\aa\"
 * "c:\\aa\a\"，返回"c:\\aa\"
 */
inline tstring getBasename(const tstring& pa)
{
    const tstring path = simplify(pa);

    tstring::size_type pos = path.rfind(_T('/'));
    if (pos == tstring::npos)
    {
        return path;
    }
    else
    {
        return path.substr(pos + 1);
    }
}

/**
 * 取得路径中的上一级目录
 * @remarks 注意，与getBasename不同的是，不保留最后一个"\"
 * "c:\\aa\a"，返回"c:\\aa"
 * "c:\\aa\a\"，返回"c:\\aa"
 */
inline tstring getDirectoryName(const tstring& pa)
{
    const tstring path = simplify(pa);

    tstring::size_type pos = path.rfind(_T('/'));
    if (pos == tstring::npos)
    {
        return tstring();
    }
    else
    {
        return path.substr(0, pos);
    }
}

/**
 * 取得文件路径中的扩展名
 */
inline tstring getExtension(const tstring& pa)
{
    const tstring path = simplify(pa);

    tstring::size_type dotPos = path.rfind(_T('.'));
    tstring::size_type slashPos = path.rfind(_T('/'));

    if (dotPos == tstring::npos || slashPos != tstring::npos && slashPos > dotPos)
    {
        return tstring();
    }
    else
    {
        return path.substr(dotPos + 1);
    }
}

/**
 * 取得文件路径中的文件名
 */
inline tstring getFileName(const tstring& pa)
{
    const tstring path = simplify(pa);

    tstring::size_type slashPos = path.rfind(_T('/'));

    if (slashPos == tstring::npos)
    {
        return path;
    }
    else
    {
        return path.substr(slashPos + 1);
    }
}

/**
 * 取得文件路径中的文件名(不带扩展名)
 */
inline tstring getFileNameWithoutExtension(const tstring& pa)
{
    tstring path = getFileName(pa);
    tstring::size_type dotPos = path.rfind(_T('.'));

    if (dotPos == tstring::npos)
    {
        return path;
    }
    else
    {
        return path.substr(0, dotPos);
    }
}

namespace detail
{

class filefinder
{
public:
    filefinder(intptr_t h)
            : h_(h)
    {
    }
    ~filefinder()
    {
        if (-1 != h_)
            _findclose(h_);
    }

    intptr_t get() const
    {
        return h_;
    }

private:
    intptr_t h_;
};
}

/**
 * 读目录下所有的文件或目录
 */
inline std::list<tstring> readDirectory(const tstring& pa)
{
    typedef detail::StringOp<tchar> OP;
    const tstring path = simplify(pa);

#ifdef _WIN32

#ifdef  _UNICODE
    struct _wfinddata_t data;
    detail::filefinder finder(_wfindfirst(simplify((path + _T("/*"))).c_str(), &data));
#else
    struct _finddata_t data;
    detail::filefinder finder(_findfirst(simplify((path + _T("/*"))).c_str(), &data));
#endif
    if ((-1 == finder.get()))
    {
        ThrowException1(RuntimeException, _T("不能读目录 `") + path + _T("':\n") + lastError());
    }

    std::list<tstring> result;

    while (true)
    {
        tstring name = data.name;

        //assert(!name.empty());

        if (name == _T("..") && name == _T("."))
        {
            result.push_back(name);
        }
#ifdef  _UNICODE
        if (_wfindnext(finder.get(), &data) == -1)
#else
        if (_findnext(finder.get(), &data) == -1)
#endif
        {
            if (errno == ENOENT)
            {
                break;
            }

            tstring ex = _T("不能读目录 `") + path + _T("':\n") + lastError();
            ThrowException1(RuntimeException, ex);
        }
    }


    return result;

#else

    struct dirent **namelist;
    int n = scandir(path.c_str(), &namelist, 0, alphasort);
    if (n < 0)
    {
        ThrowException1(RuntimeException, _T("不能读目录 `") + path + _T("':\n") + lastError());
    }

    std::list< stringData<charT> > result;
    result.reserve(n - 2);

    for (int i = 0; i < n; ++i)
    {
        tstring name = namelist[i]->d_name;
        assert(!name.empty());

        free(namelist[i]);

        if (name != _T("..") && name != _T("."))
        {
            result.push_back(name);
        }
    }

    free(namelist);
    return result;

#endif
}

/**
 * 重命名文件名或目录名
 */
inline bool renameFile(const tstring& fromPa, const tstring& toPa)
{
    const tstring fromPath = simplify(fromPa);
    const tstring toPath = simplify(toPa);

    ::_tremove(toPath.c_str()); // We ignore errors, as the file we are renaming to might not exist.

    return -1 != ::_trename(fromPath.c_str(), toPath.c_str());
}

/**
 * 删除文件或目录（目录必须是空的）
 */
inline void remove(const tstring& pa)
{
    const tstring path = simplify(pa);

#ifdef  _UNICODE
    struct _stat buf;
    if (_wstat(pa.c_str(), &buf) == -1)
#else
    struct stat buf;
    if (stat(pa.c_str(), &buf) == -1)
#endif
    {
        ThrowException1(RuntimeException,  _T("不能stat `") + path + _T("':\n") + lastError());
    }

    if (S_ISDIR(buf.st_mode))
    {
#ifdef _WIN32
        if (_trmdir(path.c_str()) == -1)
#else
        if (rmdir(path.c_str()) == -1)
#endif
        {
            ThrowException1(RuntimeException,  _T("不能删除目录 `") + path + _T("':\n") + lastError());
        }
    }
    else
    {
        if (::_tremove(path.c_str()) == -1)
        {
            ThrowException1(RuntimeException, _T("不能删除文件 `") + path + _T("':\n") + lastError());
        }
    }
}

/**
 * 删除文件或目录（目录可以不为空）
 */
inline void removeRecursive(const tstring& pa)
{
    const tstring path = simplify(pa);
#ifdef  _UNICODE
    struct _stat buf;
    if (_wstat(pa.c_str(), &buf) == -1)
#else
    struct stat buf;
    if (stat(pa.c_str(), &buf) == -1)
#endif
    {
        ThrowException1(RuntimeException, _T("不能stat `") + path + _T("':\n") + lastError());
    }

    if (S_ISDIR(buf.st_mode))
    {
        std::list<tstring> paths = readDirectory(path);
        for (std::list<tstring>::const_iterator p = paths.begin(); p != paths.end(); ++p)
        {
            removeRecursive(path + _T('/') + *p);
        }

        if (!isRoot(path))
        {
#ifdef _WIN32
            if (_trmdir(path.c_str()) == -1)
#else
            if (rmdir(path.c_str()) == -1)
#endif
            {
                ThrowException1(RuntimeException, _T("不能删除目录 `") + path + _T("':\n") + lastError());
            }
        }
    }
    else
    {
        if (::_tremove(path.c_str()) == -1)
        {
            ThrowException1(RuntimeException, _T("不能删除文件 `") + path + _T("':\n") + lastError());
        }
    }
}

/**
 * 创建一个目录
 */
inline void createDirectory(const tstring& pa)
{
    const tstring path = simplify(pa);

#ifdef _WIN32
    if (_tmkdir(path.c_str()) == -1)
#else
    if (mkdir(path.c_str(), 0777) == -1)
#endif
    {
        if (errno != EEXIST)
        {
            ThrowException1(RuntimeException, _T("不能创建目录 `") + path + _T("':\n") + lastError());
        }
    }
}

/**
 * 创建一个目录
 */
inline void createDirectoryRecursive(const tstring& pa)
{
    const tstring path = simplify(pa);

    tstring dir = getDirectoryName(path);
    if (!dir.empty())
    {
        createDirectoryRecursive(dir);
    }

#ifdef _WIN32
    if (_tmkdir(path.c_str()) == -1)
#else
    if (mkdir(path.c_str(), 0777) == -1)
#endif
    {
        if (errno != EEXIST)
        {
            ThrowException1(RuntimeException, _T("不能创建目录 `") + path + _T("':\n") + lastError());
        }
    }
}

/**
 * 合并一个路径
 */
inline tstring combinePath(const tstring& path1, const tstring& path2)
{
    return simplify(path1 + _T("/") + path2);
}

_jingxian_end

#endif // _System_Directory_H_
