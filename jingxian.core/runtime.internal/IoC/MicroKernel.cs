using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{

    public partial class MicroKernel : IKernel
    {
        private string _id;
        private IKernel _parent;
        private IDictionary<string, IComponentRegistration> _servicesById = new Dictionary<string, IComponentRegistration>();
        private IDictionary<Type, IComponentRegistration> _servicesByInterface = new Dictionary<Type, IComponentRegistration>();
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
            IComponentRegistration registration = new Registration(
                new Descriptor(id, new Type[] { typeof(IKernel), typeof(ILocator), typeof(IServiceProvider) }, GetType())
                , new activator.InstanceActivator(this)
                , null, ComponentLifestyle.Singleton);

            _servicesByInterface[typeof(IKernel)] = registration;
            _servicesByInterface[typeof(ILocator)] = registration;
            _servicesByInterface[typeof(IServiceProvider)] = registration;
        }

        public bool IsDefaultServiceType(Type type)
        {
            return typeof(IKernel) == type 
                || typeof(ILocator) == type 
                || typeof(IServiceProvider) == type;
        }

        protected virtual void addService(IComponentRegistration registration)
        {
            if (_servicesById.ContainsKey(registration.Descriptor.Id))
                throw new ComponentAlreadyExistsException(registration.Descriptor.Id);

            _servicesById[registration.Descriptor.Id] = registration;

            if (null != registration.Descriptor.Services)
            {
                foreach (Type serviceType in registration.Descriptor.Services)
                {
                    if (null != serviceType && !IsDefaultServiceType( serviceType ) )
                    {
                        if (_servicesByInterface.ContainsKey(serviceType))
                            throw new ComponentAlreadyExistsException(serviceType.ToString());
                        _servicesByInterface[serviceType] = registration;
                    }
                }
            }

            if (_isStarted)
                Start(this, registration);
        }

        protected virtual bool removeService(IComponentRegistration registration)
        {
            bool isRemoved = false;
            if (null != registration.Descriptor.Services)
            {
                foreach (Type serviceType in registration.Descriptor.Services)
                {
                    if (null != serviceType && !IsDefaultServiceType(serviceType))
                    {
                        if (_servicesByInterface.Remove(serviceType))
                            isRemoved = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(registration.Descriptor.Id)
                && _servicesById.Remove(registration.Descriptor.Id))
                isRemoved = true;
            return isRemoved;
        }

        protected virtual bool removeService(string id)
        {
            IComponentRegistration registration = null;
            if (_servicesById.TryGetValue(id, out registration))
                return removeService(registration);
            return false;
        }

        protected virtual bool removeService(Type serviceType)
        {
            IComponentRegistration registration = null;
            if (_servicesByInterface.TryGetValue(serviceType, out registration))
                return removeService(registration);
            return false;
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

            foreach (KeyValuePair<string, IComponentRegistration > kp in _servicesById)
            {
                Start(this, kp.Value);
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;

            foreach (KeyValuePair<string, IComponentRegistration> kp in _servicesById)
            {
                Stop(this, kp.Value);
            }


            _isStarted = false;
        }

        public bool Contains(string id)
        {
            if (_servicesById.ContainsKey(id))
                return true;
            if (null == _parent)
                return false;
            return _parent.Contains(id);
        }

        public bool Contains(Type service)
        {
            if (_servicesByInterface.ContainsKey(service))
                return true;
            if (null == _parent)
                return false;
            return _parent.Contains(service);
        }

        public object GetService(Type serviceType)
        {
            IComponentRegistration registration = null;

            if (_servicesByInterface.TryGetValue(serviceType, out registration))
                return Get(registration);
 
            return (null == _parent) ? null : _parent.Get(serviceType);
        }
        public object GetService(string serviceId)
        {
            IComponentRegistration registration = null;
            if (_servicesById.TryGetValue(serviceId, out registration))
                return Get(registration);

            return (null == _parent) ? null : _parent.Get(serviceId);
        }

        public object Get(IComponentRegistration registration)
        {
            ICreationContext context = null;
            bool newInstance;
            return registration.Get(context, out newInstance);
        }

        public void Release(object instance)
        {
        }

        public void Dispose()
        {
        }

        #endregion

        public  void Start(IKernel kernel, IComponentRegistration registration)
        {
            invokeMethod("Start", kernel, registration);
        }

        public  void Stop(IKernel kernel, IComponentRegistration registration)
        {
            invokeMethod("Stop", kernel, registration);
        }

        void invokeMethod(string methodName, IKernel kernel, IComponentRegistration registration)
        {
            MethodInfo methodInfo = registration.Descriptor.ImplementationType.GetMethod(methodName);
            if (null == methodInfo)
                return;

            ParameterInfo[] parameters = methodInfo.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    {
                        object instance = Get(registration);
                        methodInfo.Invoke(instance, new object[] { });
                        break;
                    }
                case 1:
                    if (parameters[0].ParameterType == typeof(IKernel))
                    {
                        object instance = Get(registration);
                        methodInfo.Invoke(instance, new object[] { kernel });
                    }
                    break;
            }
        }

        #region IKernel 成员


        public bool Contains<T>()
        {
            throw new NotImplementedException();
        }

        public IKernelBuilder CreateBuilder()
        {
            throw new NotImplementedException();
        }

        public void Connect(string id, Type classType, IEnumerable<Type> serviceTypes, ComponentLifestyle lifestyle, IEnumerable<IParameter> parameters, IProperties properties)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILocator 成员

        public object this[Type serviceType]
        {
            get { return GetService( serviceType ); }
        }

        public object this[string key]
        {
            get { return GetService(key); }
        }

        public T Get<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object Get(string id)
        {
            return GetService(id);
        }

        public T Get<T>(string id)
        {
            return (T)Get(id, typeof(T));
        }

        public object Get(string id, Type service)
        {
            object value = GetService(id);
            if (null == value)
                return value;
            if (service.IsAssignableFrom(value.GetType()))
                return value;
            return null;
        }

        public object Get(Type service)
        {
            return GetService(service);
        }

        #endregion
    }
}
