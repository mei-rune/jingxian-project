

/**
* -*- C++ -*-
* -------------------------------------------------------------------------------
* - ��q�Шr �q�Шr					 System_Directory.H,v 1.0 2005/05/17 16:41:54
*  �u�������� �q�q �Шr
* ���������| �t------
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

# include "jingxian/exception.hpp"
# include "jingxian/string/string.hpp"

_jingxian_begin

inline DWORD getApplicationDirectory(tchar *szModName, DWORD Size, bool slash = true )
{
	DWORD ps = GetModuleFileName(NULL, szModName, Size);
	while(ps > 0 && szModName[ps-1] != '\\' && szModName[ps-1] != '/' ) ps--;
	szModName[ps] = '\0';
	if( !slash && ps> 0 )
	{
		ps --;
		szModName[ps] = '\0';
	}

	return ps;
}

/**
 * ȡ��ģ�����ڵ�Ŀ¼
 * @param[ in ] slash �Ƿ�������"/"����true ������false ��������
 */
inline tstring getApplicationDirectory( bool slash = true )
{
	tchar path[ MAX_PATH ] = "";
	getApplicationDirectory( path, MAX_PATH ,slash );
	return tstring( path );
}

/**
 * ���·��
 * @param[ in ] pa ��Ҫ��񻯵�·��
 */
inline tstring simplify(const tstring& pa)
{
	tstring result = pa;

	tstring::size_type pos;

#ifdef _WIN32
	for(pos = 0; pos < result.size(); ++pos)
	{
		if(result[pos] == '\\')
		{
			result[pos] = '/';
		}
	}
#endif

	pos = 0;
	while((pos = result.find("//", pos)) != tstring::npos)
	{
		result.erase(pos, 1);
	}

	pos = 0;
	while((pos = result.find("/./", pos)) != tstring::npos)
	{
		result.erase(pos, 2);
	}

	if(result.substr(0, 2) == "./")
	{
		result.erase(0, 2);
	}

	if(result == "/." ||
		result.size() == 4 && isalpha(result[0]) && result[1] == ':' && result[2] == '/' && result[3] == '.')
	{
		return result.substr(0, result.size() - 1);
	}

	if(result.size() >= 2 && result.substr(result.size() - 2, 2) == "/.")
	{
		result.erase(result.size() - 2, 2);
	}

	if(result == "/" || result.size() == 3 && isalpha(result[0]) && result[1] == ':' && result[2] == '/')
	{
		return result;
	}

	if(result.size() >= 1 && result[result.size() - 1] == '/')
	{
		result.erase(result.size() - 1);
	}

	return result;
}

/**
 * �����ǲ��Ǿ���·��
 * @return �Ǿ���·������true,���򷵻�false
 */
inline bool isAbsolute(const tstring& pa)
{
	unsigned i = 0;
	while(isspace(pa[i]))
	{
		++i;
	}
#ifdef _WIN32
	return pa[i] == '\\' || pa[i] == '/' || pa.size() > i + 1 && isalpha(pa[i]) && pa[i + 1] == ':';
#else
	return pa[i] == '/';
#endif
}

/**
 * �����ǲ��Ǹ�Ŀ¼
 * @return �Ǹ�Ŀ¼����true,���򷵻�false
 * @remarks ע���Ŀ¼��ָ "c:\","x:\"������·��
 */
inline bool isRoot(const tstring& pa)
{
	tstring path = simplify(pa);
#ifdef _WIN32
	return path == "/" || path.size() == 3 && isalpha(path[0]) && path[1] == ':' && path[2] == '/';
#else
	return path == "/";
#endif
}


/**
 * �����ǲ���Ŀ¼
 */
inline bool isDirectory(const tstring& pa)
{
	struct stat buf;
	if(stat(pa.c_str(), &buf) == -1)
	{
		ThrowException1( RuntimeException, "����stat `" + pa + "':\n" + lastError() );
	}

	return (S_ISDIR(buf.st_mode)) != 0 ;
}

