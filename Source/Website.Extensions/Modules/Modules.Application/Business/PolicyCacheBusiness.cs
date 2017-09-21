using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class PolicyCacheBusiness<T> : BasicCacheBusiness<T> where T : PolicyData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PolicyData item in PolicyBusiness.GetAllPolicy())
            {
                dictionary.TryAdd(item.PolicyID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string policyId)
        {
            return PolicyBusiness.GetPolicy(policyId);
        }
    }
}