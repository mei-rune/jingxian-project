

using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime.Filters
{
	public interface IFilterProvider
	{
		string Name { get; }

		string Description { get; }

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Interface")]
        string Interface { get; }

		string Implementation { get; }
	}
}