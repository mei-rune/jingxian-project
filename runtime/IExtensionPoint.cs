

namespace jingxian.core.runtime
{

	/// <summary>
	/// 扩展点对象
	/// </summary>
	public interface IExtensionPoint: IRuntimePart
	{
        /// <summary>
        /// 扩展点名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 包标识符
        /// </summary>
		string BundleId{ get;}

        /// <summary>
        /// 扩展点描述
        /// </summary>
		string Description{ get; }

        /// <summary>
        /// 取得本扩展点的扩展
        /// </summary>
		IExtension[] Extensions { get; }

	}
}