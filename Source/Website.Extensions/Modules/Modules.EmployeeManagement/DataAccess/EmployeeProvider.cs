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
        private static readonly string QueryEmailDataScript =
            $"select {EmployeeTable.Email} from dbo.{EmployeeTable.TableName} with(nolock)";
        public DataTable GetAllEmployeeEmail()
        {
            Connector.ExecuteSql(QueryEmailDataScript, out DataTable dtTable);
            return dtTable;
        }

        private static readonly string QueryEmailDataByUserInputScript =
            $"{QueryEmailDataScript} where {EmployeeTable.Email} like @{EmployeeTable.Email} + '%' COLLATE Latin1_General_CI_AI";
        public DataTable GetEmployeeEmail(string email)
        {
            Connector.ExecuteSql(QueryEmailDataByUserInputScript, out DataTable dtTable);
            return dtTable;
        }

        public DataTable LoadEmployeeInfo(string email)
        {
            Connector.AddParameter(EmployeeTable.Email, SqlDbType.VarChar, email);
            Connector.ExecuteProcedure("dbo.EM_SP_LoadEmployeeInfo", out DataTable dtResult);
            return dtResult;
        }


        private const string InsertScript = @"
            begin transaction
            begin try
                truncate table dbo.EM_Employee
                update dbo.EM_EmployeeImage set ContactQRCode = ''
                {0}
                commit transaction
                select 1
            end try
            begin catch
                rollback transaction
                insert into SYS_Exception(ErrorCode, ErrorMessage, StackTrace, DateTimeCreate)
                select error_number(), error_message(), 'EmployeeProvider.InsertEmployee', dbo.SYS_FN_GetCurrentDateTime()
                select 0
            end catch
        ";

        public bool InsertEmployee(List<EmployeeData> listEmployeeData, Dictionary<string, string> dictionary)
        {
            // Build insert script
            StringBuilder script = new StringBuilder();
            string importUserID = dictionary[EmployeeTable.ImportUserID];
            string importDateTime = DateTime.Now.ToString(PatternEnum.DateTime);

            foreach (EmployeeData data in listEmployeeData)
            {
                script.Append($@"execute dbo.EM_SP_InsertEmployee 
                    '{data.EmployeeID}', N'{data.FullName}', '{data.DateOfBirth}',
                    N'{data.Gender}', N'{data.Role}', N'{data.Branch}', N'{data.Office}', 
                    N'{data.Area}', '{data.BeginWorkDate}', '{data.ContractDate}',
                    N'{data.ContractType}', '{data.IdentityNumber}', '{data.DateOfIssue}',
                    N'{data.PlaceOfIssue}', '{data.AccountNumber}', '{data.PhoneNumber}',
                    '{data.PhoneExtendNumber}', '{data.Email}', '{importUserID}', '{importDateTime}';");
            }
            Connector.ExecuteSql(string.Format(InsertScript, string.Join(Environment.NewLine, script)), out string result);
            return result == "1";
        }

        public DataSet SearchEmployee(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("EM_SP_SearchEmployee", out DataSet dtResult);
            return dtResult;
        }

        public bool UpdateEmployeeInfo(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.EM_SP_UpdateEmployeeInfo", out string result);
            return result == "1";
        }

        public bool UpdateEmployeeImage(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.EM_SP_UpdateEmployeeImage", out string result);
            return result == "1";
        }

        public bool UpdateEmployeeContactQRCode(Dictionary<string, string> employeeQRDictionary)
        {
            StringBuilder script = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in employeeQRDictionary)
            {
                script.Append(
                    $@"execute dbo.EM_SP_UpdateEmployeeContactQRCode '{pair.Key}', '{pair.Value}';");
            }
            Connector.ExecuteSql(string.Join(Environment.NewLine, script), out string result);
            return result == "1";
        }

        public List<EmployeeBranchData> LoadEmployeeBranch()
        {
            Connector.ExecuteProcedure<EmployeeBranchData, List<EmployeeBranchData>>(
                "dbo.EM_SP_LoadEmployeeBranch", out List<EmployeeBranchData> listBranch);
            return listBranch;
        }
    }
}