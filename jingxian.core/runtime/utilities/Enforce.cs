

using System;
using System.Globalization;
using System.Reflection;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.utilities;

	public static class Enforce
	{
        public static T ArgumentNotNull<T>(T value, string name)
            where T : class
		{
			if (name == null)
				throw new ArgumentNullException("name");
			
			if (value == null)
				throw new ArgumentNullException(name);

            return value;
		}

        public static object NotNull(object value, string description)
        {
            if (value == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    EnforceResources.CannotBeNull, description ));

            return value;
        }

        public static T NotNull<T>(T value)
            where T : class
        {
            return NotNull<T>(value, typeof(T).FullName);
        }

        public static T NotNull<T>(T value, string description)
            where T : class
        {
            if (value == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    EnforceResources.CannotBeNull, description ));

            return value;
        }

        public static string ArgumentNotNullOrEmpty(string value, string description)
        {
            Enforce.ArgumentNotNull(description, "description");
            Enforce.ArgumentNotNull(value, description);
            
            if (value == string.Empty)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    EnforceResources.CannotBeEmpty, description));

            return value;
        }

        public static void ArgumentTypeIsFunction(Type delegateType)
        {
            Enforce.ArgumentNotNull(delegateType, "delegateType");

            MethodInfo invoke = delegateType.GetMethod("Invoke");
            if (invoke == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    EnforceResources.NotDelegate, delegateType));
            else if (invoke.ReturnType == typeof(void))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    EnforceResources.DelegateReturnsVoid, delegateType));
        }
    }
}
