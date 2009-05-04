

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;

namespace jingxian.core.utilities
{
	public static class XmlUtils
	{
		#region Serialize

		public static void Deserialize(string xml, IXmlSerializable serializable)
		{
			Deserialize(xml, serializable, CreateFragmentReaderSettings());
		}

		public static void Deserialize(string xml, IXmlSerializable serializable, XmlReaderSettings settings)
		{
			TextReader textReader = new StringReader(xml);
			using (XmlReader reader = XmlReader.Create(textReader, settings, CreateParserContext()))
			{
				reader.MoveToContent();
				serializable.ReadXml(reader);
			}
		}

        public static string Serialize(string localName, IXmlSerializable serializable)
        {
            return Serialize(localName, serializable, CreateFragmentWriterSettings());
        }

        public static string Serialize(string localName, IXmlSerializable serializable, XmlWriterSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                WriteElement(writer, localName, serializable);
            }
            return sb.ToString();
        }


		#endregion

		#region GetSchema

		public static XmlSchema GetSchema(Stream stream)
        {
            Enforce.ArgumentNotNull<Stream>(stream, "stream");
		
			XmlSchema schema;
			using (Stream schemaStream = stream)
			{
				schemaStream.Position = 0;
				XmlSchema tempSchema = XmlSchema.Read(schemaStream, ReportCompileError);
				schema = tempSchema;
			}
			return schema;
		}

		public static XmlSchema GetSchema(Assembly asm, string manifestResourceName)
		{
			return XmlSchema.Read(asm.GetManifestResourceStream(manifestResourceName), ReportCompileError);
		}

		public static void ReportCompileError(object sender, ValidationEventArgs args)
		{
			throw new XmlException(Error.ErrorLoadingSchema + args.Message, args.Exception); /// @todo improve
		}

