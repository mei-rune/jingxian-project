
using System;

namespace jingxian.core.runtime.registrars
{
	public interface IRegistrar<TSyntax>
        where TSyntax : IRegistrar<TSyntax>
    {
        TSyntax Named(string name);

        TSyntax As<TService>();

        TSyntax As<TService1, TService2>();

        TSyntax As<TService1, TService2, TService3>();

        TSyntax As(params Type[] services);

        TSyntax WithLifestyle(ComponentLifestyle scope);

        TSyntax OnRegistered(EventHandler<RegisteredEventArgs> handler);

        TSyntax OnPreparing(EventHandler<PreparingEventArgs> handler);

        TSyntax OnActivating(EventHandler<ActivatingEventArgs> handler);

        TSyntax OnActivated(EventHandler<ActivatedEventArgs> handler);

        TSyntax WithExtendedProperty(string key, object value);
	}
}
