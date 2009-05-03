
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
			: base(implementor)
		{
            Enforce.ArgumentNotNull(implementor, "implementor");
            _implementor = implementor;
		}

        #region IReflectiveRegistrar Members

        public IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature)
        {
            Enforce.ArgumentNotNull(ctorSignature, "ctorSignature");
            ConstructorInfo constructorInfo = _implementor.GetConstructor(ctorSignature);
            if (null == constructorInfo)
            {
                var sig = new StringBuilder();
                var first = true;
                foreach (var t in ctorSignature)
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

        public IReflectiveRegistrar WithProperties(IEnumerable<NamedPropertyParameter> explicitProperties)
        {
            _explicitProperties.AddRange( Enforce.ArgumentNotNull(explicitProperties, "explicitProperties") );
            return this;
        }

        public IReflectiveRegistrar WithProperties(params NamedPropertyParameter[] explicitProperties)
        {
            return WithProperties((IEnumerable<NamedPropertyParameter>)explicitProperties);
        }

        #endregion

        //protected override IActivator CreateActivator()
        //{
        //    return new ReflectionActivator(
        //        _implementor,
        //        _additionalCtorArgs,
        //        _explicitProperties,
        //        _ctorSelector);
        //}

        protected override IReflectiveRegistrar Syntax
        {
            get { return this; }
        }
    }
}
