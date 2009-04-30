

using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using jingxian.core.utilities;

namespace jingxian.core.runtime.simpl
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	[Serializable]
	[XmlRoot(ServiceConfiguration.XmlElementName, DataType = ServiceConfiguration.XmlTypeName,
		IsNullable = false, Namespace = RuntimeConstants.CurrentXmlSchemaNamespace)]
	public sealed class ServiceConfiguration : ConfigurationElement
    {

        public const string XmlTypeName = "ServiceConfiguration";
        public const string XmlElementName = "service";
        public const string XmlInterfaceAttributeName = "interface";

		private string _ServiceInterface;
		private string _implementation;


		public ServiceConfiguration()
			: base()
		{
		}

		public ServiceConfiguration(string id)
			: base(id)
		{
		}

		public string ServiceInterface
		{
			get { return _ServiceInterface; }
			set { _ServiceInterface = value; }
		}

		public string Implementation
		{
			get { return _implementation; }
			set { _implementation = value; }
		}

		#region XML serialization related

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
			ServiceInterface = XmlUtils.ReadRequiredAttributeString(reader, XmlInterfaceAttributeName);
			Implementation = XmlUtils.ReadRequiredAttributeString(reader, RuntimeConstants.XmlImplementationAttributeName);
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
			XmlUtils.WriteRequiredAttributeString(writer, XmlInterfaceAttributeName, ServiceInterface);
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

		#endregion

		public override string ToString()
		{
			return Id;
		}
	}
}