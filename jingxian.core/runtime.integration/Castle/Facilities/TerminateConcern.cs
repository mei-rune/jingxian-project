

using Castle.Core;
using Castle.MicroKernel.LifecycleConcerns;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal sealed class TerminateConcern : ILifecycleConcern
	{
		public static readonly TerminateConcern Instance = new TerminateConcern();


		private TerminateConcern()
		{
		}


		public void Apply(ComponentModel model, object component)
		{
			IService runnable = component as IService;
			if (runnable != null)
			{
				runnable.Stop();
			}
		}
	}
}