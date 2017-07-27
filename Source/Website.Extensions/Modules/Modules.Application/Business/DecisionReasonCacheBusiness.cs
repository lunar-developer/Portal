using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class DecisionReasonCacheBusiness<T> : ICache where T : DecisionReasonData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (DecisionReasonData item in DecisionReasonBusiness.GetAllDecisionReason())
            {
                dictionary.TryAdd(item.DecisionReasonCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string decisionCode)
        {
            return DecisionReasonBusiness.GetDecisionReason(decisionCode);
        }
    }
}