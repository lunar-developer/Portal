using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CorporateSizeCacheBusiness<T> : ICache where T : CorporateSizeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateSizeData item in CorporateSizeBusiness.GetAllCorporateSize())
            {
                dictionary.TryAdd(item.CorporateSizeCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string corporateSizeCode)
        {
            return CorporateSizeBusiness.GetCorporateSize(corporateSizeCode);
        }
    }
}