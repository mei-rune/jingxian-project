

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	internal sealed class ExtensionBuilder: IExtensionBuilder
	{
        static logging.ILog _logger = logging.LogUtils.GetLogger(typeof(ExtensionBuilder));

		private readonly IAssemblyLoaderService _assemblyLoaderService;
		private readonly IBundleService _bundleService;
		private readonly IExtensionRegistry _registry;
		private readonly IObjectBuilder _objectBuilderService;

		public ExtensionBuilder(IAssemblyLoaderService assemblyLoader
            , IBundleService bundleService
            , IExtensionRegistry registry
            , IObjectBuilder objectBuilderService)
		{
			_assemblyLoaderService = assemblyLoader;
			_bundleService = bundleService;
			_registry = registry;
			_objectBuilderService = objectBuilderService;
		}


		#region IExtensionBuilder Members


		public T BuildTransient<T>(IExtension extension)
		{
			T implementation = (T)_objectBuilderService.BuildTransient(extension.Implementation);

			IExtensionAware awareImplementation = implementation as IExtensionAware;
			if (awareImplementation != null)
				awareImplementation.Configure(extension);

			return implementation;
		}


		#endregion
	}
}
