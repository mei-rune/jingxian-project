

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using jingxian.core.runtime.Filters;
using jingxian.core.runtime.simpl.Resources;

namespace jingxian.core.runtime.simpl
{
	public class BundleScanner: MarshalByRefObject
	{
        static jingxian.logging.ILog _logger = jingxian.logging.LogUtils.GetLogger(typeof(BundleScanner));

		private readonly Dictionary<string, IBundle> _bundleById = new Dictionary<string, IBundle>();

        /// <summary>
        /// 从指定目录中搜索所有exe或dll文件，载入文件，找出所有以'Bundle.xml'结尾的资源文件,创建Bundle 包
        /// </summary>
        /// <param name="directory">指定的目录</param>
        /// <param name="availableAssemblies">过滤器列表</param>
        public IBundle[] ScanForBundles(string directory, IFilter<string> availableAssemblies)
        {
            if (!Directory.Exists(directory))
            {
                _logger.WarnFormat("Bundle 路径 '{0}' 不存在.", directory);

                return new IBundle[0];
            }
            List<string> validBundleFilenames = FilterBundleDirFilenames(
                GetFilesFromBundleDir(directory), availableAssemblies);

            ReadBundlesFromFiles(validBundleFilenames);
            IBundle[] bundles = new IBundle[_bundleById.Count];
            _bundleById.Values.CopyTo(bundles, 0);
            return bundles;
        }

		private static List<string> GetFilesFromBundleDir(string directory)
		{
			string[] dlls = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories); 
			string[] exes = Directory.GetFiles(directory, "*.exe", SearchOption.AllDirectories); 
			List<string> files = new List<string>();
			files.AddRange(dlls);
			files.AddRange(exes);
			return files;
		}

		private static List<string> FilterBundleDirFilenames(IList<string> bundleDirFilenames, IFilter<string> availableAssemblies)
		{
			List<string> validBundleFilenames = new List<string>();
			for (int i = 0; i < bundleDirFilenames.Count; i++)
			{
				string simpleFilename = Path.GetFileNameWithoutExtension(bundleDirFilenames[i]);
				if (availableAssemblies.MeetsCriteria(simpleFilename))
				{
					validBundleFilenames.Add(bundleDirFilenames[i]);
				}
			}

			return validBundleFilenames;
		}

		private void ReadBundlesFromFiles(IList<string> bundleDirFiles)
		{
			for (int i = 0; i < bundleDirFiles.Count; i++)
			{
				ScanFile(bundleDirFiles[i]);
			}
		}

        private void ScanFile(string fn)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(fn);
                ScanAssembly(asm);
            }
            catch (BadImageFormatException exc)
            {
                _logger.Debug(string.Format("跳过 '{0}' - {1}", fn, exc.Message), exc);
            }
        }

        private void ScanAssembly(Assembly asm)
        {
            string[] resourceNames = asm.GetManifestResourceNames();
            bool notFound = true;
            foreach (string resourceName in resourceNames)
            {
                if (resourceName.EndsWith(RuntimeConstants.BundleConfigurationFileName, StringComparison.OrdinalIgnoreCase))
                {
                    Bundle bundle;
                    if (SafeCreateBundleFromResource(asm, resourceName, out bundle))
                    {
                        if (_bundleById.ContainsKey(bundle.Id))
                        {
                            IBundle duplicate = _bundleById[bundle.Id];

                            _logger.WarnFormat(Messages.BundleSkippedDueToDuplicateId, bundle.Name, bundle.Id, bundle.AssemblyLocation, duplicate.Name, duplicate.AssemblyLocation);
                            continue;
                        }
                        else
                        {
                            _bundleById.Add(bundle.Id, bundle);
                            notFound &= false;
                            continue;
                        }
                    }
                }
                else
                {
                    notFound &= true;
                }
            }
            if (notFound)
            {
                _logger.DebugFormat(Messages.AssemblyContainsNoBundleDefinition, asm.GetName().Name);
            }
        }

        private static bool SafeCreateBundleFromResource(Assembly asm, string resourceName, out Bundle bundle)
        {
            try
            {
                bundle = Bundle.CreateFromManifestResource(asm, resourceName);
                return true;
            }
            catch (InvalidOperationException exc) /// @todo improve (DRY!)
            {
                _logger.DebugFormat("从资源[name='{0}',assemblyPath='{1}' ]中载入 bundle 失败.", resourceName,
                                asm.Location);
                _logger.Debug("", exc);
                bundle = null;
                return false;
            }
            catch (PlatformConfigurationException exc)
            {
                _logger.DebugFormat("从资源[name='{0}',assemblyPath='{1}' ]中载入 bundle 失败.", resourceName,
                                asm.Location);
                _logger.Debug("", exc);
                bundle = null;
                return false;
            }
            catch (ReflectionTypeLoadException exc)
            {
                _logger.DebugFormat("从资源[name='{0}',assemblyPath='{1}' ]中载入 bundle 失败.", resourceName,
                                asm.Location);
                _logger.Debug("", exc);
                bundle = null;
                return false;
            }
        }

	}
}