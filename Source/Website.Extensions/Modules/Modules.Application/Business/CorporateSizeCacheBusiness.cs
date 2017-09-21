using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CorporateSizeCacheBusiness<T> : BasicCacheBusiness<T> where T : CorporateSizeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CorporateSizeData item in CorporateSizeBusiness.GetAllCorporateSize())
            {
                dictionary.TryAdd(item.CorporateSizeCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string corporateSizeCode)
        {
            return CorporateSizeBusiness.GetCorporateSize(corporateSizeCode);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<CorporateSizeData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.CorporateSizeCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}