

using Castle.Core;
using Castle.MicroKernel.LifecycleConcerns;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal sealed class ComponentIdAwareConcern : ILifecycleConcern
	{
		public const string ComponentIdAwareModelPropertyName = "jingxian.core.runtime.componentIdAware";

		public static readonly ComponentIdAwareConcern Instance = new ComponentIdAwareConcern();

		private ComponentIdAwareConcern()
		{
		}

		public void Apply(ComponentModel model, object component)
		{
			IComponentIdAware aware = component as IComponentIdAware;
			if (aware != null)
			{
				aware.ComponentId = model.Name;
			}
		}
	}
}