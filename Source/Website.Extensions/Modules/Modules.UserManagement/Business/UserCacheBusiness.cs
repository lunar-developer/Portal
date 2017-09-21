using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.UserManagement.Business
{
    public class UserCacheBusiness<T> : BasicCacheBusiness<T> where T : UserData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary
                = new OrderedConcurrentDictionary<string, CacheData>();
            foreach (UserData item in UserBusiness.GetUserExtension())
            {
                dictionary.TryAdd(item.UserID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string userID)
        {
            return UserBusiness.GetUserExtension(userID);
        }
    }
}