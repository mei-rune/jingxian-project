

using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.LifecycleConcerns;
using Castle.MicroKernel.SubSystems.Conversion;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal sealed class TypeAwareFacility : AbstractFacility
	{
		private Type _componentType;
		private string _configurationAttributeName;
		private ITypeConverter _typeConverter;
		private ILifecycleConcern _componentCommissionConcern;


		public TypeAwareFacility(string modelPropertyName, Type componentType, ILifecycleConcern componentCommissionConcern)
		{
			_configurationAttributeName = modelPropertyName;
			_componentCommissionConcern = componentCommissionConcern;
			_componentType = componentType;
		}

		protected override void Init()
		{
			_typeConverter = (ITypeConverter) Kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
			Kernel.ComponentModelCreated += new ComponentModelDelegate(Kernel_ComponentModelCreated);
		}

		private void Kernel_ComponentModelCreated(ComponentModel model)
		{
			bool isAware = _componentType.IsAssignableFrom(model.Implementation) || HasConfigurationAttributeSet(model);
			model.ExtendedProperties[_configurationAttributeName] = isAware;
			if (isAware)
			{
				model.LifecycleSteps.Add(LifecycleStepType.Commission, _componentCommissionConcern);
			}
		}

		private bool HasConfigurationAttributeSet(ComponentModel model)
		{
			bool result = false;
			if (model.Configuration != null)
			{
				string attr = model.Configuration.Attributes[_configurationAttributeName];

				if (! string.IsNullOrEmpty(attr))
				{
					result = (bool) _typeConverter.PerformConversion(attr, typeof (bool));
				}
			}
			return result;
		}
	}
}