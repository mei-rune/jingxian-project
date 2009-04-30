using System;

namespace jingxian.logging
{
	public class LogConfigurationException : Exception
	{
		public LogConfigurationException() : base()
		{}

		public LogConfigurationException(string message) : base(message)
		{
		}

		public LogConfigurationException(string message, Exception exception) : base(message, exception)
		{
		}

		public LogConfigurationException(Exception exception) : base(exception.Message, exception)
		{
		}
	}
}
