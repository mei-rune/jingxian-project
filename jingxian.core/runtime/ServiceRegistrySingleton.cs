

using System;
using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime
{
	public sealed class ServiceRegistrySingleton: Service
	{
		private static IServiceRegistry _instance;
		private readonly IServiceRegistry _serviceRegistry;

		public ServiceRegistrySingleton(IServiceRegistry serviceRegistry)
		{
			_serviceRegistry = serviceRegistry;
		}

		public static IServiceRegistry Instance
		{
			get
			{
				return _instance;
			}
			private set
			{
				if (_instance != null && value != null)
					throw new InvalidOperationException("不能初始化两次.");
				_instance = value;
			}
		}

		protected override void internalStart()
		{
			base.internalStart();
			Instance = _serviceRegistry;
		}

		protected override void internalStop()
		{
			try
			{
				base.internalStop();
			}
			finally
			{
				Instance = null;
			}
		}
	}
}
