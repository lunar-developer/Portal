using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class PositionCacheBusiness<T> : ICache where T : PositionData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PositionData item in PositionBusiness.GetList())
            {
                dictionary.TryAdd(item.PositionCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return PositionBusiness.GetItem(key);
        }
    }
}