using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CorporateStatusCacheBusiness<T> : ICache where T : CorporateStatusData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateStatusData item in CorporateStatusBusiness.GetAllCorporateStatus())
            {
                dictionary.TryAdd(item.CorporateStatusCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string corporateStatusCode)
        {
            return CorporateStatusBusiness.GetCorporateStatus(corporateStatusCode);
        }
    }
}