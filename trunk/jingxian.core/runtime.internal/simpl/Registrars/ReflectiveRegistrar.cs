
using System;
using System.Collections.Generic;
using System.Reflection;

namespace jingxian.core.runtime.simpl.mini.registrars
{
    using jingxian.core.runtime.registrars;

    public class ReflectiveRegistrar : AbstractRegistrar<IReflectiveRegistrar>, IReflectiveRegistrar
	{
        MiniKernel _kernel;
        Type _implementor;
        ComponentLifestyle _lifestyle = ComponentLifestyle.Singleton;
        int _level = int.MaxValue;
        List<IParameter> _parameters;

        public ReflectiveRegistrar(MiniKernel kernel, Type implementor)
		{
            _kernel = Enforce.ArgumentNotNull(kernel, "kernel");
            _implementor = Enforce.ArgumentNotNull(implementor, "implementor");
            _parameters = null;
		}

        public override void Configure()
        {
            Enforce.NotNull(_kernel, "kernel");

            _kernel.Connect(_id, _services, _implementor, _lifestyle, _level, _parameters, _extendedProperties);
        }

        protected override IReflectiveRegistrar Syntax
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

        #region IReflectiveRegistrar 成员

        public virtual IReflectiveRegistrar WithLifestyle(ComponentLifestyle lifestyle)
        {
            _lifestyle = lifestyle;
            return this;
        }

        public virtual IReflectiveRegistrar WithProposedLevel(int level)
        {
            _level = level;
            return this;
        }

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
    }
}
