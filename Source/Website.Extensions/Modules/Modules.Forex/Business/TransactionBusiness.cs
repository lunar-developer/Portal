using System.Collections.Generic;
using System.Data;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.Forex.Business
{
    public class TransactionBusiness
    {
        public static List<TransactionData> GetAllTransaction()
        {
            return new TransactionProvider().GetAllTransaction();
        }

        public static TransactionData GetTransactionByID(string key)
        {
            return new TransactionProvider().GetTransactionByID(key);
        }
        public static DataTable FindTransaction(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            return new TransactionProvider().FindTransaction(conditionDictionary);
        }
        public static DataTable GetTransactionHisory(string key)
        {
            return new TransactionProvider().GetTransactionHisory(key);
        }
        public static DataTable GetTransactionDailyReport()
        {
            return new TransactionProvider().GetTransactionDailyReport();
        }
        public static int RequestTransaction(Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new TransactionProvider().RequestTransaction(conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();

            if (!int.TryParse(dtResult?.Rows[0][0]?.ToString(), out int idResult))
            {
                message += " . Status ID is not valid.";
                return -1;
            }

            return idResult;
        }
        public static bool RemoveItem(string key,out string message)
        {
            DataTable dtResult = new TransactionProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public static bool UpdateCustomer(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new TransactionProvider().UpdateItem("dbo.FX_UpdateCustomerInfomation", key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
        public static bool UpdateTransaction(string key, Dictionary<string, SQLParameterData> conditionDictionary, out string message)
        {
            DataTable dtResult = new TransactionProvider().UpdateItem("dbo.FX_ChangeTransactionStatus", key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

       
    }
}
