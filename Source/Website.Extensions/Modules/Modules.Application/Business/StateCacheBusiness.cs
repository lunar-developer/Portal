using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class StateCacheBusiness<T> : ICache where T : StateData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (StateData item in StateBusiness.GetAllState())
            {
                dictionary.TryAdd(item.StateCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return StateBusiness.GetState(key);
        }
    }
}
