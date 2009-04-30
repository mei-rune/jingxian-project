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

        #region IKernel ≥…‘±


        public object this[Type serviceType]
        {
            get { return GetService(serviceType); }
        }

        public object this[string key]
        {
            get { return GetService(key); }
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }

        public void Connect(string id, Type classType, Type serviceType, ComponentLifestyle lifestyle)
        {
            throw new NotImplementedException();
        }

        public void Connect<TInterface, TImplementation>() where TImplementation : class
        {
            throw new NotImplementedException();
        }

        public void Connect<TImplementation>() where TImplementation : class
        {
            throw new NotImplementedException();
        }

        public void Connect<TInterface, TImplementation>(string id) where TImplementation : class
        {
            throw new NotImplementedException();
        }

        public void Connect<TImplementation>(string id) where TImplementation : class
        {
            throw new NotImplementedException();
        }

        public void Connect(string id, Type classType)
        {
            addService( new DefaultRef(this, id, null, classType, null, ComponentLifestyle.Singleton, null ) );
        }

        public void Connect(string id, Type classType, Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void Connect<TInterface>(string id, object instance)
        {
            throw new NotImplementedException();
        }

        public void Connect(string id, object instance, Type serviceType)
        {
            throw new NotImplementedException();
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
            return (T)GetService(id);
        }

        public object Get(string id, Type service)
        {
            object value = Get(id);
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


        public bool Disconnect(string id)
        {
            return removeService(id);
        }

        #endregion
    }
}
