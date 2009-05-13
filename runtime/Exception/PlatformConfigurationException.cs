
using System;

namespace jingxian.core.runtime
{
	[Serializable]
	public class PlatformConfigurationException: RuntimeException
	{
		public PlatformConfigurationException()
		{
		}

		public PlatformConfigurationException(string message)
			: base(message)
		{
		}

		public PlatformConfigurationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected PlatformConfigurationException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
