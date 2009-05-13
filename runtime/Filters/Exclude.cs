

using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace jingxian.core.runtime.Filters
{
	[Serializable]
	[XmlRoot(
		Exclude.XmlElementName,
		DataType = Exclude.XmlTypeName,
	 Namespace = RuntimeConstants.CurrentXmlSchemaNamespace,
		IsNullable = false)]
	public sealed class Exclude: StringFilter
	{
		public const string XmlElementName = "exclude";
		public const string XmlTypeName = "Exclude";


		public Exclude()
		{
		}

		public Exclude(string name)
			: base(name, false)
		{
		}

		public Exclude(string name, bool isRegex)
			: base(name, isRegex)
		{
		}

		#region XML Serialization related


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

		#endregion
	}
}