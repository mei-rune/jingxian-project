

using Castle.Core;
using Castle.MicroKernel.LifecycleConcerns;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal sealed class RunConcern : ILifecycleConcern
	{
		public static readonly RunConcern Instance = new RunConcern();


		private RunConcern()
		{
		}

		public void Apply(ComponentModel model, object component)
		{
			IService runnable = component as IService;
			if (runnable != null)
			{
				runnable.Start();
			}
		}
	}
}