using System;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.UserManagement.Business
{
    public class UserCacheBusiness<T> : ICache where T : UserData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary
                = new OrderedConcurrentDictionary<string, CacheData>();
            foreach (UserData item in UserBusiness.GetUserExtension())
            {
                dictionary.TryAdd(item.UserID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string userID)
        {
            return UserBusiness.GetUserExtension(userID);
        }
    }
}