

using System;
using System.Collections;

namespace jingxian.core.runtime
{
    public interface IObjectBuilder
    {
        bool TryGetType(string typeName, out Type type);

        Type GetType(string typeName);


        T BuildTransient<T>();
        // 暂时不做
        //T BuildTransient<T>(IProperties arguments);

        object BuildTransient(Type classType);
        // 暂时不做
        //object BuildTransient(Type classType, IProperties arguments);

        object BuildTransient(string classType);
        // 暂时不做
        //object BuildTransient(string classType, IProperties arguments);
    }
}