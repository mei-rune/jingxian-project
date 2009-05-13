

using System;

namespace jingxian.core.runtime.Filters
{
	public interface IFilter<T>
	{
		bool CanFilter(Type type);

		bool MeetsCriteria(T obj);
	}
}