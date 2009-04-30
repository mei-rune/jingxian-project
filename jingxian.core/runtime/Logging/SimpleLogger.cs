
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;


namespace jingxian.logging
{
    public class SimpleLogger : AbstractLogger
	{
		private const string APP_KEY_LOG_LEVEL = "jingxian.logging.SimpleLogger.Level";

		private const string APP_KEY_LOG_DIR = "jingxian.logging.SimpleLogger.LogDirectory";

        private const string APP_KEY_LOG_FILENAME = "jingxian.logging.SimpleLogger.Filename";

        private const string APP_KEY_LOG_FILESIZE = "jingxian.logging.SimpleLogger.FileSize";

		private const string APP_KEY_LOG_OUTPUTDEBUG = "jingxian.logging.SimpleLogger.OutputDebug";

        private const string APP_KEY_LOG_CONSOLEOUTPUT = "jingxian.logging.SimpleLogger.ConsoleOutput";

		private string			_logDirectory = AppDomain.CurrentDomain.BaseDirectory;
		private string			_filename = "commons.log";
		private string			_className = null;

		private StreamWriter	_swLog;
		private bool			_copyToOutDebug = false;
		private bool			_performance = false;
		private bool			_consoleOutput;
		private bool			_logToFile = true;
        private long            _fileMaxLength = 1024*1024*2;

		public SimpleLogger(string className)
		{			
			this._className = className;
			configure();
		}
		

		#region Properties

		public bool IsPerformanceEnabled
		{
			get { return this._performance; }
		}
		
		public string LogDirectory
		{
			get {return _logDirectory;}
			set {_logDirectory = value;}
		}

		public bool CopyToOutputDebugString
		{
			get {return _copyToOutDebug;}
			set {_copyToOutDebug = value;}
		}

		public bool ConsoleOutput
		{
			get {  return _consoleOutput; }
			set { _consoleOutput = value; }    
		}

		public string FileName
		{
			get {  return _filename; }
			set { _filename = value; }    
		}

		#endregion

		#region Public Methods


		public void Performance(object message)
		{
			this.LogPerformance(message);
		}

		#endregion

		#region Private and Protected methods
		
		protected void LogPerformance(Object message)
		{
			log (LogLevel.TRACE, message, null, true);
		}
		

		protected override void log(LogLevel logtype, 
						   Object message, 
						   Exception e,
						   bool	isPerformance) 
		{

            try
            {
                if (logtype == LogLevel.OFF)
                    return;

                StringBuilder buffyWithDateTimeStamp = new StringBuilder();
                StringBuilder buffy = new StringBuilder();
                String sLogType = "";

                buffyWithDateTimeStamp.Append(DateTime.Now.ToString("dd-MM-yyyy HH:MM:ss:fff"));
                buffyWithDateTimeStamp.Append(" [");
                buffyWithDateTimeStamp.Append(Convert.ToString(System.Threading.Thread.CurrentThread.ManagedThreadId));
                buffyWithDateTimeStamp.Append("]");

                if (isPerformance)
                    sLogType = "[PERFORMANCE]: ";
                else
                {
                    switch (logtype)
                    {
                        case LogLevel.TRACE: sLogType = "[TRACE]: "; break;
                        case LogLevel.DEBUG: sLogType = "[DEBUG]: "; break;
                        case LogLevel.INFO: sLogType = "[INFO]: "; break;
                        case LogLevel.WARN: sLogType = "[WARN]: "; break;
                        case LogLevel.ERROR: sLogType = "[ERROR]: "; break;
                        case LogLevel.FATAL: sLogType = "[FATAL]: "; break;
                        default: break;
                    }
                }

                buffy.Append(sLogType);

                buffy.Append(this._className);
                buffy.Append(" - ");
                buffy.Append(message.ToString());


                if (e != null)
                {
                    buffy.Append(" <");
                    buffy.Append(e.ToString());
                    buffy.Append(">");
                }

                buffyWithDateTimeStamp.Append(buffy.ToString());

                if (this.FileName != null && this.FileName.Length > 0)
                {
                    openFile();
                    writelog(buffyWithDateTimeStamp.ToString());
                    closeFile();
                }

                if (CopyToOutputDebugString || (logtype == LogLevel.DEBUG))
                    OutputDebugString(buffy.ToString());

                if (this.ConsoleOutput)
                    System.Console.Error.WriteLine(buffyWithDateTimeStamp.ToString());
            }
            catch//( Exception e)
            {
            //    System.Console.WriteLine( e );
            }
		}

