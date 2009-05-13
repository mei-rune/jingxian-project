

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using jingxian.core.runtime.utilities;
using System.Globalization;

namespace jingxian.core.runtime.Xml.Serialization
{
	[Serializable]
	public abstract class XmlSerializable: ITypedXmlSerializable
	{

		#region static methods

		public static void WriteElement<T>(XmlWriter writer, T instance) where T: ITypedXmlSerializable
		{
			XmlUtils.WriteElement(writer, instance.XmlElementName, instance);
		}

		public static void WriteElement(XmlWriter writer, ITypedXmlSerializable instance)
		{
			XmlUtils.WriteElement(writer, instance.XmlElementName, instance);
		}


		public static string WriteXmlFragment(ITypedXmlSerializable serializable)
		{
			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.CreateFragmentWriterSettings()))
				{
					xmlWriter.WriteStartElement(serializable.XmlElementName, serializable.XmlSchemaNamespace);
					serializable.WriteXml(xmlWriter);
					xmlWriter.WriteEndElement();
				}
			}
			return sb.ToString();
		}


		public static T ReadElement<T>(XmlReader reader)
            where T: ITypedXmlSerializable, new()
		{
			T instance = new T();
			if (reader.IsStartElement(instance.XmlElementName))
			{
				instance.ReadXml(reader);
			}
			else
			{
				instance = default(T);
			}
			return instance;
		}

		public static void ReadElement(XmlReader reader, ITypedXmlSerializable instance)
		{
			XmlUtils.ReadElement(reader, instance.XmlElementName, instance);
		}

		#endregion


		private bool _Deserializing;
		protected bool Deserializing
		{
			get
			{
				return _Deserializing;
			}
			private set
			{
				_Deserializing = value;
			}
		}

		protected virtual void ReadXmlcore(XmlReader reader)
		{
			Deserializing = true;
			try
			{
				string xmlElementName = GetXmlElementName();
				string xmlTypeName = GetXmlTypeName();
				if (reader.IsStartElement(xmlElementName)
					|| reader.IsStartElement(xmlTypeName))
				{
					string localXmlElementName = reader.LocalName;

					ReadXmlAttributes(reader);

					if (reader.IsEmptyElement)
					{
						reader.ReadStartElement(localXmlElementName);
					}
					else
					{
						reader.ReadStartElement(localXmlElementName);
						ReadXmlElements(reader);
						if (reader.NodeType == XmlNodeType.EndElement)
						{
							reader.ReadEndElement();
						}
					}
				}
				else
				{
					string msg =
						string.Format(CultureInfo.InvariantCulture,
													"读XML错误: 找到一个不是期望的节点 '{0}'. 期望是是: '{1}' 或 '{2}'.", reader.LocalName,
													xmlElementName, xmlTypeName);
					throw new InvalidOperationException(msg);

				}
			}
			finally
			{
				Deserializing = false;
			}
		}

		protected virtual void ReadXmlAttributes(XmlReader reader)
		{
		}

		protected virtual void ReadXmlElements(XmlReader reader)
		{
		}



		protected virtual void WriteXmlcore(XmlWriter writer)
		{
			WriteXmlAttributes(writer);
			WriteXmlElements(writer);
		}

		protected virtual void WriteXmlAttributes(XmlWriter writer)
		{
		}


		protected virtual void WriteXmlElements(XmlWriter writer)
		{
		}


		protected abstract string GetXmlSchemaNamespace();

		protected abstract string GetXmlTypeName();

		protected abstract string GetXmlElementName();

		protected abstract XmlSchema GetXmlSchema();

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			return GetXmlSchema();
		}

		public void ReadXml(XmlReader reader)
		{
			ReadXmlcore(reader);
		}

		public void WriteXml(XmlWriter writer)
		{
			WriteXmlcore(writer);
		}

		#endregion

		#region ITypedXmlSerializable Members

		string ITypedXmlSerializable.XmlSchemaNamespace
		{
            get { return GetXmlSchemaNamespace(); }
		}

		string ITypedXmlSerializable.XmlTypeName
		{
            get { return GetXmlTypeName(); }
		}

		string ITypedXmlSerializable.XmlElementName
		{
            get { return GetXmlElementName(); }
		}

		#endregion

		#region _logger
        private logging.ILog _Log;

		protected logging.ILog _logger
		{
			get
			{
				if (_Log == null)
				{
                    _Log = logging.LogUtils.GetLogger(GetType());
				}
				return _Log;
			}
		}
		#endregion

	}
}