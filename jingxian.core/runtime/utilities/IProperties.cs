using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IProperties : IEnumerable<KeyValuePair<string, object>>
    {
        int Count { get; }

        object this[string key] { get; set; }

        object Put(string key, object value);

        object Get(string key);

        string ReadString(string key);
        string ReadString(string key, string defaultValue );

        int ReadInt32(string key);
        int ReadInt32(string key, int defaultValue);

        bool ReadBoolean(string key);
        bool ReadBoolean(string key, bool defaultValue);

        object Remove(string key);

        void Clear();
    }

    public abstract class AbstractProperties : IProperties 
    {
        public abstract int Count { get;  }

        public abstract object this[string key] { get; set; }

        public abstract object Put(string key, object value);

        public abstract object Get(string key);

        public abstract object Remove(string key);

        public abstract void Clear();

        public abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();


        public string ReadString(string key)
        {
            return ReadString(null);
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
            return ReadBoolean(key, false);
        }

        public bool ReadBoolean(string key, bool defaultValue)
        {
            object value = Get(key);
            if (null == value)
                return defaultValue;
            return Convert.ToBoolean(value);
        }



        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class MapProperties : AbstractProperties, IDictionary<string, object>
    {
        IProperties _parent = null;
        IDictionary<string, object> _container;
        
        public MapProperties()
        {
            _container = new Dictionary<string, object>();
        }

        public MapProperties(IEqualityComparer<string> comparer)
        {
            _container = new Dictionary<string, object>(comparer);
        }

        public MapProperties(IDictionary<string, object> container)
        {
            _container = new Dictionary<string, object>(container);
        }

        public MapProperties(IEnumerable<KeyValuePair<string, object>> container)
        {
            _container = new Dictionary<string, object>(  );
            if (null != container)
            {
                foreach (KeyValuePair<string, object> kp in container)
                {
                    _container[kp.Key] = kp.Value;
                }
            }
        }

        public void SetParent(IProperties properties)
        {
            _parent = properties;
        }

        #region IProperties 成员

        public override object this[string key]
        {
            get { return Get(key); }
            set { Put(key, value); }
        }

        public override object Put(string key, object value)
        {
            object old = null;
            _container.TryGetValue(key, out old);
            _container[key] = value;
            return old??value;
        }

        public override object Get(string key)
        {
            object old = null;
            if( _container.TryGetValue(key, out old) )
                return old;

            if( null != _parent )
                return _parent.Get(key);
            return null;
        }

        public override object Remove(string key)
        {
            object old = null;
            if (_container.TryGetValue(key, out old))
                _container.Remove(key);
            return old;
        }

        public override void Clear()
        {
            _container.Clear();
        }

        public override int Count
        {
            get { return _container.Count; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> 成员

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        #endregion


        #region IDictionary<string,object> 成员

        public void Add(string key, object value)
        {
            Put(key, value);
        }

        public bool ContainsKey(string key)
        {
            if (_container.ContainsKey(key))
                return true;
            return null != _parent.Get(key);
        }

        public ICollection<string> Keys
        {
            get { return _container.Keys; }
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return _container.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            if (_container.TryGetValue(key, out value))
                return true;

            if (null == _parent)
                return false;

            value = _parent.Get(key);
            return null != value;
        }

        public ICollection<object> Values
        {
            get { return _container.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<string,object>> 成员

        public void Add(KeyValuePair<string, object> item)
        {
            _container.Add(item);
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _container.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _container.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return _container.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _container.Remove(item);
        }

        #endregion
    }

}
