

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
    /// <summary>
    /// 初始化组件的回调钩子
    /// 初始化castle时当遇到IExtensionRegistry服务时，从IExtensionRegistry取出
    /// 所有jingxian.core.runtime.components扩展点的扩展，注册到 castle容器中
    /// </summary>

	public class ComponentFacility: AbstractFacility
	{
		public const string ComponentTypeId = "jingxian.core.runtime.componentFacility"; 

		private bool _started;

        private IExtensionRegistry _registry;
        private IDictionary<string, ComponentConfiguration> _componentsById;
        private IDictionary<string, ComponentConfiguration> _componentsByTypeName =
            new Dictionary<string, ComponentConfiguration>();

		protected IExtensionRegistry Registry
		{
            get { return _registry; }
		}

		public IDictionary<string, ComponentConfiguration> ComponentsById
		{
            get { return _componentsById; }
		}


		public IDictionary<string, ComponentConfiguration> ComponentsByTypeName
		{
            get { return _componentsByTypeName; }
		}

		protected override void Init()
		{
			Kernel.ComponentCreated += Kernel_ComponentCreated;
		}

		private void Kernel_ComponentCreated(ComponentModel model, object instance)
		{
			if (model.Service == typeof(IExtensionRegistry))
			{
				Kernel.ComponentCreated -= Kernel_ComponentCreated;
				_registry = instance as IExtensionRegistry;
				InitRegistry();
				OnStartup();
				_started = true;
			}
		}

		protected virtual void OnStartup()
		{
			AddAllComponents();
		}

		private void AddAllComponents()
		{
			foreach (ComponentConfiguration cfg in _componentsByTypeName.Values)
			{
				Type interfaceType = Type.GetType(cfg.Interface, true, false);
				Type implementationType = Type.GetType(cfg.Implementation, true, false);
				Kernel.AddComponent(cfg.Id, interfaceType, implementationType);
			}
		}

		private void InitRegistry()
		{
			ConfigurationSupplier<ComponentConfiguration> cfgSupplier =
				new ConfigurationSupplier<ComponentConfiguration>(_registry);
			_componentsById = cfgSupplier.Fetch(Constants.Points.Components);
			foreach (ComponentConfiguration cfg in _componentsById.Values)
			{
				if (!_componentsByTypeName.ContainsKey(cfg.Interface))
				{
					_componentsByTypeName.Add(cfg.Interface, cfg);
				}
			}
		}
	}
}
