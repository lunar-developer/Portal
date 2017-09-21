using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Website.Library.Interface
{
    public interface ICache
    {
        Type GetCacheType();
        OrderedConcurrentDictionary<string, CacheData> Load();
        CacheData Reload(string key);
        bool Arrange(OrderedConcurrentDictionary<string, CacheData> cacheDictionary);
    }
}