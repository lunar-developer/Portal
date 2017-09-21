using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CorporateStatusCacheBusiness<T> : BasicCacheBusiness<T> where T : CorporateStatusData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateStatusData item in CorporateStatusBusiness.GetAllCorporateStatus())
            {
                dictionary.TryAdd(item.CorporateStatusCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string corporateStatusCode)
        {
            return CorporateStatusBusiness.GetCorporateStatus(corporateStatusCode);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<CorporateStatusData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.CorporateStatusCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}