		private void writelog(string msg) 
		{
			if (!_logToFile) return;

			if (_swLog == null) return;

			lock(_swLog)
			{
				_swLog.WriteLine(msg);
				_swLog.Flush();
			}
		}

		private void openFile() 
		{
			if (!_logToFile) return;

			if (this.LogDirectory.IndexOf("${TMP}") >= 0)
				this.LogDirectory = this.LogDirectory.Replace("${TMP}",  Path.GetTempPath());

			if (!Directory.Exists(this.LogDirectory)) 
			{
				Directory.CreateDirectory(this.LogDirectory);
			}
		
			if (!this.LogDirectory.EndsWith(@"\") && !this.LogDirectory.EndsWith("/"))
				this.LogDirectory += @"\";

			string sFilename = this.LogDirectory.Replace(@"\\", @"\") +  this.FileName;

			try 
			{
                if (!File.Exists(sFilename))
                {
                    FileStream fs = File.Create(sFilename);
                    fs.Close();
                }
                else
                {
                    if (new FileInfo(sFilename).Length > _fileMaxLength)
                    {
                        File.Delete(sFilename);
                        
                        FileStream fs = File.Create(sFilename);
                        fs.Close();
                    }
                }
				_swLog = File.AppendText(sFilename);					
			} 	
			catch (IOException ioEx)
			{
				this.LogDirectory = Path.GetTempPath();

				OutputDebugString(ioEx.StackTrace);
				System.Console.Error.WriteLine(ioEx.StackTrace);

				try 
				{
					if (!File.Exists(sFilename))
					{
						FileStream fs = File.Create(sFilename);
						fs.Close();
					}
					_swLog = File.AppendText(sFilename);					
				}
				catch (Exception iEx)
				{
					CopyToOutputDebugString = true;
					this.ConsoleOutput = true;
					_logToFile = false;
					OutputDebugString(iEx.StackTrace);
					System.Console.Error.WriteLine(iEx.StackTrace);
				}
			}
			catch (Exception e)
			{				
				CopyToOutputDebugString = true;
				this.ConsoleOutput = true;
				_logToFile = false;
				
				OutputDebugString(e.StackTrace);
				System.Console.Error.WriteLine(e.StackTrace);
			}						
		}
		
		private void closeFile() 
		{
			if (!_logToFile) return;

			if (_swLog == null) return;

			try 
			{
				_swLog.Close();
			} 
			catch 
			{	
			}
	
		}

		private void configure()
		{

			string appSettingValue = null;

			appSettingValue = System.Configuration.ConfigurationManager .AppSettings[APP_KEY_LOG_LEVEL];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
			{
				appSettingValue = appSettingValue.ToUpper();
				
				if (appSettingValue.ToUpper().Equals("TRACE"))			_currentLogLevel = LogLevel.TRACE;
				else if (appSettingValue.ToUpper().Equals("DEBUG"))		_currentLogLevel = LogLevel.DEBUG;
				else if (appSettingValue.ToUpper().Equals("INFO"))		_currentLogLevel = LogLevel.INFO;
				else if (appSettingValue.ToUpper().Equals("WARN"))		_currentLogLevel = LogLevel.WARN;
				else if (appSettingValue.ToUpper().Equals("ERROR"))		_currentLogLevel = LogLevel.ERROR;
				else if (appSettingValue.ToUpper().Equals("FATAL"))		_currentLogLevel = LogLevel.FATAL;
				else if (appSettingValue.ToUpper().Equals("OFF"))		_currentLogLevel = LogLevel.OFF;				
			}

            appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_DIR];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
				this.LogDirectory = appSettingValue;



            appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_FILENAME];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
                this.FileName = appSettingValue;

            appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_FILESIZE];

            if ((appSettingValue != null) && (appSettingValue.Length > 0))
                this._fileMaxLength = Convert.ToInt64(appSettingValue);


            appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_OUTPUTDEBUG];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
				CopyToOutputDebugString = Convert.ToBoolean(appSettingValue);



            appSettingValue = System.Configuration.ConfigurationManager.AppSettings[APP_KEY_LOG_CONSOLEOUTPUT];

			if ((appSettingValue != null) && (appSettingValue.Length > 0))
				this.ConsoleOutput = Convert.ToBoolean(appSettingValue);

            

		}

		#endregion


		[System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError=true, EntryPoint="OutputDebugStringW")]
		private static extern void OutputDebugString(
			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
			string lpOutputString);
	}
}
