using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class ActionBusiness
    {
        public static List<ActionData> GetAll()
        {
            return new ActionProvider().GetAll();
        }
        public static List<ActionData> GetListByUser(int workFlowStatusID, int userID)
        {
            return new ActionProvider().GetListByUser(workFlowStatusID, userID);
        }
        public static ActionData GetItemtByActionCode(int workFlowStatusID, int userID, string actionCode)
        {
            return new ActionProvider().GetItemtByActionCode(workFlowStatusID, userID, actionCode);
        }
        public static ActionData GetItem(string key)
        {
            return new ActionProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new ActionProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new ActionProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
      
        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new ActionProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
