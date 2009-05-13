
using System.ComponentModel;
using Castle.MicroKernel;
namespace jingxian.core.runtime.castleIntegration
{
	internal sealed class MicroKernelWrapper: DefaultKernel
	{
		public override IConfigurationStore ConfigurationStore
		{
			get
			{
				return base.ConfigurationStore;
			}
			set
			{
				base.ConfigurationStore = value;
			}
		}

        public override Castle.MicroKernel.IKernel Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		public override IReleasePolicy ReleasePolicy
		{
			get
			{
				return base.ReleasePolicy;
			}
			set
			{
				base.ReleasePolicy = value;
			}
		}

		public new IHandlerFactory HandlerFactory
		{
			get
			{
				return base.HandlerFactory;
			}
		}

		public new IComponentModelBuilder ComponentModelBuilder
		{
			get
			{
				return base.ComponentModelBuilder;
			}
		}

		public new IProxyFactory ProxyFactory
		{
			get
			{
				return base.ProxyFactory;
			}
		}

		public new IDependencyResolver Resolver
		{
			get
			{
				return base.Resolver;
			}
		}
	}
}
