

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using jingxian.core.runtime.simpl.Resources;
using jingxian.core.utilities;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	[Service(
		typeof(IBundleService), typeof(BundleService),
		RuntimeConstants.BundleServiceId,
		Constants.Bundles.Internal,
		Name = BundleService.OriginalName)]
	internal sealed class BundleService: Service, IBundleService
	{
		public const string OriginalName = "Bundle Service";
		public const string BundleScannerAppDomainName = "Bundle Scanner AppDomain";

		private readonly string _bundlePath;
		private readonly IAssemblyLoaderService _assemblyLoaderService;
		private readonly IObjectBuilder _builder;
		private readonly Dictionary<string, IBundle> _bundleById = new Dictionary<string, IBundle>();
		private readonly Dictionary<string, IBundle> _bundleByExtensionPointId = new Dictionary<string, IBundle>();
		private readonly Dictionary<string, IBundle> _bundleByExtensionId = new Dictionary<string, IBundle>();

        private bool _initializedFromCache = false;
        private IApplicationContext _context;
#if CACHE
        //private ICacheService _CacheService;

        [OptionalDependency]
        public ICacheService CacheService
        {
            get
            {
                return _CacheService;
            }
            set
            {
                _CacheService = value;
            }
        }
#endif


        public BundleService(IApplicationContext context, IAssemblyLoaderService assemblyLoaderService, IObjectBuilder builder)
        {
            _bundlePath = context.BundlePath;
            _context = context;
            _assemblyLoaderService = assemblyLoaderService;
            _builder = builder;
        }

        protected override void internalStart()
        {
            base.internalStart();

            ClearBundles();

            if (!Directory.Exists(_bundlePath))
            {
                _logger.WarnFormat("Bundle 目录 '{0}' 不存在.", _bundlePath);
                return;
            }

            //if (CacheService == null)
            //{
            Scan();
            //}
            //else
            //{
            //    string[] bundleDirFilenames;
            //    Bundle[] bundles;
            //    DateTime readTimeOfBundleDirFilenames;
            //    if (
            //        CachedBundlesAreUpToDate(out bundleDirFilenames, out readTimeOfBundleDirFilenames)
            //        &&
            //        CacheService.TryXmlDeserializeWithXmlSerializer<Bundle[]>(BundlesCacheKey, out bundles)
            //        )
            //    {
            //        foreach (Bundle bundle in bundles)
            //        {
            //            AddBundle(bundle);
            //        }
            //        _initializedFromCache = true;
            //    }
            //    else
            //    {
            //        _initializedFromCache = false;
            //        DeleteBundleCache();
            //        Scan();
            //        UpdateBundleCache(bundleDirFilenames, readTimeOfBundleDirFilenames);
            //    }
            //}


            foreach (Bundle bundle in _bundleById.Values)
            {
                bundle.Activate(_builder);
            }
        }

		protected override void internalStop()
		{
			foreach (Bundle bundle in _bundleById.Values)
			{
				bundle.Deactivate();
			}
			base.internalStop();
		}

		#region Scan

		private void Scan()
		{
            if (_context.ScanForBundlesInSecondAppDomain)
			{
				ScanForBundlesInSecondAppDomain();
			}
			else
			{
				ScanForBundles();
			}
		}

        private void ScanForBundles()
        {
            _logger.Debug("开始搜索 bundle 包...");

            BundleScanner scanner = new BundleScanner();
            IBundle[] bundles = scanner.ScanForBundles(_bundlePath, _context.AvailableAssemblies);
            for (int i = 0; i < bundles.Length; i++)
            {
                AddBundle(bundles[i] as Bundle);
            }

            _logger.DebugFormat("搜索 bundle 包完成. 共 {0} 个.", _bundleById.Count);

        }

        private void ScanForBundlesInSecondAppDomain()
        {
            _logger.Debug("开始在第二个应用程序域中搜索 bundle 包...");

            BundleScanner scanner = _assemblyLoaderService.CreateAppDomain<BundleScanner>(BundleScannerAppDomainName);
            IBundle[] bundles = scanner.ScanForBundles(_bundlePath, _context.AvailableAssemblies);
            _assemblyLoaderService.UnloadAppDomain(BundleScannerAppDomainName);
            for (int i = 0; i < bundles.Length; i++)
            {
                AddBundle(bundles[i] as Bundle);
            }

            _logger.DebugFormat("在第二个应用程序域中搜索 bundle 包完成，共 {0}个.", _bundleById.Count);
        }

		private void ClearBundles()
		{
			_bundleById.Clear();
			_bundleByExtensionId.Clear();
			_bundleByExtensionPointId.Clear();
		}

        public void AddBundle(Bundle bundle)
        {
            _bundleById.Add(bundle.Id, bundle);

            foreach (IExtensionPointConfiguration pointCfg in bundle.ContributedExtensionPoints)
            {
                if (_bundleByExtensionPointId.ContainsKey(pointCfg.Id))
                {
                    IBundle duplicate = _bundleByExtensionPointId[pointCfg.Id];
                    string msg =
                        string.Format(CultureInfo.InvariantCulture, Messages.ExtensionPointSkippedDueToDuplicateId, pointCfg.Name, pointCfg.Id,
                                                    bundle.AssemblyLocation, duplicate.Name, duplicate.AssemblyLocation);
                    throw new PlatformConfigurationException(msg);
                }
                else
                {
                    _bundleByExtensionPointId.Add(pointCfg.Id, bundle);
                }
            }

            foreach (IExtensionConfiguration cfg in bundle.ContributedExtensions)
            {
                if (_bundleByExtensionId.ContainsKey(cfg.Id))
                {
                    IBundle duplicate = _bundleByExtensionId[cfg.Id];
                    string msg = string.Format(CultureInfo.InvariantCulture, Messages.ExtensionSkippedDueToDuplicateId, cfg.Name, cfg.Id, bundle.AssemblyLocation, duplicate.Name, duplicate.AssemblyLocation);
                    throw new PlatformConfigurationException(msg);
                }
                else
                {
                    _bundleByExtensionId.Add(cfg.Id, bundle);
                }
            }


            _logger.DebugFormat( Messages.BundleAddedInfo, bundle.Name, bundle.Id, bundle.AssemblyLocation);

        }

		#endregion

		#region cache 相关的常量

		private string BundlesCacheKey
		{
			get
			{
				return Utils.CreateCompositeId(ComponentId, "bundles"); 
			}
		}


		private string LastScannedFilesCacheKey
		{
			get
			{
				return Utils.CreateCompositeId(ComponentId, "scannedFiles"); 
			}
		}

		private string LastScanTimeCacheKey
		{
			get
			{
				return Utils.CreateCompositeId(ComponentId, "lastScan"); 
			}
		}

		#endregion

        #region IBundleService

        public bool InitializedFromCache
		{
            get { return _initializedFromCache; }
		}


		public IEnumerable<IBundle> Bundles
		{
            get { return _bundleById.Values; }
		}

		public IBundle GetBundle(string bundleId)
		{
			IBundle bundle;
			if (TryGetBundle(bundleId, out bundle))
				return bundle;

			return null;
		}

		public IExtensionPointConfiguration GetExtensionPointConfigurationElement(string extensionPointId)
		{
			IExtensionPointConfiguration cfg;
			if (TryGetExtensionPointConfiguration(extensionPointId, out cfg))
				return cfg;
			return null;
		}

		public bool TryGetExtensionPointConfiguration(string extensionPointId, out IExtensionPointConfiguration pointConfiguration)
		{
			if (string.IsNullOrEmpty(extensionPointId))
				throw new StringArgumentException("extensionPointId");

			IBundle bundle;
			if (_bundleByExtensionPointId.TryGetValue(extensionPointId, out bundle))
			{
				foreach (IExtensionPointConfiguration cfg in bundle.ContributedExtensionPoints)
				{
					if (cfg.Id == extensionPointId)
					{
						pointConfiguration = cfg;
						return true;
					}
				}
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expected extension point configuration element with bundleId {0} to be contained in bundle with bundleId {1}.", extensionPointId, bundle.Id));
			}
			pointConfiguration = null;
			return false;
		}


		public IExtensionConfiguration GetExtensionConfigurationElement(string extensionId)
		{
			IBundle bundle;
			if (_bundleByExtensionId.TryGetValue(extensionId, out bundle))
			{
				foreach (IExtensionConfiguration cfg in bundle.ContributedExtensions)
				{
					if (cfg.Id == extensionId)
						return cfg;
				}
			}
            return null;
		}


		public bool TryGetBundle(string bundleId, out IBundle bundle)
		{
			if (string.IsNullOrEmpty(bundleId))
				throw new StringArgumentException("bundleId");

			return _bundleById.TryGetValue(bundleId, out bundle);
		}

		public bool TryGetBundleForExtension(string extensionId, out IBundle bundle)
		{
			if (string.IsNullOrEmpty(extensionId))
				throw new StringArgumentException("extensionId");

			return _bundleByExtensionId.TryGetValue(extensionId, out bundle);
		}

		public bool TryGetBundleForExtensionPoint(string extensionPointId, out IBundle bundle)
		{
			if (string.IsNullOrEmpty(extensionPointId))
				throw new StringArgumentException("extensionPointId"); 

			return _bundleByExtensionPointId.TryGetValue(extensionPointId, out bundle);
		}

		#endregion

		#region ToString (override)

		public override string ToString()
		{
			string info = string.Empty;
#if CACHE
			info += "InitializedFromCache: " + _initializedFromCache;
			info += Environment.NewLine;
			info += "Optional Cache Service Dependency set: ";
			if (_CacheService != null)
			{
				info += "Yes";
			}
			else
			{
				info += "No";
			}
#endif
			info += Environment.NewLine;
			info += "Bundle Path: " + _bundlePath;
			info += Environment.NewLine;
			info += "Registered Bundles: " + _bundleById.Count;
			info += Environment.NewLine;
			info += "Registered Extension Points: " + _bundleByExtensionPointId.Count;
			info += Environment.NewLine;
			info += "Registered Extensions: " + _bundleByExtensionId.Count;
			info += Environment.NewLine;
			info += "Class Type: " + base.ToString();
			info += Environment.NewLine;

			return info;
		}

		#endregion
	}
}