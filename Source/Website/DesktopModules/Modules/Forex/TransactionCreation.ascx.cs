using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using Modules.Forex.Business;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Modules.Forex.Global;
using Telerik.Web.UI;
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
            try
            {
                if (IsPostBack)
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
                        ShowMessage("Giao dịch đã bị từ chối hoặc hủy, không thể thực hiện thêm thao tác khác!",
                            ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                    else if (IsValidWorkTimeRequestTransaction(WorkflowStatusID) == false)
                    {
                        ShowMessage(WorkTimeMessage,ModuleMessage.ModuleMessageType.YellowWarning);
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
                            RateInfoChange(currencyCode, transactionTypeID);
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
            finally
            {
                if (IsRedirectAndShowNotification == false)
                {
                    ResetSessionMessage();
                }
            }
        }
        private void BindData()
        {
            TransactionInfoFrame();
            CustomerInfoFrame();
            BidFrame();
            CustomerInvoiceAmountFrame();
            #region Param Control
            HiddenAcceptToChangeStatusArr.Value = TargetStatusJsonObject;
            HiddenMaxDealerApprovalEdit.Value = $"{MaxAmountDealerApprovalEdit}";
            HiddenMaxDealerApprovalCancel.Value = $"{MaxAmountDealerApprovalCancel}";
            HiddenMaxRequestEditPercent.Value = $"{MaxAmountRequestChangePercent}";
            HiddenMaxRequestEditAmount.Value = $"{MaxAmountRequestChange}";
            #endregion
        }
        #region Panel Control
        private void TransactionInfoFrame()
        {
            TransactionInfo.Visible = true;
            //Bind Auto Select Box
            BindTransactionType(CurrentTransactionData?.TransactionTypeID);
            BindCurrencyCode(CurrentTransactionData?.CurrencyCode);
            #region  Creation Info
            
            if (IsNewTransaction(WorkflowStatusID))
            {
                txtBranchName.InnerHtml = $"{UserBranchName}";
                txtCreationUser.InnerHtml = $"{CurrentUserData.DisplayName}";
                HiddenCreationBranchID.Value = CurrentUserData.BranchID;
                HiddenCreationUserID.Value = CurrentUserData.UserID;
                HiddenMarkerID.Value = CurrentUserData.UserID;
            }
            else
            {
                txtBranchName.InnerHtml = $"{GetBranchName(CurrentTransactionData?.BranchID)}";
                txtCreationUser.InnerHtml = $"{GetDisplayNameByID(CurrentTransactionData?.CreationUserID)}";
            }
            #endregion
            #region Current Status
            if (IsNewTransaction(WorkflowStatusID) == false)
            {
                if (!string.IsNullOrWhiteSpace(CurrentTransactionData?.TransactionStatus))
                {
                    CurrentStatusPannel.Visible = true;
                    txtCurrentStatus.InnerHtml = $"<b>{CurrentTransactionData?.TransactionStatus}</b>";
                }
                
            }
            #endregion
            #region Control User Info
            string controlUserValue;
            if (IsDisplayControlUserInfo(WorkflowStatusID, CurrentTransactionData?.MarkerUserID,
                CurrentTransactionData?.DealerUserID, out controlUserValue))
            {
                UserControlPannel.Visible = true;
                txtControlInfo.InnerHtml = controlUserValue;
            }
            #endregion
            #region Quantity 
            if (!string.IsNullOrWhiteSpace(CurrentTransactionData?.QuantityTransactionAmount))
            {
                QuantityFrame.Visible = true;
                SetTextControl(txtQuantityTransactionAmount,
                    FunctionBase.FormatCurrency(CurrentTransactionData.QuantityTransactionAmount),
                    IsNewTransaction(WorkflowStatusID) || IsCanRequestEditOrCancel(WorkflowStatusID));

                HiddenCurrentQuantityAmount.Value = CurrentTransactionData?.Quantity;
            }
            #endregion

            //Calander: Value Date
            SetDatePicker(calTransactionDate, GetTransactionDate(CurrentTransactionData?.TransactionDate),
                IsNewTransaction(WorkflowStatusID));

            #region Reference Info

            if (WorkflowStatusID != WorkflowStatusEnum.Open &&
                CurrentTransactionData != null)
            {
                double rate;
                double transactionRate;
                string referenceSourcePrice = "0";
                if (double.TryParse(CurrentTransactionData.ExchangeRate, out transactionRate) &&
                    double.TryParse(CurrentTransactionData.Rate, out rate))
                {
                    referenceSourcePrice = FunctionBase.FormatCurrency($"{rate + transactionRate}");
                }
                else
                {
                    referenceSourcePrice = FunctionBase.FormatCurrency(CurrentTransactionData.Rate);
                }
                ReferenceLimitField(CurrentTransactionData.Rate, CurrentTransactionData.Margin,
                    CurrentTransactionData.Limit, CurrentTransactionData.MasterRate, referenceSourcePrice);
            }
            
            #endregion
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
        private void CustomerInfoFrame()
        {
            CustomerInfo.Visible = CurrentTransactionData != null;
            //Bind select box
            BindCustomerType(ctCustomerType, CurrentTransactionData?.CustomerTypeID, 
                IsNewTransaction(WorkflowStatusID) || IsCanRequestEditOrCancel(WorkflowStatusID));
            BindReason(ctReasonTransaction, CurrentTransactionData?.CustomerTypeID, CurrentTransactionData?.ReasonCode, 
                IsNewTransaction(WorkflowStatusID) || 
                IsCanRequestEditOrCancel(WorkflowStatusID));
            // Customer Info
            SetTextControl(txtCustomerIDNo, CurrentTransactionData?.CustomerIDNo, 
                IsNewTransaction(WorkflowStatusID) || IsCanRequestEditOrCancel(WorkflowStatusID));
            SetTextControl(txtCustomerFullname, CurrentTransactionData?.CustomerFullName, 
                IsNewTransaction(WorkflowStatusID) || IsCanRequestEditOrCancel(WorkflowStatusID));
            
        }

        private void BidFrame()
        {
            if (WorkflowStatusID >= WorkflowStatusEnum.HOReceiveRequest &&
                WorkflowStatusID != WorkflowStatusEnum.Timeout)
            {
                PannelBid.Visible = true;
                #region Capital Amount / Source Price
                SetTextControl(txtCapitalAmount, WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest ?
                        HiddenreferenceSourcePrice.Value : CurrentTransactionData?.CapitalAmount, 
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest);
                #endregion
                #region Remain Time
                string dealtime = CurrentTransactionData?.DealTime;
                if (WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest &&
                    string.IsNullOrWhiteSpace(dealtime))
                {
                    ExchangeRateData exchangeRateData =
                        GetExchangerateData(ctExchangeCode.SelectedValue, ctTransactionType.SelectedValue);
                    dealtime = exchangeRateData.DealTime;
                }

                SetTextControl(txtRemainTime,
                    GetRemainTime(WorkflowStatusID, CurrentTransactionData?.LastBidDateTime, dealtime),
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveRequest);
                lblRemainTime.Text = IsCheckDealTime(WorkflowStatusID)
                    ? GetResource("lblRemainTime.Text")
                    : GetResource("lblDealTime.Text");
                #endregion

                #region Broker Amount
                ctrlBrokerage.Visible = WorkflowStatusID >= WorkflowStatusEnum.HOReceiveBrokerage;
                SetTextControl(txtBrokerage,
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveBrokerage ? HiddenCapitalAmount.Value :
                    CurrentTransactionData?.BrokerageAmount,
                    WorkflowStatusID == WorkflowStatusEnum.HOReceiveBrokerage);
                #endregion

                #region Deposit Amount
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
                #endregion
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
        #endregion

        #region Permission

        private void DisableControl()
        {
            btnSubmit.Enabled = false;
            btnCancel.Enabled = false;
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

        private void ReloadTransactionData()
        {
            if (!string.IsNullOrWhiteSpace(TransactionID))
            {
                CurrentTransactionData = TransactionBusiness.GetTransactionByID(TransactionID);
                if (CurrentTransactionData != null)
                {
                    HiddenWorkflowStatus.Value = CurrentTransactionData.TransactionStatusID ?? string.Empty;
                    if (IsNewTransaction(WorkflowStatusID) == false)
                    {
                        //Load Control User
                        HiddenCreationBranchID.Value = CurrentTransactionData?.BranchID;
                        HiddenCreationUserID.Value = CurrentTransactionData?.CreationUserID;
                        HiddenMarkerID.Value = CurrentTransactionData?.MarkerUserID;
                        HiddenDealerID.Value = CurrentTransactionData?.DealerUserID;
                        
                    }
                    //Capital Amount 
                    HiddenCapitalAmount.Value = CurrentTransactionData?.CapitalAmount;
                }
            }
        }

        private void SetButtonPermission()
        {
            TransactionButtonControl(btnSubmit, btnCancel, WorkflowStatusID, UserInfo.UserID,
                CurrentTransactionData?.CreationDateTime);

            btnRequestAgain.Enabled = IsAcceptRole(WorkflowStatusID) &&
            (WorkflowStatusID == WorkflowStatusEnum.HOBid ||
                WorkflowStatusID == WorkflowStatusEnum.BRReceive ||
                WorkflowStatusID == WorkflowStatusEnum.BRRquestException);

            RegisterConfirmDialog(btnCancel, "Bạn có chắc muốn thực hiện thao tác từ chối/hủy?");
        }
        private void SetPermission()
        {

            ReloadTransactionData();
            AutoSetRequestID();
            SetButtonPermission();

        }
        #region Process Function
        private void ReferenceLimitField(string rate, string margin, string limit, string masterRate,
            string referenceSourcePrice, string sourcePriceStatus = null)
        {
            if (string.IsNullOrWhiteSpace(rate) || rate.Equals("0"))
            {
                ReferenceRateInfoFrame.Visible = false;
            }
            else
            {
                ReferenceRateInfoFrame.Visible = true;
                HiddenBigFigure.Value = rate;
                HiddenreferenceSourcePrice.Value = referenceSourcePrice;
                referenceSourcePrice = string.IsNullOrWhiteSpace(sourcePriceStatus) ? referenceSourcePrice :
                    ExchangeRateFormat(referenceSourcePrice, sourcePriceStatus);
                txtReferenceRate.InnerHtml = referenceSourcePrice;

            }

            if (string.IsNullOrWhiteSpace(masterRate) ||
                string.IsNullOrWhiteSpace(limit) ||
                masterRate.Equals("0") ||
                limit.Equals("0"))
            {
                MasterRateInfoFrame.Visible = false;
                HiddenMasterRate.Value = "0";
                HiddenLimitPercent.Value = "0";
            }
            else
            {

                MasterRateInfoFrame.Visible = true;
                double limitAmout = GetLimit(masterRate, limit);
                HiddenMasterRate.Value = masterRate;
                HiddenLimitPercent.Value = limit;
                double masterRateAmount;
                if (double.TryParse(masterRate, out masterRateAmount))
                {
                    string minAmount = $"{masterRateAmount - limitAmout}";
                    string maxAmount = $"{masterRateAmount + limitAmout}";
                    txtTransactionLimit.InnerHtml =
                        $"Từ: <b>{FunctionBase.FormatCurrency(minAmount)}</b> đến: <b>{FunctionBase.FormatCurrency(maxAmount)}</b>";
                }

                HiddenLimitAmount.Value = $"{limitAmout}";
            }
            double marginNum;
            if (string.IsNullOrWhiteSpace(margin) ||
                margin.Equals("0") ||
                double.TryParse(margin, out marginNum) == false)
            {
                MarginInfoFrame.Visible = false;
            }
            else
            {

                MarginInfoFrame.Visible = true;
                HiddenMargin.Value = margin;
                double capitalAmount;

                if (WorkflowStatusID >= WorkflowStatusEnum.HOBid &&
                    HiddenCapitalAmount.Value != "" &&
                    double.TryParse(HiddenCapitalAmount.Value, out capitalAmount))
                {
                    string marginAmount = "";
                    if (IsBuyTransaction)
                    {
                        marginAmount = $">={(capitalAmount + marginNum)}";
                    }
                    else if (IsSellTransaction)
                    {
                        marginAmount = $"<={(capitalAmount - marginNum)}"; ;
                    }
                    else
                    {
                        marginAmount = $"+/-{margin}";
                    }
                    txtMargin.InnerHtml = marginAmount;
                }
                else
                {
                    txtMargin.InnerHtml = $"+/-{margin}";
                }

            }

        }
        private void ToggleMainFrameInfo(bool isVisble)
        {
            CustomerInfo.Visible = isVisble;
            QuantityFrame.Visible = isVisble;
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
                    CurrencyRateData currencyRateData = CacheBase.Receive<CurrencyRateData>(currencyCode);
                    double rate;
                    double transactionRate;
                    string referrnceSourcePrice = "0";
                    if (double.TryParse(exchangeRateData.Rate, out transactionRate) &&
                        double.TryParse(currencyRateData.Rate, out rate))
                    {
                        referrnceSourcePrice = FunctionBase.FormatCurrency($"{rate + transactionRate}");
                    }
                    else
                    {
                        referrnceSourcePrice = FunctionBase.FormatCurrency(currencyRateData.Rate);
                    }
                    ReferenceLimitField(currencyRateData.Rate, currencyRateData.MarginMinProfit,
                        currencyRateData.MarginLimit, currencyRateData?.MasterRate, referrnceSourcePrice,
                        exchangeRateData.RateStatus);

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
        #endregion
        #region Get Page Value
        private TransactionData CurrentTransactionData { get; set; }
        private string WorkflowStatus => HiddenWorkflowStatus.Value;

        private int WorkflowStatusID => ParseWorkflowStatus(WorkflowStatus);
        private string TransactionID => HiddenTransactionID.Value;
        private string RequestTypeID => HiddenRequestTypeID.Value;
        private bool IsBuyTransaction => HiddenTransactionTypeID.Value != "" &&
        (HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.BuyByFundTranfer.ToString()) ||
            HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.BuyByCash.ToString()));
        private bool IsSellTransaction => HiddenTransactionTypeID.Value != "" &&
        (HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.SellByFundTranfer.ToString()) ||
            HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.SellByCash.ToString()));
        #endregion

        #endregion
        
        #region Page Control

        protected void RejectTransaction(object sender, EventArgs e)
        {
            string message;
            ModuleMessage.ModuleMessageType messageType;
            #region Notification body
            string body = string.Empty;
            if (HiddenCurrencyCode.Value != "") body += $"Cặp tiền tệ {HiddenCurrencyCode.Value}";
            if (HiddenCurrentQuantityAmount.Value != "")
                body += $"|Số lượng {HiddenCurrentQuantityAmount.Value}";
            if (HiddenCapitalAmount.Value != "")
                body += $"|Giá chào {HiddenCapitalAmount.Value}";
            if (txtCustomerInvoiceAmount.Text != "")
                body += $"|Giá khách hàng {txtCustomerInvoiceAmount.Text}";
            if (txtRemark.Text != "") body += $" | Nội dung '{txtRemark.Text}'";
            #endregion
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
                
                if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message, out messageType))
                {
                    SetPermission();
                    BindData();
                    message = "Yêu cầu duyệt thao tác hủy hồ sơ thành công";
                    ShowMessage($"{message}.", messageType);
                    #region SendNotification
                    SendNotificationMessage("Yêu cầu duyệt hủy hồ sơ", body, TransactionID, WorkflowStatus,
                        HiddenCreationBranchID.Value, HiddenMarkerID.Value, HiddenDealerID.Value, WorkflowStatusEnum.BRRequestCancel.ToString());
                    #endregion
                    Finish(TransactionID, WorkflowStatusID, messageType, message);
                }
                else
                {
                    ShowMessage($"{message}", messageType);
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
                if (TransactionBusiness.UpdateTransaction(TransactionID, rejectTransactionData, out message, out messageType))
                {
                    message = "Từ chối thành công.";
                    ShowMessage(message, messageType);
                    SetPermission();
                    #region SendNotification
                    SendNotificationMessage("Từ chối yêu cầu", body, TransactionID, WorkflowStatus,
                        HiddenCreationBranchID.Value, HiddenMarkerID.Value, HiddenDealerID.Value, WorkflowStatusEnum.Reject.ToString());
                    #endregion
                    Finish(TransactionID, WorkflowStatusID, messageType, message);
                }
                else
                {
                    ShowMessage($"{message}", messageType);
                }
            }
        }
        protected void SubmitForm(object sender, EventArgs e)
        {
            string message;
            ModuleMessage.ModuleMessageType messageType = ModuleMessage.ModuleMessageType.GreenSuccess;
            if (IsNewTransaction(WorkflowStatusID))
            {
                RequestTransaction();
            }
            else if (WorkflowStatusID == WorkflowStatusEnum.BRReceive && 
                IsExceedLimit(txtCustomerInvoiceAmount.Text, HiddenLimitPercent.Value, HiddenMasterRate.Value,out message))
            {
                messageType = ModuleMessage.ModuleMessageType.YellowWarning;
                ShowMessage($"{message}", messageType);
            }
            else if (IsCanRequestEditOrCancel(WorkflowStatusID))
            {
                RequestEditTransaction();
            }
            else
            {
                
                if (TransactionBusiness.UpdateTransaction(TransactionID, GetAcceptTransactionData, out message, out messageType))
                {
                    messageType = ModuleMessage.ModuleMessageType.GreenSuccess;
                    ShowMessage($"{message}.", messageType);
                    SetPermission();
                    BindData();
                    #region SendNotification
                    SendNotificationMessage(string.Empty, GetNotificationBodyByParam(GetAcceptTransactionData),TransactionID,WorkflowStatus,
                        HiddenCreationBranchID.Value, HiddenMarkerID.Value, HiddenDealerID.Value);
                    #endregion
                    Finish(TransactionID, WorkflowStatusID, messageType, message);
                }
                else
                {
                    ShowMessage($"{message}", messageType);
                }
            }

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
            ModuleMessage.ModuleMessageType messageType;
            if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message, out messageType))
            {
                SetPermission();
                BindData();
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.YellowWarning);
                Session[SessionEnum.MessageType] = messageType;
                Session[SessionEnum.Message] = message;
            }
            else
            {
                ShowMessage($"{message}", messageType);
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
        protected void CustomerTypeChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindReason(ctReasonTransaction, ctCustomerType.SelectedValue);
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
                    string body =
                        $"Cặp tiền tệ {ctExchangeCode.SelectedValue}, Số lượng {txtQuantityTransactionAmount.Text}";
                    if (txtRemark.Text != "") body += $" | Nội dung '{txtRemark.Text}'";
                    SendNotificationMessage($"{UserBranchName}-Yêu cầu giá", body, excStatus.ToString(), WorkflowStatusEnum.Open.ToString());
                    Finish(excStatus.ToString(), WorkflowStatusEnum.Open,
                        ModuleMessage.ModuleMessageType.GreenSuccess, "Yêu cầu giá thành công.",
                        ctExchangeCode.SelectedValue, txtQuantityTransactionAmount.Text,true);
                    //
                    ShowMessage("Yêu cầu giá thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    HiddenTransactionID.Value = excStatus.ToString();
                    SetPermission();
                    //TransactionInfoFrame();
                    //CustomerInfoFrame();
                }
                else if (excStatus == 0)
                {
                    ShowMessage($"{message}", ModuleMessage.ModuleMessageType.YellowWarning);
                }
                else
                {
                    ShowMessage("Lỗi xảy ra trong quá trình thực hiện yêu cầu giá", ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage(WorkTimeMessage,ModuleMessage.ModuleMessageType.YellowWarning);
            }
            
        }

        private void RequestEditTransaction()
        {
            double quantity;
            double currentQuantityAmount;
            double invoiceAmount;
            double currentInvoiceAmount;
            string message;
            string actionTransactionMessage = "Yêu cầu điều chỉnh thông tin";
            
            if (double.TryParse(txtCustomerInvoiceAmount?.Text?.Replace(",", ""), out invoiceAmount) == false ||
                double.TryParse(HiddenInvoiceAmount.Value, out currentInvoiceAmount) == false)
            {
                message = $"Giá khách hàng '{txtCustomerInvoiceAmount?.Text}' không hợp lệ";
            }
            else if((HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.BuyByFundTranfer.ToString()) ||
                HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.BuyByCash.ToString())) && (invoiceAmount > currentInvoiceAmount))
            {
                message = $"Điều chỉnh giá khách hàng chiều mua '{FunctionBase.FormatCurrency(txtCustomerInvoiceAmount?.Text)}'" +
                    " không được phép lớn hơn giá hiện tại " +
                    $"'{FunctionBase.FormatCurrency(HiddenCurrentQuantityAmount.Value)}'";
            }
            else if ((HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.SellByFundTranfer.ToString()) ||
                HiddenTransactionTypeID.Value.Equals(TransactionTypeEnum.SellByCash.ToString())) && (invoiceAmount < currentInvoiceAmount))
            {
                message = $"Điều chỉnh giá khách hàng chiều bán '{FunctionBase.FormatCurrency(txtCustomerInvoiceAmount?.Text)}' " +
                    "không được phép nhỏ hơn giá hiện tại " +
                    $"'{FunctionBase.FormatCurrency(HiddenCurrentQuantityAmount.Value)}'";
            }
            else if (double.TryParse(txtQuantityTransactionAmount?.Text?.Replace(",", ""), out quantity) == false ||
                double.TryParse(HiddenCurrentQuantityAmount.Value, out currentQuantityAmount) == false)
            {
                message = $"Số lượng nhập vào không hợp lệ '{txtQuantityTransactionAmount?.Text}'";
            }
            else
            {
                if (currentQuantityAmount > quantity || currentQuantityAmount < quantity)
                {
                    actionTransactionMessage +=
                        $",số lượng từ {FunctionBase.FormatCurrency(HiddenCurrentQuantityAmount.Value)} " +
                        $"sang  {FunctionBase.FormatCurrency(txtQuantityTransactionAmount?.Text)}";
                }
                if (invoiceAmount > currentInvoiceAmount || invoiceAmount < currentInvoiceAmount)
                {
                    actionTransactionMessage +=
                        $",giá khách hàng từ {FunctionBase.FormatCurrency(HiddenInvoiceAmount.Value)} " +
                        $"sang  {FunctionBase.FormatCurrency(txtCustomerInvoiceAmount?.Text)}";
                }
                Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
                {
                    { TransactionTable.CustomerIDNo, new SQLParameterData(txtCustomerIDNo.Text, SqlDbType.VarChar) },
                    { TransactionTable.CustomerFullName, new SQLParameterData(txtCustomerFullname.Text, SqlDbType.NVarChar) },
                    { TransactionTable.CustomerTypeID, new SQLParameterData(ctCustomerType.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.ReasonCode, new SQLParameterData(ctReasonTransaction.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.Remark, new SQLParameterData(txtRemark.Text ?? string.Empty, SqlDbType.NVarChar) },
                    { TransactionTable.QuantityTransactionAmount, new SQLParameterData(quantity, SqlDbType.Float) },
                    { TransactionTable.CustomerInvoiceAmount, new SQLParameterData(invoiceAmount, SqlDbType.Float) },
                    { "WorkflowStatusID", new SQLParameterData(WorkflowStatusID, SqlDbType.Int) },
                    { "TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRequestEdit, SqlDbType.Int)},
                    { "ActionTransaction", new SQLParameterData(actionTransactionMessage,SqlDbType.NVarChar)},
                    { TransactionTable.ModifiedUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                    { TransactionTable.ModifiedDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
                };
                ModuleMessage.ModuleMessageType messageType;
                if (TransactionBusiness.UpdateTransaction(TransactionID, parameterDictionary, out message, out messageType))
                {
                    SetPermission();
                    BindData();
                    message += $".{actionTransactionMessage}";
                    ShowMessage($"{message}.", messageType);

                    #region SendNotification

                    string body = string.Empty;
                    if (HiddenCurrencyCode.Value != "") body += $"Cặp tiền tệ {HiddenCurrencyCode.Value}";
                    body += $"|{actionTransactionMessage}";
                    if (txtRemark.Text != "") body += $" | Nội dung '{txtRemark.Text}'";
                    SendNotificationMessage("Yêu cầu duyệt điều chỉnh hồ sơ", body, TransactionID, WorkflowStatus,
                        HiddenCreationBranchID.Value, HiddenMarkerID.Value, HiddenDealerID.Value);
                    #endregion
                    Finish(TransactionID, WorkflowStatusID,messageType, message);
                }
                else
                {
                    message += $".{actionTransactionMessage}";
                    ShowMessage($"{message}", messageType);
                }
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
                        acceptData.Add(TransactionTable.CapitalAmount, new SQLParameterData(txtCapitalAmount?.Text?.Replace(",", ""),
                            SqlDbType.Float));
                        acceptData.Add(TransactionTable.DealTime, new SQLParameterData(txtRemainTime.Text, SqlDbType.Int));
                        if (FindControl("txtDepositAmount") != null)
                        {
                            acceptData.Add(TransactionTable.DepositAmount, new SQLParameterData(txtDepositAmount?.Text.Replace(",", ""),
                                SqlDbType.Float));
                        }
                        break;
                    case WorkflowStatusEnum.BRReceive:
                        acceptData.Add(TransactionTable.CustomerInvoiceAmount, new SQLParameterData(txtCustomerInvoiceAmount.Text, SqlDbType.Float));
                        if (IsExceedMargin(HiddenCurrencyCode.Value, IsBuyTransaction,
                            txtCustomerInvoiceAmount.Text,
                            HiddenMargin.Value, HiddenCapitalAmount.Value))
                        {
                            acceptData.Add("TargetStatus", new SQLParameterData(WorkflowStatusEnum.BRRquestException, SqlDbType.Int));
                            acceptData.Add("ActionTransaction", new SQLParameterData("Trình ngoại ngoại lệ, giá khách hàng dưới biên độ lời tối thiểu",
                                SqlDbType.NVarChar));
                        }
                        break;
                    case WorkflowStatusEnum.HOReceiveBrokerage:
                        acceptData.Add(TransactionTable.BrokerageAmount, new SQLParameterData(txtBrokerage?.Text.Replace(",", ""), SqlDbType.Float));
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

    }

}