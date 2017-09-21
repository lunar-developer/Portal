using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class TransactionTypeBusiness
    {
        public static List<TransactionTypeData> GetAll()
        {
            return new TransactionTypeProvider().GetAll();
        }

        public static TransactionTypeData GetItem(string key)
        {
            return new TransactionTypeProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new TransactionTypeProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key,out string message)
        {
            DataTable dtResult = new TransactionTypeProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new TransactionTypeProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
