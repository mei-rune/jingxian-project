
using System;
using System.Collections.Generic;
using System.Reflection;

namespace jingxian.core.runtime.simpl.mini.registrars
{
    using jingxian.core.runtime.registrars;

    public class Registrar : IReflectiveRegistrar
	{
        string _id;
        IList<Type> _services = new List<Type>();
        Type _implementor;
        ComponentLifestyle _lifestyle = ComponentLifestyle.Singleton;
        int _level = int.MaxValue;
        List<IParameter> _parameters;
        IProperties _extendedProperties;

        public Registrar(Type implementor)
		{
            _implementor = Enforce.ArgumentNotNull(implementor, "implementor");
            _parameters = null;
		}

        public IReflectiveRegistrar Named(string id)
        {
            _id = Enforce.ArgumentNotNullOrEmpty(id, "id");
            return this;
        }

        public virtual IReflectiveRegistrar As<TService>()
		{
            AddService(typeof(TService));
            return this;
		}

        public virtual IReflectiveRegistrar As<TService1, TService2>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            return this;
		}

        public virtual IReflectiveRegistrar As<TService1, TService2, TService3>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            AddService(typeof(TService3));
            return this;
		}

        public virtual IReflectiveRegistrar As(params Type[] services)
        {
            Enforce.ArgumentNotNull(services, "services");
            foreach (Type type in services)
                AddService(type);
            return this;
        }

        public virtual IReflectiveRegistrar WithLifestyle(ComponentLifestyle lifestyle)
		{
            _lifestyle = lifestyle;
            return this;
        }

        public virtual IReflectiveRegistrar WithLevel(int le)
        {
            _lifestyle = lifestyle;
            return this;
        }

        public virtual IReflectiveRegistrar WithExtendedProperty(string key, object value)
        {
            Enforce.ArgumentNotNullOrEmpty(key, "key");
            Enforce.ArgumentNotNull(value, "value");
            if (null == _extendedProperties)
                _extendedProperties = new MapProperties();

            _extendedProperties[key] = value;
            return this;
        }
        
        public virtual IReflectiveRegistrar OnRegistered(EventHandler<RegisteredEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            throw new NotImplementedException();
        }

        public virtual IReflectiveRegistrar OnPreparing(EventHandler<PreparingEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            throw new NotImplementedException();
        }

        public virtual IReflectiveRegistrar OnActivating(EventHandler<ActivatingEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            throw new NotImplementedException();
        }

        public virtual IReflectiveRegistrar OnActivated(EventHandler<ActivatedEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            throw new NotImplementedException();
        }


        protected virtual void AddService(Type service)
        {
            Enforce.ArgumentNotNull(service, "service");

            if (_implementor != typeof(object) && !service.IsAssignableFrom(_implementor))
                throw new ArgumentException(string.Format("类型 '{0}' 不支持服务接口 '{1}' ", _implementor, service));

            _services.Add(service);
        }

        #region IReflectiveRegistrar 成员

        public IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar WithArgument(IParameter additionalCtorArg)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar WithArguments(params IParameter[] additionalCtorArgs)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar WithArguments(IEnumerable<IParameter> additionalCtorArgs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConcreteRegistrar<IReflectiveRegistrar> 成员

        public IReflectiveRegistrar DefaultOnly()
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar MemberOf(string serviceName)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar MemberOf<TService>()
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar MemberOf(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar MemberOf(Service service)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Configure(MiniKernel kernel)
        {
            Enforce.ArgumentNotNull(kernel, "kernel");

            kernel.Connect(_id, _services, _implementor, _lifestyle, _parameters, _extendedProperties);
        }
    }
}
