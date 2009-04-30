

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime.Filters
{
	public interface ICompositeFilter<T>: IFilter<T>
	{
        [SuppressMessage("Microsoft.Design", "CA1006")]
        IList<IFilter<T>> Filters { get; }
	}
}