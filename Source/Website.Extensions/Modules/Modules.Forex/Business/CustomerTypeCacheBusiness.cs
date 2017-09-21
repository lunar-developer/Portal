using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class CustomerTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : CustomerTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CustomerTypeData item in CustomerTypeBusiness.GetAll())
            {
                dictionary.TryAdd(item.ID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return CustomerTypeBusiness.GetItem(key);
        }
    }
}
