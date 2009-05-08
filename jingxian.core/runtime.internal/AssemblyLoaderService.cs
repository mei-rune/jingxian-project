

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	[Service(AssemblyLoaderService.OriginalName,
		typeof(IAssemblyLoaderService) )]
	internal sealed class AssemblyLoaderService: Service, IAssemblyLoaderService
	{
		public const string OriginalName = "Assembly Loader Service";

		private readonly IApplicationContext _runtimeCfg;
		private Dictionary<object, AppDomain> _loadedAppDomains;

		private Dictionary<string, Assembly> _loadedAssemblies;

		public AssemblyLoaderService(IApplicationContext runtimeCfg)
		{
			_runtimeCfg = runtimeCfg;
		}
		private Dictionary<object, AppDomain> LoadedAppDomains
		{
			get
			{
				if (_loadedAppDomains == null)
					_loadedAppDomains = new Dictionary<object, AppDomain>();

				return _loadedAppDomains;
			}
		}

        private Dictionary<string, Assembly> LoadedAssemblies
        {
            get
            {
                if (_loadedAssemblies == null)
                    _loadedAssemblies = new Dictionary<string, Assembly>();

                return _loadedAssemblies;
            }
        }

		private static AppDomainSetup CreateDefaultAppDomainSetup()
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			setup.DynamicBase = AppDomain.CurrentDomain.DynamicDirectory;
			return setup;
        }

        #region CreateAppDomain 

        public T CreateAppDomain<T>() where T: MarshalByRefObject, new()
		{
			return CreateAppDomain<T>(typeof(T).FullName);
		}

		public T CreateAppDomain<T>(string friendlyName) where T: MarshalByRefObject, new()
		{
			return CreateAppDomain<T>(friendlyName, CreateDefaultAppDomainSetup());
		}

		public T CreateAppDomain<T>(string friendlyName, AppDomainSetup setup) where T: MarshalByRefObject, new()
		{
            Enforce.ArgumentNotNull<AppDomainSetup>(setup, "setup");
            Enforce.ArgumentNotNullOrEmpty(friendlyName, "friendlyName");

			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);
			//evidence.AddAssembly("(some assembly)");
			//evidence.AddHost("(some host)");
 
			AppDomain appDomain = AppDomain.CreateDomain(friendlyName, evidence, setup);
			T proxyInstance = (T) appDomain.CreateInstanceFromAndUnwrap(typeof(T).Assembly.Location, typeof(T).FullName);
			LoadedAppDomains.Add(proxyInstance, appDomain);

			return proxyInstance;
        }

        #endregion

        /// <summary>
        /// 根据应用程序域的名字查找应用程序域
        /// </summary>
        public AppDomain GetAppDomain(string friendlyName)
		{
            Enforce.ArgumentNotNullOrEmpty(friendlyName, "friendlyName");

			foreach (KeyValuePair<object, AppDomain> pair in LoadedAppDomains)
			{
				if (pair.Value.FriendlyName == friendlyName)
				{
					return pair.Value;
				}
			}
			return null;
		}

		public AppDomain GetAppDomain(object proxy)
		{
            Enforce.ArgumentNotNull(proxy, "proxy");

            AppDomain result = null;

            LoadedAppDomains.TryGetValue(proxy, out result);

            return result;
		}

        public void UnloadAppDomain(AppDomain appDomain)
        {
            Enforce.ArgumentNotNull<AppDomain>(appDomain, "appDomain");

            if (!LoadedAppDomains.ContainsValue(appDomain))
                throw new CannotUnloadAppDomainException(
                    string.Format(CultureInfo.InvariantCulture,
                    "不能卸载应用程序域 '{0}' , 它没有被本服务管理!.", appDomain.FriendlyName));

            object proxyKey = null;
            foreach (KeyValuePair<object, AppDomain> pair in LoadedAppDomains)
            {
                if (pair.Value != appDomain)
                {
                    continue;
                }
                proxyKey = pair.Key;
                break;
            }
            LoadedAppDomains.Remove(proxyKey);
            AppDomain.Unload(appDomain);
        }

        public void UnloadAppDomain(string friendlyName)
        {
            Enforce.ArgumentNotNullOrEmpty(friendlyName,"friendlyName");


            AppDomain appDomainToUnload = null;
            object proxyKey = null;

            foreach (KeyValuePair<object, AppDomain> pair in LoadedAppDomains)
            {
                if (pair.Value.FriendlyName != friendlyName)
                {
                    continue;
                }
                appDomainToUnload = pair.Value;
                proxyKey = pair.Key;
                break;
            }

            if (appDomainToUnload == null || proxyKey == null)
                throw new CannotUnloadAppDomainException(
                    string.Format(CultureInfo.InvariantCulture,
                    "不能卸载应用程序域 '{0}' (没有找到它).", friendlyName));

            LoadedAppDomains.Remove(proxyKey);
            AppDomain.Unload(appDomainToUnload);
        }

		public void UnloadAppDomain(object proxy)
		{
            Enforce.ArgumentNotNull(proxy, "proxy");

			if (!LoadedAppDomains.ContainsKey(proxy))
				throw new CannotUnloadAppDomainException(
					string.Format(CultureInfo.InvariantCulture,
					"不能为 '{0}' 卸载 AppDomain, 它不是本服务管理的.", proxy));

				AppDomain appDomain = LoadedAppDomains[proxy];
				LoadedAppDomains.Remove(proxy);
				AppDomain.Unload(appDomain);
        }

        public Assembly LoadAssembly(string simpleName)
        {
            simpleName = AssertIsSimpleName(Enforce.ArgumentNotNullOrEmpty(simpleName, "simpleName"));

            if (!_runtimeCfg.AvailableAssemblies.MeetsCriteria(simpleName))
                throw new AssemblyUnavailableException(simpleName);

            Assembly assembly = null;
            if (LoadedAssemblies.TryGetValue(simpleName, out assembly))
            {
                return assembly;
            }
            else
            {
                return DoLoadAssembly(simpleName);
            }
        }

        private string GetFileName(string simpleName)
        {
            string path = Path.Combine(_runtimeCfg.BundlePath, simpleName + ".dll");
            if (File.Exists(path))
                return path;
            path = Path.Combine(_runtimeCfg.BundlePath, simpleName + ".exe");
            if (File.Exists(path))
                return path;
            path = Path.Combine(_runtimeCfg.BundlePath, simpleName );
            if (File.Exists(path))
                return path;
            return null;
        }

        private Assembly DoLoadAssembly(string simpleName)
        {
            try
            {
                string path = GetFileName(simpleName);
                if (string.IsNullOrEmpty(path))
                {
                    string msg = string.Format(CultureInfo.InvariantCulture, "不能载入程序集 '{0}' - 没有找到程序集.", simpleName);
                    throw new RuntimeException(msg);
                }

                return Assembly.LoadFile(path);
            }
            catch (Exception exc)
            {
                string msg = string.Format(CultureInfo.InvariantCulture, "不能载入程序集 '{0}'.", simpleName);
                throw new RuntimeException(msg, exc);
            }
        }


		private static string AssertIsSimpleName(string simpleName)
		{
			if (simpleName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
			{
				simpleName = RemoveFileExtension(simpleName, ".dll");
			}
			else if (simpleName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) 
			{
				simpleName = RemoveFileExtension(simpleName, ".exe"); 
			}
			return simpleName;

			//return Path.GetFileNameWithoutExtension(simpleName);
		}

		private static string RemoveFileExtension(string simpleName, string ext)
		{
			return simpleName.Remove(simpleName.IndexOf(ext, StringComparison.OrdinalIgnoreCase));
		}

		public bool TryLoadAssembly(string name, out Assembly assembly)
		{
			assembly = null;
			try
			{
				assembly = LoadAssembly(name);
			}
			catch (Exception exc)
            {
                _logger.Warn(string.Concat("! 程序集 '", name, "' 不能载入."), exc);
			}
			return assembly != null;
		}

		public bool TryGetLoadedAssembly(string simpleName, out Assembly assembly)
		{
			Enforce.ArgumentNotNullOrEmpty(simpleName, "simpleName");

			return LoadedAssemblies.TryGetValue(simpleName, out assembly);
		}


        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            string simpleName = args.LoadedAssembly.GetName().Name;

            _logger.DebugFormat("程序集 '{0}' 已经载入.", simpleName);

            if (!LoadedAssemblies.ContainsKey(simpleName))
            {
                LoadedAssemblies.Add(simpleName, args.LoadedAssembly);
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            _logger.DebugFormat("程序集 '{0}' 没有找到.", args.Name);

            return null;
        }

		protected override void internalStart()
		{
			base.internalStart();
			AddAlreadyLoadedAssemblies();
			AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}


		private void AddAlreadyLoadedAssemblies()
		{
			Assembly[] alreadyLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < alreadyLoadedAssemblies.Length; i++)
			{
				string simpleName = alreadyLoadedAssemblies[i].GetName().Name;
				if (!LoadedAssemblies.ContainsKey(simpleName))
				{
					LoadedAssemblies.Add(simpleName, alreadyLoadedAssemblies[i]);
				}
			}
		}

		protected override void internalStop()
		{
			base.internalStop();
			//LoadedAppDomains.Clear(); 
			LoadedAssemblies.Clear();
			AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
			AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
		}

	}
}
