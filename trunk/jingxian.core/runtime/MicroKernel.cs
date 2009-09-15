using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Betanetworks
{
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
        protected Dictionary<string, object> _componentsById = new Dictionary<string, object>();
        protected Dictionary<Type, object> _componentsByType = new Dictionary<Type, object>();

        protected IServiceProvider _parent;
        protected MethodInfo _getMethodInfo;


        protected bool _isStarted = false;

        public MicroKernel()
        {
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
                instance = createInstance(implementor, out newInstance);
            else
                instance = createInstance(instance, out newInstance);

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
                instance = createInstance(implementor, out newInstance);
            else
                instance = createInstance(instance, out newInstance);

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
                throw new ArgumentNullException("serviceTypes");

            _componentsById[serviceId] = serviceTypes[0];
            innerConnect(serviceTypes, instance, null);
        }

        public void Connect(string serviceId, Type[] serviceTypes, Type classType)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");
            Enforce.ArgumentNotNull(serviceTypes, "serviceTypes");
            if (0 == serviceTypes.Length)
                throw new ArgumentNullException("serviceTypes");

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
                instance = createInstance(instance, out newInstance);
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
                instance = createInstance(instance, out newInstance);
                if( newInstance )
                    _componentsByType[serviceType] = instance;
                return instance;
            }
            if (null == _parent)
                return _parent;
            return _parent.GetService(serviceType);
        }

        protected virtual object createInstance(Type implementor, out bool newInstance)
        {
            if (implementor.IsInterface)
            {
                //NOTICE: 因为不是本次查找的key直接创建的，所以不作为新实例
                newInstance = false;
                return GetService(implementor);
            }

            newInstance = true;
            object instance = invokeConstructors(implementor);
            invokeSetters(instance);
            return instance;
        }

        public object Create(Type implementor)
        {            
            object instance = invokeConstructors(implementor);
            invokeSetters(instance);
            return instance;
        }

        public object Create(object instance)
        {
            invokeSetters(instance);
            return instance;
        }

        protected virtual object createInstance(object instance, out bool newInstance)
        {
            Type implementor = instance as Type;
            if (null == implementor)
            {
                newInstance = false;
                return instance;
            }

            return createInstance(implementor, out newInstance);
        }

        protected object invokeConstructors(Type implementor)
        {
            ConstructorInfo[] constructors = implementor.GetConstructors();

            List<KeyValuePair<ConstructorInfo, object[]>> ciList = new List<KeyValuePair<ConstructorInfo, object[]>>();

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                if (constructorParameters.Length == 0)
                {
                    ciList.Add(new KeyValuePair<ConstructorInfo, object[]>(constructor, new object[0]));
                    continue;
                }

                bool hasNull = false;
                object[] parameters = new object[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    parameters[i] = GetService(constructorParameters[i].Name);
                    if (null == parameters[i])
                        parameters[i] = GetService(constructorParameters[i].ParameterType);
                    if (null == parameters[i])
                        hasNull = true;
                }

                if (!hasNull)
                    ciList.Add(new KeyValuePair<ConstructorInfo, object[]>(constructor, parameters));
            }


            if(0 == ciList.Count)
                throw new RuntimeException(string.Concat("创建对象 '", implementor, "' 失败,没有适合的构造器!"));

            KeyValuePair<ConstructorInfo, object[]>? selectedCI = null;
            foreach (KeyValuePair<ConstructorInfo, object[]> kp in ciList)
            {
                if (null == selectedCI || selectedCI.Value.Value.Length < kp.Value.Length)
                    selectedCI = kp;
            }

            return selectedCI.Value.Key.Invoke(selectedCI.Value.Value);
        }

        protected void invokeSetters(object instance)
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
                    object val = GetService(name);
                    if(null == val)
                        val = GetService(propertyType);

                    if (null != val)
                        propertyInfo.SetValue(instance, val, null);
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