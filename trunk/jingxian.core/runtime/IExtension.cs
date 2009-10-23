
using System.Xml.Serialization;

namespace jingxian.core.runtime
{

	/// <summary>
	/// 扩展接口
	/// </summary>
	public interface IExtension: IRuntimePart
	{
        /// <summary>
        /// 扩展名
        /// </summary>
		string Name { get; }

        /// <summary>
        /// 扩展的描述
        /// </summary>
		string Description { get; }

        /// <summary>
        /// 实现扩展的类名
        /// </summary>
		string Implementation { get; }

        /// <summary>
        /// 扩展点名
        /// </summary>
		string Point { get; }

        /// <summary>
        /// 扩展的配置
        /// </summary>
		string Configuration { get; }

        /// <summary>
        /// 扩展所在的包名
        /// </summary>
		string BundleId { get; }

        /// <summary>
        /// 创建扩展对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
		T Build<T>();

        /// <summary>
        /// 是不是有配置
        /// </summary>
		bool HasConfiguration { get; }

        /// <summary>
        /// 是不是有实现
        /// </summary>
        bool HasImplementation { get; }
	}
}