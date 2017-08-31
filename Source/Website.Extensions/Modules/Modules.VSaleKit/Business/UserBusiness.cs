using System.Collections.Generic;
using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;

namespace Modules.VSaleKit.Business
{
    public static class UserBusiness
    {
        public static List<PermissionData> GetUserPermission(string roleName)
        {
            return new UserProvider().GetUserPermission(roleName);
        }
    }
}