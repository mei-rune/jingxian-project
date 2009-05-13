

using System;

namespace jingxian.core.runtime.Filters
{
    using jingxian.core.runtime.utilities;

	public sealed class FilterConfiguration : ITypedXmlSerializable, IFilterProvider, ICloneable
	{
		#region constants

		public const string XmlSchemaResourceName = "jingxian.core.runtime.Schemas.MessageFilterContribution.xsd"; 
		public const string XmlConfigurationElementName = "messageFilter";
		public const string XmlConfigurationTypeName = "MessageFilter";
		public const string XmlImplementationPropertyName = "implementation";
		public const string XmlNamePropertyName = "name";
		public const string XmlDescriptionPropertyName = "description";
		public const string XmlInterfacePropertyName = "interface"; 

		#endregion


        private string _name;
        private string _description;
        private string _interface;
        private string _implementation;

		public FilterConfiguration()
		{
		}

		public FilterConfiguration(string name, string description, string iface, string implementation)
		{
			Name = name;
			Description = description;
			Interface = iface;
			Implementation = implementation;
		}

		#region Serializable

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return XmlUtils.GetSchema(typeof (FilterConfiguration).Assembly, XmlSchemaResourceName);
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			reader.MoveToContent();
			Implementation = reader.GetAttribute(XmlImplementationPropertyName);
			Name = reader.GetAttribute(XmlNamePropertyName);
			Interface = reader.GetAttribute(XmlInterfacePropertyName);
			Description = reader.GetAttribute(XmlDescriptionPropertyName);
		}

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteAttributeString(XmlNamePropertyName, Name);
			writer.WriteAttributeString(XmlDescriptionPropertyName, Description);
			writer.WriteAttributeString(XmlInterfacePropertyName, Interface);
			writer.WriteAttributeString(XmlImplementationPropertyName, Implementation);
		}


		public string XmlSchemaNamespace
		{
            get { return RuntimeConstants.CurrentXmlSchemaNamespace; }
		}


		public string XmlTypeName
		{
			get { return XmlConfigurationTypeName; }
		}


		public string XmlElementName
		{
			get { return XmlConfigurationElementName; }
		}

		#endregion

		#region IFilterProvider Member

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
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

		#endregion


		#region ICloneable Member

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion
	}
}