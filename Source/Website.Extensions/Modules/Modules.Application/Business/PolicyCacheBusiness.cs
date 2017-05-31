using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class PolicyCacheBusiness<T> : ICache where T : PolicyData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PolicyData item in PolicyBusiness.GetAllPolicy())
            {
                dictionary.TryAdd(item.PolicyID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string policyId)
        {
            return PolicyBusiness.GetPolicy(policyId);
        }
    }
}