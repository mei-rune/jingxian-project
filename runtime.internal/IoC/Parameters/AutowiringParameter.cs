using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    internal class AutowiringParameter : IParameter
    {
        public bool TryGetProvider(System.Reflection.ParameterInfo pi, ICreationContext context, out Func<object> valueProvider)
        {
            IComponentRegistration registration;
            if (context.TryGetRegistered(pi.ParameterType, out registration ))
            {
                valueProvider = delegate() { return MatchTypes(pi, context.Get( registration )); };
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
