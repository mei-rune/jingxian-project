﻿
using System;
using System.Text;
using System.Globalization;

namespace jingxian.core.runtime
{
    using jingxian.core.utilities;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ServiceAttribute: ExtensionAttribute
	{
		private string _configuration;
		private readonly Type _ServiceInterface;

		public ServiceAttribute(Type serviceInterface, Type implementation, string id, string bundleId)
			: base(id, bundleId, Constants.Points.Services, implementation)
		{
			if (serviceInterface == null)
				throw new ArgumentNullException("serviceInterface");

		
			_ServiceInterface = serviceInterface;
		}

		public Type ServiceInterface
		{
            get { return _ServiceInterface; }
		}

		public override string Configuration
		{
			get
			{
				if (_configuration == null)
				{
					var builder = new StringBuilder();
					builder.Append(
						string.Format(CultureInfo.InvariantCulture,
						"<service interface='{0}' implementation='{1}' id='{2}' />",
						Utils.GetImplementationName(ServiceInterface),
						Utils.GetImplementationName(Implementation),
						Id));
					_configuration = builder.ToString();
				}
				return _configuration;
			}
			set
			{
				_configuration = value;
			}
		}
	}
}