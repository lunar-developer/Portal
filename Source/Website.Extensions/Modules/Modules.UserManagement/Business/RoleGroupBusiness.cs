using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.Business
{
    public static class RoleGroupBusiness
    {
        public static DataTable GetRoleGroupSetting(string roleGroupID)
        {
            return new RoleGroupProvider().GetRoleGroupSetting(roleGroupID);
        }

        public static bool UpdateRoleGroupSetting(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            return new RoleGroupProvider().UpdateRoleGroupSetting(parametediDictionary);
        }
    }
}