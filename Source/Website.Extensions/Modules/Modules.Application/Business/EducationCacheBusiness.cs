using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class EducationCacheBusiness<T> : ICache where T : EducationData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (EducationData item in EducationBusiness.GetList())
            {
                dictionary.TryAdd(item.EducationCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return EducationBusiness.GetItem(key);
        }
    }
}