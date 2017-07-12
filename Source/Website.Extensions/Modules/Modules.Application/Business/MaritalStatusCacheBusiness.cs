using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class MaritalStatusCacheBusiness<T> : ICache where T : MaritalStatusData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (MaritalStatusData item in MaritalStatusBusiness.GetList())
            {
                dictionary.TryAdd(item.MaritalStatusCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return MaritalStatusBusiness.GetItem(key);
        }
    }
}