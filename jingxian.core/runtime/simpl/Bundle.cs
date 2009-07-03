

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;
using System.Diagnostics;

namespace jingxian.core.runtime.simpl
{
    using jingxian.core.runtime.simpl.Resources;
    using jingxian.core.runtime.utilities;

	[Serializable]
	[XmlRoot(
		ElementName = Bundle.XmlElementName,
		DataType = Bundle.XmlTypeName,
		Namespace = RuntimeConstants.CurrentXmlSchemaNamespace,
		IsNullable = false)]
	public sealed class Bundle: IBundle, IXmlSerializable, ITypedXmlSerializable
    {
        static jingxian.logging.ILog _logger = jingxian.logging.LogUtils.GetLogger(typeof(Bundle));
        
		public const string XmlSchemaResource = "jingxian.core.runtime.Schemas.Bundle.xsd";
		public const string XmlTypeName = "Bundle";
		public const string XmlElementName = "bundle";

        private static XmlReaderSettings _xmlReaderSettings;
        private readonly Dictionary<string, ExtensionConfiguration> _contributedExtensions;
        private readonly Dictionary<string, ExtensionPointConfiguration> _contributedExtensionPoints;
        private string _Id;
        private string _name;
        private Version _version;
        private string _provider;
        private string _description;
        private string _assemblyLocation;
        private string _activatorClass;
        private object _activatorInstance;
        private BundleState _State;

        public Bundle()
        {
            _contributedExtensionPoints = new Dictionary<string, ExtensionPointConfiguration>();
            _contributedExtensions = new Dictionary<string, ExtensionConfiguration>();
        }

        public Bundle(string id, string name, string version, string provider, string description, string assemblyLocation,
                                    string activatorClass)
            : this()
        {
            _Id = id;
            _name = name;
            _version = new Version(version);
            _provider = provider;
            _description = description;
            _assemblyLocation = assemblyLocation;
            _activatorClass = activatorClass;
        }

		internal static Bundle CreateFromManifestResource(Assembly asm, string resourceName)
		{
			using (Stream resourceStream = asm.GetManifestResourceStream(resourceName))
			{
				if (resourceStream == null || resourceStream.Length == 0)
					throw new InvalidOperationException(
						string.Format(CultureInfo.InvariantCulture, Messages.FailedToLoadBundleFromResource, resourceName, asm.FullName, asm.Location));
				
                using (XmlReader reader = XmlReader.Create(resourceStream, XmlReaderSettings))
                {
                    Bundle bundle = XmlUtils.CreateFromXml<Bundle>(reader);
                    bundle._assemblyLocation = asm.Location;
                    ScanBundleForExtensionsAndExtensionPoints(asm, bundle);
                    ScanBundleFileVersion(asm, bundle);
                    bundle._State = BundleState.Installed;

                    _logger.DebugFormat("Bundle '{0}' 成功创建.", bundle);

                    return bundle;
                }
			}
		}

		private static void ScanBundleFileVersion(Assembly asm, Bundle bundle)
		{
			FileVersionInfo bundleFileVersion = FileVersionInfo.GetVersionInfo(asm.Location);
			bundle._version = new Version(bundleFileVersion.FileVersion);
		}

		private static XmlReaderSettings XmlReaderSettings
		{
			get
			{
				if (_xmlReaderSettings == null)
				{
					_xmlReaderSettings = XmlUtils.CreateFragmentReaderSettings(
						new XmlSchema[] { GetSchema() });
				}
				return _xmlReaderSettings;
			}
		}

		#region IXmlSerializable Members

		XmlSchema IXmlSerializable.GetSchema()
		{
			return GetSchema();
		}

