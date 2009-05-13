

using Castle.Core;
using Castle.MicroKernel;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal class LazyComponentFacility: ComponentFacility
	{
		protected override void OnStartup()
		{
			Kernel.ComponentModelCreated += Kernel_ComponentModelCreated;
			Kernel.ComponentRegistered += Kernel_ComponentRegistered;
		}

		private void Kernel_ComponentRegistered(string key, IHandler handler)
		{
		}

		private void Kernel_ComponentModelCreated(ComponentModel model)
		{
		}
	}
}