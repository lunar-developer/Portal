using System.Collections.Generic;
using System.Data;
using Modules.Disbursement.Database;
using Modules.Disbursement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

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
            Connector.AddParameter(DisbursementTable.CustomerName, SqlDbType.NVarChar, data.CustomerName);
            Connector.AddParameter(DisbursementTable.CurrencyCode, SqlDbType.VarChar, data.CurrencyCode);
            Connector.AddParameter(DisbursementTable.Amount, SqlDbType.Decimal, data.Amount);
            Connector.AddParameter(DisbursementTable.DisbursementDate, SqlDbType.Int, data.DisbursementDate);
            Connector.AddParameter(DisbursementTable.DisbursementMethod, SqlDbType.VarChar, data.DisbursementMethod);
            Connector.AddParameter(DisbursementTable.DisbursementPurpose, SqlDbType.NVarChar, data.DisbursementPurpose);
            Connector.AddParameter(DisbursementTable.LoanMethod, SqlDbType.VarChar, data.LoanMethod);
            Connector.AddParameter(DisbursementTable.CollectAmount, SqlDbType.Decimal, data.CollectAmount);
            Connector.AddParameter(DisbursementTable.DisbursementStatus, SqlDbType.Int, data.DisbursementStatus);
            Connector.AddParameter(DisbursementTable.CreateUserID, SqlDbType.Int, data.CreateUserID);
            Connector.AddParameter(DisbursementTable.CreateDateTime, SqlDbType.BigInt, data.CreateDateTime);
            Connector.ExecuteProcedure("dbo.DB_SP_InsertDisbursement", out DataTable result);
            return result;
        }

        public DataTable UpdateRecord(DisbursementData data)
        {
            DataTable table;
            Connector.AddParameter(DisbursementTable.DisbursementID, SqlDbType.Int, data.DisbursementID);
            Connector.AddParameter(DisbursementTable.CustomerID, SqlDbType.VarChar, data.CustomerID);
            Connector.AddParameter(DisbursementTable.CustomerName, SqlDbType.NVarChar, data.CustomerName);
            Connector.AddParameter(DisbursementTable.CurrencyCode, SqlDbType.VarChar, data.CurrencyCode);
            Connector.AddParameter(DisbursementTable.Amount, SqlDbType.Decimal, data.Amount);
            Connector.AddParameter(DisbursementTable.DisbursementDate, SqlDbType.Int, data.DisbursementDate);
            Connector.AddParameter(DisbursementTable.DisbursementMethod, SqlDbType.VarChar, data.DisbursementMethod);
            Connector.AddParameter(DisbursementTable.DisbursementPurpose, SqlDbType.NVarChar, data.DisbursementPurpose);
            Connector.AddParameter(DisbursementTable.LoanMethod, SqlDbType.VarChar, data.LoanMethod);
            Connector.AddParameter(DisbursementTable.CollectAmount, SqlDbType.Decimal, data.CollectAmount);
            Connector.AddParameter(DisbursementTable.ModifyUserID, SqlDbType.Int, data.ModifyUserID);
            Connector.AddParameter(DisbursementTable.ModifyDateTime, SqlDbType.BigInt, data.ModifyDateTime);
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
    }
}