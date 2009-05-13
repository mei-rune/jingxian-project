
using System;

namespace jingxian.core.runtime
{

	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class ExtensionContractAttribute: Attribute
	{
		private readonly string _point;

		public ExtensionContractAttribute()
		{
		}

		public ExtensionContractAttribute(string point)
		{
			_point = point;
		}

		public string Point
		{
            get { return _point; }
		}

	}
}
