using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class WardCacheBusiness<T> : BasicCacheBusiness<T> where T : WardData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary = new OrderedConcurrentDictionary<string, CacheData>();
            foreach(WardData data in WardBusiness.GetAllWard())
            {
                dictionary.TryAdd(data.WardCode, data);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return WardBusiness.GetWard(key);
        }
    }
}