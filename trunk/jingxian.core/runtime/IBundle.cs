
using System;
using System.ComponentModel;

namespace jingxian.core.runtime
{
	public interface IBundle: IRuntimePart
	{
        string Name { get; }

		string AssemblyLocation { get; }

		string Description { get; }

		Version Version { get; }

		string Provider { get; }

		IExtensionConfiguration[] ContributedExtensions { get; }

        IExtensionPointConfiguration[] ContributedExtensionPoints { get; }

        BundleState State { get; }

	}
}