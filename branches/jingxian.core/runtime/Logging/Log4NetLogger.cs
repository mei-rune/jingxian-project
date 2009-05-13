
using System;
using System.IO;
using System.Xml;

#if LOG4NET

using log4net;
using log4net.Config;

namespace jingxian.logging
{
    public class Log4NetLogger : ILog
    {
        private log4net.ILog _logger = null;

        #region Constructors

        public Log4NetLogger(log4net.ILog logger)
        {
            this._logger = logger;
        }

        #endregion

        #region Properties

        public log4net.ILog Logger
        {
            get { return _logger; }
        }

        public bool IsDebugEnabled
        {
            get { return this.Logger.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this.Logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return this.Logger.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return this.Logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return this.Logger.IsFatalEnabled; }
        }


        #endregion

        #region Implementation

        public void Debug(object message)
        {
            this.Logger.Debug(message);
        }

        public void Debug(object message, Exception ex)
        {
            this.Logger.Debug(message, ex);
        }


        public void DebugFormat(string format, params object[] args)
        {
            this.Logger.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            this.Logger.Info(message);
        }

        public void Info(object message, Exception ex)
        {
            this.Logger.Info(message, ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.Logger.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            this.Logger.Warn(message);
        }

        public void Warn(object message, Exception ex)
        {
            this.Logger.Warn(message, ex);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.Logger.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            this.Logger.Error(message);
        }

        public void Error(object message, Exception ex)
        {
            this.Logger.Error(message, ex);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.Logger.ErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            this.Logger.Fatal(message);
        }

        public void Fatal(object message, Exception ex)
        {
            this.Logger.Fatal(message, ex);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.Logger.FatalFormat(format, args);
        }

        #endregion
    }
}

#endif
