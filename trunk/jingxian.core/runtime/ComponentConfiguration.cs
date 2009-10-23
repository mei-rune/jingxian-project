
using System;
using System.Xml;
using System.Xml.Serialization;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.utilities;

	[Serializable]
	[XmlRoot(ComponentConfiguration.XmlElementName,
		DataType=ComponentConfiguration.XmlTypeName,
	 IsNullable = false, Namespace = RuntimeConstants.CurrentXmlSchemaNamespace)]
	public sealed class ComponentConfiguration : ConfigurationElement
	{		
        public const string XmlTypeName = "ComponentConfiguration";
		public const string XmlElementName = "component";
		private string _implementation;
		private string _interface;

		public ComponentConfiguration()
			: base()
		{
		}

		public ComponentConfiguration(string id)
			: base(id)
		{
		}

		public string Interface
		{
			get { return _interface; }
			set { _interface = value; }
		}

		public string Implementation
		{
			get { return _implementation; }
			set { _implementation = value; }
        }

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
            Interface = XmlUtils.ReadRequiredAttributeString(reader, "interface");

			Implementation = XmlUtils.ReadRequiredAttributeString(reader, RuntimeConstants.XmlImplementationAttributeName);
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
            XmlUtils.WriteRequiredAttributeString(writer, "interface", Interface);
			XmlUtils.WriteRequiredAttributeString(writer, RuntimeConstants.XmlImplementationAttributeName, Implementation);
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

		protected override System.Xml.Schema.XmlSchema GetXmlSchema()
		{
			throw new NotImplementedException();
		}
	}
}