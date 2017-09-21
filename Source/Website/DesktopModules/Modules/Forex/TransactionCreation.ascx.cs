using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using Modules.Forex.Business;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Modules.Forex.Global;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Forex
{
    public partial class TransactionCreation : TransactionCreationBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            FormInit();
        }
        protected override void OnLoad(EventArgs e)
        {
            if(IsPostBack)
            {
                return;
            }
            HiddenTransactionID.Value = Request.QueryString[TransactionTable.ID] ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(CurrentUserData?.BranchID))
            {
                SetPermission();
                BindData();
                if (WorkflowStatusID == WorkflowStatusEnum.Reject ||
                    WorkflowStatusID == WorkflowStatusEnum.HOFinishCancel)
                {
                    ShowMessage("Giao dịch đã bị từ chối hoặc hủy, không thể thực hiện thêm thao tác trên giao dịch này được!",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                }
                else if (IsValidWorkTimeRequestTransaction(WorkflowStatusID) == false)
                {
                    ShowMessage(
                        $"Thời gian cho phép tạo giao dịch: đối với từ thứ 2 đến thứ 6 {FormatTime(BeginWorkingMorning.ToString())} - " +
                        $"{FormatTime(EndWorkingMorning.ToString())}, " +
                        $"Thứ 7 bắt đầu từ {FormatTime(BeginWorkingSaturday.ToString())} - {FormatTime(EndWorkingSaturday.ToString())}",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                }
                else
                {
                    string transactionTypeID = Request.QueryString[TransactionTable.TransactionTypeID];
                    string currencyCode = Request.QueryString[TransactionTable.CurrencyCode];
                    if (!string.IsNullOrWhiteSpace(transactionTypeID) &&
                        !string.IsNullOrWhiteSpace(currencyCode) &&
                        IsNewTransaction(WorkflowStatusID))
                    {
                        BindTransactionType(transactionTypeID);
                        currencyCode = currencyCode.Replace("_", "/");
                        BindCurrencyCode(currencyCode);
                        RateInfoChange(currencyCode,transactionTypeID);
                    }
                }
            }
            else
            {
                ShowMessage("User đăng nhập chưa được cấu hình đơn vị trực thuộc, vui lòng liên hệ người quản trị",
                    ModuleMessage.ModuleMessageType.YellowWarning);
                DisableControl();
            }
        }
        private void BindData()
        {
            TransactionInfoFrame();
            CustomerInfoFrame();
            BidFrame();
            CustomerInvoiceAmountFrame();
            //Hidden Control
            HiddenAcceptToChangeStatusArr.Value = TargetStatusJsonObject;
            HiddenMaxDealerApprovalEdit.Value = $"{MaxAmountDealerApprovalEdit}";
            HiddenMaxDealerApprovalCancel.Value = $"{MaxAmountDealerApprovalCancel}";
            HiddenMaxRequestEditPercent.Value = $"{MaxAmountRequestChangePercent}";
            HiddenMaxRequestEditAmount.Value = $"{MaxAmountRequestChange}";
        }
        #region Draw Page Control
        private void TransactionInfoFrame()
        {
            TransactionInfo.Visible = true;
            //Bind Auto Select Box
            BindTransactionType(CurrentTransactionData?.TransactionTypeID);
            BindCurrencyCode(CurrentTransactionData?.CurrencyCode);
            //Creation Info
            SetTextControl(txtBranchName, GetBranchName(CurrentTransactionData?.BranchID), false);
            SetTextControl(txtMarker, GetCreationUserByID(CurrentTransactionData?.CreationUserID), false);

            //Amount
            QuantityFrameBindData();

            //Calander: Value Date
            SetDatePicker(calTransactionDate, GetTransactionDate(CurrentTransactionData?.TransactionDate), IsNewTransaction(WorkflowStatusID));

            //Reference Info
            ReferenceLimitField(CurrentTransactionData?.Rate, CurrentTransactionData?.Margin,
                CurrentTransactionData?.Limit, CurrentTransactionData?.MasterRate, CurrentTransactionData?.ExchangeRate);

        }

        private void BindTransactionType(string transactionTypeID)
        {
            BindTransactionType(ctTransactionType, transactionTypeID, IsNewTransaction(WorkflowStatusID));
            HiddenTransactionTypeID.Value = transactionTypeID;
        }

        private void BindCurrencyCode(string currencyCode)
        {
            BindCurrency(ctExchangeCode, currencyCode, IsNewTransaction(WorkflowStatusID));
            HiddenCurrencyCode.Value = CurrentTransactionData?.CurrencyCode;
        }
        private void QuantityFrameBindData()
        {
            if (!string.IsNullOrWhiteSpace(CurrentTransactionData?.QuantityTransactionAmount))
            {
                QuantityFrame.Visible = true;
                SetTextControl(txtQuantityTransactionAmount,
                    FunctionBase.FormatCurrency(CurrentTransactionData.QuantityTransactionAmount),
                    IsNewTransaction(WorkflowStatusID) || IsCanRequestEditOrCancel(WorkflowStatusID));

                HiddenCurrentQuantityAmount.Value = CurrentTransactionData?.Quantity;
            }
        }
        private void CustomerInfoFrame()
        {
            CustomerInfo.Visible = CurrentTransactionData != null;
            //Bind select box
            BindCustomerType(ctCustomerType, CurrentTransactionData?.CustomerTypeID, btnUpdateCustomerInfo.Enabled);
            BindReason(ctReasonTransaction, CurrentTransactionData?.ReasonCode, btnUpdateCustomerInfo.Enabled);
            // Customer Info
            SetTextControl(txtCustomerIDNo, CurrentTransactionData?.CustomerIDNo, btnUpdateCustomerInfo.Enabled);
            SetTextControl(txtCustomerFullname, CurrentTransactionData?.CustomerFullName, btnUpdateCustomerInfo.Enabled);
            
        }
        private void BidFrame()
        {
            if (WorkflowStatusID >= WorkflowStatusEnum.HOReceiveRequest &&
                WorkflowStatusID != WorkflowStatusEnum.Timeout)
            {
                PannelBid.Visible = true;
                SetTextControl(txtCapitalAmount, CurrentTransactionData?.CapitalAmount, 
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest);
                string dealtime = CurrentTransactionData?.DealTime;
                if (WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest &&
                    string.IsNullOrWhiteSpace(dealtime))
                {
                    ExchangeRateData exchangeRateData =
                        GetExchangerateData(ctExchangeCode.SelectedValue, ctTransactionType.SelectedValue);
                    dealtime = exchangeRateData.DealTime;
                }
                
                SetTextControl(txtRemainTime, 
                    GetRemainTime(WorkflowStatusID,CurrentTransactionData?.LastBidDateTime, dealtime),
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest);
                lblRemainTime.Text = IsCheckDealTime(WorkflowStatusID)
                    ? GetResource("lblRemainTime.Text")
                    : GetResource("lblDealTime.Text");
                //
                ctrlBrokerage.Visible = WorkflowStatusID >= WorkflowStatusEnum.HOReceiveBrokerage; 
                SetTextControl(txtBrokerage, CurrentTransactionData?.BrokerageAmount, WorkflowStatusID == WorkflowStatusEnum.HOReceiveBrokerage);
                //
                DepositAmountField();

                //Hidden
                HiddenCapitalAmount.Value = CurrentTransactionData?.CapitalAmount;
            }
            else
            {
                PannelBid.Visible = false;
            }
        }
        private void CustomerInvoiceAmountFrame()
        {
            //
            if (WorkflowStatusID > WorkflowStatusEnum.HOBid &&
                WorkflowStatusID != WorkflowStatusEnum.Timeout)
            {
                PannelCustomerInvoiceAmount.Visible = true;
                SetTextControl(txtCustomerInvoiceAmount, CurrentTransactionData?.CustomerInvoiceAmount,
                    WorkflowStatusID == WorkflowStatusEnum.BRReceive);
                HiddenInvoiceAmount.Value = CurrentTransactionData?.CustomerInvoiceAmount;
            }
            else
            {
                PannelCustomerInvoiceAmount.Visible = false;
            }
            
        }
        private void DepositAmountField()
        {
            int percentDeposit = GetDepositPercent(CurrentTransactionData?.CurrencyCode,
                CurrentTransactionData?.TransactionDate);
            //
            if (WorkflowStatusID >= WorkflowStatusEnum.HOReceiveRequest &&
                percentDeposit > 0)
            {
                ctrlDepositAmount.Visible = true;
                ctrlDepositAmount.Visible = true;
                SetTextControl(txtDepositAmount, GetDepositAmountValue(percentDeposit, CurrentTransactionData?.QuantityTransactionAmount,
                        CurrentTransactionData?.Rate),
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest);
            }
        }

        private void ReferenceLimitField(string rate, string margin, string limit, string masterRate, string exchageRate)
        {
            if (string.IsNullOrWhiteSpace(rate) || rate.Equals("0"))
            {
                ReferenceRateInfoFrame.Visible = false;
            }
            else
            {
                ReferenceRateInfoFrame.Visible = true;
                SetTextControl(txtBigFigure, FunctionBase.FormatCurrency(rate), false);
                txtReferenceRate.InnerHtml = exchageRate;
            }

            if (string.IsNullOrWhiteSpace(masterRate) ||
                string.IsNullOrWhiteSpace(limit) ||
                masterRate.Equals("0") ||
                limit.Equals("0"))
            {
                MasterRateInfoFrame.Visible = false;
                HiddenMasterRate.Value = "0";
                HiddenLimit.Value = "0";
            }
            else
            {
                
                MasterRateInfoFrame.Visible = true;
                double limitAmout = GetLimit(masterRate, limit);
                SetTextControl(txtMasterRate, FunctionBase.FormatCurrency(masterRate), false);
                SetTextControl(txtLimitRate, $"+/-{limitAmout}", false);
                SetTextControl(txtLimit, $"(+/-{limit}%)", false);
                HiddenMasterRate.Value = masterRate;
                HiddenLimit.Value = $"{limitAmout}";
            }

            if (string.IsNullOrWhiteSpace(margin) ||
                margin.Equals("0"))
            {
                MarginInfoFrame.Visible = false;
            }
            else
            {
                
                MarginInfoFrame.Visible = true;
                SetTextControl(txtMargin, $"+/-{margin}", false);
            }
            
        }

        private void ToggleMainFrameInfo(bool isVisble)
        {
            CustomerInfo.Visible = isVisble;
            QuantityFrame.Visible = isVisble;
        }
        
        
        #endregion
        #region Permission

        private void DisableControl()
        {
            btnSubmit.Enabled = false;
            btnCancel.Enabled = false;
            btnUpdateCustomerInfo.Enabled = false;
        }

        private void AutoSetRequestID()
        {
            string requestID = Request.QueryString["Request"];
            int workflowStatusID = ParseWorkflowStatus(WorkflowStatus);
            if (string.IsNullOrWhiteSpace(requestID))
            {
                if (workflowStatusID >= WorkflowStatusEnum.BRRequestEdit &&
                    workflowStatusID < WorkflowStatusEnum.HOFinishEdit)
                {
                    HiddenRequestTypeID.Value = RequestTypeEnum.Edit.ToString();
                }
                else if (workflowStatusID >= WorkflowStatusEnum.BRRequestCancel &&
                    workflowStatusID <= WorkflowStatusEnum.HOFinishCancel)
                {
                    HiddenRequestTypeID.Value = RequestTypeEnum.Cancel.ToString();
                }
                else
                {
                    HiddenRequestTypeID.Value = RequestTypeEnum.Transaction.ToString();
                }
            }
            else
            {
                int requestTypeID;
                if (int.TryParse(RequestTypeID, out requestTypeID) == false ||
                    requestTypeID < RequestTypeEnum.Transaction || 
                    requestTypeID > RequestTypeEnum.Cancel ||
                        (workflowStatusID <= WorkflowStatusEnum.HOFinishTransaction &&
                            (requestTypeID == RequestTypeEnum.Cancel || requestTypeID == RequestTypeEnum.Edit)))
                {
                    ShowMessage("Yêu cầu không hợp lệ", ModuleMessage.ModuleMessageType.RedError);
                    DisableControl();
                }
                else
                {
                    HiddenRequestTypeID.Value = requestID;
                }
            }

        }

        private void SetPermission()
        {
            
            string workflowStatus = string.Empty;
            if (!string.IsNullOrWhiteSpace(TransactionID))
            {
                CurrentTransactionData = TransactionBusiness.GetTransactionByID(TransactionID);
                workflowStatus = CurrentTransactionData.TransactionStatusID;

                AutoSetRequestID();
            }

            HiddenWorkflowStatus.Value = workflowStatus;

            TransactionButtonControl(btnSubmit, btnUpdateCustomerInfo, btnCancel, WorkflowStatusID, UserInfo.UserID,
                CurrentTransactionData?.CreationDateTime);

            btnRequestAgain.Enabled = IsAcceptRole(WorkflowStatusID) &&
               (WorkflowStatusID == WorkflowStatusEnum.HOBid ||
                WorkflowStatusID == WorkflowStatusEnum.BRReceive ||
                WorkflowStatusID == WorkflowStatusEnum.BRRquestException);

            RegisterConfirmDialog(btnCancel, "Bạn có chắc muốn thực hiện thao tác từ chối/hủy?");
            RegisterConfirmDialog(btnUpdateCustomerInfo, "Bạn có chắc muốn thực hiện cập nhật thông tin khách hàng?");
        }
        #region Get Page Value
        private TransactionData CurrentTransactionData { get; set; }
        private string WorkflowStatus => HiddenWorkflowStatus.Value;

        private int WorkflowStatusID => ParseWorkflowStatus(WorkflowStatus);
        private string TransactionID => HiddenTransactionID.Value;
        private string RequestTypeID => HiddenRequestTypeID.Value;
        #endregion

        #endregion
        #region Control
        protected void UpdateCustomerInfo(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { TransactionTable.CustomerIDNo, new SQLParameterData(txtCustomerIDNo.Text, SqlDbType.VarChar) },
                { TransactionTable.CustomerFullName, new SQLParameterData(txtCustomerFullname.Text, SqlDbType.NVarChar) },
                { TransactionTable.CustomerTypeID, new SQLParameterData(ctCustomerType.SelectedValue, SqlDbType.Int) },
                { TransactionTable.ReasonCode, new SQLParameterData(ctReasonTransaction.SelectedValue, SqlDbType.Int) },
                { TransactionTable.Remark, new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) },
                { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
            };
            string message;
            if (TransactionBusiness.UpdateCustomer(TransactionID, parameterDictionary, out message))
            {
                ShowMessage("Cập nhật thông tin khách hàng thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
        }
        protected void RejectTransaction(object sender, EventArgs e)
        {
            if (IsCanRequestEditOrCancel(WorkflowStatusID))
            {
                Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
                {
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text ?? string.Empty, SqlDbType.NVarChar) },
                    { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                    { "TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRequestCancel, SqlDbType.Int)},
                    { "ActionTransaction", new SQLParameterData("Yêu cầu hủy giao dịch",SqlDbType.NVarChar)},
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
                };
                string message;
                if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message))
                {
                    SetPermission();
                    BindData();
                    ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

                }
                else
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                Dictionary<string, SQLParameterData> rejectTransactionData = new Dictionary<string, SQLParameterData>
                {
                    { "IsReject", new SQLParameterData("True", SqlDbType.Bit) },
                    { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text ?? string.Empty, SqlDbType.NVarChar) },
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
                };
                string message;
                if (TransactionBusiness.UpdateTransaction(TransactionID, rejectTransactionData, out message))
                {
                    ShowMessage("Từ chối thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    SetPermission();
                }
                else
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
                }
            }
            
        }
        protected void SubmitForm(object sender, EventArgs e)
        {
            string message;
            if (IsNewTransaction(WorkflowStatusID))
            {
                RequestTransaction();
            }
            else if (WorkflowStatusID == WorkflowStatusEnum.BRReceive && 
                IsExceedLimit(txtCustomerInvoiceAmount.Text, HiddenLimit.Value, HiddenMasterRate.Value,out message))
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
            else if (IsCanRequestEditOrCancel(WorkflowStatusID))
            {
                RequestEditTransaction();
            }
            else
            {
                
                if (TransactionBusiness.UpdateTransaction(TransactionID, GetAcceptTransactionData, out message))
                {
                    SetPermission();
                    BindData();
                    ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

                }
                else
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
                }
            }

            
        }
        private void RateInfoChange(string currencyCode, string transactionType)
        {
            if (!string.IsNullOrWhiteSpace(currencyCode) &&
                !string.IsNullOrWhiteSpace(transactionType))
            {
                
                ExchangeRateData exchangeRateData =
                    GetExchangerateData(currencyCode, transactionType);
                if (FunctionBase.ConvertToBool(exchangeRateData.IsDisable) == false)
                {

                    ToggleMainFrameInfo(true);

                    CurrencyRateData currencyRateData = GetCurrencyData(currencyCode);
                    ReferenceLimitField(currencyRateData.Rate, currencyRateData.MarginMinProfit,
                        currencyRateData.MarginLimit, currencyRateData?.MasterRate,
                        ExchangeRateFormat(exchangeRateData.Rate, exchangeRateData.RateStatus));

                }
                else
                {
                    ShowMessage("Chiều giao dịch hiện không cho phép",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                    ToggleMainFrameInfo(false);
                }

            }
            else
            {
                ToggleMainFrameInfo(false);
            }
        }
        protected void TransactionTypeChange(object sender, EventArgs e)
        {
            HiddenTransactionTypeID.Value = ctTransactionType.SelectedValue;
            RateInfoChange(ctExchangeCode.SelectedValue, ctTransactionType.SelectedValue);
        }
        protected void ExChangeCodeChange(object sender, EventArgs e)
        {
            HiddenCurrencyCode.Value = ctExchangeCode.SelectedValue;
            RateInfoChange(ctExchangeCode.SelectedValue, ctTransactionType.SelectedValue);
        }
        #endregion
        #region Submit Button Process
        private void RequestTransaction()
        {
            if (IsWorkTime)
            {
                CurrencyRateData currencyRateData = CacheBase.Receive<CurrencyRateData>(ctExchangeCode.SelectedValue);
                ExchangeRateData exchangeRate =
                    GetExchangerateData(ctExchangeCode.SelectedValue, ctTransactionType.SelectedValue);
                if (CurrentUserBranchData?.BranchCode == null || UserInfo?.UserID == null || UserInfo?.UserID <= 0)
                {
                    ShowMessage("Người dùng chưa được xác thực.", ModuleMessage.ModuleMessageType.YellowWarning);
                    return;
                }
                if (currencyRateData == null || exchangeRate == null)
                {
                    ShowMessage("Dữ liệu không hợp lê.", ModuleMessage.ModuleMessageType.YellowWarning);
                    return;
                }
                string transactionDate = "";
                if (calTransactionDate.SelectedDate != null)
                {
                    transactionDate = ((DateTime) calTransactionDate.SelectedDate).ToString(PatternEnum.Date);
                }
                else
                {
                    ShowMessage("Ngày giao dịch không hợp lệ.", ModuleMessage.ModuleMessageType.YellowWarning);
                    return;
                }
                if (!RequestTypeID.Equals(RequestTypeEnum.Transaction.ToString()))
                {
                    ShowMessage("Loại yêu cầu không hợp lệ.", ModuleMessage.ModuleMessageType.YellowWarning);
                    return;
                }
                Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
                {
                    { TransactionTable.CifNo, new SQLParameterData("-1", SqlDbType.VarChar) },
                    {
                        TransactionTable.TransactionTypeID,
                        new SQLParameterData(ctTransactionType.SelectedValue, SqlDbType.Int)
                    },
                    {
                        TransactionTable.CurrencyCode,
                        new SQLParameterData(ctExchangeCode.SelectedValue, SqlDbType.VarChar)
                    },
                    { TransactionTable.Rate, new SQLParameterData(currencyRateData.Rate, SqlDbType.Float) },
                    { TransactionTable.MasterRate, new SQLParameterData(currencyRateData.MasterRate, SqlDbType.Float) },
                    { TransactionTable.Limit, new SQLParameterData(currencyRateData.MarginLimit, SqlDbType.Float) },
                    {
                        TransactionTable.Margin, new SQLParameterData(currencyRateData.MarginMinProfit, SqlDbType.Float)
                    },
                    { TransactionTable.ExchangeRate, new SQLParameterData(exchangeRate.Rate, SqlDbType.Float) },
                    { TransactionTable.TransactionDate, new SQLParameterData(transactionDate, SqlDbType.VarChar) },
                    {
                        TransactionTable.QuantityTransactionAmount,
                        new SQLParameterData(txtQuantityTransactionAmount.Text, SqlDbType.Float)
                    },
                    { TransactionTable.CustomerIDNo, new SQLParameterData(txtCustomerIDNo.Text, SqlDbType.VarChar) },
                    {
                        TransactionTable.CustomerFullName,
                        new SQLParameterData(txtCustomerFullname.Text, SqlDbType.NVarChar)
                    },
                    {
                        TransactionTable.CustomerTypeID,
                        new SQLParameterData(ctCustomerType.SelectedValue, SqlDbType.Int)
                    },
                    {
                        TransactionTable.ReasonCode,
                        new SQLParameterData(ctReasonTransaction.SelectedValue, SqlDbType.Int)
                    },
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) },
                    {
                        TransactionTable.BranchID,
                        new SQLParameterData(CurrentUserBranchData?.BranchID, SqlDbType.VarChar)
                    },
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    {
                        TransactionTable.ModifiedDateTime,
                        new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                    }
                };
                string message;
                int excStatus = TransactionBusiness.RequestTransaction(parameterDictionary, out message);
                if (excStatus > 0)
                {
                    ShowMessage("Yêu cầu giá thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    HiddenTransactionID.Value = excStatus.ToString();
                    SetPermission();
                    TransactionInfoFrame();
                    CustomerInfoFrame();
                }
                else
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage("Thời gian cho phép tạo giao dịch: đối với từ thứ 2 đến thứ 6 8:00 - 16:30, Thứ 7 bắt đầu từ 8:00 - 11:30",
                    ModuleMessage.ModuleMessageType.YellowWarning);
            }
            
        }

        private void RequestEditTransaction()
        {
            double quantity;
            if (double.TryParse(txtQuantityTransactionAmount?.Text?.Replace(",", ""), out quantity) &&
                IsExceedRequestChange(txtQuantityTransactionAmount?.Text?.Replace(",", ""), HiddenCurrentQuantityAmount.Value) == false)
            {
                Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
                {
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text ?? string.Empty, SqlDbType.NVarChar) },
                    { TransactionTable.QuantityTransactionAmount, new SQLParameterData(quantity, SqlDbType.Float) },
                    { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                    { "TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRequestEdit, SqlDbType.Int)},
                    { "ActionTransaction", new SQLParameterData("Yêu cầu điều chỉnh số lượng từ " +
                        $"{FunctionBase.FormatCurrency(HiddenCurrentQuantityAmount.Value)} " +
                        $"sang {FunctionBase.FormatCurrency(txtQuantityTransactionAmount?.Text)}",SqlDbType.NVarChar)},
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
                };
                string message;
                if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message))
                {
                    SetPermission();
                    BindData();
                    ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

                }
                else
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage($"Yêu cầu thay đổi số lượng {txtQuantityTransactionAmount?.Text} không hợp lệ, " +
                    $"số lượng không được phép lớn hơn {MaxAmountRequestChangePercent}% hoặc lớn hơn {MaxAmountRequestChange}", 
                    ModuleMessage.ModuleMessageType.YellowWarning);
            }
            
        }

        private Dictionary<string, SQLParameterData> GetAcceptTransactionData
        {
            get
            {
                Dictionary<string, SQLParameterData> acceptData = new Dictionary<string, SQLParameterData>
                {
                    { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text ?? string.Empty, SqlDbType.NVarChar) },
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
                };
                switch (WorkflowStatusID)
                {
                    case WorkflowStatusEnum.Open:
                        acceptData.Add(TransactionTable.QuantityTransactionAmount, new SQLParameterData(txtQuantityTransactionAmount?.Text?.Replace(",", ""), SqlDbType.Float));
                        break;
                    case WorkflowStatusEnum.HOReceiveRequest:
                        acceptData.Add(TransactionTable.CapitalAmount, new SQLParameterData(txtCapitalAmount.Text, SqlDbType.Float));
                        acceptData.Add(TransactionTable.DealTime, new SQLParameterData(txtRemainTime.Text, SqlDbType.Int));
                        if (FindControl("txtDepositAmount") != null)
                        {
                            acceptData.Add(TransactionTable.DepositAmount, new SQLParameterData(txtDepositAmount?.Text.Replace(",", ""), SqlDbType.Float));
                        }
                        break;
                    case WorkflowStatusEnum.BRReceive:
                        acceptData.Add(TransactionTable.CustomerInvoiceAmount, new SQLParameterData(txtCustomerInvoiceAmount.Text, SqlDbType.Float));
                        if (IsExceedMargin(HiddenCurrencyCode.Value, HiddenTransactionTypeID.Value,
                            txtCustomerInvoiceAmount.Text,
                            txtMargin.Text, HiddenCapitalAmount.Value))
                        {
                            acceptData.Add("TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRquestException, SqlDbType.Int));
                            acceptData.Add("ActionTransaction", new SQLParameterData("Trình ngoại ngoại lệ, giá khách hàng dưới biên độ lời tối thiểu",
                                SqlDbType.NVarChar));
                        }
                        break;
                    case WorkflowStatusEnum.HOReceiveBrokerage:
                        acceptData.Add(TransactionTable.BrokerageAmount, new SQLParameterData(txtBrokerage.Text, SqlDbType.Float));
                        break;
                    case WorkflowStatusEnum.BRApprovalCancel:
                        if (IsDealerApprovalCancelExceedLimit(CurrentTransactionData?.QuantityTransactionAmount))
                        {
                            acceptData.Add("TargetStatus", new SQLParameterData(WorkflowStatusEnum.HORequestCancelException, SqlDbType.Int));
                        }
                        break;
                    case WorkflowStatusEnum.BRApprovalEdit:
                        if (IsDealerApprovalEditExceedLimit(HiddenCurrentQuantityAmount.Value,
                            CurrentTransactionData?.QuantityTransactionAmount))
                        {
                            acceptData.Add("TargetStatus",
                                new SQLParameterData(WorkflowStatusEnum.HORequestEditException, SqlDbType.Int));
                        }
                        break;

                }

                return acceptData;
            }
        }

        #endregion
        private void FormInit()
        {
            calTransactionDate.MaxDate = DateTime.Now.AddDays(365);
            calTransactionDate.MinDate = DateTime.Now;

            calTransactionDate.Attributes.Add("placeholder", GetResource("lblTransactionDate.Help"));
            ctTransactionType.Attributes.Add("placeholder", GetResource("lblTransactionType.Help"));
            txtQuantityTransactionAmount.Attributes.Add("placeholder", GetResource("lblQuantityTransactionAmount.Help"));
            txtCustomerIDNo.Attributes.Add("placeholder", GetResource("lblCustomerIDNo.Help"));
            txtCustomerFullname.Attributes.Add("placeholder", GetResource("lblCustomerFullname.Help"));
            ctCustomerType.Attributes.Add("placeholder", GetResource("lblCustomerType.Help"));
            ctReasonTransaction.Attributes.Add("placeholder", GetResource("lblReasonTransaction.Help"));
            txtRemark.Attributes.Add("placeholder", GetResource("lblRemark.Help"));
            txtCapitalAmount.Attributes.Add("placeholder", GetResource("lblCapitalAmount.Help"));
            txtDepositAmount.Attributes.Add("placeholder", GetResource("lblDepositAmount.Help"));
            txtRemainTime.Attributes.Add("placeholder", GetResource("lblRemainTime.Help"));
            txtCustomerInvoiceAmount.Attributes.Add("placeholder", GetResource("lblCustomerInvoiceAmount.Help"));
        }

        protected void UpdateTimeout(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { "TargetStatus", new SQLParameterData(WorkflowStatusEnum.Timeout, SqlDbType.Int) },
                { "ActionTransaction", new SQLParameterData("Yêu cầu hết thời gian qui định", SqlDbType.NVarChar) },
                { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
            };
            string message;
            if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message))
            {
                SetPermission();
                BindData();
                ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

            }
            else
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }

}