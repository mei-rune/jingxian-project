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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using Empinia.Core.Runtime;
using Empinia.Core.Utilities;

namespace jingxian.ui.Serialization
{
	internal class ToolbarAreaConfiguration: ITypedXmlSerializable
	{
        private string m_Name;
        private ToolbarRowConfiguration[] m_RowConfigurations;

		internal ToolbarAreaConfiguration(XmlReader reader)
		{
			ReadXml(reader);
		}

        internal ToolbarAreaConfiguration(ToolStripPanel area)
		{
			m_Name = area.Name;
			m_RowConfigurations = new ToolbarRowConfiguration[area.Rows.Length];
			for (int rowIndex = 0; rowIndex < area.Rows.Length; rowIndex++)
			{
				m_RowConfigurations[rowIndex] = new ToolbarRowConfiguration(area.Rows[rowIndex], rowIndex);
			}
		}


		internal string Name
		{
			get{return m_Name;}
		}

		internal ToolbarRowConfiguration[] RowConfigurations
		{
			get { return m_RowConfigurations; }
		}

		#region ITypedXmlSerializable Member

		public const string XmlTypeName = "DockToolbarAreaConfiguration"; //NON-NLS-1
		public const string XmlElementName = "area"; //NON-NLS-1
		private const string XmlAttributeName = "name"; //NON-NLS-1

		#region XmlSchemaNamespace
		string ITypedXmlSerializable.XmlSchemaNamespace
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion
      
		#region XmlTypeName
		string ITypedXmlSerializable.XmlTypeName
		{
			get
			{
				return XmlTypeName;
			}
		}
		#endregion

		#region XmlElementName
		string ITypedXmlSerializable.XmlElementName
		{
			get
			{
				return XmlElementName;
			}
		}
		#endregion

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
			   reader.ReadStartElement();
			}
			else
			{m_Name = XmlUtils.ReadRequiredAttributeString(reader, XmlAttributeName);
			   reader.ReadStartElement();

				ICollection<ToolbarRowConfiguration> rows = new List<ToolbarRowConfiguration>();
            while(reader.IsStartElement(ToolbarRowConfiguration.XmlElementName))
				{

					rows.Add(new ToolbarRowConfiguration(reader));
				}
				m_RowConfigurations = new ToolbarRowConfiguration[rows.Count];
				rows.CopyTo(m_RowConfigurations, 0);


				
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(XmlElementName);

			writer.WriteAttributeString(XmlAttributeName, m_Name);

			if (m_RowConfigurations.Length> 0)
			{
				for (int row = 0; row < m_RowConfigurations.Length; row++)
				{
					m_RowConfigurations[row].WriteXml(writer);
				}
			}
			writer.WriteEndElement();
		}

		#endregion
	}
}