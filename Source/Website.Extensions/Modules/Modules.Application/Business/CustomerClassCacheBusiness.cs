using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class CustomerClassCacheBusiness<T> : BasicCacheBusiness<T> where T : CustomerClassData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (CustomerClassData item in CustomerClassBusiness.GetAllCustomerClass())
            {
                dictionary.TryAdd(item.CustomerClassCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return CustomerClassBusiness.GetCustomerClass(key);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<CustomerClassData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.CustomerClassCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}