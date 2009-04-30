using System;
using System.Collections.Generic;

namespace jingxian.core.runtime.registrars
{
    public interface IReflectiveRegistrar : IConcreteRegistrar<IReflectiveRegistrar>
    {
        IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature);

        IReflectiveRegistrar WithArguments(params IParameter[] additionalCtorArgs);

        IReflectiveRegistrar WithArguments(IEnumerable<IParameter> additionalCtorArgs);

        IReflectiveRegistrar WithProperties(params NamedPropertyParameter[] explicitProperties);

        IReflectiveRegistrar WithProperties(IEnumerable<NamedPropertyParameter> explicitProperties);
    }
}
