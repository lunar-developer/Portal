using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class IdentityTypeCacheBusiness<T> : ICache where T : IdentityTypeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (IdentityTypeData item in IdentityTypeBusiness.GetAllIdentityType())
            {
                dictionary.TryAdd(item.IdentityTypeID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string identityTypeID)
        {
            return IdentityTypeBusiness.GetIdentityType(identityTypeID);
        }
    }
}