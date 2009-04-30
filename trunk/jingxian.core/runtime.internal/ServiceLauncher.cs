

using System;

namespace jingxian.core.runtime.simpl
{
	[ExtensionAttribute(
        "jingxian.core.runtime.simpl.serviceLauncher", Constants.Bundles.Internal,
		Constants.Points.Applications,
		typeof(ServiceLauncher),
		Name = ServiceLauncher.OriginalName,
		Description = "")]
	internal sealed class ServiceLauncher: IApplicationLaunchable
	{
		public const string OriginalName = "Service Launcher"; 

        public int Launch(IApplicationContext context)
        {
            return 0;
        }
    }
}