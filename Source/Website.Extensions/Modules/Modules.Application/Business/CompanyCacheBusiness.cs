using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CompanyCacheBusiness<T> : BasicCacheBusiness<T> where T : CompanyData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CompanyData item in CompanyBusiness.GetAllCompany())
            {
                dictionary.TryAdd(item.TaxCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string taxCode)
        {
            return CompanyBusiness.GetCompany(taxCode);
        }
    }
}