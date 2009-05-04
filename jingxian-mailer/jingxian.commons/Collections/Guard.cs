

using System;

namespace jingxian.collections
{
    public static class Guard
    {
        #region Methods

        public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("×Ö·û´®²»ÄÜÎª¿Õ!", argumentName);
            }
        }


        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        #endregion
    }
}