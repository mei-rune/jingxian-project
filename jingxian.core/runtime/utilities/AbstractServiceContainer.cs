using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    using  System.ComponentModel.Design;

    public abstract class AbstractServiceContainer : MarshalByRefObject, IServiceContainer
    {
        private readonly  IServiceContainer parent;
        private readonly IServiceProvider parentProvider;
        private IDictionary< Type, object> type2Service;

        public AbstractServiceContainer()
        {
        }

        public AbstractServiceContainer(IServiceProvider parentProvider)
        {
            this.parentProvider = parentProvider;
        }

        public AbstractServiceContainer(IServiceContainer parent)
        {
            this.parent = parent;
        }


        public void AddService(Type serviceType, object serviceInstance)
        {
            AddService(serviceType, serviceInstance, false);
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            if (promote)
            {
                IServiceContainer parentContainer = ParentContainer;

                if (parentContainer != null)
                {
                    parentContainer.AddService(serviceType, serviceInstance, promote);
                    return;
                }
            }

            if (type2Service == null)
            {
                type2Service = new Dictionary< Type, object >();
            }

            type2Service[serviceType] = serviceInstance;
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            throw new NotImplementedException();
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            throw new NotImplementedException();
        }

        public void RemoveService(Type serviceType)
        {
            RemoveService(serviceType, false);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            if (promote)
            {
                IServiceContainer parentContainer = ParentContainer;

                if (parentContainer != null)
                {
                    parentContainer.RemoveService(serviceType, promote);
                    return;
                }
            }

            if (type2Service != null)
            {
                type2Service.Remove(serviceType);
            }
        }

        public virtual object GetService(Type serviceType)
        {
            object service = null;

            if (serviceType == typeof(IServiceContainer))
                return this;

            if (serviceType == typeof(IServiceProvider))
                return this;

            if (type2Service != null && type2Service.TryGetValue(serviceType, out service ) )
               return service;


           if (service == null && parentProvider != null)
               service = parentProvider.GetService(serviceType);

            if (service == null && parent != null)
                service = parent.GetService(serviceType);

            
            return service;
        }

        private IServiceContainer ParentContainer
        {
            get
            {
                if (null != parent)
                    return parent;

                if (null == parentProvider)
                    return null;

                return parentProvider.GetService(typeof(IServiceContainer)) as IServiceContainer;
            }
        }

        //public void SetParent(IServiceProvider serviceProvider)
        //{
        //    parentProvider = serviceProvider;
        //}

        //public void SetParent(IServiceContainer serviceContainer)
        //{
        //    parent = serviceContainer;
        //}
    }
}
