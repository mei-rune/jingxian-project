using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{
    using jingxian.core.runtime.registrars;

    public class KernelBuilder : IKernelBuilder
    {
        List<IComponentDescriptor> _descriptors = new List<IComponentDescriptor>();

        MiniKernel _kernel;
        ComponentLifestyle _defaultScope = ComponentLifestyle.Singleton;

        public KernelBuilder(MiniKernel kernel)
        {
            _kernel = kernel;
        }

        #region IKernelBuilder 成员

        public ComponentLifestyle DefaultScope
        {
            get { return _defaultScope; }
        }

        public IDisposable SetDefaultScope(ComponentLifestyle scope)
        {
            throw new NotImplementedException();
        }

        public IKernel Build()
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
            }

            return _kernel;
        }


        IComponentDescriptor GetDescriptor(string serviceId)
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
                if (serviceId == descriptor.Id)
                    return descriptor;
            }
            return null;
        }

        IComponentDescriptor GetDescriptor(Type contract)
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
                if (null == descriptor.Services)
                    continue;
                foreach (Type type in descriptor.Services)
                {
                    if (type == contract)
                        return descriptor;
                }
            }
            return null;
        }

        public void RegisterModule(IModule module)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar Register<T>()
        {
            return Register(typeof(T));
        }

        public IReflectiveRegistrar Register(Type implementor)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(ComponentActivator<T> creator)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(ComponentActivatorWithParameters<T> creator)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(T instance)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
