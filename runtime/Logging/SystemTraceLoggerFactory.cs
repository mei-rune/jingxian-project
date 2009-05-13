
using System;
using System.Collections;


namespace jingxian.logging
{
    public class SystemTraceLoggerFactory : ILogFactory, ILogConfiguration
    {
        #region ILogConfiguration 成员

        public void Init(string path)
        {
        }

        #endregion

        #region ILogFactory 成员

        public ILog GetLogger(Type classType)
        {
            return new SystemTraceLogger(classType.FullName);
        }

        public ILog GetLogger(string className)
        {
            return new SystemTraceLogger(className);
        }

        public ILogFactory NewFactory(string className)
        {
            return this;
        }

        public ILogFactory NewFactory(Type classType)
        {
            return this;
        }

        #endregion
    }
}
