using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class CustomerClassCacheBusiness<T> : ICache where T : CustomerClassData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CustomerClassData item in CustomerClassBusiness.GetAllCustomerClass())
            {
                dictionary.TryAdd(item.CustomerClassCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return CustomerClassBusiness.GetCustomerClass(key);
        }
    }
}