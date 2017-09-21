using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class PolicyDocumentCacheBusiness<T> : BasicCacheBusiness<T> where T : PolicyDocumentData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PolicyDocumentData item in PolicyDocumentBusiness.GetAllDocumentType())
            {
                string key = PolicyDocumentBusiness.GetCacheKey(item.PolicyID, item.DocumentTypeID);
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

            string policyID = arrayValues[0];
            string documentTypeID = arrayValues[1];
            return PolicyDocumentBusiness.GetDocumentType(policyID, documentTypeID);
        }
    }
}