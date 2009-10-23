
using System;
using System.ComponentModel;

namespace jingxian.core.runtime
{
    /// <summary>
    /// 包接口
    /// </summary>
	public interface IBundle: IRuntimePart
	{
        /// <summary>
        /// 包名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 程序集路径
        /// </summary>
		string AssemblyLocation { get; }

        /// <summary>
        /// 包描述
        /// </summary>
		string Description { get; }

        /// <summary>
        /// 包的版本号
        /// </summary>
		Version Version { get; }

        /// <summary>
        /// 提供者描述
        /// </summary>
		string Provider { get; }

        /// <summary>
        /// 取得所有的扩展配置对象
        /// </summary>
		IExtensionConfiguration[] ContributedExtensions { get; }

        /// <summary>
        /// 取得所有的扩展点配置对象
        /// </summary>
        IExtensionPointConfiguration[] ContributedExtensionPoints { get; }

        /// <summary>
        /// 包状态
        /// </summary>
        BundleState State { get; }
	}
}