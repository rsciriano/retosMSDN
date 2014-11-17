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
    public class Cache 
    {
        Dictionary<object, WeakReference> _cache = new Dictionary<object, WeakReference>();

        [System.Diagnostics.DebuggerDisplay("Key = {Key}, Value = {Value}")]
        public class KeyValuePairs
        {
            public object Key { get; set; }
            public object Value { get; set; }
        }

        // internal debug view class for hashtable
        internal class CacheDebugView
        {
            Dictionary<object, WeakReference> _cache = new Dictionary<object, WeakReference>();

            public CacheDebugView(Cache cache)
            {
                if (cache == null)
                {
                    throw new ArgumentNullException("cache");
                }

                this._cache = cache._cache;
            }


            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePairs[] Items
            {
                get
                {
                    return _cache.Select(v => new KeyValuePairs { Key = v.Key, Value = v.Value.IsAlive ? v.Value.Target : null }).ToArray();
                }
            }
        }

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
    }
}
