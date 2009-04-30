
using System.Xml.Serialization;

namespace jingxian.core.runtime
{

	/// <summary>
	/// 扩展接口
	/// </summary>
	public interface IExtension: IRuntimePart
	{
		string Name { get; }

		string Description { get; }

		string Implementation { get; }

		string Point { get; }

		string Configuration { get; }

		string BundleId { get; }

		T[] BuildConfigurationsFromXml<T>() where T: IXmlSerializable, new();

		T BuildConfigurationFromXml<T>() where T: IXmlSerializable, new();

		T BuildTransient<T>();

		bool HasConfiguration { get; }

        bool HasImplementation { get; }

	}
}