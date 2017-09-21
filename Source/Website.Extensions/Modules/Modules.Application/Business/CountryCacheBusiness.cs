using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CountryCacheBusiness<T> : BasicCacheBusiness<T> where T : CountryData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CountryData item in CountryBusiness.GetAllCountry())
            {
                dictionary.TryAdd(item.CountryCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return CountryBusiness.GetCountry(key);
        }
    }
}