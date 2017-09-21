using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class RequestTypeBusiness
    {
        public static List<RequestTypeData> GetAll()
        {
            return new RequestTypeProvider().GetAll();
        }

        public static RequestTypeData GetItem(string key)
        {
            return new RequestTypeProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new RequestTypeProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new RequestTypeProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new RequestTypeProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
