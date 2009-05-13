
using System;
using System.Collections;

using jingxian.logging;

namespace jingxian.logging
{

    //public class SimpleTrace : ITrace
    //{
    //    string _name;
    //    ILog _logger;

    //    public SimpleTrace(string name, ILog logger )
    //    {
    //        _name = name;
    //        _logger = logger;
    //    }

    //    public bool IsDebugEnabled { get { return _logger.IsDebugEnabled; } }
    //    public bool IsErrorEnabled { get { return _logger.IsErrorEnabled; } }
    //    public bool IsFatalEnabled { get { return _logger.IsFatalEnabled; } }
    //    public bool IsInfoEnabled { get { return _logger.IsInfoEnabled; } }
    //    public bool IsWarnEnabled { get { return _logger.IsWarnEnabled; } }

    //    #region ITrace 成员

    //    public void WriteInboundBuffer(IOBuffer IOBuffer)
    //    {
    //        //try
    //        //{
    //        //    if ( null != IOBuffer && 0 < IOBuffer.Count )
    //        //        Console.WriteLine(_name + " In : [" + Encoding.UTF8.GetString(IOBuffer.Array, IOBuffer.Begin, IOBuffer.Count) + "]");
    //        //}
    //        //catch
    //        //{
    //        //    Console.WriteLine(_name + " In : [无法解码]");
    //        //}
    //    }
    //    public void WriteOutboundBuffer(IOBuffer IOBuffer)
    //    {
    //        //    try
    //        //    {
    //        //        if (null != IOBuffer && 0 < IOBuffer.Count)
    //        //            Console.WriteLine(_name + " Out : [" + Encoding.UTF8.GetString(IOBuffer.Array, IOBuffer.Begin, IOBuffer.Count) + "]");
    //        //    }
    //        //    catch
    //        //    {
    //        //        Console.WriteLine(_name + " Out : [无法解码]");
    //        //    }
    //    }

    //    public void Debug(TransportWay way, string message)
    //    {
    //        if (IsDebugEnabled)
    //            _logger.Debug(_name + way + " : " + message);
    //    }
    //    public void Debug(TransportWay way, string message, Exception exception)
    //    {
    //        if (IsDebugEnabled)
    //        {
    //            _logger.Debug(_name + way + " : " + message);
    //            _logger.Debug(exception);
    //        }
    //    }
    //    public void DebugFormat(TransportWay way, string format, params object[] args)
    //    {
    //        if (IsDebugEnabled)
    //            _logger.DebugFormat(_name + way + " : " + format, args);
    //    }

    //    public void Error(TransportWay way, string message)
    //    {
    //        if (IsErrorEnabled)
    //            _logger.Error(_name + way + " : " + message);
    //    }
    //    public void Error(TransportWay way, string message, Exception exception)
    //    {
    //        if (IsErrorEnabled)
    //        {
    //            _logger.Error(_name + way + " : " + message);
    //            _logger.Error(exception);
    //        }
    //    }
    //    public void ErrorFormat(TransportWay way, string format, params object[] args)
    //    {
    //        if (IsErrorEnabled)
    //            _logger.ErrorFormat(_name + way + " : " + format, args);
    //    }

    //    public void Fatal(TransportWay way, string message)
    //    {
    //        if (IsFatalEnabled)
    //            _logger.Fatal(_name + way + " : " + message);
    //    }
    //    public void Fatal(TransportWay way, string message, Exception exception)
    //    {
    //        if (IsFatalEnabled)
    //        {
    //            _logger.Fatal(_name + way + " : " + message);
    //            _logger.Fatal(exception);
    //        }
    //    }

    //    public void FatalFormat(TransportWay way, string format, params object[] args)
    //    {
    //        if (IsFatalEnabled)
    //            _logger.FatalFormat(_name + way + " : " + format, args);
    //    }

    //    public void Info(TransportWay way, string message)
    //    {
    //        if (IsInfoEnabled)
    //            _logger.Info(_name + way + " : " + message);
    //    }
    //    public void Info(TransportWay way, string message, Exception exception)
    //    {
    //        if (IsInfoEnabled)
    //        {
    //            _logger.Info(_name + way + " : " + message);
    //            _logger.Info(exception);
    //        }
    //    }
    //    public void InfoFormat(TransportWay way, string format, params object[] args)
    //    {
    //        if (IsInfoEnabled)
    //            _logger.InfoFormat(_name + way + " : " + format, args);
    //    }

    //    public void Warn(TransportWay way, string message)
    //    {
    //        if (IsWarnEnabled)
    //            _logger.Warn(_name + way + " : " + message);
    //    }
    //    public void Warn(TransportWay way, string message, Exception exception)
    //    {
    //        if (IsWarnEnabled)
    //        {
    //            _logger.Warn(_name + way + " : " + message);
    //            _logger.Warn(exception);
    //        }
    //    }
    //    public void WarnFormat(TransportWay way, string format, params object[] args)
    //    {
    //        if (IsWarnEnabled)
    //            _logger.WarnFormat(_name + way + " : " + format, args);
    //    }

    //    #endregion
    //}

    public class SimpleLoggerFactory : ILogFactory, ILogConfiguration
	{
        #region ILogConfiguration 成员

        public void Init(string path)
        {
        }

        #endregion

        private string _prefix = string.Empty;

        public SimpleLoggerFactory()
        {
        }

        public SimpleLoggerFactory(string prefix)
        {
            _prefix = prefix;
        }

        #region ILogFactory 成员

        public ILog GetLogger(Type classType)
        {
            return new SimpleLogger(classType.FullName);
        }

        public ILog GetLogger(string className)
        {
            return new SimpleLogger(className);
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
