using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ApplicationStatusCacheBusiness<T> : BasicCacheBusiness<T> where T : ApplicationStatusData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationStatusData item in ApplicationStatusBusiness.GetAllApplicationStatus())
            {
                dictionary.TryAdd(item.ApplicationStatusID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string applicationStatusID)
        {
            return ApplicationStatusBusiness.GetApplicationStatus(applicationStatusID);
        }
    }
}