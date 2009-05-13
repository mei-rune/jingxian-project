using System;
using System.Collections.Generic;

namespace jingxian.core.runtime.registrars
{
    public interface IReflectiveRegistrar : IConcreteRegistrar<IReflectiveRegistrar>
    {
        IReflectiveRegistrar WithLifestyle(ComponentLifestyle scope);

        IReflectiveRegistrar WithProposedLevel(int level);

        IReflectiveRegistrar UsingConstructor(params Type[] ctorSignature);

        IReflectiveRegistrar WithArgument( IParameter additionalCtorArg);

        IReflectiveRegistrar WithArguments(params IParameter[] additionalCtorArgs);

        IReflectiveRegistrar WithArguments(IEnumerable<IParameter> additionalCtorArgs);
    }
}
