

using System.Collections.Generic;

namespace jingxian.core.runtime.simpl
{
	internal sealed class ExtensionPoint : IExtensionPoint
    {
        private List<IExtension> _extensions = new List<IExtension>();
        private IExtensionPointConfiguration _configElement;

		public ExtensionPoint(IExtensionPointConfiguration cfg)
		{
			_configElement = cfg;
		}

		public string Id
		{
			get { return ConfigElement.Id; }
		}

		public string Name
		{
			get { return ConfigElement.Name; }
		}

		public string BundleId
		{
			get { return ConfigElement.BundleId; }
		}

		public string Description
		{
			get { return ConfigElement.Description; }
		}

		public IExtension[] Extensions
		{
			get { return _extensions.ToArray(); }
		}

		private IExtensionPointConfiguration ConfigElement
		{
			get { return _configElement; }
		}

		internal void AddExtension(IExtension ext)
		{
			_extensions.Add(ext);
		}

		public override string ToString()
		{
			return Id;
		}
	}
}