using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    public class RoleGroupProvider : DataProvider
    {
        public DataTable GetRoleGroupSetting(string roleGroupID)
        {
            Connector.AddParameter(RoleGroupTable.RoleGroupID, SqlDbType.Int, roleGroupID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetRoleGroupSetting", out DataTable result);
            return result;
        }

        public bool UpdateRoleGroupSetting(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parametediDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateRoleGroupSetting", out string result);
            return result == "1";
        }
    }
}