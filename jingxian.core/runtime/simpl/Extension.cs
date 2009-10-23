

using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace jingxian.core.runtime.simpl
{
	internal sealed class Extension: IExtension
	{
		private IExtensionConfiguration _configElement;
		private IExtensionBuilder _builder;

		public Extension(IExtensionConfiguration cfg, IExtensionBuilder builder)
		{
			_configElement = cfg;
			_builder = builder;
		}

		public string Id
		{
            get { return ConfigElement.Id; }
		}

		public string BundleId
		{
			get{	return _configElement.BundleId;}
		}

		public string Name
		{
			get{	return ConfigElement.Name;}
		}

		public string Description
		{
			get{	return _configElement.Description;}
		}

		public string Point
		{
			get{	return ConfigElement.Point;}
		}

		public string Configuration
		{
			get{	return ConfigElement.Configuration;}
		}

		private IExtensionConfiguration ConfigElement
		{
			get{	return _configElement;}
		}

		public string Implementation
		{
			get{	return _configElement.Implementation;}
        }

        public bool HasConfiguration
        {
            get { return !string.IsNullOrEmpty(Configuration); }
        }

        public bool HasImplementation
        {
            get { return !string.IsNullOrEmpty(Implementation); }
        }

        public T Build<T>()
        {
            return _builder.Build<T>(this);
        }

        //public CFG GetConfiguration<CFG>()
        //    where CFG : IXmlSerializable, new()
        //{
        //    return _builder.BuildConfigurationFromXml<CFG>(this);
        //}

        public override string ToString()
        {
            return Id;
        }
    }
}