using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class LanguageCacheBusiness<T> : ICache where T : LanguageData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (LanguageData item in LanguageBusiness.GetAllLanguage())
            {
                dictionary.TryAdd(item.LanguageID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return LanguageBusiness.GetLanguage(key);
        }
    }
}