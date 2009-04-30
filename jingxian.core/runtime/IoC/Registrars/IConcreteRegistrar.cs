

using System;

namespace jingxian.core.runtime.registrars
{
    public interface IConcreteRegistrar : IConcreteRegistrar<IConcreteRegistrar>
    {
    }

    public interface IConcreteRegistrar<TSyntax> : IRegistrar<TSyntax>
        where TSyntax : IConcreteRegistrar<TSyntax>
    {
        TSyntax DefaultOnly();

        TSyntax MemberOf(string serviceName);

        TSyntax MemberOf<TService>();

        TSyntax MemberOf(Type serviceType);

        TSyntax MemberOf(Service service);
    }
}
