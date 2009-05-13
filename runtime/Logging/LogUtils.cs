
using System;

namespace jingxian.logging
{

    public class LogUtils
    {
        public const string Log4NET = "jingxian.logging.Log4NetFactory";
        public const string SimpleLogger = "jingxian.logging.SimpleLoggerFactory";
        public const string SystemTraceLogger = "jingxian.logging.SystemTraceLoggerFactory";
#if LOG4NET
        private static ILogFactory _factory = new Log4NetFactory();
#elif SYSTEMTRACE
        private static ILogFactory _factory = new SystemTraceLoggerFactory();
#else
        private static ILogFactory _factory = new StdFactory();
#endif
        public static ILogFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public static ILog GetLogger(Type classType)
        {
            return Factory.GetLogger(classType);
        }

        public static ILog GetLogger(String className)
        {
            return Factory.GetLogger(className);
        }

        public static ILogFactory NewFactory(string className)
        {
            return Factory.NewFactory(className);
        }

        public static ILogFactory NewFactory(Type classType)
        {
            return Factory.NewFactory(classType);
        }

        public static void Initialize( string type, string path)
        {
            Type logType = Type.GetType(type);
            if (null != logType)
            {
                _factory = (ILogFactory)Activator.CreateInstance(logType);
            }
            Initialize(path);
        }

        public static void Initialize( string path )
        {
            if (System.IO.File.Exists(path))
            {
                ((ILogConfiguration)_factory).Init(path);
            }
        }
    }
}
