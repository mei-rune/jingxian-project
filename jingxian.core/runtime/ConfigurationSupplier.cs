

//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Diagnostics;
//using System.IO;
//using System.Xml;
//using System.Xml.Schema;
//using System.Xml.Serialization;

//namespace jingxian.core.runtime
//{
//    using jingxian.logging;
//    using jingxian.core.runtime.utilities;

//    public sealed class ConfigurationSupplier<T> where T : IXmlSerializable, new()
//    {
//        private readonly IExtensionRegistry _extensionRegistry;
//        private readonly IAssemblyLoaderService _assemblyLoaderService;
//        private readonly IBundleService _bundleService;


//        private IDictionary<string, T> _configurations;




//        public ConfigurationSupplier(IExtensionRegistry extensionRegistry
//            , IAssemblyLoaderService assemblyLoader
//            , IBundleService bundleService )
//        {
//            _extensionRegistry = Enforce.ArgumentNotNull<IExtensionRegistry>(extensionRegistry, "extensionRegistry");
//            _assemblyLoaderService = assemblyLoader;
//            _bundleService = bundleService;
//        }

//        public IDictionary<string, T> Configurations
//        {
//            get
//            {
//                if (_configurations == null)
//                    throw new InvalidOperationException("必须先调用 'Fetch' 方法");
//                return _configurations;
//            }
//        }

//        public IDictionary<string, T> Fetch(string pointId)
//        {
//            if (_logger.IsDebugEnabled)
//                _logger.DebugFormat("取扩展点{0} 的 contributions ...", pointId);


//            _configurations = new Dictionary<string, T>();
//            IExtension[] extensions = _extensionRegistry.GetExtensions(pointId);

//            if (extensions.Length == 0)
//            {
//                _logger.WarnFormat("没有找到扩展点'{0}'的 contributions.", pointId);
//                _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
//                    , pointId
//                    , _configurations.Count);
//                return _configurations;
//            }
//            foreach (IExtension ext in extensions)
//            {
//                if (!ext.HasConfiguration)
//                {
//                    _logger.WarnFormat("扩展 '{0}' 没有配置.",
//                            ext.Id);

//                    continue;
//                }

//                T configuration = ext.GetConfiguration<T>();

//                if (null == configuration)
//                {
//                    _logger.WarnFormat("扩展 '{0}' 没有有效的配置", ext.Id);
//                    continue;
//                }

//                if (Equals(configuration, default(T)))
//                    throw new PlatformConfigurationException(
//                        string.Format("扩展 '{0}' 有一个无效的配置.", ext.Id));


//                Debug.Assert(!_configurations.ContainsKey(ext.Id),
//                    string.Format("检测到配置类 '{0}' 一个重复的 ID '{1}'.",
//                    configuration.GetType(), ext.Id));

//                _configurations.Add(ext.Id, configuration);
//                _logger.DebugFormat("     + 添加一个id为 '{0}' 的 Contribution.", ext.Id);
//            }

//            _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
//                , pointId
//                , _configurations.Count);

//            return _configurations;
//        }

//        private ILog _log;

//        private ILog _logger
//        {
//            get
//            {
//                if (null == _log)
//                {
//                    string loggerName = string.Format(CultureInfo.InvariantCulture, "jingxian.core.runtime.configurationSupplier.{0}", typeof(T).Name);
//                    _log = LogUtils.GetLogger(loggerName);
//                }
//                return _log;
//            }
//        }
//    }
//}