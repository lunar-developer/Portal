using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PolicyDocumentProvider : DataProvider
    {
        private static readonly string ScriptGetAllDocumentType =
            $@"
                select *
                from dbo.{PolicyDocumentTable.TableName} with(nolock)
                order by {PolicyDocumentTable.PolicyID}, {PolicyDocumentTable.OrderNo}
            ";

        public List<PolicyDocumentData> GetAllDocumentType()
        {
            Connector.ExecuteSql<PolicyDocumentData, List<PolicyDocumentData>>(
                ScriptGetAllDocumentType, out List<PolicyDocumentData> result);
            return result;
        }

        private static readonly string ScriptGetDocumentType = 
            $@"
                select *
                from dbo.{PolicyDocumentTable.TableName} with(nolock)
                where 
                    {PolicyDocumentTable.PolicyID} = @{PolicyDocumentTable.PolicyID}
                and {PolicyDocumentTable.DocumentTypeID} = @{PolicyDocumentTable.DocumentTypeID}";

        public PolicyDocumentData GetDocumentType(string policyID, string documentTypeID)
        {
            Connector.AddParameter(PolicyDocumentTable.PolicyID, SqlDbType.Int, policyID);
            Connector.AddParameter(PolicyDocumentTable.DocumentTypeID, SqlDbType.Int, documentTypeID);
            Connector.ExecuteSql(ScriptGetDocumentType, out PolicyDocumentData result);
            return result;
        }
    }
}