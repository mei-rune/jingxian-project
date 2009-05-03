
using System;
using System.Collections.Generic;
using System.Globalization;

namespace jingxian.core.runtime.registrars
{
    public abstract class ConcreteRegistrar<TSyntax> : Registrar<TSyntax>, IModule, IConcreteRegistrar<TSyntax>
        where TSyntax : IConcreteRegistrar<TSyntax>
    {
        Type _implementor;
        string _id = Guid.NewGuid().ToString();


        protected ConcreteRegistrar(Type implementor, Type defaultService)
        {
            _implementor = Enforce.ArgumentNotNull(implementor, "implementor");
            AddService(Enforce.ArgumentNotNull<Type>(defaultService, "defaultService"));
        }

        protected override void DoConfigure(IContainer container)
        {
            Enforce.ArgumentNotNull(container, "container");

            var services = new List<Service>(Services);

            var activator = CreateActivator();
            Enforce.NotNull(activator);

            var descriptor = new Descriptor(Id, services, _implementor, ExtendedProperties);
            var cr = RegistrationCreator(descriptor, activator, Scope.ToIScope(), Ownership);

            RegisterComponent(container, cr);
        }

        public TSyntax Named(string name)
        {
            _id = name;
            return Syntax;
        }

        public TSyntax As(params Type[] services)
        {
            Enforce.ArgumentNotNull(services, "services");
            AddServices(services);
            return Syntax;
        }

        protected override IEnumerable<Type> Services
        {
            get { return base.Services; }
        }

        protected override void AddService(Type service)
        {
            Enforce.ArgumentNotNull(service, "service");

            Type serviceType = service;
            if (serviceType != null && _implementor != typeof(object) && !serviceType.ServiceType.IsAssignableFrom(_implementor))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    ConcreteRegistrarResources.ComponentDoesNotSupportService, _implementor, service));

            base.AddService(service);
        }

        protected abstract IActivator CreateActivator();

        public string Id
        {
            get { return _id; }
        }
    }
}
