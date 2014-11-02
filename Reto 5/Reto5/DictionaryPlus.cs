using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto5
{
    public class DictionaryPlus<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public IEnumerable<TValue> this[params TKey[] keys] 
        { 
            get
            {
                foreach (var item in keys)
                {
                    yield return base[item];
                }
            }
        }
    }
}
