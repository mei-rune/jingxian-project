
using System;
using System.Runtime.Serialization;

namespace jingxian.core.runtime
{
	[Serializable]
	public class DependencyResolutionException: RuntimeException
	{
		public DependencyResolutionException()
		{
		}

		public DependencyResolutionException(string message)
			: base(message)
		{
		}

		public DependencyResolutionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DependencyResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}