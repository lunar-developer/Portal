using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class HomeOwnershipCacheBussiness<T> : ICache where T : HomeOwnershipData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (HomeOwnershipData item in HomeOwnershipBussiness.GetList())
            {
                dictionary.TryAdd(item.HomeOwnershipCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return HomeOwnershipBussiness.GetItem(key);
        }
    }
}