

using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace jingxian.core.runtime.Filters
{
	[Serializable]
	[XmlRoot(
		AssemblyFileSet.XmlElementName,
		DataType = AssemblyFileSet.XmlTypeName,
	    Namespace = RuntimeConstants.CurrentXmlSchemaNamespace,
		IsNullable = false)]
	public sealed class AssemblyFileSet: IncludeExcludeSet
	{

		public const string XmlElementName = "assemblyFileSet";
		public const string XmlTypeName = "AssemblyFileSet";

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