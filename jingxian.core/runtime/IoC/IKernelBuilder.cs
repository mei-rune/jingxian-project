using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.registrars;

    public delegate T ComponentActivator<T>(ICreationContext context);
    public delegate T ComponentActivatorWithParameters<T>(ICreationContext context, IEnumerable<IParameter> parameters);


    public interface IKernelBuilder
    {
        ComponentLifestyle DefaultScope { get; }

        IDisposable SetDefaultScope(ComponentLifestyle scope);

        IKernel Build();
        void Build(IKernel container);

        void RegisterModule(IModule module);

        IGenericRegistrar RegisterGeneric(Type implementor);
        IReflectiveRegistrar Register<T>();
        IReflectiveRegistrar Register(Type implementor);
        IGenericRegistrar RegisterTypesMatching(Predicate<Type> predicate);
        IGenericRegistrar RegisterTypesFromAssembly(Assembly assembly);
        IGenericRegistrar RegisterTypesAssignableTo<T>();
        IConcreteRegistrar RegisterCollection<T>();
        IConcreteRegistrar RegisterCollection(Type collectionType);
        IConcreteRegistrar Register<T>(ComponentActivator<T> creator);
        IConcreteRegistrar Register<T>(ComponentActivatorWithParameters<T> creator);
        IConcreteRegistrar Register<T>(T instance);
    }
}