/**
 * ȡ��·���е���һ��Ŀ¼
 * @remarks ע�⣬����·����һ���ļ�����һ��Ŀ¼����ɾ�����һ��
 * "\"������ַ���ͬʱ����"\",��
 * "c:\\aa\a"������"c:\\aa\"
 * "c:\\aa\a\"������"c:\\aa\"
 */
inline tstring getBasename(const tstring& pa)
{
	const tstring path = simplify(pa);

	tstring::size_type pos = path.rfind('/');
	if(pos == tstring::npos)
	{
		return path;
	}
	else
	{
		return path.substr(pos + 1);
	}
}

/**
 * ȡ��·���е���һ��Ŀ¼
 * @remarks ע�⣬��getBasename��ͬ���ǣ����������һ��"\"
 * "c:\\aa\a"������"c:\\aa"
 * "c:\\aa\a\"������"c:\\aa"
 */
inline tstring getDirectoryName(const tstring& pa)
{
	const tstring path = simplify(pa);

	tstring::size_type pos = path.rfind('/');
	if(pos == tstring::npos)
	{
		return tstring();
	}
	else
	{
		return path.substr(0, pos);
	}
}


namespace detail
{

	class filefinder
	{
	public:
		filefinder( intptr_t h )
			: h_( h )
		{
		}
		~filefinder()
		{
			if( -1 != h_ )
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
 * ��Ŀ¼�����е��ļ���Ŀ¼
 */
inline std::list<tstring> readDirectory(const tstring& pa)
{
	typedef detail::StringOp<tchar> OP;
	const tstring path = simplify(pa);

#ifdef _WIN32

	struct _finddata_t data;
	detail::filefinder finder( _findfirst(simplify((path + "/*")).c_str(), &data) );
	if( ( -1 == finder.get()) )
	{
		ThrowException1( RuntimeException, "���ܶ�Ŀ¼ `" + path + "':\n" + lastError() );
	}

   std::list<tstring> result;

	while(true)
	{
		tstring name = data.name;

		//assert(!name.empty());

	   if( name == ".." && name == ".")
		{
			result.push_back(name);
		}

		if(_findnext(finder.get(), &data) == -1)
		{
			if(errno == ENOENT)
			{
				break;
			}

			tstring ex = "���ܶ�Ŀ¼ `" + path + "':\n" + lastError();
			ThrowException1( RuntimeException, ex );
		}
	}


	return result;

#else

	struct dirent **namelist;
	int n = scandir(path.c_str(), &namelist, 0, alphasort);
	if(n < 0)
	{
		ThrowException1( RuntimeException, "���ܶ�Ŀ¼ `" + path + "':\n" + lastError() );
	}

	std::list< stringData<charT> > result;
	result.reserve(n - 2);

	for(int i = 0; i < n; ++i)
	{
		tstring name = namelist[i]->d_name;
		assert(!name.empty());

		free(namelist[i]);

		if(name != ".." && name != ".")
		{
			result.push_back(name);
		}
	}

	free(namelist);
	return result;

#endif
}

/**
 * �������ļ�����Ŀ¼��
 */
inline void rename(const tstring& fromPa, const tstring& toPa)
{
	const tstring fromPath = simplify(fromPa);
	const tstring toPath = simplify(toPa);

	::remove(toPath.c_str()); // We ignore errors, as the file we are renaming to might not exist.

	if(::rename(fromPath.c_str(), toPath.c_str()) == -1)
	{
		ThrowException1( RuntimeException,  "���ܽ��ļ� `" + fromPath + "' ������Ϊ  `" + toPath + "': " + lastError() );
	}
}

/**
 * ɾ���ļ���Ŀ¼��Ŀ¼�����ǿյģ�
 */
inline void remove(const tstring& pa)
{
	const tstring path = simplify(pa);
	//#ifdef _WIN32
	//	struct _stat  buf;
	//    if(_stat(path.c_str(), &buf) == -1)
	//#else
	struct stat buf;
	if(stat(path.c_str(), &buf) == -1)
		//#endif
	{
		ThrowException1( RuntimeException,  "����stat `" + path + "':\n" + lastError() );
	}

	if(S_ISDIR(buf.st_mode))
	{
#ifdef _WIN32
		if(_rmdir(path.c_str()) == -1)
#else
		if(rmdir(path.c_str()) == -1)
#endif
		{
			ThrowException1( RuntimeException,  "����ɾ��Ŀ¼ `" + path + "':\n" + lastError() );
		}
	}
	else
	{
		if(::remove(path.c_str()) == -1)
		{
			ThrowException1( RuntimeException, "����ɾ���ļ� `" + path + "':\n" + lastError());
		}
	}
}

/**
 * ɾ���ļ���Ŀ¼��Ŀ¼���Բ�Ϊ�գ�
 */
inline void removeRecursive(const tstring& pa)
{
	const tstring path = simplify(pa);
	struct stat buf;
	if(stat(path.c_str(), &buf) == -1)
	{
		ThrowException1( RuntimeException, "����stat `" + path + "':\n" + lastError() );
	}

	if(S_ISDIR(buf.st_mode))
	{
		std::list<tstring> paths = readDirectory(path);
		for(std::list<tstring>::const_iterator p = paths.begin(); p != paths.end(); ++p)
		{
			removeRecursive(path + '/' + *p);
		}

		if(!isRoot(path))
		{
#ifdef _WIN32
			if(_rmdir(path.c_str()) == -1)
#else
			if(rmdir(path.c_str()) == -1)
#endif
			{
				ThrowException1( RuntimeException, "����ɾ��Ŀ¼ `" + path + "':\n" + lastError() );
			}
		}
	}
	else
	{
		if(::remove(path.c_str()) == -1)
		{
			ThrowException1( RuntimeException, "����ɾ���ļ� `" + path + "':\n" + lastError() );
		}
	}
}

/**
 * ����һ��Ŀ¼
 */
inline void createDirectory(const tstring& pa)
{
	const tstring path = simplify(pa);

#ifdef _WIN32
	if(_mkdir(path.c_str()) == -1)
#else
	if(mkdir(path.c_str(), 0777) == -1)
#endif
	{
		if(errno != EEXIST)
		{
			ThrowException1( RuntimeException, "���ܴ���Ŀ¼ `" + path + "':\n" + lastError() );
		}
	}
}

/**
 * ����һ��Ŀ¼
 */
inline void createDirectoryRecursive(const tstring& pa)
{
	const tstring path = simplify(pa);

	tstring dir = getDirectoryName(path);
	if(!dir.empty())
	{
		createDirectoryRecursive(dir);
	}

#ifdef _WIN32
	if(_mkdir(path.c_str()) == -1)
#else
	if(mkdir(path.c_str(), 0777) == -1)
#endif
	{
		if(errno != EEXIST)
		{
			ThrowException1( RuntimeException, "���ܴ���Ŀ¼ `" + path + "':\n" + lastError() );
		}
	}
}

/**
 * �ϲ�һ��·��
 */
inline tstring combinePath(const tstring& path1,const tstring& path2)
{
	return simplify(path1 + "/" + path2 );
}

/**
 * ȡ���ļ�·���е���չ��
 */
inline tstring getExtension (const tstring& pa)
{
	const tstring path = simplify(pa);

	tstring::size_type dotPos = path.rfind('.');
	tstring::size_type slashPos = path.rfind('/');

	if(dotPos == tstring::npos || slashPos != tstring::npos && slashPos > dotPos)
	{
		return tstring();
	}
	else
	{
		return path.substr(dotPos + 1);
	}
}

/**
 * ȡ���ļ�·���е��ļ���
 */
inline tstring getFileName(const tstring& pa)
{
	const tstring path = simplify(pa);

	tstring::size_type slashPos = path.rfind('/');

	if(slashPos == tstring::npos)
	{
		return path;
	}
	else
	{
		return path.substr( slashPos + 1 );
	}
}

/**
 * ȡ���ļ�·���е��ļ���(������չ��)
 */
inline tstring getFileNameWithoutExtension(const tstring& pa)
{
	tstring path = getFileName(pa);
	tstring::size_type dotPos = path.rfind('.');

	if(dotPos == tstring::npos )
	{
		return path;
	}
	else
	{
		return path.substr( 0, dotPos);
	}
}

_jingxian_end

#endif // _System_Directory_H_