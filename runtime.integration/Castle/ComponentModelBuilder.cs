

using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.ModelBuilder.Inspectors;

namespace jingxian.core.runtime.castleIntegration
{
	internal sealed class ComponentModelBuilder: DefaultComponentModelBuilder
	{
        public ComponentModelBuilder(Castle.MicroKernel.IKernel kernel)
			: base(kernel)
		{
		}

		protected override void InitializeContributors()
		{
			AddContributor(new GenericInspector());
			//AddContributor(new ConfigurationModelInspector());
			//AddContributor(new ConfigurationParametersInspector());
			AddContributor(new LifestyleModelInspector());
			AddContributor(new ConstructorDependenciesModelInspector());
			AddContributor(new OptionalPropertiesDependenciesModelInspector());  // 替换 PropertiesDependenciesModelInspector
            AddContributor(new ExplicitLifecycleModelInspector());  // 替换 LifecycleModelInspector
			//AddContributor(new InterceptorInspector());
			AddContributor(new ComponentActivatorInspector());
			//AddContributor(new ComponentProxyInspector());
		}
	}
}
