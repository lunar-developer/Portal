using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ApplicationTypeCacheBusiness<T> : ICache where T : ApplicationTypeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationTypeData item in ApplicationTypeBusiness.GetAllApplicationType())
            {
                dictionary.TryAdd(item.ApplicationTypeID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string applicationTypeID)
        {
            return ApplicationTypeBusiness.GetApplicationType(applicationTypeID);
        }
    }
}