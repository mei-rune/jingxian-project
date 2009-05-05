

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using jingxian.core.runtime.utilities;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	internal sealed class ExtensionBuilder: IExtensionBuilder
	{
        static logging.ILog _logger = logging.LogUtils.GetLogger(typeof(ExtensionBuilder));

		private readonly IAssemblyLoaderService _assemblyLoaderService;
		private readonly IBundleService _bundleService;
		private readonly IExtensionRegistry _registry;
		private readonly IObjectBuilder _objectBuilderService;
		private XmlReaderSettings _settings;

		public ExtensionBuilder(IAssemblyLoaderService assemblyLoader
            , IBundleService bundleService
            , IExtensionRegistry registry
            , IObjectBuilder objectBuilderService)
		{
			_assemblyLoaderService = assemblyLoader;
			_bundleService = bundleService;
			_registry = registry;
			_objectBuilderService = objectBuilderService;
		}

		private XmlReaderSettings Settings
		{
			get
			{
				if (_settings == null)
				{
					_settings = XmlUtils.CreateFragmentReaderSettings();
					_settings.Schemas = GetXmlSchemaSet();
					_settings.ValidationType = ValidationType.None;
					_settings.ValidationEventHandler += OnXmlSchemaValidationErrors;
					//_settings.ValidationFlags = XmlSchemaValidationFlags.None;
				}
				return _settings;
			}
		}

		private void OnXmlSchemaValidationErrors(object sender, ValidationEventArgs e)
		{
			_logger.Error("XML Schema 验证发生错误.");
			if (e.Severity == XmlSeverityType.Error)
			{
				_logger.Error(e.Message, e.Exception);
			}
			if (e.Severity == XmlSeverityType.Warning)
			{
				_logger.Warn(e.Message, e.Exception);
			}
		}


		#region Helpers

        private bool CanReadXml<T>(XmlReader reader) where T : new()
        {
            XmlRootAttribute attr;
            if (Utils.TryGetCustomAttribute<XmlRootAttribute, T>(out attr, false))
            {
                if (attr.ElementName == reader.LocalName)
                {
                    return true;
                }
            }
            else
            {
                if (typeof(ITypedXmlSerializable).IsAssignableFrom(typeof(T)))
                {
                    T t = new T();
                    ITypedXmlSerializable typed = (ITypedXmlSerializable)t;
                    if (typed.XmlElementName == reader.LocalName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private XmlSchemaSet GetXmlSchemaSet()
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            ExtensionXmlSchema extXmlSchema = new ExtensionXmlSchema();
            XmlSchema extensionSchema;


            extensionSchema =
                ExtensionXmlSchema.CreateFromContributions(_registry
                , _bundleService, _assemblyLoaderService);


            AddExternalSchemas(extensionSchema.Includes);

            schemaSet.Add(extensionSchema);
            schemaSet.Compile();


            return schemaSet;
        }

		private void AddExternalSchemas(XmlSchemaObjectCollection includes)
		{
			/// @todo handle imports in an extendable manner (via ExtPoint).
			XmlSchema xmlSchema =
				XmlUtils.GetSchema(_assemblyLoaderService.LoadAssembly("jingxian.core.runtime"), "jingxian.core.runtime.Schemas.xml.xsd");
			XmlSchemaImport xmlImport = new XmlSchemaImport();
			xmlImport.Schema = xmlSchema;
			xmlImport.Namespace = xmlSchema.TargetNamespace;

			includes.Add(xmlImport);
		}

		private void ConfigureConfigurationElement<T>(T instance, IExtension extension)
		{
			IConfigurationElement configElement = instance as IConfigurationElement;
			if (configElement != null)
			{
				configElement.Configure(extension);
			}
			else
			{
                //TODO:   adfasdf
			}
		}

		#endregion

		#region IExtensionBuilder Members


		public T BuildTransient<T>(IExtension extension)
		{
			T implementation = (T)_objectBuilderService.BuildTransient(extension.Implementation);

			IExtensionAware awareImplementation = implementation as IExtensionAware;
			if (awareImplementation != null)
				awareImplementation.Configure(extension);

			return implementation;
		}


		public T BuildConfigurationFromXml<T>(IExtension extension) where T: IXmlSerializable, new()
		{
			using ( XmlReader reader = XmlReader.Create(
					new StringReader(extension.Configuration), Settings, XmlUtils.CreateParserContext()))
			{
				reader.MoveToContent();

				if (CanReadXml<T>(reader)) 
				{
					T instance = new T();
					instance.ReadXml(reader);
					ConfigureConfigurationElement(instance, extension);
					return instance;
				}
			}

			return default(T);
		}

		public T[] BuildConfigurationsFromXml<T>(IExtension extension) where T: IXmlSerializable, new()
		{
			List<T> result = new List<T>();
			using ( XmlReader reader = XmlReader.Create(
					new StringReader(extension.Configuration), Settings, XmlUtils.CreateParserContext()))
			{
				reader.MoveToContent();

				if (CanReadXml<T>(reader)) // check if the type T can be read with current reader  /// @todo TS REVIEW
				{
					while (reader.IsStartElement())
					{
						string xmlElementName = reader.LocalName;

						T instance = new T();
						XmlReader subtreeReader = reader.ReadSubtree();
						subtreeReader.MoveToContent();
						instance.ReadXml(subtreeReader);
						ConfigureConfigurationElement(instance, extension);
						result.Add(instance);

						if (!reader.IsStartElement())
						{
							reader.ReadEndElement();
						}
						else
						{
							reader.ReadToNextSibling(xmlElementName);
						}
					}
				}
			}
			return result.ToArray();
		}

		#endregion
	}
}
