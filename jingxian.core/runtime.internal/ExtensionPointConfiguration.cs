

using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using jingxian.core.runtime.Xml.Serialization;

namespace jingxian.core.runtime.simpl
{
	[Serializable]
	internal sealed class ExtensionPointConfiguration: XmlSerializableIdentifiable, IExtensionPointConfiguration
	{


        private string _bundleId = string.Empty;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _configuration = string.Empty;

		public ExtensionPointConfiguration()
		{
		}

		public ExtensionPointConfiguration(string bundleId)
		{
			_bundleId = bundleId;
		}

		public ExtensionPointConfiguration(string id, string bundleId, string name, string description)
		{
			Id = id;
			BundleId = bundleId;
			Name = name;
			Description = description;
		}

		public ExtensionPointConfiguration(ExtensionPointAttribute attribute)
		{
			Id = attribute.Id;
			BundleId = attribute.BundleId;
			Name = attribute.Name;
			Description = attribute.Description;
			Configuration = attribute.Configuration;
		}

		public string BundleId
		{
			get{	return _bundleId;}
			private set{_bundleId = value ?? string.Empty;}
		}


		public string Name
		{
			get{	return _name;}
			private set{	_name = value ?? string.Empty;}
		}

		public string Description
		{
			get{	return _description;}
			private set{	_description = value ?? string.Empty;}
		}


		public string Configuration
		{
			get{	return _configuration;}
			private set{_configuration = value ?? string.Empty;}
		}


		#region XML serialization related

		public const string XmlElementName = "extensionPoint"; 
		public const string XmlTypeName = "ExtensionPoint"; 


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
			return Bundle.GetSchema();
		}

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
			Name = reader.GetAttribute("name");
			Description = reader.GetAttribute("description");
		}

		protected override void ReadXmlElements(XmlReader reader)
		{
			base.ReadXmlElements(reader);
			while (reader.IsStartElement())
			{
				Configuration = reader.ReadOuterXml();
			}
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("description", Description);
		}

		protected override void WriteXmlElements(XmlWriter writer)
		{
			base.WriteXmlElements(writer);
			writer.WriteRaw(Configuration);
		}

		#endregion


		public void Merge(ExtensionPointAttribute attr)
		{
			Trace.Assert(Id == attr.Id);

			if (string.IsNullOrEmpty(Name))
			{
				Name = attr.Name;
			}
			if (string.IsNullOrEmpty(Description))
			{
				Description = attr.Description;
			}
			if (string.IsNullOrEmpty(Configuration))
			{
				Configuration = attr.Configuration;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			ExtensionPointConfiguration other = (ExtensionPointConfiguration) obj;
			if (
				Id != other.Id
				|| BundleId != other.BundleId
				|| Name != other.Name
				|| Description != other.Description
				|| Configuration != other.Configuration
				)
			{
				return false;
			}
			return true;
		}


		public override int GetHashCode()
		{
			return string.IsNullOrEmpty(Id) ? base.GetHashCode() : Id.GetHashCode();
		}

		public override string ToString()
		{
			return Id;
		}

	}
}