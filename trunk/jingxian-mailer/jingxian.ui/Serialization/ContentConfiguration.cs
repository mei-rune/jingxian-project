using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Windows.Forms;

namespace jingxian.ui.Serialization
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Utilities;

    internal class ContentConfiguration : ITypedXmlSerializable
    {
        internal ContentConfiguration(XmlReader reader)
        {
            ReadXml(reader);
        }

        internal ContentConfiguration(ContentWidget content)
        {
        }

        #region ITypedXmlSerializable Member

        public const string XmlTypeName = "ContentConfiguration"; //NON-NLS-1
        public const string XmlElementName = "content"; //NON-NLS-1

        string ITypedXmlSerializable.XmlSchemaNamespace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string ITypedXmlSerializable.XmlTypeName
        {
            get
            {
                return XmlTypeName;
            }
        }

        string ITypedXmlSerializable.XmlElementName
        {
            get
            {
                return XmlElementName;
            }
        }

        #endregion

        #region IXmlSerializable Member

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement(XmlElementName);
            }
            else
            {
                reader.ReadStartElement(XmlElementName);
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlElementName);

            writer.WriteEndElement();
        }

        #endregion

        public override string ToString()
        {
            return "ContentConfiguration";
        }
    }
}