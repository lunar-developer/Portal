using System;
using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.Enum;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.Forex.Global
{
    public class TransactionManagementBase: ForexModulesBase
    {
        #region Grid Command
        protected static bool IsValidTransactionDate(string createDateTime)
        {
            return !string.IsNullOrWhiteSpace(createDateTime) &&
                createDateTime.Length >= 8 &&
                createDateTime.Substring(0, 8).Equals(DateTime.Now.Date.ToString(PatternEnum.Date));
        }
        protected string AcceptImageButton(string transactionStatusID, string createDateTime)
        {
            int statusID = ParseWorkflowStatus(transactionStatusID);
            return (IsValidTransactionDate(createDateTime) &&
                statusID == WorkflowStatusEnum.HOFinishTransaction && (IsBRMaker || IsBRManager) ? "fa fa-pencil" : "fa fa-check");
        }
        protected string RejectImageButton(string transactionStatusID, string createDateTime)
        {
            int statusID = ParseWorkflowStatus(transactionStatusID);
            return (IsValidTransactionDate(createDateTime) &&
                statusID == WorkflowStatusEnum.HOFinishTransaction && (IsBRMaker || IsBRManager) ? "fa fa-trash" : "fa fa-ban");
        }
        protected string AcceptTooltipButton(string transactionStatusID, string createDateTime)
        {
            int statusID = ParseWorkflowStatus(transactionStatusID);
            return (IsValidTransactionDate(createDateTime) &&
                statusID == WorkflowStatusEnum.HOFinishTransaction && (IsBRMaker || IsBRManager) ? "Yêu cầu điều chỉnh" : "Nhận hoặc duyệt");
        }
        protected string RejectTooltipButton(string transactionStatusID, string createDateTime)
        {
            int statusID = ParseWorkflowStatus(transactionStatusID);
            return (IsValidTransactionDate(createDateTime) &&
                statusID == WorkflowStatusEnum.HOFinishTransaction && (IsBRMaker || IsBRManager) ? "Yêu cầu hủy giao dịch" : "Từ chối");
        }

        protected bool IsVisibleBranchName => IsHODealer || IsHOManager || IsHOViewer;
        protected bool IsCommandVisible(string commandType, string transactionStatusID, string createDateTime)
        {
            int statusID = ParseWorkflowStatus(transactionStatusID);
            bool isTransactionEnd = statusID == WorkflowStatusEnum.Reject ||
                statusID == WorkflowStatusEnum.HOFinishCancel;
            switch (commandType)
            {
                case CommandTypeEnum.History:
                    return true;
                case CommandTypeEnum.ViewEdit:
                    return isTransactionEnd == false &&
                        IsValidTransactionDate(createDateTime) && statusID >= WorkflowStatusEnum.Open;
                case CommandTypeEnum.Accept:
                    return isTransactionEnd == false && IsValidTransactionDate(createDateTime) && IsAcceptRole(statusID);
                case CommandTypeEnum.Reject:
                    return isTransactionEnd == false &&
                        (IsValidTransactionDate(createDateTime) || statusID < WorkflowStatusEnum.BRApprovalTransaction) &&
                        IsRejectRole(statusID);

            }
            return false;
        }
        protected static bool ValidateSelectDate(DateTime fromDate, DateTime toDate, out string message)
        {
            message = string.Empty;

            if (fromDate > toDate)
            {
                message = "Ngày bắt đầu lớn hơn ngày kết thúc";
                return false;
            }

            return true;
        }
        #endregion
        protected Dictionary<string, SQLParameterData> GetDefaultParamDataByUser
        {
            get
            {
                Dictionary<string, SQLParameterData> processParam = new Dictionary<string, SQLParameterData>
                {
                    {
                        TransactionTable.BranchID, new SQLParameterData(CurrentUserBranchData?.BranchID,
                            SqlDbType.Int)
                    },
                    {
                        "UserID", new SQLParameterData(UserInfo.UserID,
                            SqlDbType.Int)
                    }
                };
                List<int> condition = new List<int>();
                if (IsBRMaker)
                {
                    condition.Add(WorkflowStatusEnum.HOBid);
                    condition.Add(WorkflowStatusEnum.BRReceive);
                    condition.Add(WorkflowStatusEnum.Timeout);
                    condition.Add(WorkflowStatusEnum.BRApprovalException);
                    condition.Add(WorkflowStatusEnum.BRApprovalTransaction);
                    condition.Add(WorkflowStatusEnum.HOFinishTransaction);
                }
                if (IsBRManager)
                {
                    condition.Add(WorkflowStatusEnum.BRRquestException);
                    condition.Add(WorkflowStatusEnum.HOInputBrokerage);
                    condition.Add(WorkflowStatusEnum.BRRequestEdit);
                    condition.Add(WorkflowStatusEnum.BRRequestCancel);
                    if (processParam.ContainsKey("UserID"))
                    {
                        processParam.Remove("UserID");
                    }
                }
                if (IsHODealer)
                {
                    condition.Add(WorkflowStatusEnum.BRAsk);
                    condition.Add(WorkflowStatusEnum.HOReceiveRequest);
                    condition.Add(WorkflowStatusEnum.BRInputCustomerInvoiceAmount);
                    condition.Add(WorkflowStatusEnum.BRApprovalEdit);
                    condition.Add(WorkflowStatusEnum.BRApprovalCancel);
                    condition.Add(WorkflowStatusEnum.BRApprovalException);
                    condition.Add(WorkflowStatusEnum.HOReceiveBrokerage);
                    condition.Add(WorkflowStatusEnum.BRApprovalTransaction);
                }
                if (IsHOManager)
                {
                    condition.Add(WorkflowStatusEnum.HORequestEditException);
                    condition.Add(WorkflowStatusEnum.HORequestCancelException);
                    if (processParam.ContainsKey("UserID"))
                    {
                        processParam.Remove("UserID");
                    }
                }

                if (condition.Count == 0)
                {
                    condition.Add(WorkflowStatusEnum.HOFinishTransaction);
                    condition.Add(WorkflowStatusEnum.HOFinishEdit);
                    condition.Add(WorkflowStatusEnum.HOFinishCancel);

                }

                if (IsHODealer || IsHOManager || IsHOViewer || IsHOAdmin)
                {
                    if (processParam.ContainsKey(TransactionTable.BranchID))
                    {
                        processParam.Remove(TransactionTable.BranchID);
                    }
                }

                processParam.Add("Condition",
                    new SQLParameterData(string.Join(";", condition),
                        SqlDbType.VarChar));

                return processParam;
            }
        }
        protected Dictionary<string, SQLParameterData> GetAcceptTransactionData => new Dictionary<string, SQLParameterData>
        {
            { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
            { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
        };
        protected Dictionary<string, SQLParameterData> GetRejectTransactionData => new Dictionary<string, SQLParameterData>
        {
            { "IsReject", new SQLParameterData("True", SqlDbType.Bit) },
            { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
            { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
        };
        protected Dictionary<string, SQLParameterData> GetRequestCancelTransactionData => new Dictionary<string, SQLParameterData>
        {
            { "TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRequestCancel, SqlDbType.Int)},
            { "ActionTransaction", new SQLParameterData("Yêu cầu hủy giao dịch",SqlDbType.NVarChar)},
            { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
            { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
        };
        
        protected static string GetAcceptClientClass(string num)
        {
            return $"btnAccept-{num} btn btn-icon btn-icon-color-primary";
        }

        protected static string GetAcceptCssLink(string num)
        {
            return $"a.btnAccept-{num}";
        }
        protected static string GetRejectClientClass(string num)
        {
            return $"btnReject-{num} btn btn-icon btn-icon-color-danger";
        }
        protected static string GetRejectCssLink(string num)
        {
            return $"a.btnReject-{num}";
        }
        protected static List<string> AcceptToChangeStatus => new List<string>
        {
            WorkflowStatusEnum.BRAsk.ToString() ,
            WorkflowStatusEnum.HOBid.ToString(),
            WorkflowStatusEnum.BRInputCustomerInvoiceAmount.ToString() ,
            WorkflowStatusEnum.BRApprovalException.ToString(),
            WorkflowStatusEnum.HOInputBrokerage.ToString(),
            WorkflowStatusEnum.BRApprovalTransaction.ToString(),
            WorkflowStatusEnum.HOApprovalEditException.ToString(),
            WorkflowStatusEnum.HOApprovalCancelException.ToString()
        };
        protected bool IsAcceptToChangeStatus(string currentStatusID)
        {
            foreach (var item in AcceptToChangeStatus)
            {
                if (currentStatusID.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        protected static string GetTargetStatusMessage(string currentWorkflowStatusID)
        {
            if (TargetStatusDictionary.ContainsKey(currentWorkflowStatusID))
            {
                return TargetStatusDictionary[currentWorkflowStatusID];
            }
            return "Bạn có muốn thực hiện thao tác";
        }
        protected static int GetReloadBrInboxTime
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.ReloadBrInboxTime), out int time) &&
                    time > 0)
                {
                    return time * 1000;
                }
                return -1;
            }
        }
        protected static int GetReloadHOInboxTime
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.ReloadHOInboxTime), out int time) &&
                    time > 0)
                {
                    return time * 1000;
                }
                return -1;
            }
        }
        protected bool IsNotificationSuccessMessage(out string message)
        {
            if (IsNotificationMessage)
            {
                if (Session[SessionEnum.Message] != null)
                {
                    message = Session[SessionEnum.Message].ToString();

                }
                else if (Session[SessionEnum.CurrencyCode] != null &&
                        Session[SessionEnum.QuantityAmount] != null)
                {
                    string currencyCode = Session[SessionEnum.CurrencyCode].ToString();
                    string quantityAmount = FunctionBase.FormatCurrency(Session[SessionEnum.QuantityAmount].ToString());
                    message = $"Yêu cầu giá ({currencyCode}) với số lượng {quantityAmount} thành công.";
                    
                    Session.Remove(SessionEnum.CurrencyCode);
                    Session.Remove(SessionEnum.QuantityAmount);
                    
                }
                else
                {
                    message = "Yêu cầu xử lí thành công.";
                }

                RemoveNotificationMessage();
                ResetSessionTransactionID();
                ResetSessionMessage();
                RemoveRedirectPage();

                return true;
            }
            message = string.Empty;
            return false;
        }
        protected void GetPopupTransactionDeatil(string transactionID, string closeUrl = "BtnReloadTransactionManagementGridView")
        {
            Session[SessionEnum.TransactionID] = transactionID;
            Session[SessionEnum.IsRedirect] = "1";
            Session[SessionEnum.IsNotificationMessage] = "1";
            if (string.IsNullOrWhiteSpace(closeUrl) && (IsHODealer || IsHOManager || IsHOAdmin))
            {
                closeUrl = "BtnReloadBidManagementGridView";
            }
            string url = EditUrl(TransactionCreationUrl, 600, 600, true,
                new Dictionary<string, string> { { TransactionTable.ID, transactionID } }, closeUrl);
            RegisterScript(url);
        }
    }
}
