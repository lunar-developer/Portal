using System.Collections.Generic;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security.Roles;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.UserManagement.Business
{
    public static class RoleExtensionBusiness
    {
        public static List<RoleExtensionData> GetRoleInfo()
        {
            return new RoleExtensionProvider().GetRoleInfo();
        }

        public static RoleExtensionData GetRoleInfo(string roleID)
        {
            return new RoleExtensionProvider().GetRoleInfo(roleID);
        }

        public static bool UpdateRoleInfo(Dictionary<string, SQLParameterData> dictionary)
        {
            bool result = new RoleExtensionProvider().UpdateRoleInfo(dictionary);
            if (result)
            {
                CacheBase.Reload<RoleExtensionData>(dictionary[BaseTable.RoleID].ParameterValue.ToString());
            }
            return result;
        }

        public static bool DeleteRoleInfo(string roleID)
        {
            bool result = new RoleExtensionProvider().DeleteRoleInfo(roleID);
            if (result)
            {
                CacheBase.Remove<RoleExtensionData>(roleID);
            }
            return result;
        }
    }
}