

using System;
using System.Collections.Generic;
using System.Globalization;

namespace jingxian.core.runtime.utilities.Collections.Generic
{
    using jingxian.core.runtime.Resources;

	public sealed class ObjectCache<TKey, TValue>
	{

		private Dictionary<TKey, WeakReference> cache;

		public ObjectCache()
		{
			cache = new Dictionary<TKey, WeakReference>();
		}

		public ObjectCache(int capacity)
		{
			cache = new Dictionary<TKey, WeakReference>(capacity);
		}

        public bool TryGetValue(TKey key, out TValue value)
        {
            WeakReference reference = null;
            if (cache.TryGetValue(key, out reference))
            {
                if (reference.IsAlive)
                {
                    value = (TValue)reference.Target;
                    return true;
                }

                cache.Remove(key);
            }
            value = default(TValue);
            return false;
        }

		public TValue this[TKey key]
		{
            get
            {

                TValue value;
                if (TryGetValue(key, out value))
                    return value;

                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Error.KeyNotExistInObjectCache, key));
            }

            set
            {
                cache[key] = new WeakReference(value, false);
            }
		}

        public bool ContainsKey(TKey key)
        {
            WeakReference reference = null;
            if (cache.TryGetValue(key, out reference))
            {
                if (reference.IsAlive)
                    return true;

                cache.Remove(key);
            }
            return false;
        }


        private bool IsObjectAlive(ref TKey key)
        {
            WeakReference reference = null;
            if (cache.TryGetValue(key, out reference))
                return reference.IsAlive;

            return false;
        }
	}
}