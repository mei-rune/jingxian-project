using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl   
{
    public abstract class ObjectMap<T> : IEnumerable< KeyValuePair<string, T> >
    {
        protected Dictionary<string, T> _servicesById = new Dictionary<string, T>();
        protected Dictionary<Type, T> _servicesByInterface = new Dictionary<Type, T>();

        protected abstract bool Remove(string id, T instance);

        protected bool IsValidServiceType(Type serviceType)
        {
            return typeof(IKernel) != serviceType
                && typeof(ILocator) != serviceType
                && typeof(IServiceProvider) != serviceType;
        }

        public virtual void Add(string id, IEnumerable<Type> services, T instance)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (_servicesById.ContainsKey(id))
                    throw new ComponentAlreadyExistsException(id);

                _servicesById[id] = instance;
            }
            else if (null == services)
            { 
                //TODO: 本地化
                throw new ArgumentException("id 和 services 不能同进为 null!");
            }

            if (null == services)
            {
                foreach (Type serviceType in services)
                {
                    if (!IsValidServiceType(serviceType))
                        continue;

                    if (_servicesByInterface.ContainsKey(serviceType))
                        throw new ComponentAlreadyExistsException(serviceType.ToString());

                    _servicesByInterface[serviceType] = instance;
                }
            }
        }

        public bool Remove(T instance)
        {
            return Remove(null, instance);
        }

        public bool Remove(string id)
        {
            T instance;
            if (_servicesById.TryGetValue(id, out instance))
                return Remove(id, instance);
            return false;
        }

        public bool Remove(Type serviceType)
        {
            T instance;
            if (_servicesByInterface.TryGetValue(serviceType, out instance))
                return Remove(null, instance);
            return false;
        }

        public bool Contains(string id)
        {
            return _servicesById.ContainsKey(id);
        }

        public bool Contains(Type service)
        {
            return _servicesByInterface.ContainsKey(service);
        }

        public bool TryGetValue(Type serviceType, out T value)
        {
            return _servicesByInterface.TryGetValue(serviceType, out value);
        }

        public bool TryGetValue(string  serviceId, out T value)
        {
            return _servicesById.TryGetValue(serviceId, out value);
        }

        #region IEnumerable<KeyValuePair<string,T>> 成员

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _servicesById.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _servicesById.GetEnumerator();
        }

        #endregion
    }

    public class InstanceMap : ObjectMap<object>
    {
        protected override bool Remove(string id, object instance)
        {
            List<Type> typeKeys = new List<Type>();

            foreach (KeyValuePair<Type, object> kp in _servicesByInterface)
            {
                if (kp.Value == instance)
                    typeKeys.Add(kp.Key);
            }

            if (string.IsNullOrEmpty(id))
            {
                foreach (KeyValuePair<string, object> kp in _servicesById)
                {
                    if (kp.Value == instance)
                    {
                        id = kp.Key;
                        break;
                    }
                }
            }

            foreach (Type type in typeKeys)
                _servicesByInterface.Remove(type);

            if (!string.IsNullOrEmpty(id)
                && _servicesById.Remove(id))
                return true;

            return 0 != typeKeys.Count;
        }
    }

    public class ComponentMap : ObjectMap<Descriptor>
    {
        public void Add(Descriptor descriptor)
        {
            Add(descriptor.Id, descriptor.Services, descriptor);
        }
        protected override bool Remove(string id, Descriptor instance)
        {
            bool isRemoved = false;
            if (null != instance.Services)
            {
                foreach (Type type in instance.Services)
                {
                    if (IsValidServiceType(type))
                        isRemoved = isRemoved ? isRemoved : _servicesByInterface.Remove(type);
                }
            }

            if (!string.IsNullOrEmpty(id))
                isRemoved = isRemoved ? isRemoved : _servicesById.Remove(id);
            else if (!string.IsNullOrEmpty(instance.Id))
                isRemoved = isRemoved ? isRemoved : _servicesById.Remove(instance.Id);

            return isRemoved;
        }
    }
}
