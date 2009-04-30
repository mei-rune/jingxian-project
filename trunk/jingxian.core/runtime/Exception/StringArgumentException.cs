

using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace jingxian.core.runtime
{
using jingxian.core.runtime.Resources;

	[Serializable]
    public sealed class StringArgumentException: ArgumentException
	{
		public StringArgumentException()
		{
		}

		public StringArgumentException(string parameterName)
			: base(GetMessage(parameterName), parameterName)
		{
		}

		public StringArgumentException(string parameterName, Exception inner)
			: base(GetMessage(parameterName), parameterName, inner)
		{
		}

		private StringArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		private static string GetMessage(string parameterName)
		{
			return string.Format(CultureInfo.InvariantCulture,
				ExceptionMessages.StringArgumentNullOrEmpty,
				parameterName);
		}
	}
}
