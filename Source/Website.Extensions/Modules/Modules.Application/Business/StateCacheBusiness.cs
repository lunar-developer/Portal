using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class StateCacheBusiness<T> : BasicCacheBusiness<T> where T : StateData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (StateData item in StateBusiness.GetAllState())
            {
                dictionary.TryAdd(item.StateCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return StateBusiness.GetState(key);
        }
    }
}
