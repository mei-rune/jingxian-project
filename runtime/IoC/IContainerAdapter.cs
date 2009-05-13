

using System;
using System.Collections;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    //public interface IContainerAdapter: IKernel, IDisposable
    //{
    //    //string Name { get; }

    //    //object this[string key] { get; }

    //    //object this[Type service] { get; }

    //    bool Contains(string id);

    //    bool Contains(Type service);

    //    bool Contains<T>();

    //    void Add(string id, Type classType, Type serviceType, ComponentLifestyle lifestyle);

    //    void Add<TInterface, TImplementation>() where TImplementation : class;

    //    void Add<TImplementation>() where TImplementation : class;

    //    void Add<TInterface, TImplementation>(string id) where TImplementation : class;

    //    void Add<TImplementation>(string id) where TImplementation : class;

    //    void Add(string id, Type classType);

    //    void Add(string id, Type classType, Type serviceType);

    //    void Add<TInterface>(string id, object instance);

    //    void Add(string id, object instance, Type serviceType);

    //    T Resolve<T>();

    //    T Resolve<T>(IDictionary arguments);

    //    object Resolve(string id);

    //    object Resolve(string id, IDictionary arguments);

    //    T Resolve<T>(string id);

    //    T Resolve<T>(string id, IDictionary arguments);

    //    object Resolve(string id, Type service);

    //    object Resolve(string id, Type service, IDictionary arguments);

    //    object Resolve(Type service);

    //    void Release(object instance);

    //    bool Remove(string id);

    //    void InitializeContainer(IApplicationContext context 
    //        , PredefinedService[] predefinedServices );
    //}
}