using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class DocumentTypeProvider : DataProvider
    {
        private static readonly string ScriptGetAllDocumentType =
            $"Select * from dbo.{DocumentTypeTable.TableName} with(nolock)";

        public List<DocumentTypeData> GetAllDocumentType()
        {
            Connector.ExecuteSql<DocumentTypeData, List<DocumentTypeData>>(
                ScriptGetAllDocumentType, out List<DocumentTypeData> result);
            return result;
        }

        private static readonly string ScriptGetDocumentType =
            ScriptGetAllDocumentType + $" where {DocumentTypeTable.DocumentTypeID} = @{DocumentTypeTable.DocumentTypeID}";

        public DocumentTypeData GetDocumentType(string documentTypeID)
        {
            Connector.AddParameter(DocumentTypeTable.DocumentTypeID, SqlDbType.Int, documentTypeID);
            Connector.ExecuteSql(ScriptGetDocumentType, out DocumentTypeData result);
            return result;
        }
    }
}