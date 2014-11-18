using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto_6
{
    [DebuggerTypeProxy(typeof(Cache.CacheDebugView))]
    [System.Diagnostics.DebuggerDisplay("Count = {Count}, ActiveCount = {ActiveCount}")]
    public class Cache : IEnumerable<Cache.CacheEntry>
    {
        [System.Diagnostics.DebuggerDisplay("Key = {Key}, Value = {Value}")]
        public class CacheEntry
        {
            public object Key { get; set; }
            public object Value { get; set; }
        }

        class CacheEntryEnumerator : IEnumerator<CacheEntry>
        {
            IEnumerator<KeyValuePair<object, WeakReference>> innerEnumerator;
            protected internal CacheEntryEnumerator(IDictionary<object, WeakReference> cacheEntries)
            {
                if (cacheEntries == null)
                    throw new ArgumentNullException("cacheEntries");
                innerEnumerator = cacheEntries.GetEnumerator();
            }
            public CacheEntry Current
            {
                get
                {
                    return new CacheEntry
                    {
                        Key = innerEnumerator.Current.Key,
                        Value = innerEnumerator.Current.Value.IsAlive ? innerEnumerator.Current.Value.Target : null
                    };
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Dispose()
            {
                innerEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return innerEnumerator.MoveNext();
            }

            public void Reset()
            {
                innerEnumerator.Reset();
            }
        }

        // internal debug view class for cache
        internal class CacheDebugView
        {
            private Cache cache;

            public CacheDebugView(Cache cache)
            {
                if (cache == null)
                {
                    throw new ArgumentNullException("cache");
                }

                this.cache = cache;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public CacheEntry[] Items
            {
                get
                {
                    return cache.ToArray();
                }
            }
        }

        Dictionary<object, WeakReference> _cache = new Dictionary<object, WeakReference>();

        public int ActiveCount
        {
            get
            {
                return _cache.Where(d => d.Value.IsAlive).Count();
            }
        }
        public int Count
        {
            get
            {
                return _cache.Count;
            }
        }

        public void Add(object key, object value)
        {
            _cache.Add(key, new WeakReference(value));            
        }

        public object this[object key]
        {
            get
            {
                WeakReference reference;
                if (_cache.TryGetValue(key, out reference))
                {
                    if (reference.IsAlive)
                        return reference.Target;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerator<CacheEntry> GetEnumerator()
        {
            return new CacheEntryEnumerator(this._cache);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
