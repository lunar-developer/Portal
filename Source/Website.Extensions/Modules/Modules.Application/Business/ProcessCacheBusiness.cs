using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ProcessCacheBusiness<T> : BasicCacheBusiness<T> where T : ProcessData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ProcessData item in ProcessBusiness.GetAllProcessData())
            {
                dictionary.TryAdd(item.ProcessID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string processID)
        {
            return ProcessBusiness.GetProcesseData(processID);
        }
    }
}