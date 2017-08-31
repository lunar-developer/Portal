using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Global;
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

        public List<string> Arrange()
        {
            List<CustomerClassData> list = CacheBase.Receive<CustomerClassData>();
            return list.OrderBy(item => int.Parse(item.SortOrder)).Select(item => item.CustomerClassCode).ToList();
        }
    }
}