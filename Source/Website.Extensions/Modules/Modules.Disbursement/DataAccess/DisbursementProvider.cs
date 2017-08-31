using System.Collections.Generic;
using System.Data;
using Modules.Disbursement.Database;
using Modules.Disbursement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;
using Modules.Disbursement.Enum;

namespace Modules.Disbursement.DataAccess
{
    public class DisbursementProvider : DataProvider
    {
        public DataSet GetDisbursementByID(string id)
        {
            Connector.AddParameter(DisbursementTable.DisbursementID, SqlDbType.BigInt, id);
            Connector.ExecuteProcedure("dbo.DB_SP_GetDisbursement", out DataSet data);
            return data;
        }

        public DataTable InsertNewRecord(DisbursementData data)
        {
            Connector.AddParameter(DisbursementTable.BranchID, SqlDbType.Int, data.BranchID);
            Connector.AddParameter(DisbursementTable.CustomerID, SqlDbType.VarChar, data.CustomerID);
            Connector.AddParameter(DisbursementTable.OrganizationID, SqlDbType.VarChar, data.OrganizationID);
            Connector.AddParameter(DisbursementTable.CustomerName, SqlDbType.NVarChar, data.CustomerName);
            Connector.AddParameter(DisbursementTable.CurrencyCode, SqlDbType.VarChar, data.CurrencyCode);
            Connector.AddParameter(DisbursementTable.Amount, SqlDbType.Decimal, data.Amount);
            Connector.AddParameter(DisbursementTable.DisbursementDate, SqlDbType.Int, data.DisbursementDate);
            Connector.AddParameter(DisbursementTable.DisbursementMethod, SqlDbType.VarChar, data.DisbursementMethod);
            Connector.AddParameter(DisbursementTable.DisbursementPurpose, SqlDbType.NVarChar, data.DisbursementPurpose);
            Connector.AddParameter(DisbursementTable.LoanMethod, SqlDbType.NVarChar, data.LoanMethod);
            //Connector.AddParameter(DisbursementTable.CollectAmount, SqlDbType.Decimal, data.CollectAmount);
            //Connector.AddParameter(DisbursementTable.DisbursementStatus, SqlDbType.Int, data.DisbursementStatus);
            Connector.AddParameter(DisbursementTable.CreateUserID, SqlDbType.Int, data.CreateUserID);
            Connector.AddParameter(DisbursementTable.CreateDateTime, SqlDbType.BigInt, data.CreateDateTime);
            Connector.AddParameter(DisbursementTable.InterestRate, SqlDbType.Decimal, decimal.Parse(data.InterestRate));
            Connector.AddParameter(DisbursementTable.CustomerType, SqlDbType.VarChar, data.CustomerType);
            Connector.AddParameter(DisbursementTable.LoanExpire, SqlDbType.VarChar, data.LoanExpire);
            Connector.AddParameter(DisbursementTable.Note, SqlDbType.VarChar, data.Note);

            Connector.ExecuteProcedure("dbo.DB_SP_InsertDisbursement", out DataTable result);
            return result;
        }

        public DataTable UpdateRecord(DisbursementData data)
        {
            DataTable table;
            Connector.AddParameter(DisbursementTable.DisbursementID, SqlDbType.Int, data.DisbursementID);
            Connector.AddParameter(DisbursementTable.CustomerID, SqlDbType.VarChar, data.CustomerID);
            Connector.AddParameter(DisbursementTable.OrganizationID, SqlDbType.VarChar, data.OrganizationID);
            Connector.AddParameter(DisbursementTable.CustomerName, SqlDbType.NVarChar, data.CustomerName);
            Connector.AddParameter(DisbursementTable.CurrencyCode, SqlDbType.VarChar, data.CurrencyCode);
            Connector.AddParameter(DisbursementTable.Amount, SqlDbType.Decimal, data.Amount);
            Connector.AddParameter(DisbursementTable.DisbursementDate, SqlDbType.Int, data.DisbursementDate);
            Connector.AddParameter(DisbursementTable.DisbursementMethod, SqlDbType.VarChar, data.DisbursementMethod);
            Connector.AddParameter(DisbursementTable.DisbursementPurpose, SqlDbType.NVarChar, data.DisbursementPurpose);
            Connector.AddParameter(DisbursementTable.LoanMethod, SqlDbType.NVarChar, data.LoanMethod);
            //Connector.AddParameter(DisbursementTable.CollectAmount, SqlDbType.Decimal, data.CollectAmount);
            Connector.AddParameter(DisbursementTable.Remark, SqlDbType.NVarChar, data.Remark);
            Connector.AddParameter(DisbursementTable.ModifyUserID, SqlDbType.Int, data.ModifyUserID);
            Connector.AddParameter(DisbursementTable.ModifyDateTime, SqlDbType.BigInt, data.ModifyDateTime);
            Connector.AddParameter(DisbursementTable.InterestRate, SqlDbType.Decimal, data.InterestRate);
            Connector.AddParameter(DisbursementTable.CustomerType, SqlDbType.VarChar, data.CustomerType);
            Connector.AddParameter(DisbursementTable.LoanExpire, SqlDbType.VarChar, data.LoanExpire);
            Connector.AddParameter(DisbursementTable.Note, SqlDbType.VarChar, data.Note);
            Connector.AddParameter("ModifyTimeWhenView", SqlDbType.BigInt, data.ModifyTimeWhenView);

            Connector.ExecuteProcedure("dbo.DB_SP_UpdateDisbursement", out table);
            return table;
        }

