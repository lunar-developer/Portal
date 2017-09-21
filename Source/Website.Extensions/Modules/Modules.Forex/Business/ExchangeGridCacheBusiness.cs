using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class ExchangeGridCacheBusiness<T> : BasicCacheBusiness<T> where T : ExchangeRateGridData
    {

        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ExchangeRateGridData item in ExchangeRateBusiness.GetGridData())
            {
                dictionary.TryAdd(item.CurrencyCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return ExchangeRateBusiness.GetGridItem(key);
        }
    }
}
