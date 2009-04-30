

using System;
using System.Collections;

namespace jingxian.core.runtime
{

    public interface IKernel : ILocator, IDisposable
    {
        bool Contains(string id);

        bool Contains(Type service);

        bool Contains<T>();

        void Connect(string id, Type classType, Type serviceType, ComponentLifestyle lifestyle);

        void Connect<TInterface, TImplementation>() where TImplementation : class;

        void Connect<TImplementation>() where TImplementation : class;

        void Connect<TInterface, TImplementation>(string id) where TImplementation : class;

        void Connect<TImplementation>(string id) where TImplementation : class;

        void Connect(string id, Type classType);

        void Connect(string id, Type classType, Type serviceType);

        void Connect<TInterface>(string id, object instance);

        void Connect(string id, object instance, Type serviceType);

        void Release(object instance);

        bool Disconnect(string id);

        void Start();

        void Stop();
    }
}