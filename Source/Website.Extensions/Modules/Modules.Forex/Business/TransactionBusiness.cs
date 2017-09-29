using System.Collections.Generic;
using System.Data;
using System.Web.Management;
using DotNetNuke.UI.Skins.Controls;
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
            }
            if (idResult < 0)
            {
                throw new SqlExecutionException(dtResult?.Rows[0][1]?.ToString());
            }
            return idResult;
        }
        public static bool RemoveItem(string key,out string message)
        {
            DataTable dtResult = new TransactionProvider().RemoveItem(key);
            message = dtResult?.Rows[0][1]?.ToString();
            if (int.TryParse(dtResult?.Rows[0][0]?.ToString(), out int statusID) == false || statusID < 0)
            {
                throw new SqlExecutionException(dtResult?.Rows[0][1]?.ToString());
            }
            return statusID == 1;
        }

        public static bool UpdateCustomer(string key, Dictionary<string, SQLParameterData> conditionDictionary, 
            out string message, out ModuleMessage.ModuleMessageType messageType)
        {
            DataTable dtResult = new TransactionProvider().UpdateItem("dbo.FX_UpdateCustomerInfomation", key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            if (int.TryParse(dtResult?.Rows[0][0]?.ToString(), out int statusID)  || statusID >= 0)
            {
                messageType = statusID == 1
                    ? ModuleMessage.ModuleMessageType.GreenSuccess
                    : ModuleMessage.ModuleMessageType.YellowWarning;
                return statusID == 1;
                
            }
            messageType = ModuleMessage.ModuleMessageType.RedError;
            message = "Lỗi xảy ra trong quá trình cập nhật thông tin khách hàng, vui lòng liên hệ người quản trị";
            throw new SqlExecutionException(dtResult?.Rows[0][1]?.ToString());
        }
        public static bool UpdateTransaction(string key, Dictionary<string, SQLParameterData> conditionDictionary, 
            out string message, out ModuleMessage.ModuleMessageType messageType)
        {
            DataTable dtResult = new TransactionProvider().UpdateItem("dbo.FX_ChangeTransactionStatus", key, conditionDictionary);
            message = dtResult?.Rows[0][1]?.ToString();
            if (int.TryParse(dtResult?.Rows[0][0]?.ToString(), out int statusID) && statusID >= 0)
            {
                messageType = statusID == 1
                    ? ModuleMessage.ModuleMessageType.GreenSuccess
                    : ModuleMessage.ModuleMessageType.YellowWarning;
                return statusID == 1;
            }
            messageType = ModuleMessage.ModuleMessageType.RedError;
            message = "Lỗi xảy ra trong quá trình xử lí, vui lòng liên hệ người quản trị";
            throw new SqlExecutionException(dtResult?.Rows[0][1]?.ToString());
        }

       
    }
}
