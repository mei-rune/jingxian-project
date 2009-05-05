using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl.mini.registrars
{
    using jingxian.core.runtime.registrars;


    public abstract class AbstractRegistrar<TSyntax> : IRegistrar<TSyntax>, IModule
        where TSyntax : IRegistrar<TSyntax>
    {
        protected string _id;
        protected IList<Type> _services = new List<Type>();
        protected IProperties _extendedProperties;

        public abstract void Configure();
        protected abstract TSyntax Syntax { get; }
        protected abstract void AddService(Type service);

        #region IRegistrar<TSyntax> 成员

        public TSyntax Named(string id)
        {
            _id = Enforce.ArgumentNotNullOrEmpty(id, "id");
            return Syntax;
        }

        public TSyntax As<TService>()
        {
            AddService(typeof(TService));
            return Syntax;
        }

        public TSyntax As<TService1, TService2>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            return Syntax;
        }

        public TSyntax As<TService1, TService2, TService3>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            AddService(typeof(TService3));
            return Syntax;
        }

        public TSyntax As(params Type[] services)
        {
            Enforce.ArgumentNotNull(services, "services");
            foreach (Type type in services)
                AddService(type);
            return Syntax;
        }

        public TSyntax OnRegistered(EventHandler<RegisteredEventArgs> handler)
        {
            throw new NotImplementedException();
        }

        public TSyntax OnPreparing(EventHandler<PreparingEventArgs> handler)
        {
            throw new NotImplementedException();
        }

        public TSyntax OnActivating(EventHandler<ActivatingEventArgs> handler)
        {
            throw new NotImplementedException();
        }

        public TSyntax OnActivated(EventHandler<ActivatedEventArgs> handler)
        {
            throw new NotImplementedException();
        }

        public TSyntax WithExtendedProperty(string key, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
