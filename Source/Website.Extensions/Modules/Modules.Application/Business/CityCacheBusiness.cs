using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CityCacheBusiness<T> : ICache where T : CityData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CityData item in CityBusiness.GetAllCity())
            {
                dictionary.TryAdd(item.CityCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return CityBusiness.GetCity(key);
        }
    }
}
