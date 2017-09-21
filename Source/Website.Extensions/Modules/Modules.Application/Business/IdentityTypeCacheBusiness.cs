using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class IdentityTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : IdentityTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (IdentityTypeData item in IdentityTypeBusiness.GetAllIdentityType())
            {
                dictionary.TryAdd(item.IdentityTypeID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string identityTypeID)
        {
            return IdentityTypeBusiness.GetIdentityType(identityTypeID);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<IdentityTypeData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.IdentityTypeID)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}