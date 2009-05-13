
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;


namespace jingxian.logging
{
	public class SystemTraceLogger : AbstractLogger
	{

		private const string APP_KEY_LOG_LEVEL = "jingxian.logging.systemTraceLogger.Level";
			
		private string			_className = null;				
		private TraceSwitch		_traceSwitch = null;
		private string			_traceSwitchName = String.Empty;
		private bool			_performance = false;		
	

		public SystemTraceLogger(string className)
		{			
			this._className = className;
			configure();
		}

		#region Properties
		
		public bool IsPerformanceEnabled
		{
			get { return this._performance; }
		}

		public override LogLevel CurrentLogLevel
		{
			get { return _currentLogLevel; }
			set 
			{ 
				_currentLogLevel = value;

				switch(this.CurrentLogLevel)
				{
					case LogLevel.OFF: this._traceSwitch.Level = TraceLevel.Off; break;
					case LogLevel.TRACE: this._traceSwitch.Level = TraceLevel.Verbose; break;
					case LogLevel.DEBUG: this._traceSwitch.Level = TraceLevel.Verbose ; break;
					case LogLevel.INFO: this._traceSwitch.Level = TraceLevel.Info; break;
					case LogLevel.WARN: this._traceSwitch.Level = TraceLevel.Warning; break;
					case LogLevel.ERROR: this._traceSwitch.Level = TraceLevel.Error; break;
					case LogLevel.FATAL: this._traceSwitch.Level = TraceLevel.Error; break;
				}			
			}
		}

		public TraceSwitch LoggerTraceSwitch
		{
			get { return _traceSwitch; }
			set 
			{
				_traceSwitch = value; 			

				switch (this._traceSwitch.Level)
				{ 
					case TraceLevel.Verbose: this._currentLogLevel = LogLevel.DEBUG; break;
					case TraceLevel.Info:	 this._currentLogLevel = LogLevel.INFO; break;
					case TraceLevel.Warning: this._currentLogLevel = LogLevel.WARN; break;
					case TraceLevel.Error: 	 this._currentLogLevel = LogLevel.ERROR; break;
					case TraceLevel.Off:	 this._currentLogLevel = LogLevel.OFF; break;
				}			
			}
		}

		
		public TraceListenerCollection Listeners
		{
			get { return System.Diagnostics.Trace.Listeners; }
		}

		#endregion


		#region Public methods
		
		public void Performance(object message)
		{
			this.log(LogLevel.TRACE, message, null, true);
		}

		#endregion

	
		#region Private methods

		protected override void log(LogLevel logtype, 
						   Object message, 
						   Exception e,
						   bool isPerformance) 
		{

			if (logtype == LogLevel.OFF)
				return;
			
			StringBuilder buffy = new StringBuilder();		
			String sLogType = "";
			
			buffy.Append(DateTime.Now.ToString("dd-MM-yyyy HH:MM:ss:fff"));
			buffy.Append(" [");
			buffy.Append(Convert.ToString( System.Threading.Thread.CurrentThread.ManagedThreadId ));
			buffy.Append("]");
			
			if (isPerformance)
			{
				sLogType = "[PERFORMANCE]: ";
			}
			else
			{
				switch(logtype) 
				{
					case LogLevel.TRACE:	sLogType = "[TRACE]: ";	break;
					case LogLevel.DEBUG:	sLogType = "[DEBUG]: ";	break;
					case LogLevel.INFO:		sLogType = "[INFO]: ";	break;
					case LogLevel.WARN:		sLogType = "[WARN]: ";break;
					case LogLevel.ERROR:	sLogType = "[ERROR]: ";	break;
					case LogLevel.FATAL:	sLogType = "[FATAL]: ";	break;
					
					default: break;

				}
			}
			buffy.Append(sLogType);

			buffy.Append(this._className);
			buffy.Append( " - ");
			buffy.Append(message.ToString());
			

			if(e != null) 
			{
				buffy.Append(" <");
				buffy.Append(e.ToString());
				buffy.Append(">");
			}

			System.Diagnostics.Trace.WriteLine(buffy);					
		}


		private void configure()
		{
			this.LoggerTraceSwitch = new TraceSwitch(this.GetType().Name + "TraceSwitch", "The default TraceSwitch used by " + this.GetType().Name);

			string appSettingValue = null;

			appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_LEVEL];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
			{				
				appSettingValue = appSettingValue.ToUpper();
				
				if (appSettingValue.ToUpper().Equals("TRACE"))			this.CurrentLogLevel = LogLevel.TRACE;
				else if (appSettingValue.ToUpper().Equals("DEBUG"))		this.CurrentLogLevel = LogLevel.DEBUG;
				else if (appSettingValue.ToUpper().Equals("INFO"))		this.CurrentLogLevel = LogLevel.INFO;
				else if (appSettingValue.ToUpper().Equals("WARN"))		this.CurrentLogLevel = LogLevel.WARN;
				else if (appSettingValue.ToUpper().Equals("ERROR"))		this.CurrentLogLevel = LogLevel.ERROR;
				else if (appSettingValue.ToUpper().Equals("FATAL"))		this.CurrentLogLevel = LogLevel.FATAL;
				else if (appSettingValue.ToUpper().Equals("OFF"))		this.CurrentLogLevel = LogLevel.OFF;

			}					
		}
		#endregion

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError=true, EntryPoint="OutputDebugStringW")]
		private static extern void OutputDebugString(
			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
			string lpOutputString);
	}
}
