using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class ActionProvider : DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{ActionTable.TableName} with(nolock)";
        public List<ActionData> GetAll()
        {
            Connector.ExecuteSql<ActionData, List<ActionData>>(ScriptAll, out List<ActionData> outList);
            return outList;
        }
        public List<ActionData> GetListByUser(int workFlowStatusID, int userID)
        {
            Connector.AddParameter(ActionTable.WorkflowStatusID, SqlDbType.Int, workFlowStatusID);
            Connector.AddParameter("UserID", SqlDbType.Int, userID);
            Connector.ExecuteProcedure<ActionData, List<ActionData>>("FX_GetActionByUserID", out List<ActionData> outList);
            return outList;
        }
        public ActionData GetItemtByActionCode(int workFlowStatusID, int userID, string actionCode)
        {
            Connector.AddParameter(ActionTable.WorkflowStatusID, SqlDbType.Int, workFlowStatusID);
            Connector.AddParameter("UserID", SqlDbType.Int, userID);
            Connector.AddParameter(ActionTable.ActionCode, SqlDbType.VarChar, actionCode);
            Connector.ExecuteProcedure("FX_GetActionByUserID", out ActionData data);
            return data;
        }
        private readonly string Script = $@"Select * from dbo.{ActionTable.TableName} with(nolock)
                                            Where {ActionTable.ID} = @{ActionTable.ID}";
        public ActionData GetItem(string key)
        {
            Connector.ExecuteSql(Script, out ActionData data);
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
