using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{

    public class ObjectMapper : IProperties, IDictionary<string, object>
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
            return Utils.invokeSetter(_instance, key, value, Converter.Instance);
        }

        public object Get(string key)
        {
            return Utils.invokeGetter(_instance, key);
        }

        public object Remove(string key)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _instanceType.GetProperties().Length; }
        }


        #region IEnumerable<KeyValuePair<string,object>> 成员


        public IEnumerator<KeyValuePair<string, object>> internalGetEnumerator()
        {
            foreach ( PropertyInfo propertyInfo in _instanceType.GetProperties())
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

        #endregion

        #region IDictionary<string,object> 成员

        public void Add(string key, object value)
        {
            Put(key, value);
        }

        public bool ContainsKey(string key)
        {
            return null != Get(key);
        }

        public ICollection<string> Keys
        {
            get
            {
                List<string> result = new List<string>();

                foreach (KeyValuePair<string, object> kp in this)
                {
                    result.Add(kp.Key);
                }
                return result;
            }
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            value = Get(key);
            return null != value;
        }

        public ICollection<object> Values
        {
            get
            {
                List<object> result = new List<object>();

                foreach (KeyValuePair<string, object> kp in this)
                {
                    result.Add(kp.Value);
                }
                return result;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,object>> 成员

        public void Add(KeyValuePair<string, object> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IProperties 成员


        public string ReadString(string key)
        {
            return ReadString( key , null  );
        }

        public string ReadString(string key, string defaultValue)
        {
            object value = Get(key);
            if (null == value)
                return defaultValue;
            return value.ToString();
        }

        public int ReadInt32(string key)
        {
            return ReadInt32(key, 0);
        }

        public int ReadInt32(string key, int defaultValue)
        {
            object value = Get(key);
            if (null == value)
                return defaultValue;
            return Convert.ToInt32( value );
        }

        public bool ReadBoolean(string key)
        {
            return ReadBoolean(key, false );
        }

        public bool ReadBoolean(string key, bool defaultValue)
        {
            object value = Get(key);
            if (null == value)
                return defaultValue;
            return Convert.ToBoolean(value);
        }

        #endregion
    }
}
