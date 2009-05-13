

using System;
using jingxian.core.runtime.Filters;

namespace jingxian.core.runtime.Filters
{
	public sealed class LogicalNot<T>: CompositeFilter<T>
	{

		public override bool MeetsCriteria(T obj)
		{
			if (Filters.Count < 1)
                throw new InvalidOperationException("Not过滤器的了过滤器的个数只能为1.");

            if (Filters.Count > 1)
                throw new InvalidOperationException("Not过滤器的了过滤器的个数只能为1.");


			return (!Filters[0].MeetsCriteria(obj));
		}
	}
}