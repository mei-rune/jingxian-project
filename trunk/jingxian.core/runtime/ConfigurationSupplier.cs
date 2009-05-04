

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using System.Diagnostics;

namespace jingxian.core.runtime
{
    using jingxian.logging;

	public sealed class ConfigurationSupplier<T> where T: IConfigurationElement, IXmlSerializable, new()
	{
		private readonly IExtensionRegistry _extensionRegistry;
		private bool _allowElementsWithoutId;
		private IList<T> _elementsWithoutId;
        private bool _ensureUniqueId = true;
        private IDictionary<string, T> _configurations;
		private IList<T> _nonUniqueElements;

        public ConfigurationSupplier(IExtensionRegistry extensionRegistry)
        {
            _extensionRegistry = Enforce.ArgumentNotNull<IExtensionRegistry>(extensionRegistry, "extensionRegistry");
        }


        public bool AllowElementsWithoutId
        {
            get { return _allowElementsWithoutId; }
            set { _allowElementsWithoutId = value; }
        }

		public IList<T> ElementsWithoutId
		{
			get
			{
				if (_elementsWithoutId == null)
					_elementsWithoutId = new List<T>();

				return _elementsWithoutId;
			}
		}

		public bool EnsureUniqueId
		{
            get { return _ensureUniqueId; }
            set { _ensureUniqueId = value; }
		}


        public IDictionary<string, T> Configurations
		{
			get
			{
				if (_configurations == null)
					throw new InvalidOperationException("必须先调用 'Fetch' 方法");
				return _configurations;
			}
		}


		public IList<T> NonUniqueElements
		{
			get
			{
				if (_nonUniqueElements == null)
					_nonUniqueElements = new List<T>();

				return _nonUniqueElements;
			}
		}

        public IDictionary<string, T> Fetch(string pointId)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat("取扩展点{0} 的 contributions ...", pointId);


            _configurations = new Dictionary<string, T>();
            IExtension[] extensions = _extensionRegistry.GetExtensions(pointId);

            if (extensions.Length == 0)
            {
                _logger.WarnFormat("没有找到扩展点'{0}'的 contributions.", pointId);
                _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
                    , pointId
                    , _configurations.Count + NonUniqueElements.Count + ElementsWithoutId.Count);
                return _configurations;
            }
            foreach (IExtension ext in extensions)
            {
                if (!ext.HasConfiguration)
                {
                    _logger.WarnFormat("扩展 '{0}' 没有配置.",
                            ext.Id);

                    continue;
                }

                T[] configurations = ext.BuildConfigurationsFromXml<T>();
                _logger.DebugFormat("处理扩展 '{0}' 的 {1}个配置 ...", ext.Id, configurations.Length);

                if (configurations.Length != 0)
                {
                    _logger.WarnFormat("扩展 '{0}' 没有有效的配置", ext.Id);
                    continue;
                }

                foreach (T cfgElement in configurations)
                {
                    if (Equals(cfgElement, default(T)))
                        throw new PlatformConfigurationException(
                            string.Format("扩展 '{0}' 有一个无效的配置.", ext.Id));

                    if (!EnsureUniqueId)
                    {
                        NonUniqueElements.Add(cfgElement);

                        _logger.DebugFormat("     + 增加一个id不是唯一的 contribution .");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(cfgElement.Id))
                    {
                        Debug.Assert(!_configurations.ContainsKey(cfgElement.Id),
                            string.Format("检测到配置类 '{0}' 一个重复的 ID '{1}'.",
                            cfgElement.GetType(), cfgElement.Id));

                        _configurations.Add(cfgElement.Id, cfgElement);
                        _logger.DebugFormat("     + 添加一个id为 '{0}' 的 Contribution.", cfgElement.Id);
                    }
                    else if (!AllowElementsWithoutId)
                    {
                        string msg = string.Format("配置类 '{0}' 有一个没有 Id 的 实例.", cfgElement.GetType());
                        throw new InvalidOperationException(msg);
                    }
                    else
                    {
                        ElementsWithoutId.Add(cfgElement);
                        _logger.DebugFormat("     + 添加一个没有id 的 Contribution.");
                    }
                }

            }

            _logger.DebugFormat("搜索扩展点'{0}'的 contributions 结束，共找到 {1} 个."
                , pointId
                , _configurations.Count + NonUniqueElements.Count + ElementsWithoutId.Count);

            return _configurations;
        }

		private ILog _Log;

		private ILog _logger
		{
			get
			{
				if (_Log == null)
				{
					string loggerName = string.Format(CultureInfo.InvariantCulture, "jingxian.core.runtime.configurationSupplier.{0}", typeof(T).Name);
					_Log = LogUtils.GetLogger(loggerName);
				}
				return _Log;
			}
		}
	}
}