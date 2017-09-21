using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class SICCacheBusiness<T> : BasicCacheBusiness<T> where T : SICData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (SICData item in SICBusiness.GetList())
            {
                dictionary.TryAdd(item.SICCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return SICBusiness.GetItem(key);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<SICData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.SICCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}