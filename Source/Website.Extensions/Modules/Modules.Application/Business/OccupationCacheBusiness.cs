using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class OccupationCacheBusiness<T> : BasicCacheBusiness<T> where T : OccupationData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (OccupationData item in OccupationBusiness.GetAllOccupation())
            {
                dictionary.TryAdd(item.OccupationCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string occupationCode)
        {
            return OccupationBusiness.GetOccupation(occupationCode);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<OccupationData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.OccupationCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}