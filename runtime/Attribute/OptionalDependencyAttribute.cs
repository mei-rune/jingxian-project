

using System;

namespace jingxian.core.runtime
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class OptionalDependencyAttribute: Attribute
	{
	}
}