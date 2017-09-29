using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
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
    public partial class BidManagement : TransactionManagementBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    return;
                }
                TransactionIDLastest.Value = Request.QueryString[TransactionTable.ID] ?? "0";
                string message;
                if (IsNotificationSuccessMessage(out message))
                {
                    ModuleMessage.ModuleMessageType messageType = GetMessageType();
                    ShowMessage($"{message}",
                        messageType);
                }
                GridBind();
                ReloadTime.Value = GetReloadHOInboxTime.ToString();
            }
            finally
            {
                if (IsRedirectAndShowNotification == false)
                {
                    ResetRedirectAndShowNotification();
                }
            }
        }
        #region Grid Data
        private void GridBind()
        {
            CurrentRate.Visible = IsHODealer || IsHOManager || IsHOViewer;
            CurrentRate.LocalResourceFile = LocalResourceFile;
            CurrentRate.DataBind();

            TransactionDailyResportGrid.Visible = IsHODealer || IsHOManager || IsHOViewer;
            TransactionDailyResportGrid.LocalResourceFile = LocalResourceFile;
            BindTrasactionDailyReport();



            BidExchangeGird.Visible = IsHODealer || IsHOManager;
            BidExchangeGird.LocalResourceFile = LocalResourceFile;
            BidExchangeBindData();
        }
        private static DataTable BidPriceTemplate4Grid
        {
            get
            {
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("#");
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.CurrencyCode);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.BigFigure);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.FundTransferGroup);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.CashGroup);
                return dtResult;
            }
        }
        private static DataTable BidPriceGridData()
        {
            DataTable dtResult = BidPriceTemplate4Grid;
            int count = 0;
            foreach (ExchangeRateGridData rate in CacheBase.Receive<ExchangeRateGridData>())
            {
                
                CurrencyRateData currencyRateData = CacheBase.Receive<CurrencyRateData>(rate.CurrencyCode);
                if (currencyRateData != null)
                {
                    count++;
                    DataRow dr = dtResult.NewRow();
                    dr["#"] = count;
                    dr[ExchangeRateGridFieldEnum.CurrencyCode] = EditExchangeRateUrl(rate.CurrencyCode);
                    dr[ExchangeRateGridFieldEnum.BigFigure] = string.IsNullOrWhiteSpace(currencyRateData?.Rate) ? string.Empty :
                        FunctionBase.FormatCurrency(currencyRateData.Rate);
                    dr[ExchangeRateGridFieldEnum.FundTransferGroup] = $"{rate.BuyRateFT}/{rate.SellRateFT}";
                    dr[ExchangeRateGridFieldEnum.CashGroup] = $"{rate.BuyRateCash}/{rate.SellRateCash}";
                    dtResult.Rows.Add(dr);
                    dtResult.AcceptChanges();
                }
                
            }

            return dtResult;
        }

        private void BindTrasactionDailyReport()
        {
            TransactionDailyResportGrid.DataSource = TransactionBusiness.GetTransactionDailyReport();
            TransactionDailyResportGrid.DataBind();
        }
        private void BidExchangeBindData()
        {
            BidExchangeGird.DataSource = TransactionBusiness.FindTransaction(GetDefaultParamDataByUser);
            BidExchangeGird.DataBind();
        }
        protected void CurrentRateOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentRate.DataSource = BidPriceGridData();
        }
        protected void TransactionDailyResportGridOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            TransactionDailyResportGrid.DataSource = TransactionBusiness.GetTransactionDailyReport();
        }
        protected void BidExchangeGirdGridOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BidExchangeGird.DataSource = TransactionBusiness.FindTransaction(GetDefaultParamDataByUser);
        }
        protected void GridOnDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            string num = item?["NumRecord"].Text;
            string branchName = item?["Branch"].Text;
            LinkButton buttonAccept = item?["ActionColumn"].Controls[3] as LinkButton;
            LinkButton buttonReject = item?["ActionColumn"].Controls[5] as LinkButton;
            if (buttonAccept != null)
            {
                string workflowStatusID = buttonAccept.CommandArgument?.ToString();
                if (IsAcceptToChangeStatus(workflowStatusID))
                {
                    RegisterConfirmDialog($"{GetAcceptCssLink(num)}", $"{GetTargetStatusMessage(workflowStatusID)}" +
                        $" thuộc đơn vị <b> {branchName} </b> (dòng {num}) ?");
                }
            }
            if (buttonReject != null)
            {
                RegisterConfirmDialog($"{GetRejectCssLink(num)}", "Bạn có chắc muốn thực hiện thao tác từ chối/hủy " +
                    $"thuộc đơn vị <b> {branchName} </b> (dòng {num}) ?");
            }
        }
        protected void GridOnItemCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item == null)
            {
                return;
            }
            string transactionID = item.GetDataKeyValue("ID").ToString();
            string workflowStatusID = e.CommandArgument?.ToString();
            string makerID = item?["MarkerUserID"].Text;
            string dealerID = item?["DealerUserID"].Text;
            string branchID = item?["BranchID"].Text;
            switch (e.CommandName)
            {
                case CommandTypeEnum.Reject:
                    ProcessTransaction(transactionID, workflowStatusID, GetRejectTransactionData,branchID,makerID,dealerID);
                    break;
                case CommandTypeEnum.Accept:
                    if (IsAcceptToChangeStatus(workflowStatusID))
                    {
                        ProcessTransaction(transactionID, workflowStatusID, GetAcceptTransactionData, branchID, makerID, dealerID);
                    }
                    else
                    {
                        GetPopupTransactionDeatil(transactionID, "BtnReloadBidManagementGridView");
                    }
                    break;
                case CommandTypeEnum.ViewEdit:
                    GetPopupTransactionDeatil(transactionID, "BtnReloadBidManagementGridView");
                    break;
                default:
                    ShowMessage("Thao tác không hợp lệ",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                    break;
            }


        }
        #endregion

        private void ProcessTransaction(string key, string workflowStatusID, Dictionary<string, SQLParameterData> parameterDatas,
            string branchID = null, string markerID = null, string dealerID = null)
        {
            string message;
            ModuleMessage.ModuleMessageType messageType;
            parameterDatas.Add("WorkflowStatusID", new SQLParameterData(workflowStatusID, SqlDbType.Int));
            if (TransactionBusiness.UpdateTransaction(key, parameterDatas, out message, out messageType))
            {
                ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);
                string targetStatus = string.Empty;
                if (parameterDatas.ContainsKey("IsReject") &&
                    parameterDatas["IsReject"].ParameterValue.Equals("True"))
                {
                    targetStatus = WorkflowStatusEnum.Reject.ToString();
                }
                SendNotificationMessage(string.Empty, string.Empty, workflowStatusID, branchID, markerID, dealerID,
                    targetStatus);
            }
            else
            {
                ShowMessage($"{message}", messageType);
            }
            Finish(key, ParseWorkflowStatus(workflowStatusID), messageType, message, null, null, true,
                $"{BidManagementUrl}/{TransactionTable.ID}/{key}");
        }
        private const string NotificationHtml = @"<span class=""badge badge-notify"">{0}</span>";
        protected string IsNewItemFormat(string value, string transactionID, string workflowStatus)
        {
            int statusID;
            if (TransactionIDLastest.Value.Equals(transactionID) && int.TryParse(workflowStatus, out statusID) &&
                statusID >= WorkflowStatusEnum.BRAsk && statusID < WorkflowStatusEnum.HOFinishTransaction)
            {
                return string.Format(NotificationHtml, value);
            }
            return value;
        }
        private static string EditExchangeRateUrl(string exchangeCode)
        {
            return $"<a href='#'>{exchangeCode}</a>";
        }


        protected void ExportReport(object sender, EventArgs e)
        {
            DataTable dtResult = TransactionBusiness.GetTransactionDailyReport();
            if (dtResult?.Rows?.Count > 0)
            {
                ExportToExcel(dtResult, "FX_DailyTran_" + DateTime.Now.ToString(PatternEnum.DateTime));
            }
            else
            {
                ShowMessage("Không tìm thấy dữ liệu để xuất báo cáo",
                    ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

        protected void ReloadInbox(object sender, EventArgs e)
        {
            if (IsRedirectAndShowNotification)
            {
                string url = $"{BidManagementUrl}/{TransactionTable.ID}/{GetSessionTransactionID}";
                string script = GetWindowOpenScript(url, null, false);
                RegisterScript(script);
            }
            else
            {
                BidExchangeBindData();
                BindTrasactionDailyReport();
            }
            
        }
    }
    
}