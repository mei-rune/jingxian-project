using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{

    public class MicroKernel : IServiceProvider, IDisposable
    {
        protected Dictionary<string, object> _componentsById = new Dictionary<string, object>();
        protected Dictionary<Type, object> _componentsByType = new Dictionary<Type, object>();

        protected IServiceProvider _parent;
        protected MethodInfo _getMethodInfo;


        bool _isStarted = false;

        public MicroKernel()
        { }

        public MicroKernel( IServiceProvider parent )
        {
           _parent = Enforce.ArgumentNotNull(parent, "parent");
           _getMethodInfo = _parent.GetType().GetMethod("GetService", new Type[] { typeof(string) });
        }

        public void Connect(Type serviceType, object instance)
        {
            innerConnect( serviceType, instance, null );
        }

        public void Connect(Type serviceType, Type classType)
        {
            innerConnect(serviceType, null, classType);
        }

        protected void innerConnect(Type serviceType, object instance, Type implementor)
        {
            Enforce.ArgumentNotNull(serviceType, "serviceType");

            if (null != instance && null != implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时有值!");
            if (null == instance && null == implementor)
                throw new ArgumentException("参数 '{0}' 和 'instance' 不能同时为 null!");

            if (!_isStarted)
            {
                _componentsByType[serviceType] = instance ?? implementor;
                return;
            }

            bool newInstance = false;

            if (null == instance)
                instance = createInstance(implementor, out newInstance);
            else
                instance = createInstance(instance, out newInstance);

            _componentsByType[serviceType] = instance;
            Start(instance);
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
            Start(instance);
        }


        public void Connect(string serviceId, Type serviceType, object instance)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");
            _componentsById[serviceId] = serviceType;
            innerConnect(serviceType, instance, null);
        }

        public void Connect(string serviceId, Type serviceType, Type classType)
        {
            Enforce.ArgumentNotNullOrEmpty(serviceId, "serviceId");
            _componentsById[serviceId] = serviceType;
            innerConnect(serviceType, null, classType);
        }

        public bool Disconnect(Type serviceType)
        {
            return _componentsByType.Remove(serviceType);
        }

        public bool Disconnect(string serviceId)
        {
            return _componentsById.Remove( serviceId );
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
            if (constructors.Length > 1)
                throw new RuntimeException(string.Format("类型 '{0}' 有多个构造函数", implementor.FullName));
            else if (constructors.Length != 1)
                throw new RuntimeException(string.Format("类型 '{0}' 没有构造函数", implementor.FullName));

            ConstructorInfo constructor = constructors[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();

            if (constructorParameters.Length == 0)
                return Activator.CreateInstance(implementor);

            object[] parameters = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
                parameters[i] = GetService(constructorParameters[i].ParameterType);

            return constructor.Invoke(parameters);
        }

        protected void invokeSetters(object instance)
        {
            Enforce.ArgumentNotNull(instance, "instance");

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                if (!propertyInfo.CanWrite || null == propertyInfo.GetSetMethod())
                    continue;

                Type propertyType = propertyInfo.PropertyType;

                try
                {
                    object val = GetService(propertyType);
                    if (null != val)
                        propertyInfo.SetValue(instance, val, null);
                }
                catch (Exception exception)
                {
                    throw new RuntimeException(string.Format("创建服务 '" + propertyType.Name + "' 失败", exception));
                }
            }
        }

        #endregion


        public void Start()
        {
            if (_isStarted)
                return;

            foreach (KeyValuePair<string, object> kp in _componentsById)
            {
                GetService(kp.Key);
            }

            foreach (KeyValuePair<Type, object> kp in _componentsByType)
            {
                GetService(kp.Key);
            }


            foreach (KeyValuePair<string, object> kp in _componentsById)
            {
                if (this != kp.Value)
                    Start(kp.Value);
            }

            foreach (KeyValuePair<Type, object> kp in _componentsByType)
            {
                if (this != kp.Value)
                    Start(kp.Value);
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;

            foreach (KeyValuePair<string, object> kp in _componentsById)
            {
                if (this != kp.Value)
                    Stop(kp.Value);
            }

            foreach (KeyValuePair<Type, object> kp in _componentsByType)
            {
                if (this != kp.Value)
                    Stop(kp.Value);
            }

            _isStarted = false;
        }

        public void Dispose()
        {
            Stop();


            foreach (KeyValuePair<string, object> kp in _componentsById)
            {
                if (this != kp.Value)
                    Dispose(kp.Value);
            }

            foreach (KeyValuePair<Type, object> kp in _componentsByType)
            {
                if (this != kp.Value)
                    Dispose(kp.Value);
            }
        }

        public static void Start(object instance)
        {
            invokeMethod("Start", instance);
        }

        public static void Stop( object instance)
        {
            invokeMethod("Stop", instance);
        }

        public static void Dispose(object instance)
        {
            invokeMethod("Dispose", instance);
        }

        static void invokeMethod(string methodName, object instance)
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
            }
        }
    }
}