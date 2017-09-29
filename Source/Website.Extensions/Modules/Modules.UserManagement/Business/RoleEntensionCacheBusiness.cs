using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.UserManagement.Business
{
    public class RolesEntensionCacheBusiness<T> : BasicCacheBusiness<T> where T : RoleExtensionData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (RoleExtensionData item in RoleExtensionBusiness.GetRoleInfo())
            {
                dictionary.TryAdd(item.RoleID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string roleID)
        {
            return RoleExtensionBusiness.GetRoleInfo(roleID);
        }
    }
}