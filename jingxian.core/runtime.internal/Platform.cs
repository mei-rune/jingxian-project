

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Globalization;
using System.Threading;


namespace jingxian.core.runtime.simpl
{
    //using jingxian.core.runtime.castleIntegration;
    using jingxian.core.runtime.simpl.Resources;

    public static class Platform
    {
        private static logging.ILog _logger = logging.LogUtils.GetLogger(typeof(Platform));


        private static readonly PredefinedService[] _predefinedServices = new PredefinedService[]
			{
				new PredefinedService(RuntimeConstants.AssemblyLoaderServiceId, typeof (IAssemblyLoaderService), typeof (AssemblyLoaderService)),
				new PredefinedService(RuntimeConstants.BundleServiceId, typeof (IBundleService), typeof (BundleService)),
				new PredefinedService(RuntimeConstants.ExtensionRegistryId, typeof (IExtensionRegistry), typeof (ExtensionRegistry)),
			    //new PredefinedService(RuntimeConstants.ServiceRegistryId, typeof (IServiceRegistry), typeof (ServiceRegistry)),
			};

        internal static PredefinedService[] PredefinedServices
        {
            get { return _predefinedServices; }
        }


        public static IApplicationLaunchable BuildApplicationLaunchable(IApplicationContext context, IExtensionRegistry registry, IObjectBuilder builder)
        {
            IExtension launchableExtension;
            if (!registry.TryGetExtension(context.ApplicationLaunchableId, out launchableExtension))
            {
                string msg = string.Format(CultureInfo.InvariantCulture, "启动 {0} 时发生异常 (没有找到扩展 {1} ).",
                        context.Name, context.ApplicationLaunchableId);
                _logger.Error(msg);
                throw new PlatformConfigurationException(msg);
            }
            IApplicationLaunchable launchable =
                builder.BuildTransient(launchableExtension.Implementation) as IApplicationLaunchable;

            return launchable;
        }

        public static int Launch(IApplicationContext context, ICommandLineArguments arguments)
        {
            int exitCode = 1;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Enforce.ArgumentNotNull<IApplicationContext>( context,"context");
            Enforce.ArgumentNotNullOrEmpty(context.ApplicationLaunchableId,"context.ApplicationLaunchableId");

            _logger.Debug("开始启动...");

            try
            {
                using (KernelAdapter containerAdapter = new KernelAdapter())
                {
                    containerAdapter.Connect( typeof(IApplicationContext), context );

                    foreach (PredefinedService predefinedService in PredefinedServices)
                    {
                        containerAdapter.Connect(predefinedService.Id, predefinedService.Service, predefinedService.Implementation);
                    }

                    containerAdapter.Start();

                    IExtensionRegistry registry = containerAdapter.Get <IExtensionRegistry>();
                    IObjectBuilder builder = containerAdapter.Get<IObjectBuilder>();


                    IApplicationLaunchable launchable = BuildApplicationLaunchable(context, registry, builder);
                    exitCode = launchable.Launch(context);

                    containerAdapter.Stop();
                }

            }
            finally
            {

                const string exitMsg = "退出代码 {0}.";
                _logger.InfoFormat(exitMsg, exitCode);
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            }
            return exitCode;
        }


        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.ErrorFormat("发生未抓住的异常  '{0}' .", e.ExceptionObject.GetType());
            _logger.ErrorFormat("	引发者: '{0}'", sender);
            _logger.ErrorFormat("	程序现在 {0}.", e.IsTerminating ? "中断" : "继续运行");
            _logger.Error(e.ExceptionObject);
        }

    }
}