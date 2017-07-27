using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class IncompleteReasonCacheBusiness<T> : ICache where T : IncompleteReasonData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (IncompleteReasonData item in IncompleteReasonBusiness.GetAllIncompleteReason())
            {
                dictionary.TryAdd(item.IncompleteReasonCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string incompleteCode)
        {
            return IncompleteReasonBusiness.GetIncompleteReason(incompleteCode);
        }
    }
}