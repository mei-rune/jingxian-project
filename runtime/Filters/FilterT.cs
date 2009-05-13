

using System;

namespace jingxian.core.runtime.Filters
{
	[Serializable]
	public abstract class Filter<T> : IFilter<T>
	{
		public virtual bool CanFilter(Type type)
		{
			return type is T;
		}

		public abstract bool MeetsCriteria(T obj);
	}
}