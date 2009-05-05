

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using jingxian.core.runtime.utilities;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	internal sealed class ExtensionXmlSchema: IXmlSerializable
	{
		public const string CacheKey = "jingxian.core.runtime.extensions.xsd";
		private XmlSchema _schema;

		public ExtensionXmlSchema()
		{
		}

        public ExtensionXmlSchema(XmlSchema schema)
            : this()
        {
            _schema = Enforce.ArgumentNotNull<XmlSchema>(schema, "schema");
        }


		public XmlSchema Schema
		{
			get
			{
				return _schema;
			}
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			throw new NotSupportedException("不能创建一个 schema, 因为它自己就是一个 schema.");
		}

		public void ReadXml(XmlReader reader)
		{
			_schema = XmlSchema.Read(reader, XmlUtils.ReportCompileError);
		}

		public void WriteXml(XmlWriter writer)
		{
			_schema.Write(writer);
		}

		#endregion

		#region static factory methods

		internal static XmlSchema CreateFromContributions(IExtensionRegistry registry, IBundleService bundleService, IAssemblyLoaderService assemblyLoader)
		{
			IExtension[] extensions = registry.GetExtensions(Constants.Points.XmlSchemas);

			XmlSchema extensionSchema = new XmlSchema();

			XmlSchema[] contributedSchemas = GetContributedXmlSchemas(extensions, bundleService, assemblyLoader);

			for (int i = 0; i < contributedSchemas.Length; i++)
			{
				CopyAllItemTypes(contributedSchemas[i], extensionSchema.Items);
			}

			extensionSchema.AttributeFormDefault = XmlSchemaForm.Unqualified;
			extensionSchema.ElementFormDefault = XmlSchemaForm.Qualified;
			extensionSchema.Version = typeof(ExtensionXmlSchema).Assembly.GetName().Version.ToString();
			extensionSchema.Namespaces.Add(string.Empty, RuntimeConstants.CurrentXmlSchemaNamespace);
			extensionSchema.TargetNamespace = RuntimeConstants.CurrentXmlSchemaNamespace;

			return extensionSchema;
		}

		private static XmlSchema[] GetContributedXmlSchemas(IEnumerable<IExtension> xmlSchemaExtensions, IBundleService bundleService, IAssemblyLoaderService assemblyLoader)
		{
			List<XmlSchema> contributedSchemas = new List<XmlSchema>();

            foreach (IExtension ext in xmlSchemaExtensions)
            {
                IBundle bundle;
                if (!bundleService.TryGetBundleForExtension(ext.Id, out bundle))
                {
                    throw new PlatformConfigurationException(
                        string.Format(CultureInfo.InvariantCulture, "不能为扩展 '{0}' 获取 Bundle .跳过它.", ext.Id));

                }
                Assembly assembly;
                if (assemblyLoader.TryGetLoadedAssembly(
                    Utils.GetAssemblySimpleNameFromLocation(bundle.AssemblyLocation), out assembly))
                {
                    if (ext.HasImplementation)
                    {
                        XmlSchema schema = XmlUtils.GetSchema(assembly, ext.Implementation);
                        contributedSchemas.Add(schema);
                    }
                }
                else
                {
                    if (assemblyLoader.TryLoadAssembly(
                        Utils.GetAssemblyFileNameFromLocation(bundle.AssemblyLocation), out assembly))
                    {
                        if (ext.HasImplementation)
                        {
                            XmlSchema schema = XmlUtils.GetSchema(assembly, ext.Implementation);
                            contributedSchemas.Add(schema);
                        }
                    }
                    else
                    {
                        throw new PlatformConfigurationException(
                            string.Format("不能载入程序集 '{0}' 中的 XML Schema 扩展 '{1}'. 跳过它.", bundle.AssemblyLocation, ext.Id));
                    }
                }
            }

			return contributedSchemas.ToArray();
		}

		private static void CopyAllItemTypes(XmlSchema source, XmlSchemaObjectCollection destination)
		{
			for (int i = 0; i < source.Items.Count; i++)
			{
				if (!destination.Contains(source.Items[i]))
				{
					destination.Add(source.Items[i]);
				}
			}
		}

		#endregion

	}
}