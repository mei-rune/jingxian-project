

using System;
using System.Text;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;


namespace jingxian.core.runtime
{
    using jingxian.core.runtime.utilities;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class ComponentAttribute: ExtensionAttribute
    {
        private readonly Type _InterfaceType;
		private string _configuration;

		public ComponentAttribute(Type interfaceType, Type implementation, string id, string bundleId)
			: base(id, bundleId, Constants.Points.Components, implementation)
		{
			_InterfaceType = interfaceType;
		}


		public Type InterfaceType
		{
            get { return _InterfaceType; }
		}

		public override string Configuration
		{
			get
			{
				if (_configuration == null)
				{
					_configuration = GetConfigurationXml();
				}
				return _configuration;
			}
			set
			{
				_configuration = value;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public string GetConfigurationXml()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format(CultureInfo.InvariantCulture,
				"<component interface='{0}' implementation='{1}' id='{2}' />",
				Utils.GetImplementationName(InterfaceType),
				Utils.GetImplementationName(Implementation),
				Id));
			return builder.ToString();
		}
	}
}