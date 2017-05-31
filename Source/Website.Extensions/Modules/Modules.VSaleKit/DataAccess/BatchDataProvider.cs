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
        private static readonly string ScriptInsert = $@"
            insert into dbo.{BatchDataTable.TableName}(
                {BatchDataTable.ApplicID},
                {BatchDataTable.ApplicationTypeID},
                {BatchDataTable.PolicyID},
                {BatchDataTable.LegalType},
                {BatchDataTable.CustomerID},
                {BatchDataTable.CustomerName},
                {BatchDataTable.CreditLimit},
                {BatchDataTable.Priority},
                {BatchDataTable.UserName},
                {BatchDataTable.Description},
                {BatchDataTable.ImportUserID},
                {BatchDataTable.ImportDateTime},
                {BatchDataTable.ProcessStatus},
                {BatchDataTable.ProcessCode})";
        public DataTable Insert(List<BatchData> listData, Dictionary<string, string> dictionary)
        {
            // Build insert script
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();
            const string sql = @"
                begin transaction
                begin try
                    {0}
                    commit transaction
                    select 1
                end try
                begin catch
                    rollback transaction
                    select 0
                end catch";

            // Create insert script
            string applicationTypeID = dictionary[BatchDataTable.ApplicationTypeID];
            string policyID = dictionary[BatchDataTable.PolicyID];
            string importUserID = dictionary[BatchDataTable.ImportUserID];
            string importDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            foreach (BatchData data in listData)
            {
                listSQL.Add($@"(
                    '', {applicationTypeID}, {policyID}, '{data.LegalType}',
                    '{data.CustomerID}', N'{data.CustomerName}', {data.CreditLimit},
                    {data.Priority}, '{data.UserName}', N'{data.Description}',
                    {importUserID}, {importDateTime}, 0, '')");

                if (listSQL.Count < 1000)
                {
                    continue;
                }
                script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");
                listSQL = new List<string>();
            }
            script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");


            string result;
            Connector.ExecuteSql(string.Format(sql, script), out result);
            if (result == "0")
            {
                return new DataTable();
            }

            DataTable dtResult;
            Connector.AddParameter(BatchDataTable.ImportUserID, SqlDbType.Int, importUserID);
            Connector.AddParameter(BatchDataTable.ImportDateTime, SqlDbType.BigInt, importDateTime);
            Connector.ExecuteProcedure("dbo.VSK_ProcessBatchUpload", out dtResult);
            return dtResult;
        }
    }
}