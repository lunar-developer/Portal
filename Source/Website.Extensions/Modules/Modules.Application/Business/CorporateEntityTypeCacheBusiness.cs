using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CorporateEntityTypeCacheBusiness<T> : ICache where T : CorporateEntityTypeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateEntityTypeData item in CorporateEntityTypeBusiness.GetAllCorporateEntityType())
            {
                dictionary.TryAdd(item.CorporateEntityTypeCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string corporateEntityTypeCode)
        {
            return CorporateEntityTypeBusiness.GetCorporateEntityType(corporateEntityTypeCode);
        }
    }
}