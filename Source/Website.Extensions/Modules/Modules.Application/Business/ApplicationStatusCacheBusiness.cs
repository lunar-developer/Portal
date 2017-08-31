using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ApplicationStatusCacheBusiness<T> : ICache where T : ApplicationStatusData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationStatusData item in ApplicationStatusBusiness.GetAllApplicationStatus())
            {
                dictionary.TryAdd(item.ApplicationStatusID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string applicationStatusID)
        {
            return ApplicationStatusBusiness.GetApplicationStatus(applicationStatusID);
        }
    }
}