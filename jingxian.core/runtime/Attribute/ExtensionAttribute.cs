
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
	public class ExtensionAttribute: Attribute, IRuntimePart
	{

        private readonly string _Id;
        private readonly string _bundleId;
        private readonly string _point;
        private readonly Type _implementation;
        private string _configuration;
        private string _description;
        private string _name;

		private ExtensionAttribute(
			string id,
			string bundleId)
		{
			if (string.IsNullOrEmpty(id))
				throw new StringArgumentException("id");
			if (string.IsNullOrEmpty(bundleId))
				throw new StringArgumentException("bundleId");


			_Id = id;
			_bundleId = bundleId;
		}
        
        public ExtensionAttribute(
			string id,
			string bundleId,
			string point)
			: this(id, bundleId)
		{
			if (string.IsNullOrEmpty(point))
				throw new StringArgumentException("point");

			_point = point;
		}

		public ExtensionAttribute(
			string id,
			string bundleId,
			string point,
			Type implementation)
			: this(id, bundleId, point)
		{
			if (implementation == null)
				throw new ArgumentNullException("implementation");

			_implementation = implementation;
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

		public virtual string Configuration
        { 
            get { return _configuration; } 
            set { _configuration = value; } 
        }

		public string Point
		{
            get { return _point; }
		}

		public Type Implementation
		{
            get { return _implementation; }
		}

	}
}