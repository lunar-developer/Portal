using Modules.Forex.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Forex.Business
{
    public class ActionCacheBusiness<T> : BasicCacheBusiness<T> where T : ActionData
    {

        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ActionData item in ActionBusiness.GetAll())
            {
                dictionary.TryAdd(item.ID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return ActionBusiness.GetItem(key);
        }

    }
}
