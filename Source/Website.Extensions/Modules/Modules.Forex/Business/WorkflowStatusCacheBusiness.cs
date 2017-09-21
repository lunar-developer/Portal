using System;
using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class WorkflowStatusCacheBusiness<T> : BasicCacheBusiness<T> where T : WorkflowStatusData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (WorkflowStatusData item in WorkflowStatusBusiness.GetAll())
            {
                dictionary.TryAdd(item.ID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return WorkflowStatusBusiness.GetItem(key);
        }
    }
}
