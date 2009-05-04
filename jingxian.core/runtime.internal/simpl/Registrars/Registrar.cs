
using System;
using System.Collections.Generic;

namespace jingxian.core.runtime.simpl.registrars
{
    using jingxian.core.runtime.registrars;

	public abstract class MiniRegistrar<TSyntax> : IRegistrar<TSyntax>, IModule
        where TSyntax : IRegistrar<TSyntax>
	{
        string _id;
        IList<Type> _services = new List<Type>();
        ComponentLifestyle _lifestyle = ComponentLifestyle.Singleton;
        IList<EventHandler<PreparingEventArgs>> _preparingHandlers = new List<EventHandler<PreparingEventArgs>>();
        IList<EventHandler<ActivatingEventArgs>> _activatingHandlers = new List<EventHandler<ActivatingEventArgs>>();
        IList<EventHandler<ActivatedEventArgs>> _activatedHandlers = new List<EventHandler<ActivatedEventArgs>>();
        IList<EventHandler<RegisteredEventArgs>> _registeredHandlers = new List<EventHandler<RegisteredEventArgs>>();
        IDictionary<string, object> _extendedProperties = new Dictionary<string, object>();
        
        protected abstract TSyntax Syntax { get; }

        public virtual void Configure( IKernel container )
        {
            Enforce.ArgumentNotNull(container, "container");

            DoConfigure(container);
        }

        public TSyntax Named(string id)
        {
            Enforce.ArgumentNotNullOrEmpty(_id, "id");
            _id = Enforce.ArgumentNotNullOrEmpty(id, "id");
            return Syntax;
        }

        protected abstract void DoConfigure(IKernel container);

        public virtual TSyntax As<TService>()
		{
            AddService(typeof(TService));
			return Syntax;
		}

        public virtual TSyntax As<TService1, TService2>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            return Syntax;
		}

        public virtual TSyntax As<TService1, TService2, TService3>()
        {
            AddService(typeof(TService1));
            AddService(typeof(TService2));
            AddService(typeof(TService3));
            return Syntax;
		}

        public virtual TSyntax As(params Type[] services)
        {
            Enforce.ArgumentNotNull(services, "services");
            AddServices( services );
            return Syntax;
        }

        public virtual TSyntax WithLifestyle(ComponentLifestyle lifestyle)
		{
            throw new NotImplementedException();
            //_lifestyle = lifestyle;
            //return Syntax;
		}
        
        public virtual TSyntax OnRegistered(EventHandler<RegisteredEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            _registeredHandlers.Add(handler);
            return Syntax;
        }

        public virtual TSyntax OnPreparing(EventHandler<PreparingEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            _preparingHandlers.Add(handler);
            return Syntax;
        }

        public virtual TSyntax OnActivating(EventHandler<ActivatingEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            _activatingHandlers.Add(handler);
            return Syntax;
        }

        public virtual TSyntax OnActivated(EventHandler<ActivatedEventArgs> handler)
        {
            Enforce.ArgumentNotNull(handler, "handler");
            _activatedHandlers.Add(handler);
            return Syntax;
        }

        public virtual TSyntax WithExtendedProperty(string key, object value)
        {
            Enforce.ArgumentNotNull(key, "key");
            throw new NotImplementedException();
            //_extendedProperties.Add(key, value);
            //return Syntax;
        }

		protected virtual IEnumerable<Type> Services
		{
            get { return _services; }
		}

        protected virtual void AddService(Type service)
        {
            Enforce.ArgumentNotNull(service, "service");
            _services.Add(service);
        }

        protected virtual void AddServices(IEnumerable<Type> services)
        {
            Enforce.ArgumentNotNull(services, "services");
            foreach (var service in services)
                AddService(service);
        }

        protected virtual ComponentLifestyle Lifestyle
		{
            get { return _lifestyle; }
            set { _lifestyle = value; }
		}

        protected virtual IEnumerable<EventHandler<PreparingEventArgs>> PreparingHandlers
        {
            get { return _preparingHandlers; }
        }

        protected virtual IEnumerable<EventHandler<ActivatingEventArgs>> ActivatingHandlers
        {
            get { return _activatingHandlers; }
        }

        protected virtual IEnumerable<EventHandler<ActivatedEventArgs>> ActivatedHandlers
        {
            get { return _activatedHandlers; }
        }

        protected virtual void FireRegistered(RegisteredEventArgs e)
        {
            Enforce.ArgumentNotNull(e, "e");
            foreach (EventHandler<RegisteredEventArgs> handler in _registeredHandlers)
                handler(this, e);
        }

        protected virtual IDictionary<string, object> ExtendedProperties
        {
            get { return _extendedProperties; }
        }

        protected virtual void RegisterComponent(IKernel container, IComponentRegistration cr)
        {
            Enforce.ArgumentNotNull(container, "container");
            Enforce.ArgumentNotNull(cr, "cr");

            foreach (EventHandler<PreparingEventArgs> preparingHandler in PreparingHandlers)
                cr.Preparing += preparingHandler;

            foreach (EventHandler<ActivatingEventArgs> activatingHandler in ActivatingHandlers)
                cr.Activating += activatingHandler;

            foreach (EventHandler<ActivatedEventArgs> activatedHandler in ActivatedHandlers)
                cr.Activated += activatedHandler;

            //container.RegisterComponent(cr);

            RegisteredEventArgs registeredEventArgs = new RegisteredEventArgs();
            registeredEventArgs.Container = container;
            registeredEventArgs.Registration = cr;

            FireRegistered(new RegisteredEventArgs() { Container = container, Registration = cr });
        }
    }
}
