
using System;

namespace jingxian.core.runtime
{

    /// <summary>
    /// 扩展对象的配置
    /// </summary>
	public interface IExtensionConfiguration
	{
        string Id { get; }

        string BundleId { get; }

        string Name { get; }

        string Description { get; }

        string Point { get; }

        string Implementation { get; }

        string Configuration { get; }

		void Merge(ExtensionAttribute attribute);
	}
}