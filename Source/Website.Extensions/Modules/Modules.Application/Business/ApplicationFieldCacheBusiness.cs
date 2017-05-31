using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ApplicationFieldCacheBusiness<T> : ICache where T : ApplicationFieldData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationFieldData item in ApplicationFieldBusiness.GetAllField())
            {
                dictionary.TryAdd(item.FieldName, item);
            }
            return dictionary;
        }

        public CacheData Reload(string fieldName)
        {
            return ApplicationFieldBusiness.GetField(fieldName);
        }
    }
}