using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Website.Library.Business
{
    public abstract class BasicCacheBusiness<T> : ICache where T : class
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public virtual bool Arrange(OrderedConcurrentDictionary<string, CacheData> cacheDictionary)
        {
            return true;
        }

        public abstract OrderedConcurrentDictionary<string, CacheData> Load();

        public virtual CacheData Reload(string key)
        {
            return null;
        }
    }
}