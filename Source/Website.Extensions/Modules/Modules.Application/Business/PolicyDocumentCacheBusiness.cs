using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class PolicyDocumentCacheBusiness<T> : ICache where T : PolicyDocumentData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
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

        public CacheData Reload(string key)
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