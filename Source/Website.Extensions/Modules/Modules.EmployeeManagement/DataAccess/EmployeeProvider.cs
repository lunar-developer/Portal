using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.EmployeeManagement.Database;
using Modules.EmployeeManagement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Modules.EmployeeManagement.DataAccess
{
    public class EmployeeProvider : DataProvider
    {
        private static readonly string ScriptInsert = $@"
             begin transaction
                begin try
                    truncate table dbo.{EmployeeTable.TableName}
                    {{0}}
                    commit transaction
                    select 1
                end try
                begin catch
                    rollback transaction
                    insert into SYS_Exception(ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
                    select error_number(), error_message(), 'EmployeeProvider.InsertEmployee', dbo.SYS_FN_GetCurrentDateTime()
                    select 0
                end catch
            ";

        private static readonly string ScriptInsertContent = $@"
            insert into dbo.{EmployeeTable.TableName}(
                {EmployeeTable.EmployeeID},
                {EmployeeTable.FullName},
                {EmployeeTable.DateOfBirth},
                {EmployeeTable.Gender},
                {EmployeeTable.Role},
                {EmployeeTable.Branch},
                {EmployeeTable.Office},
                {EmployeeTable.Area},
                {EmployeeTable.BeginWorkDate},
                {EmployeeTable.ContractDate},
                {EmployeeTable.ContractType},
                {EmployeeTable.IdentityNumber},
                {EmployeeTable.DateOfIssue},
                {EmployeeTable.PlaceOfIssue},
                {EmployeeTable.AccountNumber},
                {EmployeeTable.PhoneNumber},
                {EmployeeTable.PhoneExtendNumber},
                {EmployeeTable.Email},
                {EmployeeTable.ImportUserID},
                {EmployeeTable.ImportDateTime})";
       
        public bool InsertEmployee(List<EmployeeData> listEmployeeData, Dictionary<string,string> dictionary)
        {
            // Build insert script
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();
            string importUserID = dictionary[EmployeeTable.ImportUserID];
            string importDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            foreach (EmployeeData data in listEmployeeData)
            {
                listSQL.Add($@"(
                    '{data.EmployeeID}', N'{data.FullName}', '{data.DateOfBirth}',
                    N'{data.Gender}', N'{data.Role}', N'{data.Branch}', N'{data.Office}', 
                    N'{data.Area}', '{data.BeginWorkDate}', '{data.ContractDate}',
                    N'{data.ContractType}', '{data.IdentityNumber}', '{data.DateOfIssue}',
                    N'{data.PlaceOfIssue}', '{data.AccountNumber}', '{data.PhoneNumber}',
                    '{data.PhoneExtendNumber}', '{data.Email}', '{importUserID}', '{importDateTime}')");
                
                if (listSQL.Count < 1000)
                {
                    continue;
                }
                script.Append($"{ScriptInsertContent} values {string.Join(",", listSQL)};");
                listSQL = new List<string>();
            }
            if (listSQL.Count > 0)
            {
                script.Append($"{ScriptInsertContent} values {string.Join(",", listSQL)};");
            }

            Connector.ExecuteSql(string.Format(ScriptInsert, script), out string result);
            return result == "1";
        }

        public DataTable SearchEmployee(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("EM_SP_SearchEmployee", out DataTable dtResult);
            return dtResult;
        }

    }
}