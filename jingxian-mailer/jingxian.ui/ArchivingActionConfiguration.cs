using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Utilities;
    using Empinia.UI.Dialogs;
    using Empinia.UI.Navigator;

    public class ArchivingActionConfiguration : ConfigurationElement
	{
		public ArchivingActionConfiguration()
			: base()
		{
		}

		public ArchivingActionConfiguration(string id)
			: base(id)
		{
		}

		private string m_Name;
		private string m_Description;
		private string m_CommandId;

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public string Description
		{
			get { return m_Description; }
			set { m_Description = value; }
		}

		public string CommandId
		{
			get { return m_CommandId; }
			set { m_CommandId = value; }
		}

		public const string XmlElementName = "archivingAction"; //NON-NLS-1
		public const string XmlTypeName = "ArchivingActionConfiguration"; //NON-NLS-1
		public const string XmlSchemaNamespace = RuntimeConstants.CurrentXmlSchemaNamespace;

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
			Name = reader.GetAttribute("name");
			Description = reader.GetAttribute("description");
			CommandId = reader.GetAttribute("commandId");
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
			XmlUtils.WriteAttribute(writer, "name", Name);
			XmlUtils.WriteAttribute(writer, "description", Description);
			XmlUtils.WriteAttribute(writer, "commandId", CommandId);
		}


		protected override string GetXmlSchemaNamespace()
		{
			return XmlSchemaNamespace;
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

		public override string ToString()
		{
			return Name;
		}
	}
}