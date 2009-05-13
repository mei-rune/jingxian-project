

using System;
using jingxian.core.runtime.Filters;

namespace jingxian.core.runtime.Filters
{
	public sealed class LogicalAnd<T> : CompositeFilter<T>
	{

		public override bool MeetsCriteria(T obj)
		{
            if (Filters.Count == 0)
                throw new InvalidOperationException(Resources.ExceptionMessages.NoComponentsForCompositeFilterDefined);

			bool result = true;
			foreach (IFilter<T> filter in Filters)
			{
				result &= filter.MeetsCriteria(obj);
				if (!result)
				{
					return false;
				}
			}
			return result;
		}
	}
}