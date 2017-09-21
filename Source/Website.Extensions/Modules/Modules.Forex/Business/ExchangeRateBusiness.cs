using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class ExchangeRateBusiness
    {
        public static List<ExchangeRateData> GetAll()
        {
            return new ExchangeRateProvider().GetAll();
        }
        public static List<ExchangeRateGridData> GetGridData()
        {
            return new ExchangeRateProvider().GetGridData();
        }

        public static ExchangeRateGridData GetGridItem(string key)
        {
            return new ExchangeRateProvider().GetGridItem(key);
        }
        
        public static bool AddItem(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new ExchangeRateProvider().AddItem(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool RemoveItem(string key, out string message)
        {
            DataTable dtResult = new ExchangeRateProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateExchangeRate(ExchangeRateGridData dataUpdate, ExchangeRateGridData currentData,
            string remark,string modifiedUserID, string modifiedDateTime, out string message)
        {
            DataTable dtResult =
                new ExchangeRateProvider().UpdateExchangeRate(dataUpdate, currentData, remark, modifiedUserID,
                    modifiedDateTime);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool InsertAndReview(List<ExchangeRateData> listData, int userID, out string message)
        {
            return new ExchangeRateProvider().InsertAndReview(listData, userID, out message);
        }

        public static DataTable GetDataUpload()
        {
            return new ExchangeRateProvider().GetDataUpload();
        }

        public static bool ApprovalChangeCurrency(out string message)
        {
            return new ExchangeRateProvider().ApprovalChangeCurrency(out message);
        }
    }
}
