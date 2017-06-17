using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    public class RoleTemplateProvider : DataProvider
    {
        public DataTable GetRoleTemplate(string branchID)
        {
            Connector.AddParameter(BranchTable.BranchID, SqlDbType.Int, branchID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetRoleTemplate", out DataTable result);
            return result;
        }

        public DataSet GetRoleTemplateDetail(string templateID)
        {
            Connector.AddParameter(RoleTemplateTable.TemplateID, SqlDbType.Int, templateID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetRoleTemplateDetail", out DataSet result);
            return result;
        }

        public int UpdateRoleTemplate(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parametediDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateRoleTemplate", out string result);
            return int.Parse(result);
        }

        public bool DeleteRoleTemplate(string templateID)
        {
            Connector.AddParameter(RoleTemplateTable.TemplateID, SqlDbType.Int, templateID);
            Connector.ExecuteProcedure("dbo.UM_SP_DeleteRoleTemplate");
            return true;
        }
    }
}