using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.logging
{
    public abstract class AbstractLogger : ILog
    {

        #region static methods

        public static string ConvertLevelToString(LogLevel logLevel)
        {
            string sLogType = null;

            switch (logLevel)
            {
                case LogLevel.TRACE: sLogType = "TRACE"; break;
                case LogLevel.DEBUG: sLogType = "DEBUG"; break;
                case LogLevel.INFO: sLogType = "INFO"; break;
                case LogLevel.WARN: sLogType = "WARN"; break;
                case LogLevel.ERROR: sLogType = "ERROR"; break;
                case LogLevel.FATAL: sLogType = "FATAL"; break;

                default: break;

            }

            return sLogType;
        }


        public static LogLevel ConvertStringToLevel(string slogLevel)
        {

            slogLevel = slogLevel.ToUpper();

            if ("OFF".Equals(slogLevel)) return LogLevel.OFF;
            if ("TRACE".Equals(slogLevel)) return LogLevel.TRACE;
            if ("DEBUG".Equals(slogLevel)) return LogLevel.DEBUG;
            if ("INFO".Equals(slogLevel)) return LogLevel.INFO;
            if ("WARN".Equals(slogLevel)) return LogLevel.WARN;
            if ("ERROR".Equals(slogLevel)) return LogLevel.ERROR;
            if ("FATAL".Equals(slogLevel)) return LogLevel.FATAL;

            return LogLevel.OFF;
        }


        public static String[] GetLogLevelNames()
        {
            string[] allLevels = Enum.GetNames(typeof(LogLevel));
            return allLevels;
        }


        public static LogLevel GetLogLevel(ILog logger)
        {
            //if (logger.IsTraceEnabled) return LogLevel.TRACE;
            if (logger.IsDebugEnabled) return LogLevel.DEBUG;
            if (logger.IsInfoEnabled) return LogLevel.INFO;
            if (logger.IsWarnEnabled) return LogLevel.WARN;
            if (logger.IsErrorEnabled) return LogLevel.ERROR;
            if (logger.IsFatalEnabled) return LogLevel.FATAL;
            return LogLevel.OFF;
        }

        #endregion

        protected LogLevel _currentLogLevel = LogLevel.TRACE;

        public bool IsTraceEnabled
        {
            get { return isLevelEnabled(LogLevel.TRACE); }
        }

        public bool IsDebugEnabled
        {
            get { return isLevelEnabled(LogLevel.DEBUG); }
        }

        public bool IsInfoEnabled
        {
            get { return isLevelEnabled(LogLevel.INFO); }
        }

        public bool IsWarnEnabled
        {
            get { return isLevelEnabled(LogLevel.WARN); }
        }

        public bool IsErrorEnabled
        {
            get { return isLevelEnabled(LogLevel.ERROR); }
        }

        public bool IsFatalEnabled
        {
            get { return isLevelEnabled(LogLevel.FATAL); }
        }

        public bool IsLogOff
        {
            get { return isLevelEnabled(LogLevel.OFF); }
        }

        public string LogThresholdLevel
        {
            get { return ConvertLevelToString(_currentLogLevel); }
            set { this.CurrentLogLevel = ConvertStringToLevel(value); }
        }

        public virtual LogLevel CurrentLogLevel
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        } 

        protected virtual bool isLevelEnabled(LogLevel logLevel)
        {
            return (logLevel >= _currentLogLevel);
        }

        public void SetLogLevel(LogLevel newLogLevel)
        {
            this._currentLogLevel = newLogLevel;
        }

        public void Trace(object message)
        {
            if (this.IsTraceEnabled)
                this.log(LogLevel.TRACE, message);
        }

        public void Trace(object message, Exception ex)
        {
            if (this.IsTraceEnabled)
                this.log(LogLevel.TRACE, message, ex, false);
        }

        public void Debug(object message)
        {
            if (this.IsDebugEnabled)
                this.log(LogLevel.DEBUG, message);
        }

        public void Debug(object message, Exception ex)
        {
            if (this.IsDebugEnabled)
                this.log(LogLevel.DEBUG, message, ex, false);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
                this.log(LogLevel.DEBUG, string.Format(format, args));
        }

        public void Info(object message)
        {
            if (this.IsInfoEnabled)
                this.log(LogLevel.INFO, message);
        }

        public void Info(object message, Exception ex)
        {
            if (this.IsInfoEnabled)
                this.log(LogLevel.INFO, message, ex, false);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
                this.log(LogLevel.INFO, string.Format(format, args));
        }

        public void Warn(object message)
        {
            this.log(LogLevel.WARN, message);
        }

        public void Warn(object message, Exception ex)
        {
            this.log(LogLevel.WARN, message, ex, false);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
                this.log(LogLevel.WARN, string.Format(format, args));
        }

        public void Error(object message)
        {
            if (this.IsErrorEnabled)
                this.log(LogLevel.ERROR, message);
        }

        public void Error(object message, Exception ex)
        {
            if (this.IsErrorEnabled)
                this.log(LogLevel.ERROR, message, ex, false);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
                this.log(LogLevel.ERROR, string.Format(format, args));
        }

        public void Fatal(object message)
        {
            this.log(LogLevel.FATAL, message);
        }

        public void Fatal(object message, Exception ex)
        {
            this.log(LogLevel.FATAL, message, ex, false);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
                this.log(LogLevel.FATAL, string.Format(format, args));
        }
        protected void log(LogLevel logtype, Object message)
        {
            log(logtype, message, null, false);
        }

        protected abstract void log(LogLevel logtype,
                   Object message,
                   Exception e,
                   bool isPerformance);
		

    }
}
