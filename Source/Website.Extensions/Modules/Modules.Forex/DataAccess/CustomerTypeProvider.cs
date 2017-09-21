using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class CustomerTypeProvider: DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{CustomerTypeTable.TableName} with(nolock)";
        public List<CustomerTypeData> GetAll()
        {
            Connector.ExecuteSql<CustomerTypeData, List<CustomerTypeData>>(ScriptAll, out List<CustomerTypeData> outList);
            return outList;
        }
        private readonly string Script = $@"Select * from dbo.{CustomerTypeTable.TableName} with(nolock)
                                            Where {CustomerTypeTable.ID} = @{CustomerTypeTable.ID}";
        public CustomerTypeData GetItem(string key)
        {
            Connector.ExecuteSql(Script, out CustomerTypeData data);
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
