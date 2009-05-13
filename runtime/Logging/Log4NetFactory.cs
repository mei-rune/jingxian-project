
using System;
using System.Collections;

#if LOG4NET

namespace jingxian.logging
{
    public class Log4NetFactory : ILogFactory, ILogConfiguration
	{
        //log4net.Repository.ILoggerRepository _repository;

        #region ILogFactory 成员

        public ILog GetLogger(Type classType)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger( classType ));
        }

        public ILog GetLogger(string className)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(className));
        }

        public ILogFactory NewFactory(string className)
        {
            return this;
        }

        public ILogFactory NewFactory(Type classType)
        {
            return this;
        }

        public static void Initialize( string path )
        {
            if (null == LogUtils.LogFactory || !(LogUtils.LogFactory is Log4NetFactory) )
                LogUtils.LogFactory = new Log4NetFactory();
            if (System.IO.File.Exists(path))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(System.IO.Path.GetFullPath(path)));
                //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(System.IO.Path.GetFullPath(path)));
            }
        }

        #endregion

        #region ILogConfiguration 成员

        public void Init(string path)
        {
            Initialize(path);
        }

        #endregion
    }
}

#endif