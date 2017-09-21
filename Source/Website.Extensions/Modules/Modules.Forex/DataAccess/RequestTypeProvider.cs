using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class RequestTypeProvider:DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{RequestTypeTable.TableName} with(nolock)";
        public List<RequestTypeData> GetAll()
        {
            Connector.ExecuteSql<RequestTypeData, List<RequestTypeData>>(ScriptAll, out List<RequestTypeData> outList);
            return outList;
        }
        private readonly string Script = $@"Select * from dbo.{RequestTypeTable.TableName} with(nolock)
                                            Where {RequestTypeTable.ID} = @{RequestTypeTable.ID}";
        public RequestTypeData GetItem(string key)
        {
            Connector.ExecuteSql(Script, out RequestTypeData data);
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
