using System;
using System.Collections.Generic;
using System.Text;


namespace jingxian.core.runtime
{

    public interface ILocator : IServiceProvider
    {
        object this[Type serviceType] { get; }

        object this[string key] { get; }

        T  Get<T>();

        T Get<T>(string id);

        object Get(string id, Type service);

        object GetService(string id);
    }
}
