using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{
    //public class EventClass
    //{
    //    int BeginCreating;
    //    int EndCreated;

    //    int BeginStarting;
    //    int EndStarted;


    //    int BeginStopping;
    //    int EndStopped;

    //    int BeginDestroy;
    //    int EndDestroy;
    //}


    public partial class MicroKernel : IKernel
    {
        private string _id;
        private IKernel _parent;
        private IDictionary<string, IServiceRef> _servicesById = new Dictionary<string, IServiceRef>();
        private IDictionary<Type, IServiceRef> _servicesByInterface = new Dictionary<Type, IServiceRef>();
        bool _isStarted = false;
        bool _isResolved = false;


        public MicroKernel(string id, IKernel parent)
            : this(id, parent, null)
        {
        }

        public MicroKernel(IProperties properties)
            : this("root", null, properties)
        {
        }

        public MicroKernel(string id, IKernel parent, IProperties properties)
        {
            _parent = parent;
            _id = id;
            InstanceRef instanceRef = new InstanceRef(this, _id??"__kernel__", typeof(IKernel), this, null, null);
            _servicesByInterface[typeof(IKernel)] = instanceRef;
            _servicesByInterface[typeof(IServiceProvider)] = instanceRef;
        }

        protected virtual void addService(IServiceRef serviceRef)
        {
            if (_servicesById.ContainsKey(serviceRef.Id))
                throw new ComponentAlreadyExistsException(serviceRef.Id);

            _servicesById[serviceRef.Id] = serviceRef;
            if (null != serviceRef.ServiceType && typeof(IKernel) != serviceRef.ServiceType)
            {
                if (_servicesByInterface.ContainsKey(serviceRef.ServiceType))
                    throw new ComponentAlreadyExistsException(serviceRef.ServiceType.ToString());
                _servicesByInterface[serviceRef.ServiceType] = serviceRef;
            }

            if (_isStarted)
                Start(this, serviceRef);
        }

        protected virtual bool removeService(IServiceRef serviceRef)
        {
            if (null != serviceRef.ServiceType && typeof(IKernel) != serviceRef.ServiceType)
                _servicesByInterface.Remove(serviceRef.ServiceType);

            if (!string.IsNullOrEmpty(serviceRef.Id))
                return _servicesById.Remove(serviceRef.Id);
            return false;
        }

        protected virtual bool removeService(string id)
        {
            IServiceRef serviceRef = null;
            if (_servicesById.TryGetValue(id, out serviceRef))
                return removeService(serviceRef);

            return false;
        }

        protected virtual bool removeService(Type serviceType)
        {
            IServiceRef serviceRef = null;
            if (_servicesByInterface.TryGetValue(serviceType, out serviceRef))
                return removeService(serviceRef);

            return false;
        }

        protected virtual IServiceRef findService(string id)
        {
            IServiceRef serviceRef = null;
            if (_servicesById.TryGetValue(id, out serviceRef))
                return serviceRef;
            return null;
        }

        protected virtual IServiceRef findService(Type interfaceType)
        {
            IServiceRef serviceRef = null;
            if (_servicesByInterface.TryGetValue(interfaceType, out serviceRef))
                return serviceRef;
            return null;
        }


        public void Resolve()
        {
            if (_isResolved)
                return;

            _isResolved = true;
        }


        #region IKernel 成员

        public void Start()
        {
            if (_isStarted)
                return;

            foreach (KeyValuePair<string, IServiceRef> kp in _servicesById)
            {
                Start(this, kp.Value);
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;

            foreach (KeyValuePair<string, IServiceRef> kp in _servicesById)
            {
                Stop(this, kp.Value);
            }


            _isStarted = false;
        }

        public bool Contains(string id)
        {
            if( _servicesById.ContainsKey(id) )
                return true;
            if( null == _parent )
                return false;
            return _parent.Contains( id );
        }

        public bool Contains(Type service)
        {
            if( _servicesByInterface.ContainsKey( service ))
                return true;
            if( null == _parent )
                return false;
            return _parent.Contains( service );
        }

        public object GetService(Type serviceType)
        {
            IServiceRef serviceRef = findService(serviceType);
            if (null != serviceRef)
                serviceRef.Get(null);

            return (null == _parent) ? null : _parent.Get(serviceType);
        }
        public object GetService(string serviceId)
        {
            IServiceRef serviceRef = findService(serviceId);
            if (null != serviceRef)
                serviceRef.Get(null);

            return (null == _parent) ? null : _parent.Get(serviceId);
        }


        public void Release(object instance)
        {
        }

        public void Dispose()
        {
        }

        #endregion

        protected virtual CreationContext createContext(IServiceRef serviceRef)
        {
            return new CreationContext(this, serviceRef);
        }

        public virtual object createService(CreationContext context)
        {
            try
            {
                object instance = invokeConstructors(context);
                invokeSetters(context, instance);
                return instance;
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new RuntimeException("创建服务[ " + context.Id + "]失败", exception);
            }
        }

        public object findParameter(CreationContext context, Type parameterType, object parameter)
        {
            if (parameterType.IsPrimitive)
            {
                if (null == parameter)
                    return null;

                return Converter.Instance.ConvertTo(parameter, parameterType, context);

            }
            else if (parameterType.IsInterface || parameterType.IsAbstract)
            {
                if (null != parameter)
                    return context.Kernel[Helper.ExtractReference(parameter.ToString())];

                return context.Kernel[parameterType];
            }

            if (null != parameter)
            {
                if (Helper.IsReference(parameter.ToString()))
                    return context.Kernel[Helper.ExtractReference(parameter.ToString())];
                else
                    return Converter.Instance.ConvertTo(parameter, parameterType, context);
            }

            return context.Kernel[parameterType];
        }

        protected object invokeConstructors(CreationContext context)
        {
            ConstructorInfo[] constructors = context.ImplementationType.GetConstructors();
            if (constructors.Length > 1)
                throw new RuntimeException(string.Format("不能创建服务[{0}]实例,类型[{1}]有多个构造函数", context.Id, context.ImplementationType.FullName));
            else if (constructors.Length != 1)
                throw new RuntimeException(string.Format("不能创建服务[{0}]实例,类型[{1}]没有构造函数", context.Id, context.ImplementationType.FullName));

            ConstructorInfo ctor = constructors[0];

            ParameterInfo[] parameters = ctor.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; ++i)
            {
                object val = findParameter(context, parameters[i].ParameterType
                    , context.Parameters[parameters[i].ParameterType.Name]);

                //if (null != val)
                args[i] = val;
            }

            return ctor.Invoke(args);
        }

        protected void invokeSetters(CreationContext context, object instance)
        {
            if (null == instance)
                return;

            foreach (PropertyInfo descriptor in instance.GetType().GetProperties())
            {
                if (!descriptor.CanWrite || null == descriptor.GetSetMethod())
                    continue;

                Type propertyType = descriptor.PropertyType;

                try
                {
                    object val = findParameter(context, propertyType
                        , context.Parameters[propertyType.Name]);

                    if (null != val)
                        descriptor.SetValue(instance, val, null);
                }
                catch (Exception exception)
                {
                    throw new RuntimeException(string.Format("创建服务[ " + propertyType.Name + "]失败", exception));
                }
            }
        }

        public static void Start(IKernel kernel, IServiceRef serviceRef)
        {
            invokeMethod("Start", kernel, serviceRef);
        }

        public static void Stop(IKernel kernel, IServiceRef serviceRef)
        {
            invokeMethod("Stop", kernel, serviceRef);
        }

        static void invokeMethod(string methodName, IKernel kernel, IServiceRef serviceRef)
        {
            MethodInfo methodInfo = serviceRef.ImplementationType.GetMethod(methodName);
            if (null == methodInfo)
                return;

            ParameterInfo[] parameters = methodInfo.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    {
                        object instance = serviceRef.Get( null );
                        methodInfo.Invoke(instance, new object[] { });
                        break;
                    }
                case 1:
                    if (parameters[0].ParameterType == typeof(IKernel))
                    {
                        object instance = serviceRef.Get(null);
                        methodInfo.Invoke(instance, new object[] { kernel });
                    }
                    break;
            }
        }

        public virtual IComponentActivator CreateActivator(ComponentLifestyle lifestyleType, IServiceRef serviceRef)
        {
            switch (lifestyleType)
            {
                case ComponentLifestyle.Singleton:
                    return new SingletonActivator(this, serviceRef, this.OnCreation, this.OnDestruction);
                default:
                    return null;
            }
        }

        protected virtual void OnCreation(IServiceRef serviceRef, object instance)
        {
        }

        protected virtual void OnDestruction(IServiceRef serviceRef, object instance)
        {
        }
    }
}
