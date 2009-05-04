using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian
{

    public class ObjectMapper : IEnumerable< KeyValuePair< string, object> >
    {
        object _instance;
        Type _instanceType;

        public ObjectMapper(object instance)
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            _instance = instance;
            _instanceType = _instance.GetType();
        }

        public object this[string key]
        {
            get { return Get(key); }
            set { Put(key, value); }
        }

        public object Put(string key, object value)
        {
            try
            {
                return Utils.invokeSetter(_instance, key, value);
            }
            catch
            { }
            return null;
        }

        public object Get(string key)
        {
            return Utils.invokeGetter(_instance, key);
        }

        public int Count
        {
            get { return _instanceType.GetProperties().Length; }
        }

        public IEnumerator<KeyValuePair<string, object>> internalGetEnumerator()
        {
            foreach (PropertyInfo propertyInfo in _instanceType.GetProperties())
            {
                if (null == propertyInfo)
                    continue;
                if (!propertyInfo.CanRead)
                    continue;

                yield return new KeyValuePair<string, object>(propertyInfo.Name, propertyInfo.GetValue(_instance, null));
            }

            foreach (FieldInfo fieldInfo in _instanceType.GetFields())
            {
                yield return new KeyValuePair<string, object>(fieldInfo.Name, fieldInfo.GetValue(_instance));
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return internalGetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return internalGetEnumerator();
        }
    }
}
