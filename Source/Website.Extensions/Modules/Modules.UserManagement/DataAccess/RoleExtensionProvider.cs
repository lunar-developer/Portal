using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Database;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    internal class RoleExtensionProvider : DataProvider
    {
        public List<RoleExtensionData> GetRoleInfo()
        {
            Connector.ExecuteProcedure<RoleExtensionData, List<RoleExtensionData>>(
                RoleExtensionTable.StoreGetRoleExtension, out List<RoleExtensionData> result);
            return result;
        }

        public RoleExtensionData GetRoleInfo(string roleID)
        {
            Connector.AddParameter(BaseTable.RoleID, SqlDbType.Int, roleID);
            Connector.ExecuteProcedure(
                RoleExtensionTable.StoreGetRoleExtension, out RoleExtensionData result);
            return result;
        }

        public bool UpdateRoleInfo(Dictionary<string, SQLParameterData> dictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure(RoleExtensionTable.StoreUpdateRoleExtension, out string result);
            return result == "1";
        }

        public bool DeleteRoleInfo(string roleID)
        {
            Connector.AddParameter(BaseTable.RoleID, SqlDbType.VarChar, roleID);
            Connector.ExecuteProcedure(RoleExtensionTable.StoreDeleteRoleExtension, out string result);
            return result == "1";
        }
    }
}
