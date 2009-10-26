
using System;
using System.Reflection;


namespace jingxian.core.runtime
{

#if !DOTNET35
    public delegate TResult Func<TResult>();
    public delegate TResult Func<T, TResult>(T arg);
#endif

    public interface IParameter
    {
        bool TryGetProvider(ParameterInfo pi, ICreationContext context, out Func<object> valueProvider);
    }
}
