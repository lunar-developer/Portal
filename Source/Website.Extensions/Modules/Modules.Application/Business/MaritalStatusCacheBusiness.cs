using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class MaritalStatusCacheBusiness<T> : BasicCacheBusiness<T> where T : MaritalStatusData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (MaritalStatusData item in MaritalStatusBusiness.GetList())
            {
                dictionary.TryAdd(item.MaritalStatusCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return MaritalStatusBusiness.GetItem(key);
        }
    }
}