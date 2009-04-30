

using System.Collections.Generic;

namespace jingxian.core.runtime
{
	public interface IExtensionRegistry
	{
		/// <summary>
		/// 取得扩展点的所有扩展
		/// </summary>
        /// <param name="pointId">扩展点的id</param>
        /// <returns>如果没有扩展点返回空数组</returns>
		IExtension[] GetExtensions(string pointId);


        /// <summary>
        /// 取得指定的扩展
        /// </summary>
        /// <param name="extensionId">扩展对象的id</param>
        /// <returns>没有时返回null</returns>
		IExtension GetExtension(string extensionId);

        /// <summary>
        /// 取得指定的扩展点对象
        /// </summary>
        /// <param name="extensionId">扩展点对象的id</param>
        /// <returns>没有时返回null</returns>
		IExtensionPoint GetExtensionPoint(string extensionPointId);


		/// <summary>
        /// 尝试取得指定id的扩展对象
		/// </summary>
        /// <param name="extensionId">扩展对象的 id.</param>
        /// <param name="extension">扩展对象的引用</param>
		/// <returns>取到时返回true,没有找到返回false</returns>
		bool TryGetExtension(string extensionId, out IExtension extension);


		/// <summary>
		/// 列出所有扩展点对象
		/// </summary>
        IEnumerable<IExtensionPoint> ExtensionPoints { get; }

        /// <summary>
        /// 取得指定id的扩展对象的配置
        /// </summary>
        /// <param name="id">扩展对象的id</param>
		IExtensionConfiguration GetExtensionConfigurationElement(string id);

        /// <summary>
        /// 取得指定id的扩展点对象的配置
        /// </summary>
        /// <param name="id">扩展点对象的id</param>
		IExtensionPointConfiguration GetExtensionPointConfigurationElement(string id);
	}
}