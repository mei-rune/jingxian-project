
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.registrars
{
	public class ReflectiveRegistrar : Registrar<IReflectiveRegistrar>, IReflectiveRegistrar
	{
        Type _implementor;
        ConstructorInfo _constructorInfo;
        List<IParameter> _additionalCtorArgs = new List<IParameter>();
        List<NamedPropertyParameter> _explicitProperties = new List<NamedPropertyParameter>();

        public ReflectiveRegistrar(Type implementor)
		{
            _implementor = Enforce.ArgumentNotNull(implementor, "implementor");
		}

        protected override void AddService(Type service)
        {
            Enforce.ArgumentNotNull(service, "service");

            if (_implementor != typeof(object) && !service.IsAssignableFrom(_implementor))
                throw new ArgumentException(string.Format( "类型 '{0}' 不支持服务接口 '{1}' ", _implementor, service));

            base.AddService(service);
        }

        public IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature)
        {
            Enforce.ArgumentNotNull(ctorSignature, "ctorSignature");
            ConstructorInfo constructorInfo = _implementor.GetConstructor(ctorSignature);
            if (null == constructorInfo)
            {
                StringBuilder sig = new StringBuilder();
                bool first = true;
                foreach ( Type t in ctorSignature)
                {
                    if (first)
                        first = false;
                    else
                        sig.Append(", ");
                    sig.Append(t.FullName);
                }

                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "数型 '{0}' 不能提供带有签名 '{1}' 的构造函数!",
                    _implementor, sig));
            }
            _constructorInfo = constructorInfo;
            return this;
        }


        public IReflectiveRegistrar WithArguments(params IParameter[] additionalCtorArgs)
        {
            return WithArguments((IEnumerable<IParameter>)additionalCtorArgs);
        }

        public IReflectiveRegistrar WithArguments(IEnumerable<IParameter> additionalCtorArgs)
        {
            _additionalCtorArgs.AddRange( Enforce.ArgumentNotNull(additionalCtorArgs, "additionalCtorArgs") );
            return this;
        }

        public IReflectiveRegistrar WithArgument(IParameter additionalCtorArg)
        {
            _additionalCtorArgs.Add(Enforce.ArgumentNotNull<IParameter>(
                additionalCtorArg, "additionalCtorArg"));
            return Syntax;
        }

        public IReflectiveRegistrar WithProperties(IEnumerable<NamedPropertyParameter> explicitProperties)
        {
            _explicitProperties.AddRange( Enforce.ArgumentNotNull(explicitProperties, "explicitProperties") );
            return this;
        }

        public IReflectiveRegistrar WithProperties(params NamedPropertyParameter[] explicitProperties)
        {
            return WithProperties((IEnumerable<NamedPropertyParameter>)explicitProperties);
        }

        protected override IReflectiveRegistrar Syntax
        {
            get { return this; }
        }

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

        protected override void DoConfigure(IKernel container)
        {
            throw new NotImplementedException();
        }
    }
}
