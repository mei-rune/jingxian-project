

using System.Collections.Generic;

namespace jingxian.core.runtime
{
	public interface IBundleService
	{
		IEnumerable<IBundle> Bundles { get; }

		bool InitializedFromCache { get; }

		IBundle GetBundle(string bundleId);

		IExtensionPointConfiguration GetExtensionPointConfigurationElement(string extensionPointId);

		IExtensionConfiguration GetExtensionConfigurationElement(string extensionId);

		bool TryGetBundle(string bundleId, out IBundle bundle);

		bool TryGetBundleForExtension(string extensionId, out IBundle bundle);

		bool TryGetBundleForExtensionPoint(string extensionPointId, out IBundle bundle);

		bool TryGetExtensionPointConfiguration(string extensionPointId, out IExtensionPointConfiguration pointConfiguration);
	}
}