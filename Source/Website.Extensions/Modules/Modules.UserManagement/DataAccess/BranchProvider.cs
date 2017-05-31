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

        public bool UpdateBranchPermission(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parametediDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateBranchPermission", out string result);
            return result == "1";
        }












        public List<BranchRoleGroupData> GetListBranchRoleGroup(int branchID)
        {
            // Connector.AddParameter(RoleGroupTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteProcedure<BranchRoleGroupData, List<BranchRoleGroupData>>("UM_GetRoleGroupByBranch",
                out List<BranchRoleGroupData> outList);
            return outList;
        }

        public List<BranchPositionData> GetListBranchPosition(int branchID)
        {
            List<BranchPositionData> outList;
            Connector.AddParameter(BranchPositionTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteProcedure<BranchPositionData, List<BranchPositionData>>("UM_GetPositionByBranch",
                out outList);
            return outList;
        }

        public string GetUserPosition(int branchID, int positionCode)
        {
            BranchPositionData branchData;
            Connector.AddParameter(BranchPositionTable.BranchID, SqlDbType.Int, branchID);
            Connector.AddParameter(BranchPositionTable.PositionCode, SqlDbType.Int, positionCode);
            Connector.ExecuteProcedure<BranchPositionData>("UM_GetPositionByBranch", out branchData);
            return (branchData != null ? branchData.Title : string.Empty);
        }

        public List<BranchPositionRoleData> GetListBranchPositionRole(int branchID, int postionID)
        {
            List<BranchPositionRoleData> outList;
            Connector.AddParameter(BranchPositionRoleTable.BranchID, SqlDbType.Int, branchID);
            Connector.AddParameter(BranchPositionRoleTable.PositionCode, SqlDbType.Int, postionID);
            Connector.ExecuteProcedure<BranchPositionRoleData, List<BranchPositionRoleData>>(
                "UM_GetRoleByBranchPosition", out outList);
            return outList;
        }

        public bool UpdateRolePosition(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.NVarChar, pair.Value);
            }
            Connector.ExecuteProcedure("dbo.UM_UpdateRoleByBranchPosition", out dtResult);

            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() != "0";
        }
    }
}