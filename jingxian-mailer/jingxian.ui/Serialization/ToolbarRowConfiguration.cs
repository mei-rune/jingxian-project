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
	/// <summary>
	/// TODO: more doc!
	/// This is used to (de-)serialize a toolbar placement.
	/// </summary>
	internal class ToolbarRowConfiguration: ITypedXmlSerializable
	{
		private int m_Index;
		private ToolbarConfiguration[] m_ToolbarConfigurations;

		internal ToolbarRowConfiguration(XmlReader reader)
		{
			ReadXml(reader);
		}

        internal ToolbarRowConfiguration(ToolStripPanelRow row, int index)
		{
			m_Index = index;
			m_ToolbarConfigurations = new ToolbarConfiguration[row.Controls.Length];
			for (int position = 0; position < (row.Controls.Length); position++)
			{
				ToolStripEx toolStripEx = row.Controls[position] as ToolStripEx;
                if (toolStripEx == null)
				{
					throw new InvalidCastException("Controls in this ToolStripPanelRow must be DockToolStrips!");  //Why?
				}

				int offset;

				if(row.Orientation == Orientation.Horizontal)
				{
                    offset = toolStripEx.Left;
				}
				else //(row.Orientation == Orientation.Vertical)
				{
                    offset = toolStripEx.Top;
				}

                m_ToolbarConfigurations[position] = new ToolbarConfiguration(toolStripEx, offset);
			}
		}

		internal int Index
		{
			get{return m_Index;}
		}

		internal ToolbarConfiguration[] ToolbarConfigurations
		{
			get{	return m_ToolbarConfigurations;}
		}

		#region ITypedXmlSerializable Member

		public const string XmlTypeName = "DockToolbarRowConfiguration"; //NON-NLS-1
		public const string XmlElementName = "row"; //NON-NLS-1

		private const string XmlAttributeName = "index"; //NON-NLS-1

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
				reader.ReadStartElement();
			}
			else
			{
				m_Index = XmlUtils.ReadAttributeAsInt32(reader, XmlAttributeName);
				reader.ReadStartElement();
            ICollection<ToolbarConfiguration> toolbars = new List<ToolbarConfiguration>();
				while (reader.IsStartElement(ToolbarConfiguration.XmlElementName))
				{

					toolbars.Add(new ToolbarConfiguration(reader));
				}
				m_ToolbarConfigurations = new ToolbarConfiguration[toolbars.Count];
				toolbars.CopyTo(m_ToolbarConfigurations, 0);
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(XmlElementName);

			writer.WriteAttributeString(XmlAttributeName, m_Index.ToString());

			if (m_ToolbarConfigurations.Length> 0)
			{
				for (int row = 0; row < m_ToolbarConfigurations.Length; row++)
				{
					ToolbarConfigurations[row].WriteXml(writer);
				}
			}
			writer.WriteEndElement();
		}

		#endregion
	}
}