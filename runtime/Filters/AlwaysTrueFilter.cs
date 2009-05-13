

using System;

namespace jingxian.core.runtime.Filters
{
	public sealed class AlwaysTrueFilter<T>: IFilter<T>
	{

		public bool CanFilter(Type type)
		{
			return true;
		}

		public bool MeetsCriteria(T criteria)
		{
			return true;
		}
	}
}