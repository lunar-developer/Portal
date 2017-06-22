using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    public class BranchProvider : DataProvider
    {
        private static readonly string ScriptGetAllBranchInfo =
            $"select * from dbo.{BranchTable.TableName} with(nolock)";

        public List<BranchData> GetAllBranchInfo()
        {
            Connector.ExecuteSql<BranchData, List<BranchData>>(ScriptGetAllBranchInfo, out List<BranchData> list);
            return list;
        }


        private static readonly string ScriptGetBranchInfo =
            $"{ScriptGetAllBranchInfo} where {BranchTable.BranchID} = @{BranchTable.BranchID}";

        public BranchData GetBranchInfo(string branchID)
        {
            Connector.AddParameter(BranchTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteSql(ScriptGetBranchInfo, out BranchData branch);
            return branch;
        }

        public DataTable GetBranchPermission(string branchID)
        {
            Connector.AddParameter(BranchTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetBranchPermission", out DataTable result);
            return result;
        }

        public DataSet GetBranchPermissionAndTemplate(string branchID)
        {
            Connector.AddParameter(BranchTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetBranchPermissionAndTemplate", out DataSet result);
            return result;
        }

        public bool UpdateBranchPermission(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parametediDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateBranchPermission", out string result);
            return result == "1";
        }

        public DataTable GetBranchManager(string branchID = null)
        {
            if (string.IsNullOrWhiteSpace(branchID) == false)
            {
                Connector.AddParameter("BranchID", SqlDbType.Int, branchID);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_GetBranchManager", out DataTable userID);
            return userID;
        }
    }
}