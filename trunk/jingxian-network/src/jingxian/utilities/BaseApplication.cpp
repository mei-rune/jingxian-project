
# include "pro_config.h"
# include <windows.h>
# include <iostream>
# include "jingxian/lastError.h"
# include "jingxian/utilities/BaseApplication.h"
# include "jingxian/utilities/NTManager.h"
# include "jingxian/utilities/NTService.h"
# include "jingxian/directory.h"

_jingxian_begin

BaseApplication::BaseApplication(const tstring& name)
: name_(name)
, logger_(logging::makeLogger(_T("BaseApplication")))
{
}


BaseApplication::~BaseApplication()
{
	if(null_ptr == logger_)
		return;

	delete logger_;
	logger_ = null_ptr;
}

void BaseApplication::usage(int argc, tchar** args)
{
	tcout << _T("使用方法如下:") << std::endl;
	
	tcout << _T("安装一个后台服务:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --install Win32服务名 [Win32服务的显示名] [Win32服务的描述信息] [Win32服务的执行程序名称] [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
	
	tcout << _T("卸载一个后台服务:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --uninstall Win32服务名") << std::endl;
	
	tcout << _T("启动一个后台服务:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --start Win32服务名 [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
	
	tcout << _T("停止一个后台服务:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --stop Win32服务名") << std::endl;
	
	tcout << _T("作为一个控制台程序运行:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --console [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
	
	tcout << _T("作为一个后台服务运行:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --service [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
	
	tcout << _T("获得帮助:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --help") << std::endl;
}

int BaseApplication::main(BaseApplication* app, int argc, tchar** args)
{
	std::auto_ptr<BaseApplication> appPtr(app);
	if(2 > argc)
	{
		usage(argc, args);
		return -1;
	}

	if(0 == string_traits<tchar>::stricmp(_T("--install"), args[1]))
	{
		if(argc < 3)
		{
			LOG_ERROR(app->logger_, _T("安装服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --install Win32服务名 [Win32服务的显示名] [Win32服务的描述信息] [Win32服务的执行程序名称] [Win32服务的参数1] [Win32服务的参数2] ..."));
			return -1;
		}
		tstring name(args[2]);
		tstring display((argc >3)?args[3]:_T(""));
		tstring description((argc > 4)?args[4]:_T(""));
		tstring executable((argc>5)?args[5]:_T(""));
		std::vector<tstring> arguments;
		for(int i= 6; i < argc; ++ i)
		{
			arguments.push_back(args[i]);
		}

		//如果是安装当前服务的话,检测一下是不是有 '--service' 参数,如果没有就加上
		if(executable.empty()||
			0 == string_traits<tchar>::stricmp(_T("--install"), getFileName(args[0]).c_str()))
		{
			bool appendable= true;
			for(std::vector<tstring>::iterator it = arguments.begin()
				; it != arguments.end(); ++it)
			{
				if(0 == string_traits<tchar>::stricmp(_T("--service"),it->c_str()))
				{
					appendable = false;
					break;
				}
			}

			if(appendable)
				arguments.insert(arguments.begin(), _T("--service"));
		}

		//if(REG_SZ < description.size())
		//{
		//	LOG_ERROR(app->logger_, _T("安装服务失败 - 服务描述太长,不得超过 ") << REG_SZ << _T(" 个字符."));
		//	return -1;
		//}

		NTManager ntManager;
		return ntManager.installService(name
			, display
			, description
			, executable
			, arguments);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--uninstall"), args[1]))
	{
		if(argc != 3)
		{
			LOG_ERROR(app->logger_, _T("卸载服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --uninstall Win32服务名"));
			return -1;
		}

		NTManager ntManager;
		return ntManager.uninstallService(args[2]);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--start"), args[1]))
	{
		if(argc < 3)
		{
			LOG_ERROR(app->logger_, _T("启动服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --start Win32服务名 [Win32服务的参数1] [Win32服务的参数2] ..."));
			return -1;
		}
		std::vector<tstring> arguments;
		for(int i = 3; i < argc; ++ i)
		{
			arguments.push_back(args[i]);
		}

		NTManager ntManager;
		return ntManager.startService(args[2], arguments);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--stop"), args[1]))
	{
		if(argc != 3)
		{
			LOG_ERROR(app->logger_, _T("停止服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --stop Win32服务名"));
			return -1;
		}

		NTManager ntManager;
		return ntManager.stopService(args[2]);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--console"), args[1]))
	{
		if(argc < 2)
		{
			LOG_ERROR(app->logger_, _T("启动服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --console [Win32服务的参数1] [Win32服务的参数2] ..."));
			return -1;
		}
		std::vector<tstring> arguments;
		for(int i = 2; i < argc; ++ i)
		{
			arguments.push_back(args[i]);
		}

		return app->run(arguments);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--service"), args[1]))
	{
		if(argc < 2)
		{
			LOG_ERROR(app->logger_, _T("启动服务失败 - 参数不正确,正确使用方法为:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --service [Win32服务的参数1] [Win32服务的参数2] ..."));
			return -1;
		}

		std::vector<tstring> arguments;
		for(int i = 2; i < argc; ++ i)
		{
			arguments.push_back(args[i]);
		}

		return NTService<BaseApplication>::main(app->name(), appPtr.release());
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--help"), args[1]))
	{
		usage(argc, args);
		return 0;
	}

	usage(argc, args);
	return -1;
}

_jingxian_end