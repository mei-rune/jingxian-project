

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace jingxian.core.runtime.simpl
{
using jingxian.core.runtime.Xml.Serialization;
using jingxian.core.runtime.utilities;

	[Serializable]
	internal sealed class ExtensionConfiguration: XmlSerializableIdentifiable, IExtensionConfiguration
	{
		public const string XmlElementName = "extension"; 
		public const string XmlTypeName = "Extension";

		private string _configuration = string.Empty;
		private string _point = string.Empty;
		private string _bundleId = string.Empty;
		private string _name = string.Empty;
		private string _description = string.Empty;
		private string _implementation = string.Empty;


		internal static ExtensionConfiguration FromXml(XmlReader reader)
		{
			ExtensionConfiguration cfg = new ExtensionConfiguration();
			cfg.ReadXml(reader);
			return cfg;
		}

		internal static ExtensionConfiguration FromXml(string bundleId, XmlReader reader)
		{
			ExtensionConfiguration cfg = new ExtensionConfiguration(bundleId);
			cfg.ReadXml(reader);
			return cfg;
		}

		internal static ExtensionConfiguration FromXml(string bundleId, string xml)
		{
			ExtensionConfiguration cfg = new ExtensionConfiguration(bundleId);
			TextReader textReader = new StringReader(xml);
			using (XmlReader reader = XmlReader.Create(textReader, XmlUtils.CreateFragmentReaderSettings()))
			{
				cfg.ReadXml(reader);
			}
			return cfg;
		}

		public ExtensionConfiguration()
		{
		}

		private ExtensionConfiguration(string bundleId)
		{
			_bundleId = bundleId;
		}

		internal ExtensionConfiguration(ExtensionAttribute attr)
			: this(
			attr.Id, attr.BundleId, attr.Name, attr.Description, attr.Point,
			attr.Implementation == null ? string.Empty : Utils.GetImplementationName(attr.Implementation),
			attr.Configuration)
		{
		}

		internal ExtensionConfiguration(string id, string bundleId, string name, string description, string point,
																		string fullQualifiedImplementation, string configuration)
		{
			Id = id;
			BundleId = bundleId;
			Name = name;
			Description = description;
			Point = point;
			Implementation = fullQualifiedImplementation;
			Configuration = configuration;
		}

		public string BundleId
		{
			get{	return _bundleId;}
			private set{	_bundleId = value ?? string.Empty;}
		}

		public string Name
		{
			get{	return _name;}
			private set{_name = value ?? string.Empty;}
		}

		public string Description
		{
			get{	return _description;}
			private set{	_description = value ?? string.Empty;}
		}

		public string Implementation
		{
			get{	return _implementation;}
			private set{_implementation = value ?? string.Empty;}
		}
        
		public string Configuration
		{
			get{	return _configuration;}
			private set {	_configuration = value ?? string.Empty;}
		}


		public string Point
		{
			get{	return _point;}
			private set{_point = value ?? string.Empty;}
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
			return Bundle.GetSchema();
		}

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
			Name = reader.GetAttribute("name");
			Description = reader.GetAttribute("description");
			Implementation = reader.GetAttribute("implementation");
			Point = reader.GetAttribute("point");
			if (string.IsNullOrEmpty(_bundleId))
			{
				XmlUtils.TryReadAttributeString(reader, "bundleId", out _bundleId);
			}
		}

		protected override void ReadXmlElements(XmlReader reader)
		{
			base.ReadXmlElements(reader);
			while (reader.IsStartElement())
			{
				Configuration += reader.ReadOuterXml();
			}
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("description", Description);
			writer.WriteAttributeString("implementation", Implementation);
			writer.WriteAttributeString("point", Point);
			writer.WriteAttributeString("bundleId", BundleId);
		}

		protected override void WriteXmlElements(XmlWriter writer)
		{
			base.WriteXmlElements(writer);
			writer.WriteRaw(Configuration);
		}


		public void Merge(ExtensionAttribute attr)
		{
			System.Diagnostics.Trace.Assert(Id == attr.Id);

			if (string.IsNullOrEmpty(Name))
			{
				Name = attr.Name;
			}
			if (string.IsNullOrEmpty(Description))
			{
				Description = attr.Description;
			}
			if (string.IsNullOrEmpty(Implementation) && attr.Implementation != null)
			{
				Implementation = Utils.GetImplementationName(attr.Implementation);
			}
			if (string.IsNullOrEmpty(Point))
			{
				Point = attr.Point;
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
			ExtensionConfiguration other = (ExtensionConfiguration) obj;
			if (
				Id != other.Id
				|| BundleId != other.BundleId
				|| Name != other.Name
				|| Description != other.Description
				|| Implementation != other.Implementation
				|| Point != other.Point
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