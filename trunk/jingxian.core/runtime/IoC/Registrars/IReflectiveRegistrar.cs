using System;
using System.Collections.Generic;

namespace jingxian.core.runtime.registrars
{
    public interface IReflectiveRegistrar : IConcreteRegistrar<IReflectiveRegistrar>
    {
        IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature);

        IReflectiveRegistrar WithArgument( IParameter additionalCtorArg);

        IReflectiveRegistrar WithArguments(params IParameter[] additionalCtorArgs);

        IReflectiveRegistrar WithArguments(IEnumerable<IParameter> additionalCtorArgs);
    }
}
