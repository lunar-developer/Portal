using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class IncompleteReasonCacheBusiness<T> : BasicCacheBusiness<T> where T : IncompleteReasonData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (IncompleteReasonData item in IncompleteReasonBusiness.GetAllIncompleteReason())
            {
                dictionary.TryAdd(item.IncompleteReasonCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string incompleteCode)
        {
            return IncompleteReasonBusiness.GetIncompleteReason(incompleteCode);
        }
    }
}