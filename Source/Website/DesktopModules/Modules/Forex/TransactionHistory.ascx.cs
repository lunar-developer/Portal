using System;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using Modules.Forex.Business;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Modules.Forex.Global;
using Telerik.Web.UI;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Forex
{
    public partial class TransactionHistory : TransactionCreationBase
    {

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            HiddenTransactionID.Value = Request.QueryString[TransactionTable.ID] ?? string.Empty;
            SetPermission();
            BindData();
        }

        private void BindData()
        {
            GridBind();
            TransactionInfoFrame();
            CustomerInfoFrame();
            BidFrame();
            CustomerInvoiceAmountFrame();
        }

        private void TransactionInfoFrame()
        {
            SetTextControl(txtTransactionType, CurrentTransactionData?.TransactionType, false);
            SetTextControl(txtBranchName, GetBranchName(CurrentTransactionData?.BranchID), false);
            SetTextControl(txtMarker, GetDisplayNameByID(CurrentTransactionData?.CreationUserID), false);
            SetTextControl(txtQuantityTransactionAmount,
                GetQuantityTransactionAmount,false);
            SetTextControl(txtExchangeCode, CurrentTransactionData?.CurrencyCode, false);
            SetTextControl(txtTransactionDate, GetTransactionDate(CurrentTransactionData?.TransactionDate).ToString(PatternEnum.DateDisplay), false);


            SetTextControl(txtBigFigure, string.IsNullOrWhiteSpace(CurrentTransactionData?.Rate) ? string.Empty :
                FunctionBase.FormatCurrency(CurrentTransactionData.Rate), false);

            SetTextControl(txtMargin, $"+/-{CurrentTransactionData?.Margin}", false);

            if (!string.IsNullOrWhiteSpace(CurrentTransactionData?.MasterRate) &&
                !string.IsNullOrWhiteSpace(CurrentTransactionData?.Limit))
            {
                SetTextControl(txtMasterRate, FunctionBase.FormatCurrency(CurrentTransactionData.MasterRate), false);
                SetTextControl(txtLimit, $"(+/-{CurrentTransactionData?.Limit})%", false);
                SetTextControl(txtLimitRate, $"+/-{GetLimit(CurrentTransactionData?.MasterRate, CurrentTransactionData?.Limit)}", false);
            }

            txtReferenceRate.InnerHtml = CurrentTransactionData?.ExchangeRate ?? string.Empty;
        }
        private void CustomerInfoFrame()
        {
            SetTextControl(txtCustomerIDNo, CurrentTransactionData?.CustomerIDNo, false);
            SetTextControl(txtCustomerFullname, CurrentTransactionData?.CustomerFullName, false);
            SetTextControl(txtRemark, CurrentTransactionData?.Remark, false);
            SetTextControl(txtCustomerType, CurrentTransactionData?.CustomerType, false);
            SetTextControl(txtReasonTransaction, GetReason(CurrentTransactionData?.ReasonCode), false);
            SetTextControl(txtRemark, CurrentTransactionData.ReasonCode, false);
        }

        private string GetQuantityTransactionAmount => FunctionBase.FormatCurrency(CurrentTransactionData?.QuantityTransactionAmount);

        private void DepositAmountControl()
        {
            ctrlDepositAmount.Visible = !string.IsNullOrWhiteSpace(CurrentTransactionData?.DepositAmount);
            SetTextControl(txtDepositAmount,string.IsNullOrWhiteSpace(CurrentTransactionData?.DepositAmount) ? string.Empty :
                FunctionBase.FormatCurrency(CurrentTransactionData.DepositAmount),
                false);

        }
        private void BidFrame()
        {
            PannelBid.Visible = true;
            SetTextControl(txtCapitalAmount, 
                string.IsNullOrWhiteSpace(CurrentTransactionData?.CapitalAmount) ? string.Empty : 
                FunctionBase.FormatCurrency(CurrentTransactionData.CapitalAmount), false);
            DepositAmountControl();
            SetTextControl(txtRemainTime, CurrentTransactionData?.DealTime, false);

            SetTextControl(txtBrokerage,
                string.IsNullOrWhiteSpace(CurrentTransactionData?.BrokerageAmount) ? string.Empty :
                FunctionBase.FormatCurrency(CurrentTransactionData?.BrokerageAmount), false);
        }

        private void CustomerInvoiceAmountFrame()
        {
            PannelCustomerInvoiceAmount.Visible = true;
            SetTextControl(txtCustomerInvoiceAmount,
                string.IsNullOrWhiteSpace(CurrentTransactionData?.CustomerInvoiceAmount) ? string.Empty: 
                FunctionBase.FormatCurrency(CurrentTransactionData.CustomerInvoiceAmount),
                false);
        }
        private void GridBind()
        {
            GirdView.Visible = true;
            GirdView.LocalResourceFile = LocalResourceFile;
            GirdView.DataBind();
        }

        protected void GridOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GirdView.DataSource = TransactionBusiness.GetTransactionHisory(TransactionID);
        }

        private void SetPermission()
        {
            if (!string.IsNullOrWhiteSpace(TransactionID))
            {
                CurrentTransactionData = TransactionBusiness.GetTransactionByID(TransactionID);
            }
        }
        private TransactionData CurrentTransactionData { get; set; }
        private string TransactionID => HiddenTransactionID.Value;
        protected void ExportReport(object sender, EventArgs e)
        {
            DataTable dtResult = TransactionBusiness.GetTransactionHisory(TransactionID);
            if (dtResult?.Rows?.Count > 0)
            {
                ExportToExcel(dtResult, "FX_History_" + TransactionID + "_" + DateTime.Now.Date.ToString(PatternEnum.DateTime));
            }
            else
            {
                ShowMessage("Không tìm thấy dữ liệu để xuất",
                    ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

    }
    
}