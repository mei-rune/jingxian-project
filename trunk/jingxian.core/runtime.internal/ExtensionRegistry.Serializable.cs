

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using jingxian.core.utilities;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	internal sealed partial class ExtensionRegistry
	{
		[XmlRoot("extensionRegistry")]
		internal class Serializable: IXmlSerializable
		{
			[NonSerialized]
			private ExtensionRegistry _registry;
			[NonSerialized]
			private ExtensionBuilder _extensionBuilder;

			internal ExtensionRegistry Registry
			{
				get{return _registry;}
				set{	_registry = value;}
			}


			internal ExtensionBuilder ExtensionBuilder
			{
				get{	return _extensionBuilder;}
				set{	_extensionBuilder = value;}
			}


			XmlSchema IXmlSerializable.GetSchema()
			{
				throw new NotImplementedException();
			}

            void IXmlSerializable.ReadXml(XmlReader reader)
            {
                if (!reader.IsStartElement("extensionRegistry"))
                {
                    throw new InvalidOperationException(
                      string.Concat("无效配置.期望节点是 'extensionRegistry 便实际是 '", reader.LocalName, "'."));
                }

                reader.ReadStartElement("extensionRegistry");

                if (!reader.IsStartElement("extensionPoints"))
                {
                    throw new InvalidOperationException(
                      string.Concat("无效配置.期望节点是 'extensionRegistry 便实际是 '", reader.LocalName, "'."));
                }

                if (reader.IsEmptyElement)
                {
                    reader.ReadStartElement("extensionPoints");
                }
                else
                {
                    reader.ReadStartElement("extensionPoints");

                    while (reader.IsStartElement("extensionPoint"))
                    {
                        ExtensionPointConfiguration cfg = new ExtensionPointConfiguration();
                        cfg.ReadXml(reader);
                        List<ExtensionConfiguration> extensionConfigs = new List<ExtensionConfiguration>();
                        XmlUtils.ReadElementsIntoList<ExtensionConfiguration>(reader, "extensions", "extension", extensionConfigs);

                        IExtensionPoint point = new ExtensionPoint(cfg);
                        _registry._extensionPoints.Add(point.Id, point);

                        for (int i = 0; i < extensionConfigs.Count; i++)
                        {
                            IExtension ext = new Extension(extensionConfigs[i], _extensionBuilder);
                            _registry._extensions.Add(ext.Id, ext);
                            ((ExtensionPoint)point).AddExtension(ext);
                        }
                    }

                    reader.ReadEndElement();
                }



                if (reader.IsStartElement("extensions"))
                {
                    if (reader.IsEmptyElement)
                    {
                        reader.ReadStartElement("extensions");
                    }
                    else
                    {
                        // read the orphans
                        reader.ReadStartElement("extensions");

                        while (reader.IsStartElement("extension"))
                        {

                            ExtensionConfiguration extCfg = ExtensionConfiguration.FromXml(reader);
                            if (!_registry._extensions.ContainsKey(extCfg.Id))
                            {
                                IExtension ext = new Extension(extCfg, _extensionBuilder);
                                _registry._extensions.Add(ext.Id, ext);
                            }
                        }

                        reader.ReadEndElement();
                    }
                }

                reader.ReadEndElement(); //</extensionRegistry>
            }

			void IXmlSerializable.WriteXml(XmlWriter writer)
			{
				writer.WriteAttributeString("type", Utils.GetImplementationName<ExtensionRegistry>());
				writer.WriteAttributeString("serializableType", Utils.GetImplementationName<Serializable>());
				writer.WriteAttributeString("extensionPoints", _registry._extensionPoints.Count.ToString(CultureInfo.InvariantCulture));
				writer.WriteAttributeString("extensions", _registry._extensions.Count.ToString(CultureInfo.InvariantCulture));

				writer.WriteStartElement("extensionPoints");
				foreach (IExtensionPoint point in _registry.ExtensionPoints)
				{
					List<ExtensionConfiguration> extensionConfigs = new List<ExtensionConfiguration>();
					ExtensionPointConfiguration cfg = (ExtensionPointConfiguration) _registry.GetExtensionPointConfigurationElement(point.Id);

					for (int i = 0; i < point.Extensions.Length; i++)
					{
						ExtensionConfiguration extCfg = (ExtensionConfiguration) _registry.GetExtensionConfigurationElement(point.Extensions[i].Id);
						if( null != extCfg )
						    extensionConfigs.Add(extCfg);
					}

					XmlUtils.WriteElement<ExtensionPointConfiguration>(writer, "extensionPoint", cfg);
					XmlUtils.WriteElementsFromList<ExtensionConfiguration>(writer, "extensions", "extension", extensionConfigs);
				}
				writer.WriteFullEndElement();

				List<ExtensionConfiguration> orphanExtensionConfigs = new List<ExtensionConfiguration>();
				foreach (IExtension ext in _registry._extensions.Values)
				{
					if (string.IsNullOrEmpty(ext.Point) || IsNonExistentExtensionPoint(ext.Point))
					{
						IExtensionConfiguration cfg = _registry.GetExtensionConfigurationElement(ext.Id);

						ExtensionConfiguration extCfg = cfg as ExtensionConfiguration;
				
                        if( null != extCfg )
						    orphanExtensionConfigs.Add(extCfg);
					}
				}

				XmlUtils.WriteElementsFromList<ExtensionConfiguration>(writer, "extensions", "extension", orphanExtensionConfigs);
			}

			private bool IsNonExistentExtensionPoint(string pointId)
			{
				IExtensionPointConfiguration fooCfg;
				return _registry.TryGetExtensionPointConfiguration(pointId, out fooCfg);
			}
		}
	}
}