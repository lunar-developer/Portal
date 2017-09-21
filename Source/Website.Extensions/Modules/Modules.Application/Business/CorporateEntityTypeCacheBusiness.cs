using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CorporateEntityTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : CorporateEntityTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateEntityTypeData item in CorporateEntityTypeBusiness.GetAllCorporateEntityType())
            {
                dictionary.TryAdd(item.CorporateEntityTypeCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string corporateEntityTypeCode)
        {
            return CorporateEntityTypeBusiness.GetCorporateEntityType(corporateEntityTypeCode);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<CorporateEntityTypeData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.CorporateEntityTypeCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}