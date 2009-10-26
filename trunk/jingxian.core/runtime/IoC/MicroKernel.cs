using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
//#if !DOTNET35
//    public delegate TResult Func<TResult>();
//    public delegate TResult Func<T, TResult>(T arg);
//#endif


    public class ServiceKey
    {
        internal string stringKey;
        internal Type typeKey;

        public ServiceKey(string key)
        {
            stringKey = Enforce.ArgumentNotNullOrEmpty(key, "key");
        }

        public ServiceKey(Type key)
        {
            typeKey = Enforce.ArgumentNotNull(key, "key");
        }
    }

    public class MicroKernel : IServiceProvider, IDisposable
    {
        public interface IParameter
        {
            bool TryGetProvider(ParameterInfo pi, out Func<object> valueProvider);
        }

        protected class AutoWireParameter : IParameter
        {
            protected MicroKernel _kernal;
            public AutoWireParameter(MicroKernel kernal)
            {
                _kernal = kernal;
            }

            public bool TryGetProvider(ParameterInfo pi, out Func<object> valueProvider)
            {
                object val = _kernal.GetService(pi.Name);
                if (null != val)
                {
                    valueProvider = delegate() { return val; };
                    return true;
                }
                val = _kernal.GetService(pi.ParameterType);
                if (null != val)
                {
                    valueProvider = delegate() { return val; };
                    return true;
                }
                valueProvider = null;
                return false;
            }
        }

        protected Dictionary<string, object> _componentsById = new Dictionary<string, object>();
        protected Dictionary<Type, object> _componentsByType = new Dictionary<Type, object>();

        protected IServiceProvider _parent;
        protected MethodInfo _getMethodInfo;
        IParameter[] _internalParameters;


        protected bool _isStarted = false;

        public MicroKernel()
        {
            _internalParameters = new IParameter[] { new AutoWireParameter(this) };
            _componentsById["kernel"]= this;
            _componentsByType[typeof(MicroKernel)] = this;

            _componentsById["serviceProvider"] = this;
            _componentsByType[typeof(IServiceProvider)] = this;
        }

        public MicroKernel( IServiceProvider parent )
            : this()
        {
           _parent = Enforce.ArgumentNotNull(parent, "parent");
           _getMethodInfo = _parent.GetType().GetMethod("GetService", new Type[] { typeof(string) });
        }

        public void Connect(Type serviceType, object instance)
        {
            innerConnect(new Type[] { serviceType }, instance, null);
        }

        public void Connect(Type serviceType, Type classType)
        {
            innerConnect(new Type[] { serviceType }, null, classType);
        }

        public void Connect(Type[] serviceTypes, object instance)
        {
            innerConnect(serviceTypes, instance, null);
        }

        public void Connect(Type[] serviceTypes, Type classType)
        {
            innerConnect(serviceTypes, null, classType);
        }

        protected void innerConnect(Type[] serviceTypes, object instance, Type implementor)
        {
            Enforce.ArgumentNotNull(serviceTypes, "serviceTypes");
            if (0 == serviceTypes.Length)
                throw new ArgumentNullException("serviceTypes");

            if (null != instance && null != implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时有值!");
            if (null == instance && null == implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时为 null!");

            if (!_isStarted)
            {
                _componentsByType[serviceTypes[0]] = instance ?? implementor;
                for (int i = 1; i < serviceTypes.Length; ++i)
                {
                    _componentsByType[serviceTypes[i]] = serviceTypes[0];
                }
                return;
            }

            bool newInstance = false;

            if (null == instance)
                instance = createInstance(implementor, _internalParameters, out newInstance);
            else
                instance = createInstance(instance, _internalParameters, out newInstance);

            for (int i = 0; i < serviceTypes.Length; ++i)
            {
                _componentsByType[serviceTypes[i]] = instance;
            }

            Start(instance, this);
        }

        public void Connect(string serviceId, object instance)
        {
            innerConnect(serviceId, instance, null );
        }

        public void Connect(string serviceId, Type classType)
        {
            innerConnect(serviceId,null, classType);
        }

        protected void innerConnect(string serviceId, object instance, Type implementor)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");

            if (null != instance && null != implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时有值!");
            if (null == instance && null == implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时为 null!");

            if (!_isStarted)
            {
                _componentsById[serviceId] = instance ?? implementor;
                return;
            }

            bool newInstance = false;
            
            if (null == instance)
                instance = createInstance(implementor, _internalParameters, out newInstance);
            else
                instance = createInstance(instance, _internalParameters, out newInstance);

            _componentsById[serviceId] = instance;
            Start(instance, this);
        }

        public void Connect(string serviceId, Type serviceType, object instance)
        {
            Connect(serviceId, new Type[]{serviceType}, instance);
        }

        public void Connect(string serviceId, Type serviceType, Type classType)
        {
            Connect(serviceId, new Type[] { serviceType }, classType);
        }

        public void Connect(string serviceId, Type[] serviceTypes, object instance)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");
            Enforce.ArgumentNotNull(serviceTypes, "serviceTypes");
            if (0 == serviceTypes.Length)
            {
                _componentsById[serviceId] = instance;
                return;
            }

            _componentsById[serviceId] = serviceTypes[0];
            innerConnect(serviceTypes, instance, null);
        }

        public void Connect(string serviceId, Type[] serviceTypes, Type classType)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");
            Enforce.ArgumentNotNull(serviceTypes, "serviceTypes");
            if (0 == serviceTypes.Length)
            {
                _componentsById[serviceId] = classType;
                return;
            }

            _componentsById[serviceId] = serviceTypes[0];
            innerConnect(serviceTypes, null, classType);
        }

        public bool Disconnect(Type serviceType)
        {
            return _componentsByType.Remove(serviceType);
        }

        public bool Disconnect(string serviceId)
        {
            return _componentsById.Remove( serviceId );
        }

        public ServiceKey[] Match(Type serviceType)
        {
            List<ServiceKey> keySet = new List<ServiceKey>();
            foreach ( KeyValuePair<string, object> kp in _componentsById)
            {
                if (null == kp.Value)
                    continue;

                Type implementType = kp.Value as Type;

                if (null != implementType)
                {
                    if (serviceType.IsAssignableFrom(implementType))
                        keySet.Add(new ServiceKey(kp.Key));
                }
                else
                {
                    if (serviceType.IsAssignableFrom(kp.Value.GetType()))
                        keySet.Add(new ServiceKey(kp.Key));
                }
            }

            foreach (KeyValuePair<Type, object> kp in _componentsByType)
            {
                if (null == kp.Value)
                    continue;

                Type implementType = kp.Value as Type;

                if (null != implementType)
                {
                    if (serviceType.IsAssignableFrom(implementType))
                        keySet.Add(new ServiceKey(kp.Key));
                }
                else
                {
                    if (serviceType.IsAssignableFrom(kp.Value.GetType()))
                        keySet.Add(new ServiceKey(kp.Key));
                }
            }

            return keySet.ToArray();
        }

        public object this[string serviceId]
        {
            get{ return GetService( serviceId ); }
        }

        public object this[Type serviceType]
        {
            get { return GetService(serviceType); }
        }

        #region IServiceProvider 成员


        public object GetService(ServiceKey key)
        {
            if (!string.IsNullOrEmpty(key.stringKey))
                return GetService(key.stringKey);
            return GetService(key.typeKey);
        }

        public virtual object GetService(string serviceId)
        {
            object instance = null;
            if (_componentsById.TryGetValue(serviceId, out instance))
            {
                bool newInstance = false;
                instance = createInstance(instance, _internalParameters, out newInstance);
                if (newInstance)
                    _componentsById[serviceId] = instance;

                return instance;
            }
            if (null == _getMethodInfo)
                return null;


            return _getMethodInfo.Invoke(_parent
                , new object[] { serviceId });
        }

        public virtual object GetService(Type serviceType)
        {
            object instance = null;
            if (_componentsByType.TryGetValue(serviceType, out instance))
            {
                bool newInstance = false;
                instance = createInstance(instance, _internalParameters, out newInstance);
                if( newInstance )
                    _componentsByType[serviceType] = instance;
                return instance;
            }
            if (null == _parent)
                return _parent;
            return _parent.GetService(serviceType);
        }

        protected virtual object createInstance(Type implementor, IParameter[] parameters, out bool newInstance)
        {
            if (implementor.IsInterface)
            {
                //NOTICE: 因为不是本次查找的key直接创建的，所以不作为新实例
                newInstance = false;
                return GetService(implementor);
            }

            newInstance = true;
            object instance = invokeConstructors(implementor, parameters??_internalParameters);
            invokeSetters(instance, parameters ?? _internalParameters);
            return instance;
        }

        public object Create(Type implementor)
        {
            object instance = invokeConstructors(implementor, _internalParameters);
            invokeSetters(instance, _internalParameters);
            return instance;
        }

        public object Create(object instance)
        {
            invokeSetters(instance, _internalParameters);
            return instance;
        }

        protected virtual object createInstance(object instance, IParameter[] parameters, out bool newInstance)
        {
            Type implementor = instance as Type;
            if (null == implementor)
            {
                newInstance = false;
                return instance;
            }

            return createInstance(implementor, parameters, out newInstance);
        }

        public static Func<object> GetProvider(IParameter[] parameters, ParameterInfo parameterInfo)
        {
            Func<object> func = null;
            foreach (IParameter parameter in parameters)
            {
                if (parameter.TryGetProvider(parameterInfo, out func))
                    return func;
            }
            return null;
        }

        public static object invokeConstructors(Type implementor, IParameter[] parameters)
        {
            ConstructorInfo[] constructors = implementor.GetConstructors();

            List<KeyValuePair<ConstructorInfo, Func<object>[]>> ciList = new List<KeyValuePair<ConstructorInfo, Func<object>[]>>();

            /// 遍历所有的构造函数,判断它们的参数是不是满足,如果满足就添加到ciList列表中
            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                if (constructorParameters.Length == 0)
                {
                    ciList.Add(new KeyValuePair<ConstructorInfo, Func<object>[]>(constructor, new Func<object>[0]));
                    continue;
                }

                bool hasNull = false;
                Func<object>[] providers = new Func<object>[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    providers[i] = GetProvider(parameters, constructorParameters[i]);

                    if (null == providers[i])
                    {
                        hasNull = true;
                        break;
                    }
                }

                if (!hasNull)
                    ciList.Add(new KeyValuePair<ConstructorInfo, Func<object>[]>(constructor, providers));
            }


            if (0 == ciList.Count)
                throw new RuntimeException(string.Concat("创建对象 '", implementor, "' 失败,没有适合的构造器!"));

            /// 选择一个参数最多的构造函数
            KeyValuePair<ConstructorInfo, Func<object>[]>? selectedCI = null;
            foreach (KeyValuePair<ConstructorInfo, Func<object>[]> kp in ciList)
            {
                if (null == selectedCI || selectedCI.Value.Value.Length < kp.Value.Length)
                    selectedCI = kp;
            }

            /// 取得所有的参数
            object[] args = new object[selectedCI.Value.Value.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                args[i] = selectedCI.Value.Value[i]();
            }

            return selectedCI.Value.Key.Invoke(args);
        }

        public static void invokeSetters(object instance, IParameter[] parameters)
        {
            Enforce.ArgumentNotNull(instance, "instance");

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                if (!propertyInfo.CanWrite || null == propertyInfo.GetSetMethod())
                    continue;


                Type propertyType = propertyInfo.PropertyType;
                string name = propertyInfo.Name;

                try
                {
                    Func<object> provider = GetProvider(parameters, propertyInfo.GetSetMethod().GetParameters()[0]);
                    if (null != provider)
                        propertyInfo.SetValue(instance, provider(), null);
                }
                catch (Exception exception)
                {
                    throw new RuntimeException(string.Concat("创建服务 '", name, "':'", propertyType.Name, "' 失败"), exception);
                }
            }
        }

        #endregion

        public void Start()
        {
            if (_isStarted)
                return;

            string[] keys = new string[ _componentsById.Count];
            _componentsById.Keys.CopyTo(keys, 0);
            foreach(string key in keys)
            {
                GetService(key);
            }

            Type[] types = new Type[_componentsByType.Count];
            _componentsByType.Keys.CopyTo(types, 0);
            foreach (Type key in types)
            {
                GetService(key);
            }

            List<object> instances = new List<object>();
            foreach (object instance in _componentsById.Values)
            {
                if(!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in _componentsByType.Values)
            {
                if (!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in instances)
            {
                if (this != instance)
                    Start(instance, this);
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;

            List<object> instances = new List<object>();
            foreach (object instance in _componentsById.Values)
            {
                if (!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in _componentsByType.Values)
            {
                if (!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in instances)
            {
                if (this != instance)
                    Stop(instance, this);
            }

            _isStarted = false;
        }

        public void Dispose()
        {
            internalDispose(true);
        }

        protected virtual void internalDispose( bool disposing )
        {
            if (!disposing)
                return;

            Stop();

            List<object> instances = new List<object>();
            foreach (object instance in _componentsById.Values)
            {
                if (!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in _componentsByType.Values)
            {
                if (!instances.Contains(instance))
                    instances.Add(instance);
            }
            foreach (object instance in instances)
            {
                if (this != instance)
                    Dispose(instance, this);
            }

            _componentsById.Clear();
            _componentsByType.Clear();
        }

        public static void Start(object instance, object context)
        {
            invokeMethod("Start", instance, context);
        }

        public static void Stop( object instance, object context)
        {
            invokeMethod("Stop", instance,  context);
        }

        public static void Dispose(object instance, object context)
        {
            invokeMethod("Dispose", instance, context);
        }

        static void invokeMethod(string methodName, object instance, object context)
        {
            MethodInfo methodInfo = instance.GetType().GetMethod(methodName);
            if (null == methodInfo)
                return;

            ParameterInfo[] parameters = methodInfo.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    methodInfo.Invoke(instance, null);
                    break;
                case 1:
                    if (null != instance && parameters[0].ParameterType.IsAssignableFrom(context.GetType()))
                        methodInfo.Invoke(instance, new object[] { context });
                    break;
            }
        }
    }
}