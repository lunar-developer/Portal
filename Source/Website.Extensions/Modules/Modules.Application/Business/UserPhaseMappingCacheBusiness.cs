using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class UserPhaseMappingCacheBusiness<T> : ICache where T : UserPhaseMappingData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }
        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (UserPhaseMappingData item in UserPhaseMappingBusiness.GetListUserPhaseMapping())
            {
                dictionary.TryAdd(item.UserPhaseMappingID, item);
            }
            return dictionary;
        }
        public CacheData Reload(string id)
        {
            return UserPhaseMappingBusiness.GetUserPhaseMapping(id);
        }
    }
}
