using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class ReasonMappingCustomerTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : ReasonMappingCustomerTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ReasonMappingCustomerTypeData item in ReasonMappingCustomerTypeBusiness.GetAll())
            {
                dictionary.TryAdd(item.ID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return ReasonMappingCustomerTypeBusiness.GetItem(key);
        }
    }
}
