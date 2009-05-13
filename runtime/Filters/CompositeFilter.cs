

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime.Filters
{
	public abstract class CompositeFilter<T>: ICompositeFilter<T>
	{
		private readonly IList<IFilter<T>> _Filters = new List<IFilter<T>>();


		public virtual IList<IFilter<T>> Filters
		{
            get { return _Filters; }
		}

		public virtual bool CanFilter(Type type)
		{
			if (_Filters.Count == 0)
                throw new InvalidOperationException(Resources.ExceptionMessages.NoComponentsForCompositeFilterDefined );

			foreach (IFilter<T> filter in _Filters)
			{
				if (!filter.CanFilter(type))
				{
					return false;
				}
			}
			return true;
		}


		public abstract bool MeetsCriteria(T obj);
	}
}