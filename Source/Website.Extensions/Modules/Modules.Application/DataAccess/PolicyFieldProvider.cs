using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PolicyFieldProvider : DataProvider
    {
        public List<PolicyFieldData> GetAllPolicyField()
        {
            Connector.ExecuteProcedure<PolicyFieldData, List<PolicyFieldData>>("dbo.APP_SP_GetPolicyField", out List<PolicyFieldData> result);
            return result;
        }

        public PolicyFieldData GetPolicyField(string policyCode, string fieldName)
        {
            Connector.AddParameter(PolicyFieldTable.PolicyCode, SqlDbType.VarChar, policyCode);
            Connector.AddParameter(PolicyFieldTable.FieldName, SqlDbType.VarChar, fieldName);
            Connector.ExecuteSql("dbo.APP_SP_GetPolicyField", out PolicyFieldData result);
            return result;
        }
    }
}