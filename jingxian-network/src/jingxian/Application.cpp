// Jingxian_Network.cpp : 定义控制台应用程序的入口点。

#include "pro_config.h"
#include <iostream>
#include "jingxian/directory.h"
#include "jingxian/networks/IOCPServer.h"
#include "jingxian/protocol/EchoProtocol.h"
#include "jingxian/protocol/Proxy/Proxy.h"
#include "jingxian/protocol/EchoProtocolFactory.h"
#include "jingxian/Application.h"

# include "log4cpp/PropertyConfigurator.hh"
# include "log4cpp/Category.hh"
# include "log4cpp/Appender.hh"
# include "log4cpp/NTEventLogAppender.hh"
# include "log4cpp/NTEventLogAppender.hh"
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
    tcout << _T("使用方法如下:") << std::endl;

    tcout << _T("安装一个后台服务:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --install Win32服务名") 
		<< _T(" [Win32服务的显示名]") << _T(" [Win32服务的描述信息]") 
		<< _T(" [Win32服务的执行程序名称]") << _T(" [Win32服务的参数1]")
		<< _T(" [Win32服务的参数2] ...") << std::endl;

    tcout << _T("卸载一个后台服务:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --uninstall Win32服务名") << std::endl;

    tcout << _T("启动一个后台服务:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --start Win32服务名") 
		<< _T(" [Win32服务的参数1]") << _T(" [Win32服务的参数2] ...") << std::endl;

    tcout << _T("停止一个后台服务:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --stop Win32服务名") << std::endl;

    tcout << _T("作为一个控制台程序运行:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --console ")
		<< _T("[Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;

    tcout << _T("作为一个后台服务运行:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --service") 
		<< _T(" [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;

    tcout << _T("获得帮助:") << std::endl;
    tcout << _T("\t") << getFileName(args[0]) << _T(" --help") << std::endl;
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
            tcout << _T("安装服务失败 - 参数不正确,正确使用方法为:") << std::endl;
            tcout << _T("\t ") << args[0] << _T(" --install Win32服务名") << _T(" [Win32服务的显示名] [Win32服务的描述信息] [Win32服务的执行程序名称] [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
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
            tcout << _T("卸载服务失败 - 参数不正确,正确使用方法为:") << std::endl;
            tcout << _T("\t ") << args[0] << _T(" --uninstall Win32服务名") << std::endl;
            return -1;
        }

        return uninstallService(args[2], tcout);
    }

    if (0 == string_traits<tchar>::stricmp(_T("--start"), args[1]))
    {
        if (argc < 3)
        {
            tcout << _T("启动服务失败 - 参数不正确,正确使用方法为:") << std::endl;
            tcout << _T("\t ") << args[0] << _T(" --start Win32服务名 [Win32服务的参数1] [Win32服务的参数2] ...") << std::endl;
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
            tcout << _T("停止服务失败 - 参数不正确,正确使用方法为:") << std::endl;
            tcout << _T("\t ") << args[0] << _T(" --stop Win32服务名") << std::endl;
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
    tstring description = _T("jingxian 后台服务程序");
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
    try
    {
        tstring nm = combinePath(getApplicationDirectory(), _T("log4cpp.config"));
        log4cpp::PropertyConfigurator::configure(toNarrowString(nm));
    }
    catch (const log4cpp::ConfigureFailure& e)
    {
        log4cpp::Category::getRoot().warn(e.what());
    }
    catch (const std::exception& e)
    {
        std::cerr << e.what() << std::endl;
    }
}

Application::~Application()
{
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

    core_.listenWith(_T("tcp://0.0.0.0:6544"), new proxy::Proxy(core_.basePath()));
    core_.listenWith(_T("tcp://0.0.0.0:6543"), new EchoProtocolFactory());
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

const tstring& Application::toString() const
{
    return toString_;
}

_jingxian_end