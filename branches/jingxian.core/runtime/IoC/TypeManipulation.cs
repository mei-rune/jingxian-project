

using System;
using System.ComponentModel;

namespace jingxian.core.runtime
{
    public class TypeManipulation
    {
        public static object ChangeToCompatibleType(object value, Type destinationType)
        {
            Enforce.ArgumentNotNull(destinationType, "destinationType");

            if (value == null)
            {
                if (destinationType.IsValueType)
                    return Activator.CreateInstance(destinationType);

                return null;
            }

            if (destinationType.IsAssignableFrom(value.GetType()))
                return value;

			return TypeDescriptor.GetConverter(destinationType).ConvertFrom(value);
        }
    }
}
