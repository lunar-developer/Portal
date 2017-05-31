using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Website.Library.Extension
{
    public class OrderedConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        public List<TKey> SortKeys { get; } = new List<TKey>();

        public new bool TryAdd(TKey key, TValue value)
        {
            if (base.TryAdd(key, value) == false)
            {
                return false;
            }            
            SortKeys.Add(key);
            return true;
        }

        public new bool TryRemove(TKey key, out TValue value)
        {
            if (base.TryRemove(key, out value) == false)
            {
                return false;
            }

            SortKeys.Remove(key);
            return true;
        }
    }
}