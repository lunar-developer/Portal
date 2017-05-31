using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class PhaseMappingCacheBussiness<T> : ICache where T : PhaseMappingListData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PhaseMappingListData item in PhaseMappingBussiness.GetListPhaseMapping())
            {
                dictionary.TryAdd(item.ApplicationTypeCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return PhaseMappingBussiness.GetPhaseMapping(key);
        }
    }
}
