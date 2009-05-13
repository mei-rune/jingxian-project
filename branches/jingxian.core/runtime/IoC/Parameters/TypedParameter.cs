
using System;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class TypedParameter : ConstantParameter
    {
        Type _type;
        public Type Type
        {
            get { return _type; }
        }

        public TypedParameter(Type type, object value)
            : base(value, delegate( ParameterInfo pi ){ return pi.ParameterType == type;})
        {
            _type = Enforce.ArgumentNotNull(type, "type");
        }

		public static TypedParameter From<T>(T value)
		{
			return new TypedParameter(typeof(T), value);
		}
    }
}
