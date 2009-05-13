

using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{


    public static class Parameter
    {
        public static T Named<T>( ICollection<IParameter> parameters, string name)
        {
            Enforce.ArgumentNotNull(parameters, "parameters");
            Enforce.ArgumentNotNullOrEmpty(name, "name");

            return ConstantValue<NamedParameter, T>(parameters, delegate(NamedParameter c) { return c.Name == name; });
        }

        public static T Positional<T>(ICollection<IParameter> parameters, int position)
        {
            Enforce.ArgumentNotNull(parameters, "parameters");
            if (position < 0) throw new ArgumentOutOfRangeException("position");

            return ConstantValue<PositionalParameter, T>(parameters, delegate(PositionalParameter c) { return c.Position == position; });
        }

        public static T TypedAs<T>(ICollection<IParameter> parameters)
        {
            Enforce.ArgumentNotNull(parameters, "parameters");

            return ConstantValue<TypedParameter, T>(parameters, delegate(TypedParameter c) { return c.Type == typeof(T); });
        }

        static TValue ConstantValue<TParameter, TValue>(ICollection<IParameter> parameters, Func<TParameter, bool> predicate)
            where TParameter : ConstantParameter
        {
            Enforce.ArgumentNotNull(parameters, "parameters");
            Enforce.ArgumentNotNull(predicate, "predicate");

            foreach (IParameter parameter in parameters)
            {
                TParameter value = parameter as TParameter;
                if (null == value)
                    continue;
                if (predicate(value))
                    return (TValue)value.Value;
            }
            return default(TValue);
        }
    }
}
