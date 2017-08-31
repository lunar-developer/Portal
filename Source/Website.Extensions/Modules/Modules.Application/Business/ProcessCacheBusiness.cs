using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ProcessCacheBusiness<T> : ICache where T : ProcessData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ProcessData item in ProcessBusiness.GetAllProcessData())
            {
                dictionary.TryAdd(item.ProcessID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string processID)
        {
            return ProcessBusiness.GetProcesseData(processID);
        }
    }
}