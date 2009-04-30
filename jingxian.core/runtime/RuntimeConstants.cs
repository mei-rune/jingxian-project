

using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime
{
	public static class RuntimeConstants
	{

        public const string RuntimeConfigurationId = "jingxian.core.runtime.configuration";
        public const string BundleConfigurationFileName = "Bundle.xml";
        public const string BundleXmlSchemaResource = "jingxian.core.runtime.Schemas.Bundle.xsd"; 


        public const string CurrentXmlSchemaNamespace = XmlSchemaNamespace02;
        public const string XmlSchemaNamespace01 = @"http://www.jingxian.com/0.1/Schemas"; 
        public const string XmlSchemaNamespace02 = @"http://www.jingxian.org/schemas/Bundle.xsd";
        public const string XmlNamespace = @"http://www.w3.org/XML/1998/namespace";

        public const string MiniKernelId = "jingxian.core.runtime.miniKernel";
        public const string ProductId = "jingxian.core.runtime.context"; 

        public const string AssemblyLoaderServiceId = "jingxian.core.runtime.assemblyLoaderService";
        public const string ObjectBuilderServiceId = "jingxian.core.runtime.objectBuilderService";
        public const string BundleServiceId = "jingxian.core.runtime.bundleService"; 
        public const string ExtensionRegistryId = "jingxian.core.runtime.extensionRegistry";
        public const string ServiceRegistryId = "jingxian.core.runtime.serviceRegistry";



        public const string ParsedCommandLineArgumentsId = "jingxian.core.runtime.parsedCommandLineArguments";

        public const string XmlImplementationAttributeName = "implementation";


	}
}

