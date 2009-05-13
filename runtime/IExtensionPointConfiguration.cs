


namespace jingxian.core.runtime
{

	public interface IExtensionPointConfiguration
	{
        string Id { get; }

        string BundleId { get; }

        string Name { get; }

        string Description { get; }

        string Configuration { get; }

		void Merge(ExtensionPointAttribute attribute);
	}
}