

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace jingxian.core.runtime
{
    using jingxian.logging;
    using jingxian.core.runtime.utilities;

    public sealed class ConfigurationSupplier<T> where T : IXmlSerializable, new()
    {
        private readonly IExtensionRegistry _extensionRegistry;
        private readonly IAssemblyLoaderService _assemblyLoaderService;
        private readonly IBundleService _bundleService;


        private IDictionary<string, T> _configurations;
        private XmlReaderSettings _settings;



        public ConfigurationSupplier(IExtensionRegistry extensionRegistry
            , IAssemblyLoaderService assemblyLoader
            , IBundleService bundleService )
        {
            _extensionRegistry = Enforce.ArgumentNotNull<IExtensionRegistry>(extensionRegistry, "extensionRegistry");
			_assemblyLoaderService = assemblyLoader;
			_bundleService = bundleService;
		}

        public IDictionary<string, T> Configurations
        {
            get
            {
                if (_configurations == null)
                    throw new InvalidOperationException("必须先调用 'Fetch' 方法");
                return _configurations;
            }
        }

        public IDictionary<string, T> Fetch(string pointId)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat("取扩展点{0} 的 contributions ...", pointId);


            _configurations = new Dictionary<string, T>();
            IExtension[] extensions = _extensionRegistry.GetExtensions(pointId);

            if (extensions.Length == 0)
            {
                _logger.WarnFormat("没有找到扩展点'{0}'的 contributions.", pointId);
                _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
                    , pointId
                    , _configurations.Count);
                return _configurations;
            }
            foreach (IExtension ext in extensions)
            {
                if (!ext.HasConfiguration)
                {
                    _logger.WarnFormat("扩展 '{0}' 没有配置.",
                            ext.Id);

                    continue;
                }

                T configuration = BuildConfigurationFromXml(ext);

                if (null == configuration)
                {
                    _logger.WarnFormat("扩展 '{0}' 没有有效的配置", ext.Id);
                    continue;
                }

                if (Equals(configuration, default(T)))
                    throw new PlatformConfigurationException(
                        string.Format("扩展 '{0}' 有一个无效的配置.", ext.Id));


                Debug.Assert(!_configurations.ContainsKey(ext.Id),
                    string.Format("检测到配置类 '{0}' 一个重复的 ID '{1}'.",
                    configuration.GetType(), ext.Id));

                _configurations.Add(ext.Id, configuration);
                _logger.DebugFormat("     + 添加一个id为 '{0}' 的 Contribution.", ext.Id);
            }

            _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
                , pointId
                , _configurations.Count);

            return _configurations;
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


        public T BuildConfigurationFromXml(IExtension extension)
        {
            using (XmlReader reader = XmlReader.Create(
                    new StringReader(extension.Configuration), Settings, XmlUtils.CreateParserContext()))
            {
                reader.MoveToContent();

                if (CanReadXml(reader))
                {
                    T instance = new T();
                    instance.ReadXml(reader);
                    ConfigureConfigurationElement(instance, extension);
                    return instance;
                }
            }

            return default(T);
        }

        public T[] BuildConfigurationsFromXml(IExtension extension)
        {
            List<T> result = new List<T>();
            using (XmlReader reader = XmlReader.Create(
                    new StringReader(extension.Configuration), Settings, XmlUtils.CreateParserContext()))
            {
                reader.MoveToContent();

                if (CanReadXml(reader))
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

        #region Helpers

        private bool CanReadXml(XmlReader reader)
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
                ExtensionXmlSchema.CreateFromContributions(_extensionRegistry
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

        private void ConfigureConfigurationElement(T instance, IExtension extension)
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

        private ILog _log;

        private ILog _logger
        {
            get
            {
                if (null == _log)
                {
                    string loggerName = string.Format(CultureInfo.InvariantCulture, "jingxian.core.runtime.configurationSupplier.{0}", typeof(T).Name);
                    _log = LogUtils.GetLogger(loggerName);
                }
                return _log;
            }
        }
    }
}