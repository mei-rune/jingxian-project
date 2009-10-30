
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
	tcout << _T("ʹ�÷�������:") << std::endl;
	
	tcout << _T("��װһ����̨����:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --install Win32������ [Win32�������ʾ��] [Win32�����������Ϣ] [Win32�����ִ�г�������] [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
	
	tcout << _T("ж��һ����̨����:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --uninstall Win32������") << std::endl;
	
	tcout << _T("����һ����̨����:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --start Win32������ [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
	
	tcout << _T("ֹͣһ����̨����:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --stop Win32������") << std::endl;
	
	tcout << _T("��Ϊһ������̨��������:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --console [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
	
	tcout << _T("��Ϊһ����̨��������:") << std::endl;
	tcout << _T("\t") << getFileName(args[0]) << _T(" --service [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
	
	tcout << _T("��ð���:") << std::endl;
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
			LOG_ERROR(app->logger_, _T("��װ����ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --install Win32������ [Win32�������ʾ��] [Win32�����������Ϣ] [Win32�����ִ�г�������] [Win32����Ĳ���1] [Win32����Ĳ���2] ..."));
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

		//����ǰ�װ��ǰ����Ļ�,���һ���ǲ����� '--service' ����,���û�оͼ���
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
		//	LOG_ERROR(app->logger_, _T("��װ����ʧ�� - ��������̫��,���ó��� ") << REG_SZ << _T(" ���ַ�."));
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
			LOG_ERROR(app->logger_, _T("ж�ط���ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --uninstall Win32������"));
			return -1;
		}

		NTManager ntManager;
		return ntManager.uninstallService(args[2]);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--start"), args[1]))
	{
		if(argc < 3)
		{
			LOG_ERROR(app->logger_, _T("��������ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --start Win32������ [Win32����Ĳ���1] [Win32����Ĳ���2] ..."));
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
			LOG_ERROR(app->logger_, _T("ֹͣ����ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --stop Win32������"));
			return -1;
		}

		NTManager ntManager;
		return ntManager.stopService(args[2]);
	}
	else if(0 == string_traits<tchar>::stricmp(_T("--console"), args[1]))
	{
		if(argc < 2)
		{
			LOG_ERROR(app->logger_, _T("��������ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --console [Win32����Ĳ���1] [Win32����Ĳ���2] ..."));
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
			LOG_ERROR(app->logger_, _T("��������ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:"));
			LOG_ERROR(app->logger_, _T("\t ") << args[0] <<_T(" --service [Win32����Ĳ���1] [Win32����Ĳ���2] ..."));
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