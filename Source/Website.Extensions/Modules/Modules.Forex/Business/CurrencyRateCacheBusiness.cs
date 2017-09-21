using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class CurrencyRateCacheBusiness<T> : BasicCacheBusiness<T> where T : CurrencyRateData
    {


        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CurrencyRateData item in CurrencyRateBusiness.GetAll())
            {
                dictionary.TryAdd(item.CurrencyCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return CurrencyRateBusiness.GetItem(key);
        }
    }
}