        public bool DeleteRecord(string id)
        {
            Connector.AddParameter(DisbursementTable.DisbursementID, SqlDbType.BigInt, id);
            Connector.ExecuteProcedure("dbo.DB_SP_DeleteDisbursement", out string result);
            return result == "1";
        }

        public DataTable QueryDisbursement(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.DB_SP_QueryDisbursement", out DataTable result);
            return result;
        }

        public DataTable ProcessDisbursement(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.DB_SP_ProcessDisbursement", out DataTable result);
            return result;
        }

        public DataTable ProcessCancel(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.DB_SP_ProcessCancel", out DataTable result);
            return result;
        }

        public DataTable GetDisbursementByDate(string date)
        {
            string from = date + "000000";
            string to = date + "235959";
            string sql = $@"SELECT DisbursementID, BranchID, CustomerID, OrganizationID, CustomerName, CurrencyCode, Amount
                            FROM dbo.DB_Disbursement 
                            WHERE DisbursementStatus = '{DisbursementStatusEnum.Approved}'
                            AND ModifyDateTime BETWEEN '{from}' AND '{to}'";

            Connector.ExecuteSql(sql, out DataTable data);
            return data;
        }

        public DataSet GetComparedResult(string date) {
            Connector.AddParameter("checkdate", SqlDbType.VarChar, date);
            Connector.ExecuteProcedure("dbo.DB_SP_Compare", out DataSet result);
            return result;
        }

        public DataTable InsertResult(List<CoreBankData> data, string date)
        {
            string from = date + "0";
            string to = date + "9999999";

            string deleteSQL = $@"DELETE FROM dbo.DB_CoreBankResult WHERE ID BETWEEN '{from}' AND '{to}'";
            Connector.ExecuteSql(deleteSQL);

            string insert = string.Empty;
            for (int i = 0; i < data.Count; i++) {
                CoreBankData d = data[i];
                string personalNumber = string.IsNullOrEmpty(d.personalNumber) ? "''" : d.personalNumber;
                string organizationNumber = string.IsNullOrEmpty(d.organizationNumber) ? "''" : d.organizationNumber;
                insert += $@"INSERT INTO dbo.DB_CoreBankResult(ID, branchID, accountID, currencyCode, dateProcessed, personalNumber,
                                                                organizationNumber, customerName, amount, ModifyUserID, ModifyDateTime)
                                VALUES({date + i}, {d.branchID}, {d.accountID}, {d.currencyCode}, {d.dateProcessed}, {personalNumber}, 
                                            {organizationNumber}, '{d.customerName}', {d.amount}, {d.modifyUserID}, {d.modifyDateTime});";
            }
            string insertSQL = $@"
                BEGIN
                    BEGIN TRY
                    BEGIN TRANSACTION
                        {insert};
                    COMMIT TRANSACTION;
                SELECT 1, N'insert completed' AS MESSAGE;
                END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION
                        SELECT -1, error_message() AS MESSAGE
                    END CATCH
                END
            ";

            Connector.ExecuteSql(insertSQL, out DataTable table);
            return table;
        }

        public DataTable ApplyResult(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.DB_SP_ApplyResult", out DataTable result);
            return result;
        }
    }
}