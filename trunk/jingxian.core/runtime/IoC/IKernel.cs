

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{

    public interface IKernel : ILocator, IDisposable
    {
        bool Contains(string id);

        bool Contains(Type service);

        bool Contains<T>();

        void Release(object instance);

        IKernelBuilder CreateBuilder();

        void Connect(string id, Type classType
            , Type serviceType
            , ComponentLifestyle lifestyle
            , IEnumerable< IParameter> parameters
            , IProperties properties );

        bool Disconnect(string id);

        void Start();

        void Stop();
    }
}