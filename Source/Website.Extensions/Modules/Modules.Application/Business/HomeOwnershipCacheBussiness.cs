using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class HomeOwnershipCacheBussiness<T> : BasicCacheBusiness<T> where T : HomeOwnershipData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (HomeOwnershipData item in HomeOwnershipBussiness.GetList())
            {
                dictionary.TryAdd(item.HomeOwnershipCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return HomeOwnershipBussiness.GetItem(key);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<HomeOwnershipData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.HomeOwnershipCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}