		public static XmlParserContext CreateParserContext()
		{
			return new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None);
		}

		#endregion


		#region Create Factory methods

		public static T CreateFromXml<T>(XmlReader reader) where T : IXmlSerializable, new()
		{
			T instance = new T();
			instance.ReadXml(reader);
			return instance;
		}

		public static T CreateFromXml<T>(string xml) where T : IXmlSerializable, new()
		{
			TextReader textReader = new StringReader(xml);
			using (XmlReader reader = XmlReader.Create(textReader, CreateFragmentReaderSettings()))
			{
				return CreateFromXml<T>(reader);
			}
		}

		public static XmlWriterSettings CreateFragmentWriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CheckCharacters = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.OmitXmlDeclaration = true;
			return settings;
		}

		public static XmlWriterSettings CreateDocumentWriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CheckCharacters = true;
			settings.ConformanceLevel = ConformanceLevel.Document;
			settings.Indent = true;
			settings.NewLineOnAttributes = true;
			settings.NewLineHandling = NewLineHandling.Entitize;
			return settings;
		}

		public static XmlReaderSettings CreateFragmentReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.Schemas = new XmlSchemaSet();
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			return settings;
		}

		public static XmlReaderSettings CreateFragmentReaderSettings(XmlSchema[] schemas)
		{
			XmlReaderSettings settings = CreateFragmentReaderSettings();
			for (int i = 0; i < schemas.Length; i++)
			{
				settings.Schemas.Add(schemas[i]);
			}
			settings.Schemas.Compile();
			settings.ValidationType = ValidationType.Schema;
			return settings;
		}

		public static XmlReaderSettings CreateDocumentReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CheckCharacters = true;
			settings.ConformanceLevel = ConformanceLevel.Document;
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.IgnoreWhitespace = true;
			settings.ValidationType = ValidationType.None;
			return settings;
		}

		#endregion

		#region Read Write LocalizableStringElement

		public static string ReadLocalizableStringElement(string elementName, string xmlNamespace, XmlReader reader)
		{
			return ReadLocalizableStringElement(elementName, xmlNamespace, reader, false);
		}

		public static string ReadLocalizableStringElement(string elementName, string xmlNamespace, XmlReader reader,
		                                                  bool isOptional)
		{
			return ReadLocalizableStringElement(elementName, xmlNamespace, reader, isOptional, string.Empty);
		}

		public static string ReadLocalizableStringElement(string elementName, string xmlNamespace, XmlReader reader,
		                                                  bool isOptional, string defaultValue)
		{
			if (reader.IsStartElement(elementName, xmlNamespace))
			{
				reader.ReadStartElement(elementName, xmlNamespace);
				string result = reader.ReadString();
				reader.ReadEndElement();
				return result;
			}
			else if (isOptional)
			{
				return defaultValue;
			}
			else
			{
				throw new XmlException(
                    string.Format(CultureInfo.InvariantCulture, Error.FailedToReadNonOptionalElement, elementName, xmlNamespace));
			}
		}

		public static void WriteLocalizableStringElement(string elementName, string xmlNamespace, XmlWriter writer,
		                                                 string value)
		{
			writer.WriteStartElement(elementName, xmlNamespace);
			writer.WriteString(value);
			writer.WriteEndElement();
		}

		#endregion

		#region Read Attribute Helper

		public static string ReadOptionalAttributeString(XmlReader reader, string localName)
		{
			string value;
			if (TryReadAttributeString(reader, localName, out value))
			{
				return value;
			}
			return string.Empty;
		}

		public static bool TryReadAttributeString(XmlReader reader, string localName, out string value)
		{
			string attrValue = reader.GetAttribute(localName);
			value = attrValue ?? string.Empty;
			return !string.IsNullOrEmpty(value);
		}

        public static string ReadRequiredAttributeString(XmlReader reader, string localName)
        {
            string value;
            if (TryReadAttributeString(reader, localName, out value))
                return value;

            throw new XmlException(string.Format(CultureInfo.InvariantCulture, Error.RequiredAttributeIsMissingOrEmpty, localName));
        }

		public static string ReadAttribute(XmlReader reader, string localName)
		{
			string value = reader.GetAttribute(localName);
			return value ?? string.Empty;
		}

		public static bool ReadAttributeAsBoolean(XmlReader reader, string localName)
		{
			return Convert.ToBoolean(reader.GetAttribute(localName));
		}

		public static int ReadAttributeAsInt32(XmlReader reader, string localName)
		{
			return Convert.ToInt32(reader.GetAttribute(localName));
		}

		public static double ReadAttributeAsDouble(XmlReader reader, string attributeName)
		{
			double defaultValue = double.NaN;
			return ReadAttributeAsDouble(reader, attributeName, ref defaultValue);
		}

		public static double ReadAttributeAsDouble(XmlReader reader,
																	string attributeName,
																	ref double defaultValue)
		{
			return ReadAttributeAsDouble(reader, attributeName, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, ref defaultValue);
		}

		public static double ReadAttributeAsDouble(XmlReader reader,
																  string attributeName,
																  NumberStyles numberStyles,
																  CultureInfo cultureInfo,
																  ref double defaultValue)
		{
			string text;
			double value = defaultValue;

			if (TryReadAttributeString(reader, attributeName, out text))
			{
				if (double.TryParse(text, numberStyles, cultureInfo, out value))
				{
					return value;
				}
			}
			return value;
		}

		public static float ReadAttributeAsFloat(XmlReader reader, string localName)
		{
			return Convert.ToSingle(reader.GetAttribute(localName));
		}

		public static DateTime ReadAttributeAsDateTime(XmlReader reader, string localName)
		{
			return Convert.ToDateTime(reader.GetAttribute(localName));
		}

		#endregion

		#region Write Attribute Helper


		public static void WriteRequiredAttributeString(XmlWriter writer, string localName, string value)
		{
			if (!TryWriteAttributeString(writer, localName, value))
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Error.AttribteIsRequired, localName));
		}

		public static bool TryWriteAttributeString(XmlWriter writer, string localName, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				writer.WriteAttributeString(localName, value);
				return true;
			}
			return false;
		}

		public static void WriteAttribute(XmlWriter writer, string localName, bool value)
		{
			writer.WriteStartAttribute(localName);
			writer.WriteValue(value);
			writer.WriteEndAttribute();
		}

		public static void WriteAttribute(XmlWriter writer, string localName, int value)
		{
			writer.WriteStartAttribute(localName);
			writer.WriteValue(value);
			writer.WriteEndAttribute();
		}

		public static void WriteAttribute(XmlWriter writer, string localName, double value)
		{
			writer.WriteStartAttribute(localName);
			writer.WriteValue(value);
			writer.WriteEndAttribute();
		}

		public static void WriteAttribute(XmlWriter writer, string localName, float value)
		{
			writer.WriteStartAttribute(localName);
			writer.WriteValue(value);
			writer.WriteEndAttribute();
		}

		public static void WriteAttribute(XmlWriter writer, string localName, DateTime value)
		{
			writer.WriteStartAttribute(localName);
			writer.WriteValue(value);
			writer.WriteEndAttribute();
		}

		public static void WriteAttribute(XmlWriter writer, string localName, object value)
		{
			if (value != null)
			{
				writer.WriteStartAttribute(localName);
				writer.WriteValue(value);
				writer.WriteEndAttribute();
			}
		}

		#endregion

        #region Read or Write Element List Helper

        public static void ReadElementsIntoList<T>(XmlReader reader, string localListName, string localName,
		                                           ICollection<T> list) where T : IXmlSerializable, new()
		{
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement(localListName);
				while (reader.IsStartElement(localName))
				{
					T instance = new T();
					instance.ReadXml(reader);
					list.Add(instance);
				}
				reader.ReadEndElement();
			}
			else
			{
				reader.ReadStartElement(localListName);
			}
		}

		public static void WriteElementsFromList<T>(XmlWriter writer, string localListName, string localName,
		                                            IEnumerable<T> list) where T : IXmlSerializable
		{
			writer.WriteStartElement(localListName);
			foreach (T instance in list)
			{
				WriteElement(writer, localName, instance);
			}
			writer.WriteEndElement();
		}

		#endregion

		#region Read or Write Element Helper

		public static T ReadElement<T>(XmlReader reader, string localName) where T : IXmlSerializable, new()
		{
			if (reader.IsStartElement(localName))
			{
				T instance = new T();
				instance.ReadXml(reader);
				return instance;
			}
			else
			{
				return default(T);
			}
		}

		public static void ReadElement(XmlReader reader, string localName, IXmlSerializable instance)
		{
			if (reader.IsStartElement(localName))
			{
				instance.ReadXml(reader);
			}
		}

		public static void WriteElement<T>(XmlWriter writer, string localName, T instance) where T : IXmlSerializable
		{
			writer.WriteStartElement(localName);
			instance.WriteXml(writer);
			writer.WriteEndElement();
		}

		public static void WriteElement(XmlWriter writer, string localName, IXmlSerializable instance)
		{
			writer.WriteStartElement(localName);
			instance.WriteXml(writer);
			writer.WriteEndElement();
		}

		#endregion
	}
}