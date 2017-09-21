using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class LanguageCacheBusiness<T> : BasicCacheBusiness<T> where T : LanguageData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (LanguageData item in LanguageBusiness.GetAllLanguage())
            {
                dictionary.TryAdd(item.LanguageID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return LanguageBusiness.GetLanguage(key);
        }
    }
}