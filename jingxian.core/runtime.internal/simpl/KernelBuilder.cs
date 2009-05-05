using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl.mini
{
    using jingxian.core.runtime.registrars;

    public class KernelBuilder : IKernelBuilder
    {
        List<IModule> _registrars = new List<IModule>();

        MiniKernel _kernel;
        ComponentLifestyle _defaultLifestyle = ComponentLifestyle.Singleton;

        public KernelBuilder(MiniKernel kernel)
        {
            _kernel = kernel;
        }

        #region IKernelBuilder 成员

        public ComponentLifestyle DefaultLifestyle
        {
            get { return _defaultLifestyle; }
        }

        public void SetDefaultLifestyle(ComponentLifestyle lifestyle)
        {
            _defaultLifestyle = lifestyle;
        }

        public IKernel Build()
        {
            foreach ( IModule registrar in _registrars)
            {
                registrar.Configure();
            }

            return _kernel;
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
            registrars.ReflectiveRegistrar registrar = new registrars.ReflectiveRegistrar(_kernel, implementor);
            registrar.WithLifestyle(_defaultLifestyle);
            _registrars.Add(registrar);
            return registrar;
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
            registrars.InstanceRegistrar registrar = new registrars.InstanceRegistrar(_kernel,instance);
            registrar.As(typeof(T));
            _registrars.Add(registrar);
            return registrar;
        }

        #endregion
    }
}
