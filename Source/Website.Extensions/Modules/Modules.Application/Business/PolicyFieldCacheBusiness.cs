using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Business;

namespace Modules.Application.Business
{
    public class PolicyFieldCacheBusiness<T> : BasicCacheBusiness<T> where T : PolicyFieldData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PolicyFieldData item in PolicyFieldBusiness.GetAllPolicyField())
            {
                string key = PolicyDocumentBusiness.GetCacheKey(item.PolicyCode, item.FieldName);
                dictionary.TryAdd(key, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            string[] arrayValues = key.Split('-');
            if (arrayValues.Length != 2)
            {
                return null;
            }

            string policyCode = arrayValues[0];
            string fieldName = arrayValues[1];
            return PolicyFieldBusiness.GetPolicyField(policyCode, fieldName);
        }
    }
}