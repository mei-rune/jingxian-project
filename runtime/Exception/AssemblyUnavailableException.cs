
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Globalization;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.Resources;

	[Serializable]
	public sealed class AssemblyUnavailableException: RuntimeException
	{
		private readonly string _name;

		public AssemblyUnavailableException()
		{
		}

		public AssemblyUnavailableException(string assemblyName)
			: base(GetMessage(assemblyName))
		{
			_name = assemblyName;
		}

		public AssemblyUnavailableException(string assemblyName, Exception inner)
			: base(GetMessage(assemblyName), inner)
		{
			_name = assemblyName;
		}

		private AssemblyUnavailableException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		private static string GetMessage(string assemblyName)
		{
			return string.Format(CultureInfo.InvariantCulture,
				ExceptionMessages.AssemblyUnavailable,
				assemblyName);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Name", Name);
		}


		public string Name
		{
			get
			{
				return _name ?? string.Empty;
			}
		}
	}
}