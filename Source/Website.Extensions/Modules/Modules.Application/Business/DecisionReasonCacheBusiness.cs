using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class DecisionReasonCacheBusiness<T> : BasicCacheBusiness<T> where T : DecisionReasonData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (DecisionReasonData item in DecisionReasonBusiness.GetAllDecisionReason())
            {
                dictionary.TryAdd(item.DecisionReasonCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string decisionCode)
        {
            return DecisionReasonBusiness.GetDecisionReason(decisionCode);
        }
    }
}