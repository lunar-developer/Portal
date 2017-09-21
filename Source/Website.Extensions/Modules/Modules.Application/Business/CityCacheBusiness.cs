using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CityCacheBusiness<T> : BasicCacheBusiness<T> where T : CityData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CityData item in CityBusiness.GetAllCity())
            {
                dictionary.TryAdd(item.CityCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return CityBusiness.GetCity(key);
        }
    }
}
