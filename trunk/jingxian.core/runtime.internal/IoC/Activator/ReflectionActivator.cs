using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.activator
{
    public class ReflectionActivator : IComponentActivator
    {
        static readonly IEnumerable<IParameter> AutowiringParameterArray = new IParameter[] { new AutowiringParameter() };
        static readonly MethodInfo InternalPreserveStackTraceMethod = typeof(Exception)
            .GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        Type _componentType;
        readonly IEnumerable<IParameter> _additionalConstructorParameters;
        readonly IEnumerable<NamedPropertyParameter> _explicitPropertySetters;
        readonly ConstructorInfo _constructorInfo;


        public ReflectionActivator(Type componentType)
            : this(componentType, null)
        {
        }


        public ReflectionActivator(
            Type componentType,
            IEnumerable<IParameter> additionalConstructorParameters)
            : this( componentType,
                additionalConstructorParameters,
                null)
        {
        }


        public ReflectionActivator(
            Type componentType,
            IEnumerable<IParameter> additionalConstructorParameters,
            IEnumerable<NamedPropertyParameter> explicitPropertySetters)
            : this( componentType,
                null,
                additionalConstructorParameters,
                explicitPropertySetters )
        {
        }


        public ReflectionActivator(
            Type componentType,
            ConstructorInfo constructorInfo,
            IEnumerable<IParameter> additionalConstructorParameters,
            IEnumerable<NamedPropertyParameter> explicitPropertySetters )
        {
            Enforce.ArgumentNotNull(componentType, "componentType");
            Enforce.ArgumentNotNull(additionalConstructorParameters, "additionalConstructorParameters");
            Enforce.ArgumentNotNull(explicitPropertySetters, "explicitPropertySetters");
            Enforce.ArgumentNotNull(constructorInfo, "constructorInfo");

            _componentType = componentType;
            _constructorInfo = constructorInfo;
            _additionalConstructorParameters = this.Concat(additionalConstructorParameters, AutowiringParameterArray);
            _explicitPropertySetters = explicitPropertySetters;
        }

        public IEnumerable<T> Concat<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            if (null == a)
                return b??new T[0];
            if (null == b)
                return a;
            List<T> result = new List<T>(a);
            result.AddRange(b);
            return result;
        }

        public ConstructorInfo GetConstructorInfo( ICreationContext context, IEnumerable<IParameter> parameters, out Func<object>[] valueProviders )
        {
            if (null == _constructorInfo)
            {
                ParameterInfo[] parameterInfos = _constructorInfo.GetParameters();
                valueProviders = new Func<object>[parameterInfos.Length];

                foreach (ParameterInfo parameterInfo in parameterInfos)
                {
                    Func<object> va = null;
                    foreach (IParameter param in parameters)
                    {
                        if (param.TryGetProvider(parameterInfo, context, out va))
                            break;
                    }
                    if (null == va)
                        throw new DependencyResolutionException(
                                  string.Format("类型 '{0}' 没有找到组件:{1}"
                                  , _componentType, parameterInfo.Name));

                    valueProviders[parameterInfo.Position] = va;
                }
                return _constructorInfo;
            }

            List< KeyValuePair<ConstructorInfo, Func<object>[]> > possibleConstructors =
                new List<KeyValuePair<ConstructorInfo, Func<object>[]>>();
            bool foundPublicConstructor = false;
            StringBuilder reasons = null;

            foreach (ConstructorInfo ci in _componentType.FindMembers(
                MemberTypes.Constructor,
                BindingFlags.Instance | BindingFlags.Public,
                null,
                null))
            {
                foundPublicConstructor = true;

                Func<object>[] parameterAccessors;
                string reason;
                if (CanUseConstructor(ci, context, parameters, out parameterAccessors, out reason))
                {
                    possibleConstructors.Add( new KeyValuePair<ConstructorInfo,Func<object>[]>( ci, parameterAccessors) );
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
                    string.Format( "类型 '{0}' 没有公共构造函数.", _componentType));

            if (possibleConstructors.Count == 0)
                throw new DependencyResolutionException(
                          string.Format("类型 '{0}' 没有找到公共构造函数. 不合适的构造函数包括:{1}"
                          , _componentType, reasons ?? new StringBuilder()));

            valueProviders = null;
            ConstructorInfo result = null;
            foreach (KeyValuePair<ConstructorInfo, Func<object>[]> kp in possibleConstructors)
            {
                if (result == null ||
                    result.GetParameters().Length < kp.Key.GetParameters().Length)
                {
                    result = kp.Key;
                    valueProviders = null;
                }
            }
            return result;
        }

        public object Create(ICreationContext context, IEnumerable<IParameter> parameters)
        {
            Enforce.ArgumentNotNull(context, "context");
            Enforce.ArgumentNotNull(parameters, "parameters");
            
            IEnumerable<IParameter> argumentedParameters = Concat(parameters,_additionalConstructorParameters);

            Func<object>[] valueProviders = null;
            ConstructorInfo selectedCI = GetConstructorInfo(context, argumentedParameters, out valueProviders);
                
            object result = invokeConstructors( selectedCI, context, valueProviders);

            invokeSetters(result, context);

            return result;
        }

        public bool CanSupportNewContext
        {
            get { return true; }
        }

        public Type ImplementationType
        {
            get { return _componentType; }
            set { _componentType = Enforce.ArgumentNotNull(value, "value"); }
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

                    reasonNotUsable.AppendFormat(
                        "类型为 '{0}' 的参数 '{1}' 没有找到",
                        pi.ParameterType, pi.Name);
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

            return reasonNotUsable == null;
        }

        static Exception KeepTargetInvocationStack(TargetInvocationException ex)
        {
            InternalPreserveStackTraceMethod.Invoke(ex.InnerException, null);
            return ex.InnerException;
        }

        object invokeConstructors(ConstructorInfo ci, ICreationContext context, Func<object>[] parameterAccessors)
        {
            Enforce.ArgumentNotNull(ci, "ci");
            Enforce.ArgumentNotNull(parameterAccessors, "parameterAccessors");

            object[] parameters = new object[parameterAccessors.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                parameters[i] = parameterAccessors[i]();
            }

            return _constructorInfo.Invoke(parameters);
        }

        void invokeSetters(object instance, ICreationContext context)
        {
            Enforce.ArgumentNotNull(instance, "instance");
            Enforce.ArgumentNotNull(context, "context");

            if (null == _explicitPropertySetters)
                return;

            Type instanceType = instance.GetType();

            foreach (PropertyInfo propertyInfo in instanceType.GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty))
            {
                foreach (NamedPropertyParameter param in _explicitPropertySetters)
                {
                    Func<object> propertyValueAccessor;
                    if (param.TryGetProvider(propertyInfo.GetSetMethod().GetParameters()[0]
                        , context, out propertyValueAccessor))
                    {
                        propertyInfo.SetValue(instance, propertyValueAccessor(), null);
                    }
                }
            }
        }

        public override string ToString()
        {
            return "ReflectionActivator";
        }

        #region IComponentActivator 成员


        public void Destroy(object instance)
        {
        }

        #endregion
    }
}
