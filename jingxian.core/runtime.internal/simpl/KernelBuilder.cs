using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{
    using jingxian.core.runtime.registrars;

    public class KernelBuilder : IKernelBuilder
    {
        List<registrars.MiniRegistrar> _registrars = new List<registrars.MiniRegistrar>();

        MiniKernel _kernel;
        ComponentLifestyle _defaultScope = ComponentLifestyle.Singleton;

        public KernelBuilder(MiniKernel kernel)
        {
            _kernel = kernel;
        }

        #region IKernelBuilder 成员

        public ComponentLifestyle DefaultLifestyle
        {
            get { return _defaultScope; }
        }

        public void SetDefaultLifestyle(ComponentLifestyle scope)
        {
            _defaultScope = scope;
        }

        public IKernel Build()
        {
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
             registrars.MiniRegistrar registrar = new registrars.MiniRegistrar( implementor );
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