		internal static XmlSchema GetSchema()
		{
			return
				XmlSchema.Read( typeof(RuntimeConstants).Assembly.GetManifestResourceStream(RuntimeConstants.BundleXmlSchemaResource),
					XmlUtils.ReportCompileError);
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			_Id = reader.GetAttribute("id");
			_name = reader.GetAttribute("name");
			_description = reader.GetAttribute("description");
			_activatorClass = reader.GetAttribute("activator");
			string version = reader.GetAttribute("version");
			_version = string.IsNullOrEmpty(version) ? new Version() : new Version(version);
			_provider = reader.GetAttribute("provider");
			_assemblyLocation = reader.GetAttribute("assemblyLocation");
			// only continue, if the XML start element is correct.
			if (reader.IsStartElement("bundle", RuntimeConstants.CurrentXmlSchemaNamespace)
				|| reader.IsStartElement("Bundle"))
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement();
				}
				else
				{
					reader.ReadStartElement();
					while (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "extensionPoint")
					{
						ExtensionPointConfiguration ep = new ExtensionPointConfiguration(_Id);
						ep.ReadXml(reader);
						if (!_contributedExtensionPoints.ContainsKey(ep.Id))
						{
							_contributedExtensionPoints.Add(ep.Id, ep);
						}
						else
						{
							throw new PlatformConfigurationException(
								string.Format(CultureInfo.InvariantCulture,
								"Duplicate extension point with id '{0}' in bundle '{1}'.", ep.Id, _Id));
						}
					}
					while (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "extension")
					{
						ExtensionConfiguration ext = ExtensionConfiguration.FromXml(_Id, reader);
						if (!_contributedExtensions.ContainsKey(ext.Id))
						{
							_contributedExtensions.Add(ext.Id, ext);
						}
						else
						{
							throw new PlatformConfigurationException(
								string.Format(CultureInfo.InvariantCulture, "Duplicate extension with id '{0}' in bundle '{1}'.", ext.Id, _Id));
						}
					}
					reader.ReadEndElement();
				}
			}
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteAttributeString("id", Id);
			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("description", Description);
			writer.WriteAttributeString("activator", ActivatorClass);
			writer.WriteAttributeString("version", Version.ToString());
			writer.WriteAttributeString("provider", Provider);
			writer.WriteAttributeString("assemblyLocation", AssemblyLocation);
			foreach (IExtensionPointConfiguration ep in ContributedExtensionPoints)
			{
				writer.WriteStartElement("extensionPoint", RuntimeConstants.CurrentXmlSchemaNamespace);
				((IXmlSerializable) ep).WriteXml(writer);
				writer.WriteFullEndElement();
			}
			foreach (IExtensionConfiguration ext in ContributedExtensions)
			{
				writer.WriteStartElement("extension", RuntimeConstants.CurrentXmlSchemaNamespace);
				((IXmlSerializable) ext).WriteXml(writer);
				writer.WriteFullEndElement();
			}
		}

		string ITypedXmlSerializable.XmlTypeName
		{
            get { return XmlTypeName; }
		}

		string ITypedXmlSerializable.XmlElementName
		{
            get { return XmlElementName; }
		}

		string ITypedXmlSerializable.XmlSchemaNamespace
		{
            get { return RuntimeConstants.CurrentXmlSchemaNamespace; }
		}

		#endregion

        #region IBundle 接口

        public string Id
		{
			get
			{
				return _Id;
			}
		}

		public string Name
		{
            get { return _name; }
		}

		public Version Version
		{
            get { return _version; }
		}

		public string Provider
		{
            get { return _provider; }
		}

		public string Description
		{
            get { return _description; }
		}

		public string AssemblyLocation
		{
            get { return _assemblyLocation; }
		}

		public string ActivatorClass
		{
            get { return _activatorClass; }
        }


		public IExtensionConfiguration[] ContributedExtensions
		{
			get
			{
				ExtensionConfiguration[] extensions = new ExtensionConfiguration[_contributedExtensions.Count];
				_contributedExtensions.Values.CopyTo(extensions, 0);
				return extensions;
			}
		}

		public IExtensionPointConfiguration[] ContributedExtensionPoints
		{
			get
			{
				ExtensionPointConfiguration[] points = new ExtensionPointConfiguration[_contributedExtensionPoints.Count];
				_contributedExtensionPoints.Values.CopyTo(points, 0);
				return points;
			}
		}

		BundleState IBundle.State
		{
			get
			{
				return _State;
			}
        }

        #endregion

        private static void ScanBundleForExtensionsAndExtensionPoints(Assembly asm, Bundle bundle)
		{
			Type[] types;

            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException exc)
            {
                bundle._State = BundleState.Error;
                if (_logger.IsErrorEnabled)
                {
                    _logger.ErrorFormat("在程序集 '{0}' 中搜索 Bundle '{1}' 发生错误!.", asm.FullName, bundle);
                    _logger.Error(typeof(ReflectionTypeLoadException).FullName);
                    _logger.ErrorFormat("		{0}", exc.Message);
                    _logger.Error("	     异常列表:");
                    foreach (Exception loaderExc in exc.LoaderExceptions)
                    {
                        _logger.ErrorFormat("	       {0}", loaderExc.Message);
                    }
                }
                return;
            }


			AddExtensionsToBundle(bundle, asm.GetCustomAttributes(typeof(ExtensionAttribute), true));
			AddExtensionPointsToBundle(bundle, asm.GetCustomAttributes(typeof(ExtensionPointAttribute), true));

			foreach (Type type in types)
			{
				AddExtensionsToBundle(bundle, type.GetCustomAttributes(typeof(ExtensionAttribute), true));

				AddExtensionPointsToBundle(bundle, type.GetCustomAttributes(typeof(ExtensionPointAttribute), true));
			}
		}

		private static void AddExtensionPointsToBundle(Bundle bundle, object[] pointAttributes)
		{
			foreach (ExtensionPointAttribute pointAttr in pointAttributes)
			{
				AddExtensionPointToBundle(bundle, pointAttr);
			}
		}

		private static void AddExtensionPointToBundle(Bundle bundle, ExtensionPointAttribute pointAttr)
		{
			if (pointAttr.BundleId != bundle.Id)
			{
				_logger.WarnFormat("扩展点 '{0}' 已经由 bundle '{1}' 提供，但程序集 '{2}' '{3}'.", pointAttr.Id, pointAttr.BundleId, bundle.AssemblyLocation, bundle.Id);
				return;
			}
			ExtensionPointConfiguration cfg;
			if (bundle._contributedExtensionPoints.TryGetValue(pointAttr.Id, out cfg))
			{
				cfg.Merge(pointAttr);
			}
			else
			{
				bundle.AddExtensionPointConfiguration(new ExtensionPointConfiguration(pointAttr));
			}
		}

		private static void AddExtensionsToBundle(Bundle bundle, object[] extensionAttributes)
		{
			foreach (ExtensionAttribute extensionAttr in extensionAttributes)
			{
				AddExtensionToBundle(bundle, extensionAttr);
			}
		}

		private static void AddExtensionToBundle(Bundle bundle, ExtensionAttribute extensionAttr)
		{
			if (extensionAttr.BundleId != bundle.Id)
			{
                _logger.WarnFormat("扩展 '{0}' 提供给包 '{1}',但程序集 '{2}'   '{3}'."
                    , extensionAttr.Id, extensionAttr.BundleId, bundle.AssemblyLocation, bundle.Id);
				return;
			}
		
			ExtensionConfiguration cfg;
			if (bundle._contributedExtensions.TryGetValue(extensionAttr.Id, out cfg))
			{
				cfg.Merge(extensionAttr);
			}
			else
			{
				bundle.AddExtensionConfiguration(new ExtensionConfiguration(extensionAttr));
			}
		}

		internal void AddExtensionPointConfiguration(ExtensionPointConfiguration cfg)
		{
			_contributedExtensionPoints.Add(cfg.Id, cfg);
		}

		internal void AddExtensionConfiguration(ExtensionConfiguration cfg)
		{
			_contributedExtensions.Add(cfg.Id, cfg);
		}


		internal void Activate(IObjectBuilder builder)
		{
            if (!string.IsNullOrEmpty(_activatorClass))
            {
                try
                {
                    _activatorInstance = builder.BuildTransient(_activatorClass);
                }
                catch (Exception exc)
                {
                    _logger.FatalFormat(string.Concat("实例化 Bundle ", Id, " 时发生错误!."), exc);
                }
            }
		}

		internal void Deactivate()
		{
			if(_activatorInstance != null)
			{
				IDisposable disposable = _activatorInstance as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			_activatorInstance = null;
		}

        #region ToString (override)

        public override string ToString()
        {
            return _Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Bundle b = (Bundle)obj;
            if (
                Id != b.Id
                || Name != b.Name
                || Version != b.Version
                || Provider != b.Provider
                || Description != b.Description
                || AssemblyLocation != b.AssemblyLocation
                || ActivatorClass != b.ActivatorClass
                || ContributedExtensionPoints.Length != b.ContributedExtensionPoints.Length
                || ContributedExtensions.Length != b.ContributedExtensions.Length
                //|| BundleState != b.BundleState
                )
            {
                return false;
            }
            else
            {
                for (int i = 0; i < ContributedExtensions.Length; i++)
                {
                    if (!ContributedExtensions[i].Equals(b.ContributedExtensions[i]))
                    {
                        return false;
                    }
                }
                for (int i = 0; i < ContributedExtensionPoints.Length; i++)
                {
                    if (!ContributedExtensionPoints[i].Equals(b.ContributedExtensionPoints[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Id) ? base.GetHashCode() : Id.GetHashCode();
        }

        #endregion
    }
}
