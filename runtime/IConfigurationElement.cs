

using System.Xml.Serialization;

namespace jingxian.core.runtime
{
	[ExtensionContract]
	public interface IConfigurationElement: IRuntimePart, IXmlSerializable
	{
        IExtension DeclaringExtension { get; }

		void Configure(IExtension declaringExtension);
	}
}