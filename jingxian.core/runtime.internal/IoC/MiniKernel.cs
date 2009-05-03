using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{

    public class MiniKernel : IKernel
    {
        private string _id;
        private IKernel _parent;
        private Dictionary<string, object> _servicesById = new Dictionary<string, object>();
        private Dictionary<Type, object> _servicesByInterface = new Dictionary<Type, object>();
        bool _isStarted = false;
        bool _isResolved = false;


        public MiniKernel(string id, IKernel parent)
            : this(id, parent, null)
        {
        }

        public MiniKernel(IProperties properties)
            : this("root", null, properties)
        {
        }

        public MiniKernel(string id, IKernel parent, IProperties properties)
        {
            _parent = parent;
            _id = id;
            _servicesByInterface[typeof(IKernel)] = this;
            _servicesByInterface[typeof(ILocator)] = this;
            _servicesByInterface[typeof(IServiceProvider)] = this;
        }

        protected virtual void addService(string id, IEnumerable<Type> services, object instance)
        {
            if (_servicesById.ContainsKey(id))
                throw new ComponentAlreadyExistsException(id);

            _servicesById[id] = instance;

            if (null == services)
            {
                if (_isStarted)
                    Start(this, instance);
                return;
            }

            foreach (Type serviceType in services)
            {
                if (typeof(IKernel) == serviceType
                    || typeof(ILocator) == serviceType
                    || typeof(IServiceProvider) == serviceType)
                    continue;

                if (_servicesByInterface.ContainsKey(serviceType))
                    throw new ComponentAlreadyExistsException(serviceType.ToString());

                _servicesByInterface[serviceType] = instance;
            }

            if (_isStarted)
                Start(this, instance);
        }

        protected bool removeService(object instance)
        {
            string id = null;
            List<Type> typeKeys = new List<Type>();

            foreach (KeyValuePair<Type, object> kp in _servicesByInterface)
            {
                if (kp.Value == instance)
                    typeKeys.Add(kp.Key);
            }

            foreach (KeyValuePair<string, object> kp in _servicesById)
            {
                if (kp.Value == instance)
                {
                    id = kp.Key;
                    break;
                }
            }

            foreach (Type type in typeKeys)
                _servicesByInterface.Remove(type);

            if (!string.IsNullOrEmpty(id))
                return _servicesById.Remove(id);

            return 0 != typeKeys.Count;
        }

        protected bool removeServiceByService(object instance)
        {
            List<Type> typeKeys = new List<Type>();

            foreach (KeyValuePair<Type, object> kp in _servicesByInterface)
            {
                if (kp.Value == instance)
                    typeKeys.Add(kp.Key);
            }

            foreach (Type type in typeKeys)
                _servicesByInterface.Remove(type);

            return 0 != typeKeys.Count;
        }

        protected virtual bool removeService(string id)
        {
            object instance = null;
            if (_servicesById.TryGetValue(id, out instance))
            {
                removeServiceByService(instance);
                return _servicesById.Remove(id);
            }
            return false;
        }

        protected virtual bool removeService(Type serviceType)
        {
            object instance = null;
            if (_servicesByInterface.TryGetValue(serviceType, out instance))
                return removeService(instance);
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

            foreach (KeyValuePair<string, object> kp in _servicesById)
            {
                Start(this, kp.Value);
            }

            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                return;

            foreach (KeyValuePair<string, object> kp in _servicesById)
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
            object instance = null;
            if (_servicesByInterface.TryGetValue(serviceType, out instance))
                return instance;

            return (null == _parent) ? null : _parent.GetService(serviceType);
        }

        public object GetService(string serviceId)
        {
            object instance = null;
            if (_servicesById.TryGetValue(serviceId, out instance))
                return instance;

            return (null == _parent) ? null : _parent.Get(serviceId);
        }


        public void Release(object instance)
        {

        }

        public void Dispose()
        {
        }

        #endregion

        public static void Start(IKernel kernel, object instance)
        {
            invokeMethod("Start", kernel, instance);
        }

        public static void Stop(IKernel kernel, object instance)
        {
            invokeMethod("Stop", kernel, instance);
        }

        static void invokeMethod(string methodName, IKernel kernel, object instance)
        {
            MethodInfo methodInfo = instance.GetType().GetMethod(methodName);
            if (null == methodInfo)
                return;

            ParameterInfo[] parameters = methodInfo.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    {
                        methodInfo.Invoke(instance, new object[] { });
                        break;
                    }
                case 1:
                    if (parameters[0].ParameterType == typeof(IKernel))
                    {
                        methodInfo.Invoke(instance, new object[] { kernel });
                    }
                    break;
            }
        }

        #region IKernel 成员


        public bool Contains<T>()
        {
            return _servicesByInterface.ContainsKey( typeof(T) );
        }

        public IKernelBuilder CreateBuilder()
        {
            return new KernelBuilder(this);
        }

        public void Connect(string id, Type classType, IEnumerable<Type> serviceTypes, ComponentLifestyle lifestyle, IEnumerable<IParameter> parameters, IProperties properties)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect(string id)
        {
            return removeService(id);
        }

        #endregion

        #region ILocator 成员

        public object this[Type serviceType]
        {
            get { return GetService( serviceType ); }
        }

        public object this[string key]
        {
            get { return GetService( key ); }
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
            return (T)Get(id, typeof(T) );
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
