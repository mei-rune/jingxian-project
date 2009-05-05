using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class MicroKernel : IServiceProvider
    {
        Dictionary<string, object> _componentsById = new Dictionary<string, object>();
        Dictionary<Type, object> _componentsByType = new Dictionary<Type, object>();

        IServiceProvider _parent;
        MethodInfo _getMethodInfo;

        public MicroKernel()
        { }

        public MicroKernel( IServiceProvider parent )
        {
           _parent = Enforce.ArgumentNotNull(parent, "parent");
           _getMethodInfo = _parent.GetType().GetMethod("GetService", new Type[] { typeof(string) });
        }

        public void Connect(Type serviceType, object instance)
        {
            _componentsByType[serviceType] = instance;
        }

        public void Connect(Type serviceType, Type classType)
        {
            _componentsByType[serviceType] = classType;
        }

        public void Connect(string serviceId, object instance)
        {
            _componentsById[serviceId] = instance;
        }

        public void Connect(string serviceId, Type classType)
        {
            _componentsById[serviceId] = classType;
        }

        public bool Disconnect(Type serviceType)
        {
            return _componentsByType.Remove(serviceType);
        }

        public bool Disconnect(string serviceId, object instance)
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

        public object GetService(string serviceId)
        {
            object instance = null;
            if (!_componentsById.TryGetValue(serviceId, out instance))
            {
                Type classType = instance as Type;
                if (null != classType)
                    return instance;

                instance = invokeConstructors(classType);
                invokeSetters(instance);
                _componentsById[serviceId] = instance;
            }
            if (null == _getMethodInfo)
                return null;


            return _getMethodInfo.Invoke(_parent
                , new object[] { serviceId });
        }

        public object GetService(Type serviceType)
        {
            object instance = null;
            if (!_componentsByType.TryGetValue(serviceType, out instance))
            {
                Type classType = instance as Type;
                if (null != classType)
                    return instance;

                instance = invokeConstructors(classType);
                invokeSetters(instance);
                _componentsByType[serviceType] = instance;
            }
            if (null == _parent)
                return _parent;
            return _parent.GetService(serviceType);
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

        public static void Start(object instance)
        {
            invokeMethod("Start", instance);
        }

        public static void Stop( object instance)
        {
            invokeMethod("Stop", instance);
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
