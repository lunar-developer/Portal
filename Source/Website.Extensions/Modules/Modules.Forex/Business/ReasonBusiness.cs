using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class ReasonBusiness
    {
        public static List<ReasonData> GetAll()
        {
            return new ReasonProvider().GetAll();
        }

        public static ReasonData GetItem(string key)
        {
            return new ReasonProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new ReasonProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new ReasonProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new ReasonProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
