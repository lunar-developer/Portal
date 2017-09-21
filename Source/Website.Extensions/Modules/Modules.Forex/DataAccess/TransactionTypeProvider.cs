using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class TransactionTypeProvider:DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{TransactionTypeTable.TableName} with(nolock)";
        public List<TransactionTypeData> GetAll()
        {
            Connector.ExecuteSql<TransactionTypeData, List<TransactionTypeData>>(ScriptAll, out List<TransactionTypeData> ouList);
            return ouList;
        }
        private readonly string Script = $@"Select * from dbo.{TransactionTypeTable.TableName} with(nolock) 
                                    where {TransactionTypeTable.ID} = @{TransactionTypeTable.ID}";
        public TransactionTypeData GetItem(string key)
        {
            Connector.AddParameter(TransactionTypeTable.ID, SqlDbType.Int, key);
            Connector.ExecuteSql(Script, out TransactionTypeData data);
            return data;
        }

        public DataTable AddItem(Dictionary<string, SQLParameterData> conditionDictionary)
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

        public DataTable UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary)
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
    }
}
