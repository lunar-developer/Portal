using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class TransactionProvider:DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{TransactionTable.TableName} with(nolock)";
        public List<TransactionData> GetAllTransaction()
        {
            Connector.ExecuteSql<TransactionData, List<TransactionData>>(ScriptAll, out List<TransactionData> outList);
            return outList;
        }
        private readonly string Script = $@"Select * from dbo.{TransactionTable.TableName} with(nolock)
                                            Where {TransactionTable.ID} = @{TransactionTable.ID}";
        public TransactionData GetTransactionByID(string key)
        {
            Connector.AddParameter(TransactionTable.ID, SqlDbType.BigInt, key);
            //Connector.ExecuteSql(Script, out TransactionData data);
            Connector.ExecuteProcedure("dbo.FX_GetTransaction", out TransactionData data);
            return data;
        }
        public DataTable FindTransaction(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in conditionDictionary)
            {
                if (!string.IsNullOrWhiteSpace(pair.Value.ParameterValue.ToString()))
                {
                    Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
                }
            }
            Connector.ExecuteProcedure("dbo.FX_GetTransaction", out DataTable dtResult);
            return dtResult;
        }
        public DataTable GetTransactionHisory(string key)
        {
            Connector.AddParameter("ID", SqlDbType.BigInt, key);
            Connector.ExecuteProcedure("dbo.FX_GetTransactionHistory", out DataTable dtResult);
            return dtResult;
        }
        public DataTable GetTransactionDailyReport()
        {
            Connector.ExecuteProcedure("dbo.FX_TransactionDailyReport", out DataTable dtResult);
            return dtResult;
        }
        public DataTable RequestTransaction(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in conditionDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.FX_TransactionCreation", out DataTable dtResult);
            return dtResult;
        }
        public DataTable RemoveItem(string key)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("StatusID");
            dtResult.Columns.Add("Message");
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }

        
        public DataTable UpdateItem(string storedProcedure, string key, Dictionary<string, SQLParameterData> conditionDictionary)
        {
            Connector.AddParameter(TransactionTable.ID, SqlDbType.BigInt, key);
            foreach (KeyValuePair<string, SQLParameterData> pair in conditionDictionary)
            {
                if (!string.IsNullOrWhiteSpace(pair.Value.ParameterValue.ToString()))
                {
                    Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
                }
            }
            Connector.ExecuteProcedure(storedProcedure, out DataTable dtResult);
            return dtResult;
        }

    }
}
