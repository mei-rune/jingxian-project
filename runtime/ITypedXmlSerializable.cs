

using System.Xml.Serialization;

namespace jingxian.core.runtime
{
	[ExtensionContract]
	public interface ITypedXmlSerializable: IXmlSerializable
	{
        string XmlSchemaNamespace { get; }

        string XmlTypeName { get; }

        string XmlElementName { get; }
	}
}