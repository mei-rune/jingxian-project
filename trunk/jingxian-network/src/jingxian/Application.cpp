// Jingxian_Network.cpp : �������̨Ӧ�ó������ڵ㡣

#include "pro_config.h"
#include <iostream>
#include "jingxian/Application.h"
#include "jingxian/directory.h"
#include "jingxian/networks/IOCPServer.h"
#include "jingxian/protocol/Proxy/ProxyProtocolFactory.h"
#include "jingxian/protocol/EchoProtocolFactory.h"


# include "log4cpp/PropertyConfigurator.hh"
# include "log4cpp/Category.hh"
# include "log4cpp/Appender.hh"
# include "log4cpp/NTEventLogAppender.hh"
# include "log4cpp/RollingFileAppender.hh"
# include "log4cpp/Priority.hh"

_jingxian_begin

static IInstance* instance_ = NULL;

BOOL WINAPI handlerRoutine(DWORD ctrlType)
{
  switch (ctrlType)
    {
    case CTRL_C_EVENT:
    case CTRL_BREAK_EVENT:
    case CTRL_CLOSE_EVENT:
    case CTRL_SHUTDOWN_EVENT:
      if (NULL != instance_)
        instance_->interrupt();
      return TRUE;
    }
  return FALSE;
}

void usage(int argc, tchar** args)
{
  tcout << _T("ʹ�÷�������:") << std::endl;

  tcout << _T("\t��װһ����̨����:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --install Win32������")
  << _T(" [Win32�������ʾ��]") << _T(" [Win32�����������Ϣ]")
  << _T(" [Win32�����ִ�г�������]") << _T(" [Win32����Ĳ���1]")
  << _T(" [Win32����Ĳ���2] ...") << std::endl;

  tcout << _T("\tж��һ����̨����:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --uninstall Win32������") << std::endl;

  tcout << _T("\t����һ����̨����:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --start Win32������")
  << _T(" [Win32����Ĳ���1]") << _T(" [Win32����Ĳ���2] ...") << std::endl;

  tcout << _T("\tֹͣһ����̨����:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --stop Win32������") << std::endl;

  tcout << _T("\t��Ϊһ������̨��������:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --console ")
  << _T("[Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;

  tcout << _T("\t��Ϊһ����̨��������:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --service")
  << _T(" [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;

  tcout << _T("\t��ð���:") << std::endl;
  tcout << _T("\t\t") << getFileName(args[0]) << _T(" --help") << std::endl;

  tcout << _T("\t����ѡ������:") << std::endl;

  tcout << _T("\t\t--config=xxx\r\rָ������������ļ���") << std::endl;
  tcout << _T("\t\t--logConfig=xxx\r\rָ�� log �������ļ���") << std::endl;
}

int Application::main(int argc, tchar** args)
{
  if (2 > argc)
    {
      usage(argc, args);
      return -1;
    }

  if (0 == string_traits<tchar>::stricmp(_T("--help"), args[1]))
    {
      usage(argc, args);
      return 0;
    }

  if (0 == string_traits<tchar>::stricmp(_T("--install"), args[1]))
    {
      if (argc < 3)
        {
          tcout << _T("��װ����ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:") << std::endl;
          tcout << _T("\t ") << args[0] << _T(" --install Win32������") << _T(" [Win32�������ʾ��] [Win32�����������Ϣ] [Win32�����ִ�г�������] [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
          return -1;
        }
      tstring name(args[2]);
      tstring display((argc > 3) ? args[3] : _T(""));
      tstring description((argc > 4) ? args[4] : _T(""));
      tstring executable((argc > 5) ? args[5] : _T(""));

      std::vector<tstring> arguments;
      arguments.push_back(_T("--service"));
      arguments.push_back(_T("--service:name=") + name);
      if (!description.empty())
        arguments.push_back(_T("--service:description=") + description);

      for (int i = 6; i < argc; ++ i)
        {
          if (0 != string_traits<tchar>::stricmp(_T("--service"), args[i]))
            arguments.push_back(args[i]);
        }

      return installService(name
                            , display
                            , description
                            , executable
                            , arguments
                            , tcout);
    }

  if (0 == string_traits<tchar>::stricmp(_T("--uninstall"), args[1]))
    {
      if (argc != 3)
        {
          tcout << _T("ж�ط���ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:") << std::endl;
          tcout << _T("\t ") << args[0] << _T(" --uninstall Win32������") << std::endl;
          return -1;
        }

      return uninstallService(args[2], tcout);
    }

  if (0 == string_traits<tchar>::stricmp(_T("--start"), args[1]))
    {
      if (argc < 3)
        {
          tcout << _T("��������ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:") << std::endl;
          tcout << _T("\t ") << args[0] << _T(" --start Win32������ [Win32����Ĳ���1] [Win32����Ĳ���2] ...") << std::endl;
          return -1;
        }
      std::vector<tstring> arguments;
      for (int i = 3; i < argc; ++ i)
        {
          arguments.push_back(args[i]);
        }

      return startService(args[2], arguments, tcout);
    }

  if (0 == string_traits<tchar>::stricmp(_T("--stop"), args[1]))
    {
      if (argc != 3)
        {
          tcout << _T("ֹͣ����ʧ�� - ��������ȷ,��ȷʹ�÷���Ϊ:") << std::endl;
          tcout << _T("\t ") << args[0] << _T(" --stop Win32������") << std::endl;
          return -1;
        }

      return stopService(args[2], tcout);
    }

  int runStyle = 0;
  if (0 == string_traits<tchar>::stricmp(_T("--console"), args[1]))
    {
      runStyle = 1;
    }
  else if (0 == string_traits<tchar>::stricmp(_T("--service"), args[1]))
    {
      runStyle = 2;
    }


  tstring name = _T("jingxian-daemon");
  tstring description = _T("jingxian ��̨�������");
  std::vector<tstring> arguments;
  for (int i = 2; i < argc; ++ i)
    {
      if (0 == string_traits<tchar>::strnicmp(args[i], _T("--service:name="), 15))
        {
          name = (args[i] + 15);
        }
      else if (0 == string_traits<tchar>::strnicmp(args[i], _T("--service:description="), 22))
        {
          description = (args[i] + 22);
        }
      else
        {
          arguments.push_back(args[i]);
        }
    }

  if (2 == runStyle)
    {
      return serviceMain(new Application(name, description));
    }
  else if (1 == runStyle)
    {
      Application app(name, description);
      instance_ = &app;
      SetConsoleCtrlHandler(handlerRoutine, TRUE);
      int result = app.onRun(arguments);
      instance_ = NULL;
      return result;
    }

  usage(argc, args);
  return -1;
}

Application::Application(const tstring& name, const tstring& descr)
    : name_(name)
    , toString_(descr)
{
  networking::initializeScket();
}

Application::~Application()
{
	for(std::map<tstring, configure::callback_type*>::iterator it=callbacks_.begin()
	  ; it != callbacks_.end(); ++it)
  {
	  delete (it->second);
  }
  callbacks_.clear();

  networking::shutdownSocket();
}

const tstring& Application::name() const
{
  return name_;
}

int Application::onRun(const std::vector<tstring>& args)
{
  if (!core_.initialize(1))
    return -1;

  tstring configFile = combinePath(getApplicationDirectory(), _T("default.conf"));
  tstring logFile = combinePath(getApplicationDirectory(), _T("log4cpp.conf"));

  for (std::vector<tstring>::const_iterator it=args.begin()
       ; it != args.end(); ++ it)
    {
      if (0 == string_traits<tchar>::strnicmp(_T("--config="), it->c_str(), 9))
        {
          tstring tmp = it->substr(9);
          if (isAbsolute(tmp))
            configFile = tmp;
          else
            configFile = combinePath(getApplicationDirectory(), tmp);
        }
      else if (0 == string_traits<tchar>::strnicmp(_T("--logConfig="), it->c_str(), 12))
        {
          tstring tmp = it->substr(12);
          if (isAbsolute(tmp))
            logFile = tmp;
          else
            logFile = combinePath(getApplicationDirectory(), tmp);
        }
    }

  try
    {
      log4cpp::PropertyConfigurator::configure(toNarrowString(logFile));
    }
  catch (const log4cpp::ConfigureFailure& e)
    {
      log4cpp::Category::getRoot().warn(e.what());
      std::cerr << e.what() << std::endl;
    }
  catch (const std::exception& e)
    {
      log4cpp::Category& logger = log4cpp::Category::getRoot();
      logger.addAppender(new log4cpp::RollingFileAppender("error", "jx.log"));
      logger.warn(e.what());
      std::cerr << e.what() << std::endl;
    }

  logging::logger applicationLogger(_T("jingxian.application"));

  if (!existFile(configFile))
    {
      LOG_FATAL(applicationLogger, _T("�����ļ� '")<< configFile << _T("' ������"));
      return -1;
    }


  logging::logger configureLogger(_T("jingxian.configure"));

  std::basic_ifstream<tchar, std::char_traits<tchar> > configStream(configFile.c_str());

  configure::ContextImpl impl(configureLogger);
  configure::Context configureContext(&impl);
  configureContext.connect(this, &Application::configure);

  tstring line;
  tstring segment;

  int lineNumber = 0;
  while (std::getline(configStream, line))
    {
      ++ lineNumber;

      // ɾ��ע��
      tstring::size_type index = line.find(_T('#'));
      if (tstring::npos != index)
        line = line.substr(index);

      // ɾ���հ�
      line = trim_right(line);
      if (line.empty())
        continue;

      // �Ƿ���
      if (_T('\\') == line[line.size()-1])
        {
          segment += line;
          continue;
        }

      if (!segment.empty())
        {
		  // ɾ���հ�
          line = segment + line;
          segment.clear();
        }
			
	  line = trim_left(line);
      if (line.empty())
        continue;

      if (!impl.call(configureContext, line))
        LOG_WARN(configureLogger, _T("����ʶ����� '")
                 << lineNumber
                 << _T("' - ")
                 << toTstring(line));

      if (impl.isExit())
        return -1;
    }

  //core_.listenWith(_T("tcp://0.0.0.0:6544"), new proxy::Proxy(core_.basePath()));
  //core_.listenWith(_T("tcp://0.0.0.0:6543"), new EchoProtocolFactory());
  core_.runForever();
  return 0;
}

void Application::onControl(DWORD dwEventType
                            , LPVOID lpEventData)
{
}

void Application::interrupt()
{
  core_.interrupt();
}


bool Application::configure(configure::Context& context, const tstring& txt)
{
  tstring::size_type index = txt.find_first_of(_T(" \t"));
  tstring command = (tstring::npos == index)?txt:txt.substr(0, index);

  if (0 == string_traits<tstring::value_type>::stricmp(_T("listen"), command.c_str()))
    {
      if (tstring::npos == index)
        {
          LOG_FATAL(context.logger(), _T("���� 'listen' ��ʽ����ȷ"));
          context.exit();
          return true;
        }

	  StringArray<tstring::value_type> sa = split(txt.c_str()+index
		  , _T(" \t")
		  , StringSplitOptions::RemoveEmptyEntries);
      if (2 > sa.size())
        {
          LOG_FATAL(context.logger(), _T("���� 'listen' ��ʽ����ȷ"));
          context.exit();
          return true;
        }

      IProtocolFactory* protocolFactory = createProtocolFactory(sa.ptr(1));
      if (null_ptr == protocolFactory)
        {
          LOG_FATAL(context.logger(), _T("�������� '")<< sa.ptr(1) << _T("' ʧ��!"));
          context.exit();
          return false;
        }

      if (!core_.listenWith(sa.ptr(0), protocolFactory))
        {
          LOG_FATAL(context.logger(), _T("���� 'listen' ��ʽ����ȷ"));
          context.exit();
          return false;
        }


	  callbacks_[sa.ptr(1)] = new _connection<IProtocolFactory
		  ,bool (configure::Context& context, const tstring& txt)>
						(protocolFactory, &IProtocolFactory::configure);

      //context.connect(protocolFactory, &IProtocolFactory::configure);
      return true;
    }

  if (0 == string_traits<tstring::value_type>::strcmp(_T("<IfModule"), command.c_str()))
  {
      if (tstring::npos == index)
	  {
          LOG_FATAL(context.logger(), _T("���� 'IfModule' ��ʽ����ȷ"));
          context.exit();
          return false;
	  }

	  StringArray<tstring::value_type> sa = split(txt.c_str()+index
		  , _T(" \t\"=>")
		  , StringSplitOptions::RemoveEmptyEntries);
      if (2 != sa.size())
        {
          LOG_FATAL(context.logger(), _T("���� 'IfModule' ��ʽ����ȷ"));
          context.exit();
          return true;
        }

	  if (0 != string_traits<tstring::value_type>::stricmp(_T("name"), sa.ptr(0)))
	    {
          LOG_FATAL(context.logger(), _T("���� 'IfModule' ��ʽ����ȷ"));
          context.exit();
          return true;
	    }

	  std::map<tstring, configure::callback_type*>::iterator it = callbacks_.find(sa.ptr(1));
	  if(it == callbacks_.end())
	  {
          LOG_FATAL(context.logger(), _T("ģ�� '") << sa.ptr(1) << _T("' ����ʶ��!"));
          context.exit();
          return false;
	  }

	  // IfModule �������
	  class IfModule : public configure::callback_type
	  {
	  public:
			IfModule(configure::callback_type* ptr)
				: ptr_(ptr)
			{
			}

			virtual ~IfModule()
			{
			}

			virtual bool call(configure::Context& context, const tstring& txt)
			{
				tstring line = replace_all(replace_all(trim_all(txt)
											, 0, _T(" "), 1, _T(""), 0)
												, 0, _T("\t"), 1, _T(""), 0);
				if(0 == string_traits<tstring::value_type>::strcmp(_T("</IfModule>"), line.c_str()))
					return true;

				return ptr_->call(context, txt);
			}
	  private:
		  configure::callback_type* ptr_;
	  };

	  context.push(new IfModule(it->second));
	  return true;
  }
  
  return false;
}

IProtocolFactory* Application::createProtocolFactory(tchar* name)
{
  if (0 == string_traits<tchar>::stricmp(_T("proxy"), name))
    return new proxy::ProxyProtocolFactory(core_.basePath());

  if (0 == string_traits<tchar>::stricmp(_T("echo"), name))
    return new EchoProtocolFactory();

  return NULL;
}

const tstring& Application::toString() const
{
  return toString_;
}

_jingxian_end