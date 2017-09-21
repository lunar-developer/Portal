using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class PositionCacheBusiness<T> : BasicCacheBusiness<T> where T : PositionData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PositionData item in PositionBusiness.GetList())
            {
                dictionary.TryAdd(item.PositionCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return PositionBusiness.GetItem(key);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<PositionData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.PositionCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}