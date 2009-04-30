using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.activator
{
    public class ReflectionActivator : IComponentActivator
    {
        Type _componentType;
        readonly IEnumerable<Parameter> _additionalConstructorParameters ;
        readonly IEnumerable<NamedPropertyParameter> _explicitPropertySetters;
        //readonly IConstructorSelector _constructorSelector;
        //static readonly IConstructorInvoker DirectInvoker = new DirectConstructorInvoker();
        //IConstructorInvoker _constructorInvoker = DirectInvoker;
        static readonly IEnumerable<IParameter> AutowiringParameterArray = new IParameter[] { new AutowiringParameter() };
        static readonly MethodInfo InternalPreserveStackTraceMethod = typeof(Exception)
            .GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);


        public ReflectionActivator(Type componentType)
            : this(componentType, null)
        {
        }


        public ReflectionActivator(
            Type componentType,
            IEnumerable<IParameter> additionalConstructorParameters)
            : this(
                componentType,
                additionalConstructorParameters,
                null)
        {
        }


        public ReflectionActivator(
            Type componentType,
            IEnumerable<IParameter> additionalConstructorParameters,
            IEnumerable<NamedPropertyParameter> explicitPropertySetters)
            : this(
                componentType,
                additionalConstructorParameters,
                explicitPropertySetters,
                new MostParametersConstructorSelector())
        {
        }


        public ReflectionActivator(
            Type componentType,
            IEnumerable<IParameter> additionalConstructorParameters,
            IEnumerable<NamedPropertyParameter> explicitPropertySetters,
            IConstructorSelector constructorSelector)
        {
            Enforce.ArgumentNotNull(componentType, "componentType");
            Enforce.ArgumentNotNull(additionalConstructorParameters, "additionalConstructorParameters");
            Enforce.ArgumentNotNull(explicitPropertySetters, "explicitPropertySetters");
            Enforce.ArgumentNotNull(constructorSelector, "constructorSelector");

            _componentType = componentType;
            _constructorSelector = constructorSelector;
            _additionalConstructorParameters = additionalConstructorParameters.Concat(AutowiringParameterArray).ToArray();
            _explicitPropertySetters = explicitPropertySetters.ToArray();
        }


        public object ActivateInstance(ICreationContext context, IEnumerable<IParameter> parameters)
        {
            Enforce.ArgumentNotNull(context, "context");
            Enforce.ArgumentNotNull(parameters, "parameters");

            var possibleConstructors = new Dictionary<ConstructorInfo, Func<object>[]>();
            StringBuilder reasons = null;
            bool foundPublicConstructor = false;
            var augmentedParameters = parameters.Concat(_additionalConstructorParameters);

            foreach (ConstructorInfo ci in _componentType.FindMembers(
                MemberTypes.Constructor,
                BindingFlags.Instance | BindingFlags.Public,
                null,
                null))
            {
                foundPublicConstructor = true;

                Func<object>[] parameterAccessors;
                string reason;
                if (CanUseConstructor(ci, context, augmentedParameters, out parameterAccessors, out reason))
                {
                    possibleConstructors.Add(ci, parameterAccessors);
                }
                else
                {
                    reasons = reasons ?? new StringBuilder(reason.Length + 2);
                    reasons.AppendLine();
                    reasons.Append(reason);
                }
            }

            if (!foundPublicConstructor)
                throw new DependencyResolutionException(
                    string.Format(CultureInfo.CurrentCulture,
                        ReflectionActivatorResources.NoPublicConstructor, _componentType));

            if (possibleConstructors.Count == 0)
                throw new DependencyResolutionException(
                          string.Format(CultureInfo.CurrentCulture, ReflectionActivatorResources.NoResolvableConstructor, _componentType, reasons ?? new StringBuilder()));

            var selectedCI = _constructorSelector.SelectConstructor(possibleConstructors.Keys);

            var result = ConstructInstance(selectedCI, context, augmentedParameters, possibleConstructors[selectedCI]);

            SetExplicitProperties(result, context);

            return result;
        }

        /// <summary>
        /// A 'new context' is a scope that is self-contained
        /// and that can dispose the components it contains before the parent
        /// container is disposed. If the activator is stateless it should return
        /// true, otherwise false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can support a new context; otherwise, <c>false</c>.
        /// </value>
        public bool CanSupportNewContext
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The type that will be used to reflectively instantiate the component instances.
        /// </summary>
        /// <remarks>
        /// The actual implementation type may be substituted with a dynamically-generated subclass.
        /// Note that functionality that  relies on this feature will obviously not be available to provided instances or
        /// to delegate-created instances; interface-based AOP is recommended in these situations.
        /// </remarks>
        public Type ImplementationType
        {
            get
            {
                return _componentType;
            }
            set
            {
                _componentType = Enforce.ArgumentNotNull(value, "value");
            }
        }

        /// <summary>
        /// Gets or sets the constructor invoker.
        /// </summary>
        /// <value>The constructor invoker.</value>
        public IConstructorInvoker ConstructorInvoker
        {
            get
            {
                return _constructorInvoker;
            }
            set
            {
                _constructorInvoker = Enforce.ArgumentNotNull(value, "value");
            }
        }

        bool CanUseConstructor(ConstructorInfo ci, ICreationContext context, IEnumerable<IParameter> parameters, out Func<object>[] valueAccessors, out string reason)
        {
            Enforce.ArgumentNotNull(ci, "ci");
            Enforce.ArgumentNotNull(context, "context");
            Enforce.ArgumentNotNull(parameters, "parameters");

            StringBuilder reasonNotUsable = null;

            var ciParams = ci.GetParameters();
            var partialValueAccessors = new Func<object>[ciParams.Length];

            foreach (ParameterInfo pi in ciParams)
            {
                Func<object> va = null;

                foreach (var param in parameters)
                {
                    if (param.TryGetProvider(pi, context, out va))
                        break;
                }

                if (va != null)
                {
                    partialValueAccessors[pi.Position] = va;
                }
                else
                {
                    if (reasonNotUsable == null)
                    {
                        reasonNotUsable = new StringBuilder();
                        reasonNotUsable.Append(ci).Append(": ");
                    }
                    else
                    {
                        reasonNotUsable.Append(", ");
                    }

                    reasonNotUsable.AppendFormat(CultureInfo.CurrentCulture,
                        ReflectionActivatorResources.MissingParameter,
                        pi.Name, pi.ParameterType);
                }
            }

            if (reasonNotUsable != null)
            {
                valueAccessors = null;
                reason = reasonNotUsable.Append('.').ToString();
            }
            else
            {
                valueAccessors = partialValueAccessors;
                reason = String.Empty;
            }

            // Return true if there is no reason not to use it, i.e. reasonNotUsable is null
            return reasonNotUsable == null;
        }

        static Exception KeepTargetInvocationStack(TargetInvocationException ex)
        {
            InternalPreserveStackTraceMethod.Invoke(ex.InnerException, null);
            return ex.InnerException;
        }

        object ConstructInstance(ConstructorInfo ci, ICreationContext context, IEnumerable<IParameter> parameters, Func<object>[] parameterAccessors)
        {
            Enforce.ArgumentNotNull(ci, "ci");
            Enforce.ArgumentNotNull(parameterAccessors, "parameterAccessors");

            try
            {
                object instance = ConstructorInvoker.InvokeConstructor(context, parameters, ci, parameterAccessors);
                return instance;
            }
            catch (TargetInvocationException tie)
            {
                throw KeepTargetInvocationStack(tie);
            }
        }

        void SetExplicitProperties(object instance, ICreationContext context)
        {
            Enforce.ArgumentNotNull(instance, "instance");
            Enforce.ArgumentNotNull(context, "context");

            if (!_explicitPropertySetters.Any())
                return;

            Type instanceType = instance.GetType();

            // Rinat Abdullin: properties with signature like {private set;get;} pass the
            // BindingFlags.SetProperty but fail around "GetSetMethod()", since it returns null
            // for non-public properties
            var properties = instanceType.GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Select(p => new
                {
                    Info = p,
                    SetMethod = p.GetSetMethod()
                })
                .Where(p => p.SetMethod != null)
                .ToArray();

            foreach (var param in _explicitPropertySetters)
            {
                foreach (var propertyData in properties)
                {
                    Func<object> propertyValueAccessor;
                    if (param.CanSupplyValue(propertyData.SetMethod.GetParameters()[0], context, out propertyValueAccessor))
                    {
                        propertyData.Info.SetValue(instance, propertyValueAccessor(), null);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "ReflectionActivator";
        }
    }
}
