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
            if (IsPostBack)
            {
                return;
            }
            GridBind();
            ReloadTime.Value = GetReloadHOInboxTime.ToString();
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
            switch (e.CommandName)
            {
                case CommandTypeEnum.Reject:
                    ProcessTransaction(transactionID, workflowStatusID, GetRejectTransactionData);
                    break;
                case CommandTypeEnum.Accept:
                    if (IsAcceptToChangeStatus(workflowStatusID))
                    {
                        ProcessTransaction(transactionID, workflowStatusID, GetAcceptTransactionData);
                    }
                    else
                    {
                        ViewTransactionDetail(transactionID);
                    }
                    break;
                case CommandTypeEnum.ViewEdit:
                    ViewTransactionDetail(transactionID);
                    break;
                default:
                    ShowMessage("Thao tác không hợp lệ",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                    break;
            }


        }
        #endregion
        private void ViewTransactionDetail(string transactionID)
        {
            string script = EditUrl(ConfigurationEnum.BidManagementControlKey,
                600, 600, true,true,"BtnReloadBidManagementGridView", TransactionTable.ID, transactionID);
            RegisterScript(script);
        }

        private void ProcessTransaction(string key, string workflowStatusID, Dictionary<string, SQLParameterData> parameterDatas)
        {
            string message;
            parameterDatas.Add("WorkflowStatusID", new SQLParameterData(workflowStatusID, SqlDbType.Int));
            if (TransactionBusiness.UpdateTransaction(key, parameterDatas, out message))
            {
                BidExchangeBindData();
                if (workflowStatusID == WorkflowStatusEnum.BRApprovalTransaction.ToString() ||
                    workflowStatusID == WorkflowStatusEnum.HOApprovalCancelException.ToString() ||
                    workflowStatusID == WorkflowStatusEnum.HOApprovalCancel.ToString())
                {
                    BindTrasactionDailyReport();
                }
                ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

            }
            else
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
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
            BidExchangeBindData();
            BindTrasactionDailyReport();
        }
    }
    
}