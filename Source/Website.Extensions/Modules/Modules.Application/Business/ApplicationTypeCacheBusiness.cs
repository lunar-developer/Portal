using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ApplicationTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : ApplicationTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationTypeData item in ApplicationTypeBusiness.GetAllApplicationTypes())
            {
                dictionary.TryAdd(item.ApplicationTypeID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string applicationTypeID)
        {
            return ApplicationTypeBusiness.GetApplicationType(applicationTypeID);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<ApplicationTypeData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.ApplicationTypeID)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}