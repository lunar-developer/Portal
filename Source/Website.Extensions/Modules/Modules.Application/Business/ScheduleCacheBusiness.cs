using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ScheduleCacheBusiness<T> : BasicCacheBusiness<T> where T : ScheduleData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ScheduleData item in ScheduleBusiness.GetAllScheduleData())
            {
                dictionary.TryAdd(item.ScheduleCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return ScheduleBusiness.GetScheduleData(key);
        }
    }
}