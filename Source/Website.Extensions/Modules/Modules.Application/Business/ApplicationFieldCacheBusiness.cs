using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ApplicationFieldCacheBusiness<T> : BasicCacheBusiness<T> where T : ApplicationFieldData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ApplicationFieldData item in ApplicationFieldBusiness.GetAllFields())
            {
                dictionary.TryAdd(item.FieldName, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string fieldName)
        {
            return ApplicationFieldBusiness.GetField(fieldName);
        }
    }
}