using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CompanyCacheBusiness<T> : ICache where T : CompanyData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CompanyData item in CompanyBusiness.GetAllCompany())
            {
                dictionary.TryAdd(item.TaxCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string taxCode)
        {
            return CompanyBusiness.GetCompany(taxCode);
        }
    }
}