using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ScheduleCacheBusiness<T> : ICache where T : ScheduleData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ScheduleData item in ScheduleBusiness.GetAllScheduleData())
            {
                dictionary.TryAdd(item.ScheduleName, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return ScheduleBusiness.GetScheduleData(key);
        }
    }
}