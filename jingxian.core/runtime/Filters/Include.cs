

using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace jingxian.core.runtime.Filters
{
	[Serializable]
	[XmlRoot(
		Include.XmlElementName,
		DataType = Include.XmlTypeName,
	 Namespace = RuntimeConstants.CurrentXmlSchemaNamespace,
		IsNullable = false)]
	public sealed class Include: StringFilter
	{
		public const string XmlElementName = "include";
		public const string XmlTypeName = "Include";

		public Include()
		{
		}

		public Include(string name)
			: base(name, false)
		{
		}

		public Include(string name, bool isRegex)
			: base(name, isRegex)
		{
		}

		protected override string GetXmlSchemaNamespace()
		{
			return RuntimeConstants.CurrentXmlSchemaNamespace;
		}

		protected override string GetXmlTypeName()
		{
			return XmlTypeName;
		}

		protected override string GetXmlElementName()
		{
			return XmlElementName;
		}

		protected override XmlSchema GetXmlSchema()
		{
			throw new NotImplementedException();
		}
	}
}