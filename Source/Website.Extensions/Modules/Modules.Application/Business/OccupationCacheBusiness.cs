using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class OccupationCacheBusiness<T> : ICache where T : OccupationData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (OccupationData item in OccupationBusiness.GetAllOccupation())
            {
                dictionary.TryAdd(item.OccupationCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string occupationCode)
        {
            return OccupationBusiness.GetOccupation(occupationCode);
        }
    }
}