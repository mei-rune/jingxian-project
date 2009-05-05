
using System;
using System.Collections.Generic;
using System.Reflection;

namespace jingxian.core.runtime.simpl.mini.registrars
{
    using jingxian.core.runtime.registrars;

    public class InstanceRegistrar :AbstractRegistrar<IConcreteRegistrar>, IConcreteRegistrar
	{
        MiniKernel _kernel;
        Type _implementor;
        object _instance;

        public InstanceRegistrar(MiniKernel kernel, object instance)
		{
            _kernel = Enforce.ArgumentNotNull(kernel, "kernel");
            _instance = Enforce.ArgumentNotNull(instance, "instance");
            _implementor = instance.GetType();
		}


        public override void Configure()
        {
            _kernel.ConnectWithInstance(_instance
                , _id
                , _services
                , _implementor
                , int.MaxValue
                , null
                , _extendedProperties);
        }

        protected override IConcreteRegistrar Syntax
        {
            get { return this; }
        }

        protected override void AddService(Type service)
        {
            Enforce.ArgumentNotNull(service, "service");

            if (_implementor != typeof(object) && !service.IsAssignableFrom(_implementor))
                throw new ArgumentException(string.Format("类型 '{0}' 不支持服务接口 '{1}' ", _implementor, service));

            _services.Add(service);
        }

        #region IConcreteRegistrar<IConcreteRegistrar> 成员

        public IConcreteRegistrar DefaultOnly()
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar MemberOf(string serviceName)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar MemberOf<TService>()
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar MemberOf(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar MemberOf(Service service)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
