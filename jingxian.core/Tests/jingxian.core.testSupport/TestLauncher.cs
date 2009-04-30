using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.testSupport
{
    using jingxian.core.runtime;

	[ExtensionAttribute(
        "jingxian.core.runtime.simpl.serviceLauncher", Constants.Bundles.Internal,
		Constants.Points.Applications,
		typeof(TestLauncher),
		Name = TestLauncher.OriginalName,
		Description = "")]
	internal sealed class TestLauncher: IApplicationLaunchable
	{
		public const string OriginalName = "Test Launcher";

        IExtensionRegistry _extensionRegistry;

        public TestLauncher(IKernel kernel, IExtensionRegistry extensionRegistry)
        {
            _extensionRegistry = extensionRegistry;
        }

        public int Launch(IApplicationContext context)
        {

            foreach( IExtension extension in _extensionRegistry.GetExtensions("jingxian.core.testSupport.autoTests") )
            {
                if (extension.Point == Constants.Points.XmlSchemas)
                    continue;

                Console.WriteLine( "创建对象 - {0}" , extension.Id );
                extension.BuildTransient<object>();
            }

            return 0;
        }
    }
}