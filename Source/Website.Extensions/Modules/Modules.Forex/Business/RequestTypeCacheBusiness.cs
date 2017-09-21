using System;
using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Forex.Business
{
    public class RequestTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : RequestTypeData
    {

        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (RequestTypeData item in RequestTypeBusiness.GetAll())
            {
                dictionary.TryAdd(item.ID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return RequestTypeBusiness.GetItem(key);
        }
    }
}
