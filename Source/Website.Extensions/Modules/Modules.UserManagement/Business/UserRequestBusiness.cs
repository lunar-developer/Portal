using System.Collections.Generic;
using System.Data;
using DotNetNuke.Common.Utilities;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Modules.UserManagement.Global;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.UserManagement.Business
{
    public static class UserRequestBusiness
    {
        public static int CreateRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            int result = new UserRequestProvider().CreateRequest(parameterDictionary);
            if (result > 0)
            {
                string fromUserID = parameterDictionary[UserRequestTable.UserID].ParameterValue.ToString();
                string branchID = parameterDictionary[UserRequestTable.BranchID].ParameterValue.ToString();
                UserData userManager = BranchBusiness.GetUserManager(branchID);

                string url = $"{UserManagementModuleBase.UserRequestUrl}/{UserRequestTable.UserRequestID}/{result}";
                string body = $"Bạn có một phiếu yêu cầu cần xử lý. Click vào <a href='{url}'>đây</a> để xem chi tiết.";
                SendNotification("Thông báo", body, fromUserID, userManager.UserName);
            }
            return result;
        }

        public static int UpdateRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new UserRequestProvider().UpdateRequest(parameterDictionary);
        }

        public static int ProcessRequest(
            string userID, string requestTypeID, Dictionary<string, SQLParameterData> parameterDictionary)
        {
            int result = new UserRequestProvider().ProcessRequest(parameterDictionary);
            if (result > 0)
            {
                // Update Cache
                if (requestTypeID == RequestTypeEnum.UpdateBranch)
                {
                    CacheBase.Reload<UserData>(userID);
                }
                UserData userData = CacheBase.Receive<UserData>(userID);
                DataCache.ClearUserCache(0, userData.UserName);
            }
            return result;
        }

        public static UserRequestData LoadRequest(string requestID)
        {
            return new UserRequestProvider().LoadRequest(requestID);
        }

        public static DataTable SearchRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new UserRequestProvider().SearchRequest(parameterDictionary);
        }

        private static void SendNotification(string subject, string body, string fromUserID, string toUserName)
        {
            FunctionBase.SendNotification(subject, body, int.Parse(fromUserID), toUserName);
        }
    }
}