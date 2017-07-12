using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class SICCacheBusiness<T> : ICache where T : SICData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (SICData item in SICBusiness.GetList())
            {
                dictionary.TryAdd(item.SICCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return SICBusiness.GetItem(key);
        }
    }
}