

using System;

namespace jingxian.core.runtime.Filters
{
	public sealed class AlwaysFalseFilter<T>: IFilter<T>
	{
		public bool CanFilter(Type type)
		{
			return true;
		}

		public bool MeetsCriteria(T criteria)
		{
			return false;
		}
	}
}