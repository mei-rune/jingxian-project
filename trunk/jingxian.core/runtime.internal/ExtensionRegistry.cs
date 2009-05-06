

using System;
using System.Collections.Generic;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	[Service(
		typeof(IExtensionRegistry), typeof(ExtensionRegistry),
		RuntimeConstants.ExtensionRegistryId,
		Constants.Bundles.Internal,
		Name = ExtensionRegistry.OriginalName)]
	internal sealed partial class ExtensionRegistry: Service, IExtensionRegistry
	{
		public const string OriginalName = "Extension Registry";

        private readonly IApplicationContext _context;
		private readonly IBundleService _bundleService;
		private readonly Serializable _serializable;
		private readonly Dictionary<string, IExtension> _extensions = new Dictionary<string, IExtension>();
		private readonly Dictionary<string, IExtensionPoint> _extensionPoints = new Dictionary<string, IExtensionPoint>();
		private readonly ExtensionBuilder _extensionBuilder;



		public ExtensionRegistry(IBundleService bundleService
            , IAssemblyLoaderService assemblyLoaderService
            , IObjectBuilder builderService
            , IApplicationContext context)
		{
			_bundleService = bundleService;
            _context = context;
			_extensionBuilder =
				new ExtensionBuilder(assemblyLoaderService, bundleService, this, builderService);
			_serializable = new Serializable();
			_serializable.Registry = this;
			_serializable.ExtensionBuilder = _extensionBuilder;
		}


        private void Scan()
        {
            _logger.Debug("开始搜索扩展和扩展点...");

            foreach (IBundle bundle in _bundleService.Bundles)
            {
                _logger.DebugFormat("   从程序序集 '{0}' 中搜索 id 为 '{1}' 名为  '{2}' 的Bundle.", bundle.AssemblyLocation, bundle.Id, bundle.Name);


                foreach (IExtensionConfiguration extCfg in bundle.ContributedExtensions)
                {
                    IExtension ext = new Extension(extCfg, _extensionBuilder);
                    IExtension old = null;
                    if (_extensions.TryGetValue(ext.Id, out old))
                    {
                        throw new InvalidOperationException(string.Format(
                            "扩展Id重复,'{0}' 已存在于 '{1}'中,在 '{2}' 又发现了.", ext.Id, old.BundleId, ext.BundleId));
                    }
                    else
                    {
                        _extensions[ext.Id] = ext;
                        _logger.DebugFormat("     添加扩展 id='{0}'  name='{1}' .", ext.Id, ext.Name);
                    }
                }
                foreach (IExtensionPointConfiguration pointCfg in bundle.ContributedExtensionPoints)
                {
                    IExtensionPoint point = new ExtensionPoint(pointCfg);
                    IExtensionPoint old = null;
                    if (_extensionPoints.TryGetValue(point.Id, out old))
                    {
                        throw new InvalidOperationException(string.Format(
                            "扩展点Id重复,'{0}' 已存在于 '{1}'中,在 '{2}' 又发现了.", point.Id, old.BundleId, point.BundleId));
                    }
                    else
                    {
                        _extensionPoints[point.Id] = point;

                        _logger.DebugFormat("     添加扩展点 id='{0}'  name='{1}' .", point.Id, point.Name);
                    }
                }
            }

            foreach (IExtension ext in _extensions.Values)
            {
                string point = ext.Point;
                if (string.IsNullOrEmpty(point))
                {
                    _logger.WarnFormat("扩展[ id='{0}'  name='{1}']的扩展点属性是空白的.", ext.Id, ext.Name);
                    continue;
                }

                IExtensionPoint ep;
                if (_extensionPoints.TryGetValue(point, out ep))
                {
                    ((ExtensionPoint)ep).AddExtension(ext);
                }
                else
                {
                    _logger.WarnFormat("没有为扩展[ id='{0}'  name='{1}']找到扩展点 '{2}'.", ext.Id, ext.Name, ext.Point);
                }
            }

            _logger.Debug("搜索扩展和扩展点完成." + Environment.NewLine + Environment.NewLine + ToString());
        }


        protected override void internalStart()
        {
            base.internalStart();

            Scan();
        }

		public IExtension GetExtension(string extensionId)
		{
			Enforce.ArgumentNotNullOrEmpty(extensionId, "extensionId");

            IExtension result;
            if (_extensions.TryGetValue(extensionId, out result))
                return result;
            
            return null;
		}

		public IExtensionPoint GetExtensionPoint(string extensionPointId)
        {
            Enforce.ArgumentNotNullOrEmpty(extensionPointId, "extensionPointId");

            IExtensionPoint result;
            if (_extensionPoints.TryGetValue(extensionPointId, out result))
                return result;

			return null;
		}

		public IExtension[] GetExtensions(string pointId)
		{
            Enforce.ArgumentNotNullOrEmpty(pointId, "pointId");

			IExtension[] result;
			IExtensionPoint ep;
            if (_extensionPoints.TryGetValue(pointId, out ep))
            {
                result = ep.Extensions;
            }
            else
            {
                result = new IExtension[] { };
            }

			return result;
		}

		public IEnumerable<IExtensionPoint> ExtensionPoints
		{
			get{	return _extensionPoints.Values;}
		}

		public bool TryGetExtension(string extensionId, out IExtension extension)
        {
            Enforce.ArgumentNotNullOrEmpty(extensionId, "extensionId");

			return _extensions.TryGetValue(extensionId, out extension);
		}

		public IExtensionConfiguration GetExtensionConfigurationElement(string id)
        {
            Enforce.ArgumentNotNullOrEmpty(id, "id");

			return _bundleService.GetExtensionConfigurationElement(id);
		}

		public IExtensionPointConfiguration GetExtensionPointConfigurationElement(string id)
        {
            Enforce.ArgumentNotNullOrEmpty(id, "id");

			return _bundleService.GetExtensionPointConfigurationElement(id);
		}

		public bool TryGetExtensionPointConfiguration(string extensionPointId, out IExtensionPointConfiguration pointConfiguration)
		{
			return _bundleService.TryGetExtensionPointConfiguration(extensionPointId, out pointConfiguration);
		}

		public override string ToString()
		{
			string info = string.Empty;
			info += "注册的扩展点: " + _extensionPoints.Count; 
			info += Environment.NewLine;
			info += "注册的扩展: " + _extensions.Count; 
			info += Environment.NewLine;
			info += "类: " + base.ToString(); 
			info += Environment.NewLine;

			return info;
		}

	}
}
