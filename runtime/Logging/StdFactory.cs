using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace jingxian.logging
{

    public class StdFactory : ILogFactory
    {
        public class StdLogger : ILog
        {
            string _name;

            public StdLogger(string name)
            {
                _name = name;
            }
            #region ILog 成员

            public bool IsDebugEnabled { get { return true; } }
            public bool IsErrorEnabled { get { return true; } }
            public bool IsFatalEnabled { get { return true; } }
            public bool IsInfoEnabled { get { return true; } }
            public bool IsWarnEnabled { get { return true; } }

            public void Debug(object message)
            { 
                if (IsDebugEnabled)
                    Console.WriteLine(_name + message);
            }
            public void Debug(object message, Exception exception)
            {
                if (IsDebugEnabled)
                {
                    Console.WriteLine(_name + message);
                    Console.WriteLine(exception.StackTrace); 
                }
            }
            public void DebugFormat(string format, params object[] args)
            {
                if (IsDebugEnabled)
                    Console.WriteLine( _name + format, args );
            }

            public void Error(object message)
            {
                if (IsErrorEnabled)
                    Console.WriteLine(_name + message);
            }

            public void Error(object message, Exception exception)
            {
                if (IsErrorEnabled)
                {
                    Console.WriteLine(_name + message);
                    Console.WriteLine(exception.StackTrace);
                }
            }
            public void ErrorFormat(string format, params object[] args)
            {
                if (IsErrorEnabled)
                    Console.WriteLine(_name + format, args);
            }

            public void Fatal(object message)
            {
                if (IsFatalEnabled)
                    Console.WriteLine(_name + message);
            }
            public void Fatal(object message, Exception exception)
            {
                if (IsFatalEnabled)
                {
                    Console.WriteLine(_name + message);
                    Console.WriteLine(exception.StackTrace);
                }
            }
            public void FatalFormat(string format, params object[] args)
            {
                if (IsFatalEnabled)
                    Console.WriteLine(_name + format, args);
            }

            public void Info(object message) 
            {
                if (IsInfoEnabled)
                    Console.WriteLine(_name + message);
            }
            public void Info(object message, Exception exception)
            {
                if (IsInfoEnabled)
                {
                    Console.WriteLine(_name + message);
                    Console.WriteLine(exception.StackTrace);
                }
            }
            public void InfoFormat(string format, params object[] args)
            {
                if (IsInfoEnabled)
                    Console.WriteLine(_name + format, args);
            }

            public void Warn(object message) 
            { 
                if (IsWarnEnabled)
                    Console.WriteLine(_name + message);
            }
            public void Warn(object message, Exception exception)
            {
                if (IsWarnEnabled)
                {
                    Console.WriteLine(_name + message);
                    Console.WriteLine(exception.StackTrace);
                }
            }
            public void WarnFormat(string format, params object[] args)
            {
                if (IsWarnEnabled)
                    Console.WriteLine(_name + format, args);
            }

            #endregion
        }

        private string _prefix = string.Empty;

        public StdFactory()
        { }


        public StdFactory(string prefix)
        {
            _prefix = prefix;
        }

        #region ITraceFactory 成员

        public ILog GetLogger(string name)
        {
            if (string.IsNullOrEmpty(_prefix))
                return new StdLogger("[" + name + "]");
            else
                return new StdLogger("[" + _prefix + "." + name + "]");
        }

        public ILog GetLogger(Type classType)
        {
            if (string.IsNullOrEmpty(_prefix))
                return new StdLogger("[" + classType.ToString() + "]");
            else
                return new StdLogger("[" + _prefix + "." + classType.ToString() + "]");
        }

        public ILogFactory NewFactory(string className)
        {
            return new StdFactory(className);
        }

        public ILogFactory NewFactory(Type classType)
        {
            return new StdFactory(classType.ToString());
        }

        #endregion
    }
}
