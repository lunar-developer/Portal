﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Forex.Business;
using Modules.Forex.Database;
using Modules.Forex.Enum;
using Modules.Forex.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace DesktopModules.Modules.Forex
{
    public partial class TransactionManagement : TransactionManagementBase
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
                BindData();
                ReloadTime.Value = GetReloadHOInboxTime.ToString();
            }
            finally
            {
                SetPermission();
            }
            
        }

        private void BindData()
        {
            GirdView.Visible = true;
            GirdView.LocalResourceFile = LocalResourceFile;
            SetGirdTitle();
            GridBind();
            BindBranch(ctBranch,IsHOViewer || IsHODealer || IsHOManager ? null : CurrentUserBranchData?.BranchID,
                IsHOViewer || IsHODealer || IsHOManager);
            BindCustomerType(ctCustomerType);
            BindTransactionType(ctTransactionType);
            BindReason(ctReasonTransaction);
            BindCurrency(ctExchangeCode);
            BindWorkflowStatus(ctTransactionStatus);
        }

        

        #region Grid Control
        private void SetGirdTitle()
        {
            GridTitle.InnerHtml = IsFindData ? "Danh sách tìm kiếm" : "Danh sách chờ xử lí";
        }

        private void GridBind()
        {
            GirdView.DataSource = TransactionBusiness.FindTransaction(GetFindParamData);
            GirdView.DataBind();
        }

        protected void GridOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GirdView.DataSource = TransactionBusiness.FindTransaction(GetFindParamData);
        }
        
        protected void GridOnDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            string num = item?["NumRecord"].Text;

            string customerName = item?["CustomerName"].Text;
            LinkButton buttonAccept = item?["ActionColumn"].Controls[5] as LinkButton;
            LinkButton buttonReject = item?["ActionColumn"].Controls[7] as LinkButton;
            if (buttonAccept != null)
            {
                string workflowStatusID = buttonAccept.CommandArgument?.ToString();
                if (IsAcceptToChangeStatus(workflowStatusID))
                {
                    RegisterConfirmDialog($"{GetAcceptCssLink(num)}", $"{GetTargetStatusMessage(workflowStatusID)}" +
                        $" đối với khách hàng <b> {customerName} </b> (dòng {num}) ?");
                }
            }
            if (buttonReject != null)
            {
                RegisterConfirmDialog($"{GetRejectCssLink(num)}", "Bạn có chắc muốn thực hiện thao tác từ chối/hủy " +
                    $"đối với khách hàng <b> {customerName} </b> (dòng {num}) ?");
            }
        }
        protected void GirdOnColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            GridColumnCollection columns = e.OwnerTableView?.Columns;
            if (columns == null || IsVisibleBranchName)
            {
                return;
            }
            foreach (GridColumn column in columns)
            {
                if (column.UniqueName == "Branch")
                {
                    column.Visible = false;
                }
                if (column.UniqueName == "InvoiceAmount")
                {
                    column.Visible = true;
                }
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
            Dictionary<string, SQLParameterData> parameterDatas;

            switch (e.CommandName)
            {
                case CommandTypeEnum.Reject:
                    parameterDatas = !string.IsNullOrWhiteSpace(workflowStatusID) && 
                        workflowStatusID.Equals(WorkflowStatusEnum.HOFinishTransaction.ToString()) ? GetRequestCancelTransactionData : 
                        GetRejectTransactionData;
                    ProcessTransaction(transactionID, workflowStatusID, parameterDatas);
                    break;
                case CommandTypeEnum.Accept:
                    if (IsAcceptToChangeStatus(workflowStatusID))
                    {
                        parameterDatas = GetAcceptTransactionData;
                        ProcessTransaction(transactionID, workflowStatusID, parameterDatas);
                    }
                    else
                    {
                        ViewTransactionDetail(transactionID);
                    }
                    break;
                case CommandTypeEnum.ViewEdit:
                    ViewTransactionDetail(transactionID);
                    break;
                case CommandTypeEnum.History:
                    string script = EditUrl(ConfigurationEnum.HistoryControlKey, 
                        600, 600, true,true, "BtnReloadTransactionManagementGridView", TransactionTable.ID, transactionID);
                    RegisterScript(script);
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
            string script = EditUrl(ConfigurationEnum.TransactionManagementControlKey, 
                600, 600, true,true, "BtnReloadTransactionManagementGridView", TransactionTable.ID, transactionID);
            RegisterScript(script);
        }
        private void ProcessTransaction(string key, string workflowStatusID, Dictionary<string, SQLParameterData> parameterDatas)
        {
            string message;
            parameterDatas.Add("WorkflowStatusID", new SQLParameterData(workflowStatusID, SqlDbType.Int));
            if (TransactionBusiness.UpdateTransaction(key, parameterDatas, out message))
            {
                SetPermission();
                GridBind();
                ShowMessage($"{message}.", ModuleMessage.ModuleMessageType.GreenSuccess);

            }
            else
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
        }
        protected void FindData(object sender, EventArgs e)
        {
            IsFindDataHidden.Value = "1";
            SetGirdTitle();
            GridBind();
        }

        protected void FindProcessData(object sender, EventArgs e)
        {
            IsFindDataHidden.Value = "0";
            SetGirdTitle();
            GridBind();
        }
        protected void ExportReport(object sender, EventArgs e)
        {
            DataTable dtResult = TransactionBusiness.FindTransaction(GetFindParamData);
            if (dtResult?.Rows?.Count > 0)
            {
                ExportToExcel(dtResult, "FX_" + DateTime.Now.ToString(PatternEnum.DateTime));
            }
            else
            {
                ShowMessage("Không tìm thấy dữ liệu để xuất báo cáo",
                    ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }
        protected void ReloadGirdData(object sender, EventArgs e)
        {
            GridBind();
        }
        private Dictionary<string, SQLParameterData> GetFindParamData
        {
            get
            {
                if (IsFindData == false) return GetDefaultParamDataByUser;
                Dictionary<string, SQLParameterData> findParam = new Dictionary<string, SQLParameterData>
                {
                    { TransactionTable.BranchID,
                        new SQLParameterData(IsHOViewer || IsHODealer || IsHOManager ? ctBranch.SelectedValue : CurrentUserBranchData?.BranchID,
                            SqlDbType.Int) },
                    { TransactionTable.CustomerTypeID, new SQLParameterData(ctCustomerType.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.TransactionTypeID, new SQLParameterData(ctTransactionType.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.ReasonCode, new SQLParameterData(ctReasonTransaction.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.CurrencyCode, new SQLParameterData(ctExchangeCode.SelectedValue, SqlDbType.Int) },
                    { TransactionTable.TransactionStatusID, new SQLParameterData(ctTransactionStatus.SelectedValue, SqlDbType.Int) }
                };

                if (calTransactionFromDate.SelectedDate != null && calTransactionToDate.SelectedDate != null)
                {
                    DateTime transactionFromDate = (DateTime)calTransactionFromDate.SelectedDate;
                    DateTime transactionToDate = (DateTime)calTransactionToDate.SelectedDate;
                    string warningTransactionDate;
                    if (ValidateSelectDate(transactionFromDate, transactionToDate, out warningTransactionDate))
                    {
                        findParam.Add("TransactionFromDate",
                            new SQLParameterData(transactionFromDate.ToString(PatternEnum.Date),
                                SqlDbType.BigInt));
                        findParam.Add("TransactionToDate",
                            new SQLParameterData(transactionToDate.ToString(PatternEnum.Date),
                                SqlDbType.BigInt));
                    }
                    else
                    {
                        ShowMessage($"'Ngày giao dịch' điều kiện tìm kiếm không hợp lệ {warningTransactionDate}. " +
                            "Mặc định kết quả tìm kiếm sẽ bỏ qua ngày giao dịch (ngày giá trị)",
                            ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }
                
                if (calCreateFromDate.SelectedDate != null && calCreateToDate.SelectedDate != null)
                {
                    DateTime creationFromDate = (DateTime)calCreateFromDate.SelectedDate;
                    DateTime creationToDate = (DateTime)calCreateToDate.SelectedDate;
                    string warningCreationDate;
                    if (ValidateSelectDate(creationFromDate, creationToDate, out warningCreationDate))
                    {
                        findParam.Add("CreationFromDateTime",
                            new SQLParameterData(creationFromDate.ToString(PatternEnum.Date),
                                SqlDbType.BigInt));
                        findParam.Add("CreationToDateTime", new SQLParameterData(creationToDate.ToString(PatternEnum.Date),
                            SqlDbType.BigInt));
                    }
                    else
                    {
                        ShowMessage($"'Ngày tạo giao dịch' điều kiện tìm kiếm không hợp lệ {warningCreationDate}. " +
                            "mặc định kết quả sẽ hiển thị ngày tạo giao dịch hiện tại.",
                            ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                    
                }
                return findParam;
            }
        }

        
        private void SetPermission()
        {
            btnInbox.Enabled = IsBRMaker || IsBRManager || IsHODealer || IsHOManager;
            btnInbox.Visible = IsBRMaker || IsBRManager || IsHODealer || IsHOManager;
            btnFind.Enabled = IsBRViewer || IsBRMaker || IsBRManager || IsHOViewer || IsHODealer || IsHOManager;
            btnExportExcel.Enabled = IsBRViewer || IsBRMaker || IsBRManager || IsHOViewer || IsHODealer || IsHOManager;
        }

        private void FormInit()
        {
            calCreateFromDate.MaxDate = DateTime.Now;
            calCreateFromDate.MinDate = DateTime.Now.AddMonths(-1);

            calCreateToDate.MaxDate = DateTime.Now;
            calCreateToDate.MinDate = DateTime.Now.AddMonths(-1);

            calTransactionFromDate.MaxDate = DateTime.Now.AddDays(365);
            calTransactionFromDate.MinDate = DateTime.Now.AddMonths(-1);

            calTransactionToDate.MaxDate = DateTime.Now.AddDays(365);
            calTransactionToDate.MinDate = DateTime.Now.AddMonths(-1);

        }

        private bool IsFindData => !string.IsNullOrWhiteSpace(IsFindDataHidden.Value) && IsFindDataHidden.Value == "1";


        
    }
    
}