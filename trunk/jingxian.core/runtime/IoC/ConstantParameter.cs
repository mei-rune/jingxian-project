using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public abstract class ConstantParameter : IParameter
    {
        Predicate<ParameterInfo> _predicate;
        object _value;

        public object Value
        { get { return _value; } }

        protected ConstantParameter(object value, Predicate<ParameterInfo> predicate)
        {
            _value = value;
            _predicate = Enforce.ArgumentNotNull(predicate, "predicate");
        }


        public bool TryGetProvider(ParameterInfo pi, ICreationContext context, out Func<object> valueProvider)
        {
            Enforce.ArgumentNotNull(pi, "pi");

            if (_predicate(pi))
            {
                valueProvider = delegate() { return MatchTypes(pi, _value); };
                return true;
            }
            else
            {
                valueProvider = null;
                return false;
            }
        }

        protected object MatchTypes(ParameterInfo pi, object parameterValue)
        {
            return TypeManipulation.ChangeToCompatibleType(parameterValue, pi.ParameterType);
        }
    }
}
