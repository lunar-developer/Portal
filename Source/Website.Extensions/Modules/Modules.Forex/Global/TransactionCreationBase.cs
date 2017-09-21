using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.Forex.Business;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.Forex.Global
{
    public class TransactionCreationBase: ForexModulesBase
    {
        #region Set Control
        protected static void TransactionButtonControl(Button btnSubtmit, Button btnUpdate, Button btnReject, 
            int workflowStatusID, int userID, string creationDateTime)
        {
            List<ActionData> actionListData = ActionBusiness.GetListByUser(workflowStatusID, userID);
            foreach (ActionData action in actionListData)
            {
                if (action.WorkflowStatusID.Equals(workflowStatusID.ToString()))
                {
                    switch (action.ActionCode)
                    {
                        case ActionCodeEnum.Submit:
                            ButtonControl(btnSubtmit, action.Text, FunctionBase.ConvertToBool(action.IsRole) && 
                                IsValidWorkTimeRequestTransaction(workflowStatusID) && 
                                IsValidTransactionDate(workflowStatusID, creationDateTime),
                                bool.Parse(action.IsVisible));
                            break;
                        case ActionCodeEnum.Update:
                            ButtonControl(btnUpdate, action.Text, (workflowStatusID >= WorkflowStatusEnum.Open &&
                                workflowStatusID != WorkflowStatusEnum.HOFinishCancel) &&
                                FunctionBase.ConvertToBool(action.IsRole), 
                                bool.Parse(action.IsVisible));
                            break;
                        case ActionCodeEnum.Cancel:
                            ButtonControl(btnReject, action.Text, FunctionBase.ConvertToBool(action.IsRole) &&
                                IsValidTransactionDate(workflowStatusID, creationDateTime),
                                bool.Parse(action.IsVisible));
                            break;
                    }
                }
            }
            
        }

        
        private static void ButtonControl(Button btn, string text, bool isEnable = false, bool isVisible = true)
        {
            btn.Text = text;
            btn.Enabled = isEnable;
            btn.Visible = isVisible;
        }

        protected static int GetDepositPercent(string currencycode, string date)
        {
            double valueDate = double.Parse(date);
            double currentDate = double.Parse(DateTime.Now.ToString(PatternEnum.Date));
            double days = valueDate - currentDate;
            if (days >= 1 && days <= 2)
            {
                if (currencycode.Contains("USD"))
                {
                    return 1;
                }
                return 2;
            }
            if (currencycode.Contains("USD"))
            {
                if (days >= 3 && days <= 30)
                {
                    return 3;
                }
                if (days >= 31 && days <= 90)
                {
                    return 5;
                }
                if (days >= 91)
                {
                    return 7;
                }
            }
            else
            {
                if (days >= 3)
                {
                    return 7;
                }
            }
            return 0;
        }
        protected static string GetDepositAmountValue(int percent, string quantity, string bigfigure)
        {
            
            if (percent > 0 &&
                double.TryParse(quantity, out double quantityAmount) &&
                double.TryParse(bigfigure, out double bigfigureAmount))
            {
                double amount = quantityAmount * bigfigureAmount * percent / 100;
                return FunctionBase.FormatCurrency($"{amount}");
            }
            return "0";
        }

        protected static string GetRemainTime(int workflowStatusID, string lastBidDateTimeText,string dealTimeText)
        {
            double currentDateTime = double.Parse(DateTime.Now.ToString(PatternEnum.DateTime));
            if (IsCheckDealTime(workflowStatusID) &&
                double.TryParse(lastBidDateTimeText, out double lastBidDateTime) &&
                double.TryParse(dealTimeText, out double dealTime))
            {
                var remainTime = (currentDateTime - lastBidDateTime);
                return remainTime > dealTime ? "0" : $"{dealTime - remainTime}";
            }
            return dealTimeText;
        }

        protected static string FormatTime(string time)
        {
            return $"{time.Substring(0, 2)}:{time.Substring(2, 2)}";
        }

        #endregion


        #region Check User Exceed Limit
        protected static bool IsValidWorkTimeRequestTransaction(int workflowStatusID)
        {
            if (IsWorkTime == false &&
                IsNewTransaction(workflowStatusID))
            {
                return false;
            }
            return true;
        }
        protected static bool IsNewTransaction(int workflowStatusID)
        {
            return workflowStatusID == WorkflowStatusEnum.Open;
        }

        protected static bool IsCanRequestEditOrCancel(int workflowStatusID)
        {
            return (workflowStatusID == WorkflowStatusEnum.HOFinishTransaction ||
                    workflowStatusID == WorkflowStatusEnum.HOFinishEdit);
        }

        protected static bool IsCheckDealTime(int workflowStatusID)
        {
            return workflowStatusID == WorkflowStatusEnum.HOBid ||
                workflowStatusID == WorkflowStatusEnum.BRReceive ||
                workflowStatusID == WorkflowStatusEnum.BRRquestException;
        }

        private static string FormatMoneyToNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "-1";
            return value.Replace("(", "")
                .Replace(")", "").Replace("+", "")
                .Replace("-", "").Replace("/", "")
                .Replace("%", "");
        }
        protected static bool IsExceedLimit(string transactionAmount, string limitText,string masterRateText, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(transactionAmount) ||
                string.IsNullOrWhiteSpace(limitText) ||
                string.IsNullOrWhiteSpace(masterRateText))
            {
                message = "Giá trị nhập vào không hợp lệ";
                return true;
            }
            limitText = FormatMoneyToNumber(limitText);
            masterRateText = FormatMoneyToNumber(masterRateText);
            transactionAmount = FormatMoneyToNumber(transactionAmount);


            if (masterRateText.Equals("0") || limitText.Equals("0")) return false; //0: not check;


            double marginLimitimitAmount = GetLimit(masterRateText, limitText);
 
            if (double.TryParse(transactionAmount, out double amount) &&
                (marginLimitimitAmount >= 0) &&
                double.TryParse(masterRateText, out double masterRate) &&
                amount <= (masterRate + marginLimitimitAmount) &&
                amount >= (masterRate - marginLimitimitAmount))
            {
                return false;
            }
            message =
                $"Số tiền nhập vào {FunctionBase.FormatCurrency(transactionAmount)} vượt qui định NHNN +/-{marginLimitimitAmount}";
            return true;
        }

        protected static double GetLimit(string masterRateText, string limitText)
        {
            if (!double.TryParse(limitText, out double limit) ||
                !double.TryParse(masterRateText, out double masterRate))
            {
                return 0;
            }
            return masterRate * limit / 100;
        }

        protected static bool IsExceedMargin(string currencyCode, string transactionType,
            string customerInvoiceAmountText, string marginText,string capitalAmountText)
        {
            if (string.IsNullOrWhiteSpace(currencyCode) || 
                string.IsNullOrWhiteSpace(transactionType)||
                string.IsNullOrWhiteSpace(marginText))
            {
                return true;
            }
            marginText = FormatMoneyToNumber(marginText);
            if (marginText.Equals("0")) return false; //0: not check;
            customerInvoiceAmountText = FormatMoneyToNumber(customerInvoiceAmountText);
            capitalAmountText = FormatMoneyToNumber(capitalAmountText);
            if (double.TryParse(customerInvoiceAmountText, out double invoiceAmount) &&
                double.TryParse(marginText, out double margin) &&
                double.TryParse(capitalAmountText, out double capitalAmount))
            {
                bool isBuy = (transactionType == TransactionTypeEnum.BuyByCash.ToString() ||
                    transactionType == TransactionTypeEnum.BuyByFundTranfer.ToString());
                bool isSell = (transactionType == TransactionTypeEnum.SellByCash.ToString() ||
                    transactionType == TransactionTypeEnum.SellByFundTranfer.ToString());
                if (isBuy && (capitalAmount - invoiceAmount >= margin))
                {
                    return false;
                }
                if (isSell && (invoiceAmount - capitalAmount >= margin))
                {
                    return false;
                }
            }
            
            return true;
        }

        protected static bool IsExceedRequestChange(string quantityAmountText, string currentQuantity)
        {
            if(double.TryParse(currentQuantity, out double quantity) &&
                double.TryParse(quantityAmountText, out double quantityAmount) &&
                Math.Abs(quantityAmount - quantity) <= MaxAmountRequestChange &&
                Math.Abs(quantityAmount - quantity) <= (quantity * MaxAmountRequestChangePercent/100))
            {
                return false;
            }
            return true;
        }
        protected static bool IsDealerApprovalEditExceedLimit(string quantityAmount, string quantityProcessChange)
        {
            if (double.TryParse(quantityAmount, out double currentQuantity) &&
                double.TryParse(quantityProcessChange, out double newQuantity) &&
                Math.Abs(newQuantity - currentQuantity) <= MaxAmountDealerApprovalEdit)
            {
                return false;
            }
            return true;
        }
        protected static bool IsDealerApprovalCancelExceedLimit(string quantityAmountText)
        {
            if (double.TryParse(quantityAmountText, out double currentQuantity) &&
                currentQuantity <= MaxAmountDealerApprovalCancel)
            {
                return false;
            }
            return true;
        }
        protected static bool IsValidTransactionDate(int workflowStatusID, string creatDate)
        {
            if (IsNewTransaction(workflowStatusID)) return true;
            if (!string.IsNullOrWhiteSpace(creatDate) &&
                creatDate?.Length >= 8)
            {
                return DateTime.Now.Date.ToString(PatternEnum.Date).Equals(creatDate?.Substring(0, 8));
            }
            return false;
        }
        #endregion

    }
}
