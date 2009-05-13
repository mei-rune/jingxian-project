

using System;

namespace jingxian.core.runtime
{
	public interface IConfigurable<T>
	{
		void Configure(T cfg);
	}
}