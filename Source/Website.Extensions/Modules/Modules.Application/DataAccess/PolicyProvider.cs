using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PolicyProvider : DataProvider
    {
        private static readonly string ScriptGetAllPolicy =
            $"Select * from dbo.{PolicyTable.TableName} with(nolock)";

        public List<PolicyData> GetAllPolicy()
        {
            Connector.ExecuteSql<PolicyData, List<PolicyData>>(ScriptGetAllPolicy, out List<PolicyData> result);
            return result;
        }

        private static readonly string ScriptGetPolicy =
            ScriptGetAllPolicy + $" where {PolicyTable.PolicyID} = @{PolicyTable.PolicyID}";

        public PolicyData GetPolicy(string policyID)
        {
            Connector.AddParameter(PolicyTable.PolicyID, SqlDbType.VarChar, policyID);
            Connector.ExecuteSql(ScriptGetPolicy, out PolicyData result);
            return result;
        }
    }
}