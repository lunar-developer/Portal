using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.VSaleKit.DataAccess
{
    public class UserProvider : DataProvider
    {
        public List<PermissionData> GetUserPermission(string roleName)
        {
            Connector.AddParameter("RoleName", SqlDbType.VarChar, roleName);
            Connector.ExecuteProcedure<PermissionData, List<PermissionData>>(
                "dbo.VSK_SP_GetUserPermission", out List<PermissionData> result);
            return result;
        }
    }
}