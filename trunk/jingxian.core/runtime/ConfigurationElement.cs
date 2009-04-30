
using System;


namespace jingxian.core.runtime
{
    using jingxian.core.runtime.Xml.Serialization;

	[ExtensionContract]
	public abstract class ConfigurationElement: XmlSerializableIdentifiable, IConfigurationElement
	{
		private IExtension _declaringExtension;

		protected ConfigurationElement()
		{
		}

		protected ConfigurationElement(string id)
			: base(id)
		{
		}

		public IExtension DeclaringExtension
		{
            get { return _declaringExtension; }
		}

        void IConfigurationElement.Configure(IExtension declaringExtension)
        {
            if (declaringExtension == null)
                throw new ArgumentNullException("declaringExtension"); 


            if (_declaringExtension == null)
                _declaringExtension = declaringExtension;

            throw new WriteOnceViolatedException("declaringExtension");
        }

	}
}