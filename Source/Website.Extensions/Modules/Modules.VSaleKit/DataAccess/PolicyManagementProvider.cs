using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Enum;
using Website.Library.DataAccess;
using Modules.Application.DataTransfer;
using Modules.Application.Database;

namespace Modules.VSaleKit.DataAccess
{
    public class PolicyManagementProvider : DataProvider
    {
        private static readonly string sql = @"
                begin transaction
                begin try
                    {0}
                    commit transaction
                    select 1
                end try
                begin catch
                    rollback transaction
                    select 0, Error_Message() as Remark
                end catch";

        private static readonly string sqlDeletePolicySearch = $@"
            delete from dbo.{PolicySearchTable.TableName}
            where {PolicySearchTable.PolicyID} = [PolicyID];";

        private static readonly string sqlInsertPolicySearch = $@"
            insert into dbo.{PolicySearchTable.TableName}(
                {PolicySearchTable.PolicyID},
                {PolicySearchTable.Keyword})";

        private static readonly string sqlDeletePolicyDocument = $@"
            delete from dbo.{PolicyDocumentTable.TableName}
            where {PolicyDocumentTable.PolicyID} = [PolicyID];";

        private static readonly string sqlInsertPolicyDocument = $@"
            insert into dbo.{PolicyDocumentTable.TableName}(
                {PolicyDocumentTable.PolicyID},
                {PolicyDocumentTable.DocumentTypeID},
                {PolicyDocumentTable.OrderNo})";
        public DataTable GetPolicySearch(int policyID)
        {
            DataTable dtResult;
            Connector.AddParameter(PolicySearchTable.PolicyID, SqlDbType.Int, policyID);
            Connector.ExecuteProcedure(StoreEnum.VSK_GetPolicySearch, out dtResult);
            return dtResult;
        }
        public DataTable GetPolicyDocument(int policyID)
        {
            DataTable dtResult;
            Connector.AddParameter(PolicySearchTable.PolicyID, SqlDbType.Int, policyID);
            Connector.ExecuteProcedure(StoreEnum.VSK_GetPolicyDocument, out dtResult);
            return dtResult;
        }
        public string GetSqlUpdatePolicySearch(int policyID, List<string> listKeywork)
        {
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();

            script.Append(sqlDeletePolicySearch.Replace("[PolicyID]", policyID.ToString()));

            if(listKeywork.Count>0)
            {
                foreach (string keywork in listKeywork)
                {
                    listSQL.Add($@"({policyID}, N'{keywork}')");

                    if (listSQL.Count < 1000)
                    {
                        continue;
                    }
                    script.Append($"{sqlInsertPolicySearch} values {string.Join(",", listSQL.ToArray())};");
                    listSQL = new List<string>();
                }
                script.Append($"{sqlInsertPolicySearch} values {string.Join(",", listSQL.ToArray())};");
            }
            return script.ToString();
        }
        public string GetSqlUpdateDocumentType(int policyID, Dictionary<string,string> dicDocument)
        {
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();

            script.Append(sqlDeletePolicyDocument.Replace("[PolicyID]", policyID.ToString()));

            if (dicDocument.Count > 0)
            {
                foreach (var docID in dicDocument.Keys)
                {
                    listSQL.Add($@"({policyID}, {docID}, {dicDocument[docID]})");

                    if (listSQL.Count < 1000)
                    {
                        continue;
                    }
                    script.Append($"{sqlInsertPolicyDocument} values {string.Join(",", listSQL.ToArray())};");
                    listSQL = new List<string>();
                }
                script.Append($"{sqlInsertPolicyDocument} values {string.Join(",", listSQL.ToArray())};");
            }
            return script.ToString();
        }
        public DataTable InsertPolicy(PolicyData policyData, List<string> listKeywork, Dictionary<string,string> dicDocument)
        {
            //Insert APP_Policy
            DataTable dtTable;
            Connector.AddParameter(PolicyTable.PolicyCode, SqlDbType.VarChar, policyData.PolicyCode);
            Connector.AddParameter(PolicyTable.Name, SqlDbType.NVarChar, policyData.Name);
            Connector.AddParameter(PolicyTable.Remark, SqlDbType.NVarChar, policyData.Remark);
            Connector.ExecuteProcedure(StoreEnum.VSK_InsertPolicy, out dtTable);

            if (dtTable.Rows[0][0].ToString() == "0")
            {
                return dtTable;
            }
            //Update APP_PolicyDocument & APP_PolicySearch
            string policyID = dtTable.Rows[0][1].ToString();
            StringBuilder script = new StringBuilder();
            script.Append(GetSqlUpdatePolicySearch(int.Parse(policyID), listKeywork));
            script.Append(GetSqlUpdateDocumentType(int.Parse(policyID), dicDocument));
            Connector.ExecuteSql(string.Format(sql, script), out dtTable);

            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();

            if (dtTable.Rows[0][0].ToString() == "0")
            {
                dt.Rows.Add("0", "Thêm chính sách thành công, thêm từ khóa & tài liệu thất bại.");
                return dt;
            }
            else
            {
                dt.Rows.Add("1", "Thêm thành công.");
                return dt;
            }
        }
        public DataTable UpdatePolicy(PolicyData policyData, List<string> listKeywork, Dictionary<string, string> dicDocument)
        {
            DataTable dtTable;

            Connector.AddParameter(PolicyTable.PolicyID, SqlDbType.Int, policyData.PolicyID);

            Connector.AddParameter(PolicyTable.PolicyCode, SqlDbType.VarChar, policyData.PolicyCode);
            Connector.AddParameter(PolicyTable.Name, SqlDbType.NVarChar, policyData.Name);
            Connector.AddParameter(PolicyTable.Remark, SqlDbType.NVarChar, policyData.Remark);
            Connector.ExecuteProcedure(StoreEnum.VSK_UpdatePolicy, out dtTable);

            if (dtTable.Rows[0][0].ToString() == "0")
            {
                return dtTable;
            }

            //Update APP_PolicyDocument & APP_PolicySearch
            StringBuilder script = new StringBuilder();
            script.Append(GetSqlUpdatePolicySearch(int.Parse(policyData.PolicyID), listKeywork));
            script.Append(GetSqlUpdateDocumentType(int.Parse(policyData.PolicyID), dicDocument));
            Connector.ExecuteSql(string.Format(sql, script), out dtTable);

            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();

            if (dtTable.Rows[0][0].ToString() == "0")
            {
                dt.Rows.Add("0", "Sửa chính sách thành công, sửa từ khóa & tài liệu thất bại.");
                return dt;
            }
            else
            {
                dt.Rows.Add("1", "Sửa thành công.");
                return dt;
            }
        }
        public DataTable RemovePolicy(int policyID, int modifyUserID, long modifyDateTime)
        {
            DataTable dtTable;
            Connector.AddParameter(PolicyTable.PolicyID, SqlDbType.Int, policyID);
            Connector.AddParameter("ModifyUserID", SqlDbType.Int, modifyUserID);
            Connector.AddParameter("ModifyDateTime", SqlDbType.BigInt, modifyDateTime);
            Connector.ExecuteProcedure(StoreEnum.VSK_RemovePolicy, out dtTable);
            return dtTable;
        }
    }
}