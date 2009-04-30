
using System;
using System.ComponentModel;

namespace jingxian.core.runtime
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ExtensionPointAttribute : Attribute
	{
		private string _Id;
		private string _bundleId;
		private string _name;
        private string _description;
        private string _configuration;

        public ExtensionPointAttribute(string id, string bundleId)
        {
            _Id = id;
            _bundleId = bundleId;
        }

		public string Id
		{
			get { return _Id; }
		}

		public string BundleId
		{
			get { return _bundleId; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string Configuration
		{
			get { return _configuration; }
			set { _configuration = value; }
		}
	}
}