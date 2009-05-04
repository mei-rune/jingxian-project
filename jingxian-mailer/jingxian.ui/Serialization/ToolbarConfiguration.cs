#region Copyright and License
/* Licensed to the Empinia Project under one or more contributor
 * license agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * The Empinia Project licenses this file to You under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in 
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.empinia.org/licenses/ApacheSoftwareLicense-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and 
 * limitations under the License.
 *
 * The Empinia Project itself is located at http://www.empinia.org.
 * 
 */

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using Empinia.Core.Runtime;
using Empinia.Core.Utilities;
using System.Xml;
using System.Windows.Forms;

namespace jingxian.ui.Serialization
{
	internal class ToolbarConfiguration: ITypedXmlSerializable
	{
		private string m_ToolbarTypeId;
		private int m_Offset;
        public ToolbarConfiguration(ToolStripEx toolstrip, int offset)
		{
			m_ToolbarTypeId = toolstrip.ToolbarPart.TypeId;
            m_Offset = offset;
		}

		public ToolbarConfiguration(XmlReader reader)
		{
			ReadXml(reader);
		}

		public string ToolbarTypeId
		{
			get
			{
				return m_ToolbarTypeId;
			}
		}

		public int Offset
		{
			get
			{
				return m_Offset;
			}
		}

		#region ITypedXmlSerializable Member

		public const string XmlTypeName = "DockToolBarConfiguration"; //NON-NLS-1
		public const string XmlElementName = "toolbar"; //NON-NLS-1

		private const string XmlAttributeToolbarTypeId = "toolbarTypeId"; //NON-NLS-1
		private const string XmlAttributeOffset = "offset"; //NON-NLS-1

		public string XmlSchemaNamespace
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
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
			throw new Exception("The method or operation is not implemented.");
		}
        
        public void ReadXml(XmlReader reader)
		{
			//reader.MoveToContent();
			if (reader.IsEmptyElement)
			{
				ReadElement(reader);
				reader.ReadStartElement(XmlElementName);
			}
			else
			{
				ReadElement(reader);
				reader.ReadStartElement(XmlElementName);
				reader.ReadEndElement();
			}
		}

		private void ReadElement(XmlReader reader)
		{
			m_ToolbarTypeId = XmlUtils.ReadRequiredAttributeString(reader, XmlAttributeToolbarTypeId);
			string offset;
			if (!XmlUtils.TryReadAttributeString(reader, XmlAttributeOffset, out offset))
			{// attribute not set
				m_Offset = 0;
				return;
			}
			//TODO: This might be obsolete if value can be forced to be an integer via XSD 
			if(!int.TryParse(offset,out m_Offset))
			{// attribute value is no int
				m_Offset = 0;
			}
		}

        public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(XmlElementName);
			XmlUtils.WriteRequiredAttributeString(writer, XmlAttributeToolbarTypeId, m_ToolbarTypeId);
			if (m_Offset > 0)
			{
				XmlUtils.WriteRequiredAttributeString(writer, XmlAttributeOffset, m_Offset.ToString());
			}
			writer.WriteEndElement();
		}

		#endregion
	}
}
