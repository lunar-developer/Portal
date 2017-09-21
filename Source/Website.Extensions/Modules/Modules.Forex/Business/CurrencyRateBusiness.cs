using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class CurrencyRateBusiness
    {
        public static List<CurrencyRateData> GetAll()
        {
            return new CurrencyRateProvider().GetAll();
        }

        public static CurrencyRateData GetItem(string key)
        {
            return new CurrencyRateProvider().GetItem(key);
        }

        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new CurrencyRateProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new CurrencyRateProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new CurrencyRateProvider().UpdateItem(key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool InsertAndReview(List<CurrencyRateData> listData, int userID, out string message)
        {
            return new CurrencyRateProvider().InsertAndReview(listData, userID, out message);
        }

        public static DataTable GetDataUpload()
        {
            return new CurrencyRateProvider().GetDataUpload();
        }

        public static bool ApprovalChangeCurrency(out string message)
        {
            return new CurrencyRateProvider().ApprovalChangeCurrency(out message);
        }
    }
}
