using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Enum;

namespace Modules.VSaleKit.DataAccess
{
    public class BatchDataProvider : DataProvider
    {
        private const string ScriptInsert = @"
            begin transaction
            begin try
                {0}
                commit transaction
                select 1
            end try
            begin catch
                rollback transaction
                insert into SYS_Exception(ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
                values(Error_Number(), Error_Message(), 'BatchDataProvider.Insert', dbo.SYS_FN_GetCurrentDateTime())
                select -1
            end catch";

        // ApplicID, ApplicationTypeID, PolicyID, LegalType, CustomerID, CustomerName, CreditLimit, Priority,
        // UserName, Description, ImportUserID, ImportDateTime, ProcessStatus, ProcessCode
        private const string ScriptExecute = @"
            execute dbo.VSK_SP_InsertBatchData 
                NULL, {0}, {1}, '{2}', '{3}', N'{4}', {5}, '{6}',
                '{7}', N'{8}', {9}, {10}, {11}, '{12}';
            ";

        public DataTable Insert(List<BatchData> listData, Dictionary<string, string> dictionary)
        {
            // Build insert script
            StringBuilder batch = new StringBuilder();
            // Create insert script
            string applicationTypeID = dictionary[BatchDataTable.ApplicationTypeID];
            string policyID = dictionary[BatchDataTable.PolicyID];
            string importUserID = dictionary[BatchDataTable.ImportUserID];
            string importDateTime = DateTime.Now.ToString(PatternEnum.DateTime);

            foreach (BatchData data in listData)
            {
                batch.AppendLine(string.Format(ScriptExecute,
                    applicationTypeID, policyID, data.IdentityTypeCode, data.CustomerID, data.CustomerName, data.CreditLimit, data.Priority,
                    data.UserName, data.Description, importUserID, importDateTime, 0, string.Empty));
            }
            Connector.ExecuteSql(string.Format(ScriptInsert, batch), out string result);
            if (result == "0")
            {
                return new DataTable();
            }

            Connector.AddParameter(BatchDataTable.ImportUserID, SqlDbType.Int, importUserID);
            Connector.AddParameter(BatchDataTable.ImportDateTime, SqlDbType.BigInt, importDateTime);
            Connector.ExecuteProcedure("dbo.VSK_SP_ProcessBatchUpload", out DataTable dtResult);
            return dtResult;
        }
    }
}