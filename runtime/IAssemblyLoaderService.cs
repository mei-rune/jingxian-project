
using System;
using System.Reflection;

namespace jingxian.core.runtime
{
	public interface IAssemblyLoaderService
	{
		Assembly LoadAssembly(string simpleName);

		bool TryLoadAssembly(string simpleName, out Assembly assembly);

		bool TryGetLoadedAssembly(string simpleName, out Assembly assembly);

		T CreateAppDomain<T>() where T: MarshalByRefObject, new();

		T CreateAppDomain<T>(string friendlyName) where T: MarshalByRefObject, new();

		T CreateAppDomain<T>(string friendlyName, AppDomainSetup setup) where T: MarshalByRefObject, new();

		AppDomain GetAppDomain(string friendlyName);

		AppDomain GetAppDomain(object proxy);

		void UnloadAppDomain(AppDomain appDomain);

		void UnloadAppDomain(string friendlyName);

		void UnloadAppDomain(object proxy);
	}
}