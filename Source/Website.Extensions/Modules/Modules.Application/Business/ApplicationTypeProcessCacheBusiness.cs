using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ApplicationTypeProcessCacheBusiness<T> : BasicCacheBusiness<T> where T : ApplicationTypeProcessData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationTypeProcessData item in ApplicationTypeProcessBusiness.GetAllApplicationTypeProcess())
            {
                dictionary.TryAdd(item.ApplicationTypeID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string applicationTypeID)
        {
            return ApplicationTypeProcessBusiness.GetApplicationTypeProcess(applicationTypeID);
        }
    }
}