

using Castle.Core;
using Castle.MicroKernel.Releasers;
using Castle.MicroKernel;

namespace jingxian.core.runtime.castleIntegration
{
	internal class ExplicitReleasePolicy: LifecycledComponentsReleasePolicy
	{
		public override void Track(object instance, Burden burden)
		{
			if (ShouldTrack(burden.Model))
			{
				base.Track(instance, burden);
			}
		}

		private static bool ShouldTrack(ComponentModel model)
		{
			bool track;
			if (model.ExtendedProperties != null && model.ExtendedProperties.Contains(typeof(ComponentLifestyle)))
			{
				track = ((ComponentLifestyle) model.ExtendedProperties[typeof(ComponentLifestyle)])
					!= ComponentLifestyle.DependencyInjectionOnly;
			}
			else
			{
				track = model.LifestyleType != LifestyleType.Undefined;
			}
			return track;
		}
	}
}
