using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Website.Library.Extension
{
    public class OrderedConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private readonly object Locker = new object();
        public List<TKey> SortKeys { get; private set; } = new List<TKey>();


        public new bool TryAdd(TKey key, TValue value)
        {
            if (base.TryAdd(key, value) == false)
            {
                return false;
            }

            Monitor.TryEnter(Locker);
            SortKeys.Add(key);
            Monitor.Exit(Locker);
            return true;
        }

        public new bool TryRemove(TKey key, out TValue value)
        {
            if (base.TryRemove(key, out value) == false)
            {
                return false;
            }

            Monitor.TryEnter(Locker);
            SortKeys.Remove(key);
            Monitor.Exit(Locker);
            return true;
        }

        public bool TryArrange(List<TKey> keys)
        {
            Monitor.TryEnter(Locker);
            if (SortKeys.Count != keys.Count)
            {
                return false;
            }
            SortKeys = keys;
            Monitor.Exit(Locker);
            return true;
        }
    }
}