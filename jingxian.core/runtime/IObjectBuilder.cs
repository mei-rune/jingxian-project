

using System;
using System.Collections;

namespace jingxian.core.runtime
{
    public interface IObjectBuilder
    {
        bool TryGetType(string typeName, out Type type);
        Type GetType(string typeName);
        T BuildTransient<T>(string id, Type classType, Type contractType);
        T BuildTransient<T>(string id, Type classType, Type contractType, IDictionary arguments);
        T BuildTransient<T>(Type classType);
        T BuildTransient<T>(Type classType, IDictionary arguments);
        T BuildTransient<T>(string classType);
        T BuildTransient<T>(string classType, IDictionary arguments);
    }
}