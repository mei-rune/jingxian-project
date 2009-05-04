using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian
{
    public interface IAccessor
    {
        Type InterfaceType { get; }

        object GetValue(object instance);
        void SetValue(object instance, object value);
    }

    public class PropertyAccessor : IAccessor
    {
        PropertyInfo _propertyInfo;
        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        #region IAccessor 成员

        public Type InterfaceType
        {
            get { return _propertyInfo.PropertyType; }
        }

        public object GetValue(object instance)
        {
            return _propertyInfo.GetValue(instance, null);
        }

        public void SetValue(object instance, object value)
        {
            _propertyInfo.SetValue(instance, value, null);
        }

        #endregion
    }
}
