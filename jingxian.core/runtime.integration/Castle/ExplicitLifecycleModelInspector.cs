

using System;
using System.ComponentModel;
using Castle.MicroKernel.LifecycleConcerns;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;

namespace jingxian.core.runtime.castleIntegration
{
	internal class ExplicitLifecycleModelInspector: IContributeComponentModelConstruction
	{
        public void ProcessModel(Castle.MicroKernel.IKernel kernel, ComponentModel model)
		{
            Enforce.ArgumentNotNull<Castle.MicroKernel.IKernel>(kernel, "kernel");
			
			if (typeof(IInitializable).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Commission, InitializationConcern.Instance);
			}
			if (typeof(ISupportInitialize).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Commission, SupportInitializeConcern.Instance);
			}

			if (IsDisposalAllowed(model)
				&& typeof(IDisposable).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Decommission, DisposalConcern.Instance);
			}
		}

		private static bool IsDisposalAllowed(ComponentModel model)
		{
			bool allowed = true;
			if (model.ExtendedProperties != null && model.ExtendedProperties.Contains(typeof(ComponentLifestyle)))
			{
				allowed = ((ComponentLifestyle) model.ExtendedProperties[typeof(ComponentLifestyle)])
					!= ComponentLifestyle.DependencyInjectionOnly;
			}
			return allowed;
		}
	}
}
