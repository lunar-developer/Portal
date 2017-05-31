using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CountryCacheBusiness<T> : ICache where T : CountryData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CountryData item in CountryBusiness.GetAllCountry())
            {
                dictionary.TryAdd(item.CountryCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return CountryBusiness.GetCountry(key);
        }
    }
}