using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.Forex.Business
{
    public class WorkflowStatusBusiness
    {
        public static List<WorkflowStatusData> GetAll()
        {
            return new WorkflowStatusProvider().GetAll();
        }

        public static WorkflowStatusData GetItem(string key)
        {
            return new WorkflowStatusProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new WorkflowStatusProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new WorkflowStatusProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static WorkflowStatusData GetWorkflowStatusData(string statusID)
        {
            List<WorkflowStatusData> workflowStatusList = CacheBase.Receive<WorkflowStatusData>();
            foreach (WorkflowStatusData item in workflowStatusList)
            {
                if (item.Status.Equals(statusID))
                {
                    return item;
                }
            }
            return new WorkflowStatusData();
        }
        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new WorkflowStatusProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
