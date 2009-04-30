

using System;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;

using Castle.Core;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.SubSystems.Conversion;

namespace jingxian.core.runtime.castleIntegration
{
	[Serializable]
	internal class OptionalPropertiesDependenciesModelInspector: IContributeComponentModelConstruction
	{

		[NonSerialized]
		private IConversionManager _converter;

		protected IConversionManager Converter
		{
			get
			{
				return _converter;
			}
		}

        protected virtual void ProcessModel(Castle.MicroKernel.IKernel kernel, ComponentModel model)
		{
			if (_converter == null)
			{
				_converter = (IConversionManager) kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
			}

			InspectProperties(model);
		}

		protected virtual void InspectProperties(ComponentModel model)
		{
			if (model.InspectionBehavior == PropertiesInspectionBehavior.Undefined)
			{
				model.InspectionBehavior = GetInspectionBehaviorFromConfiguration(model.Configuration);
			}

			if (model.InspectionBehavior == PropertiesInspectionBehavior.None)
			{
				// Nothing to be inspected
				return;
			}

			BindingFlags bindingFlags;

			if (model.InspectionBehavior == PropertiesInspectionBehavior.DeclaredOnly)
			{
				bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			}
			else // if (model.InspectionBehavior == PropertiesInspectionBehavior.All) or Undefined
			{
				bindingFlags = BindingFlags.Public | BindingFlags.Instance;
			}

			Type targetType = model.Implementation;

			PropertyInfo[] properties = targetType.GetProperties(bindingFlags);

			foreach (PropertyInfo property in properties)
			{
				if (!property.CanWrite)
				{
					continue;
				}

				ParameterInfo[] indexerParams = property.GetIndexParameters();
				Trace.Assert(indexerParams != null);

				if (indexerParams.Length != 0)
				{
					continue;
				}

				if (property.IsDefined(typeof(DoNotWireAttribute), true))
				{
					continue;
				}

				DependencyModel dependency;

				Type propertyType = property.PropertyType;

				// All these dependencies are simple guesses
				// So we make them optional (the 'true' parameter below)

				if (Converter.IsSupportedAndPrimitiveType(propertyType))
				{
					dependency = new DependencyModel(DependencyType.Parameter, property.Name, propertyType, true);
				}
				else if (propertyType.IsInterface || propertyType.IsClass)
				{
					// The default Castle behavior makes these properties optional.
					// For the Platform, we only want this to happen, if the OptionalDependencyAttribute is defined.
					if (property.IsDefined(typeof(OptionalDependencyAttribute), true))
					{
						dependency = new DependencyModel(DependencyType.Service, property.Name, propertyType, true);
					}
					else
					{
						continue;
					}
				}
				else
				{
					// What is it?!
					// Awkward type, probably.

					continue;
				}

				model.Properties.Add(new PropertySet(property, dependency));
			}
		}

		private static PropertiesInspectionBehavior GetInspectionBehaviorFromConfiguration(IConfiguration config)
		{
			if (config == null || config.Attributes["inspectionBehavior"] == null)
			{
				return PropertiesInspectionBehavior.All;
			}

			string enumStringValue = config.Attributes["inspectionBehavior"];

			try
			{
				return (PropertiesInspectionBehavior)
							 Enum.Parse(typeof(PropertiesInspectionBehavior), enumStringValue, true);
			}
			catch (Exception)
			{
				string[] enumNames = Enum.GetNames(typeof(PropertiesInspectionBehavior));

				string message = string.Format( "Error on properties inspection. " + "Could not convert the inspectionBehavior attribute value into an expected enum value. " + "Value found is '{0}' while possible values are '{1}'", enumStringValue, string.Join(",", enumNames));

				throw new KernelException(message);
			}
		}

        void IContributeComponentModelConstruction.ProcessModel(Castle.MicroKernel.IKernel kernel, ComponentModel model)
		{
			ProcessModel(kernel, model);
		}

	}
}