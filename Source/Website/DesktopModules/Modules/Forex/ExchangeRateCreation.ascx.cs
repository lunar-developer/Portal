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
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Forex
{
    public partial class ExchangeRateCreation : ForexModulesBase
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
                string currencyCode = Request.QueryString[TransactionTable.CurrencyCode];
                if (!string.IsNullOrWhiteSpace(currencyCode))
                {
                    currencyCode = currencyCode.Replace("_", "/");
                    BindCurrency(ctExchangeCode,currencyCode);
                    BindPriceInfo(currencyCode);
                }
            }
            finally 
            {
                SetPermission();
            }
        }

        private void BindData()
        {
            //Thong tin gia
            BindCurrency(ctExchangeCode);
            BindCurrencyGrid();
            BindExchangeGrid();
        }
        
        private void BindPriceInfo(string currencyCode)
        {
            
            if (IsSelectBoxChangeFormValue(currencyCode))
            {
                txtRemark.Enabled = IsChangePermission;
                CurrencyRateData currencyRateData = CacheBase.Receive<CurrencyRateData>(currencyCode);

                SetTextControl(txtBigFigure, FunctionBase.FormatCurrency(currencyRateData.Rate),false);
                SetTextControl(txtMasterRate, FunctionBase.FormatCurrency(currencyRateData.MasterRate), false);
                SetTextControl(txtMargin, currencyRateData.MarginMinProfit, false);
                SetTextControl(txtLimit, currencyRateData.MarginLimit, false);

                ExchangeRateGridData exchangeRateData  = CacheBase.Receive<ExchangeRateGridData>(currencyCode);

                chkBuyRateFT.InnerHtml =
                    CheckBoxControl(ControlCheckBoxEnum.BuyRateFT, 
                    !bool.Parse(exchangeRateData.IsDisableBuyFT), exchangeRateData.IsDisableBuyFT, IsChangePermission);
                SetTextControl(txtBuyRateFT, exchangeRateData.BuyRateFT, IsChangePermission);
                SetTextControl(txtDealTimeBuyFT, exchangeRateData.DealTimeBuyFT, IsChangePermission);

                chkSellRateFT.InnerHtml = CheckBoxControl(ControlCheckBoxEnum.SellRateFT, 
                    !bool.Parse(exchangeRateData.IsDisableSellFT), exchangeRateData.IsDisableSellFT, IsChangePermission);
                SetTextControl(txtSellRateFT, exchangeRateData.SellRateFT, IsChangePermission);
                SetTextControl(txtDealTimeSellFT, exchangeRateData.DealTimeSellFT, IsChangePermission);

                chkBuyRateCash.InnerHtml = CheckBoxControl(ControlCheckBoxEnum.BuyRateCash, 
                    !bool.Parse(exchangeRateData.IsDisableBuyCash), exchangeRateData.IsDisableBuyCash, IsChangePermission);
                SetTextControl(txtBuyRateCash, exchangeRateData.BuyRateCash, IsChangePermission);
                SetTextControl(txtDealTimeBuyCash, exchangeRateData.DealTimeBuyCash, IsChangePermission);

                chkSellRateCash.InnerHtml = CheckBoxControl(ControlCheckBoxEnum.SellRateCash, 
                    !bool.Parse(exchangeRateData.IsDisableSellCash), exchangeRateData.IsDisableSellCash, IsChangePermission);
                SetTextControl(txtSellRateCash, exchangeRateData.SellRateCash, IsChangePermission);
                SetTextControl(txtDealTimeSellCash, exchangeRateData.DealTimeSellCash, IsChangePermission);
            }
            else
            {
                ResetForm();
            }

        }

        private void ResetForm()
        {
            SetTextControl(txtBigFigure, string.Empty, false);
            SetTextControl(txtMasterRate, string.Empty, false);
            SetTextControl(txtMargin, string.Empty, false);
            SetTextControl(txtLimit, string.Empty, false);

            chkBuyRateFT.InnerHtml = string.Empty;
            SetTextControl(txtBuyRateFT, string.Empty, false);
            SetTextControl(txtDealTimeBuyFT, string.Empty, false);

            chkSellRateFT.InnerHtml = string.Empty;
            SetTextControl(txtSellRateFT, string.Empty, false);
            SetTextControl(txtDealTimeSellFT, string.Empty, false);

            chkBuyRateCash.InnerHtml = string.Empty;
            SetTextControl(txtBuyRateCash, string.Empty, false);
            SetTextControl(txtDealTimeBuyCash, string.Empty, false);

            chkSellRateCash.InnerHtml = string.Empty;
            SetTextControl(txtSellRateCash, string.Empty, false);
            SetTextControl(txtDealTimeSellCash, string.Empty, false);
            txtRemark.Text = string.Empty;
            txtRemark.Enabled = false;
        }
        protected void ExChangeCodeChange(object sender, EventArgs e)
        {
            BindPriceInfo(ctExchangeCode.SelectedValue);
        }

        private string CheckBoxValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true.ToString();
            return false.ToString();
        }

        private ExchangeRateGridData GetFormData => new ExchangeRateGridData
        {
            CurrencyCode = ctExchangeCode.SelectedValue,
            IsDisableBuyFT = CheckBoxValue(Request.Params[ControlCheckBoxEnum.BuyRateFT]),
            BuyRateFT = txtBuyRateFT.Text,
            DealTimeBuyFT = txtDealTimeBuyFT.Text,
            IsDisableSellFT = CheckBoxValue(Request.Params[ControlCheckBoxEnum.SellRateFT]),
            SellRateFT = txtSellRateFT.Text,
            DealTimeSellFT = txtDealTimeSellFT.Text,
            IsDisableBuyCash = CheckBoxValue(Request.Params[ControlCheckBoxEnum.BuyRateCash]),
            BuyRateCash = txtBuyRateCash.Text,
            DealTimeBuyCash = txtDealTimeBuyCash.Text,
            IsDisableSellCash = CheckBoxValue(Request.Params[ControlCheckBoxEnum.SellRateCash]),
            SellRateCash = txtSellRateCash.Text,
            DealTimeSellCash = txtDealTimeSellCash.Text
        };

        protected void UpdateExchangeRate(object sender, EventArgs e)
        {
            ExchangeRateGridData currentData = CacheBase.Receive<ExchangeRateGridData>(ctExchangeCode.SelectedValue);

            ExchangeRateGridData dataUpdate = GetFormData;
            string message;
            if (ExchangeRateBusiness.UpdateExchangeRate(dataUpdate,
                currentData, txtRemark.Text, UserInfo.UserID.ToString(),
                DateTime.Now.ToString(PatternEnum.DateTime), out message))
            {
                ShowMessage("Cập nhật yêu cầu thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                CacheBase.Reload<ExchangeRateGridData>();
                BindPriceInfo(ctExchangeCode.SelectedValue);
            }
            else
            {
                ShowMessage($"{message}", ModuleMessage.ModuleMessageType.RedError);
            }
        }
        
        private bool IsSelectBoxChangeFormValue(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode)) return false;
            return true;
        }
        
        private bool IsChangePermission => IsHOAdmin;
        private void SetPermission()
        {
            btnSubmit.Enabled = !string.IsNullOrWhiteSpace(ctExchangeCode.SelectedValue) && IsChangePermission;
        }
        private void FormInit()
        {

            ctExchangeCode.Attributes.Add("placeholder", GetResource("lblExchangeCode.Help"));

            txtBuyRateFT.Attributes.Add("placeholder", GetResource("lblBuyRateFT.Help"));
            txtDealTimeBuyFT.Attributes.Add("placeholder", GetResource("lblDealTimeBuyFT.Text"));
            txtSellRateFT.Attributes.Add("placeholder", GetResource("lblSellRateFT.Help"));
            txtDealTimeSellFT.Attributes.Add("placeholder", GetResource("lblDealTimeSellFT.Text"));

            txtBuyRateCash.Attributes.Add("placeholder", GetResource("lblBuyRateCash.Help"));
            txtDealTimeBuyCash.Attributes.Add("placeholder", GetResource("lblDealTimeBuyCash.Text"));
            txtSellRateCash.Attributes.Add("placeholder", GetResource("lblSellRateCash.Help"));
            txtDealTimeSellCash.Attributes.Add("placeholder", GetResource("lblDealTimeSellCash.Text"));

            txtRemark.Attributes.Add("placeholder", GetResource("lblRemark.Help"));
            
        }

        protected string LinkTemplateCurrency => $"{Request.ApplicationPath}/DesktopModules/Modules/Forex/Asset/Template/FX_CurrencyRate.xlsx".Replace(@"//", "/");
        protected string LinkTemplateExchange => $"{Request.ApplicationPath}/DesktopModules/Modules/Forex/Asset/Template/FX_ExchangeRate.xlsx".Replace(@"//", "/");
        internal class ControlCheckBoxEnum
        {
            public const string BuyRateFT = "CtrlCheckBuyRateFT";
            public const string SellRateFT = "CtrlCheckSellRateFT";
            public const string BuyRateCash = "CtrlCheckBuyRateCash";
            public const string SellRateCash = "CtrlCheckSellRateCash";
        }

        protected void UploadCurrency(object sender, EventArgs e)
        {
            if (fupFileCurrency.HasFile)
            {
                try
                {
                    List<CurrencyRateData> listResult = FunctionBase.ImportExcel<CurrencyRateData>(fupFileCurrency?.FileContent);
                    string message;
                    if (CurrencyRateBusiness.InsertAndReview(listResult, UserInfo.UserID, out message))
                    {
                        ShowMessage($"{message}",
                            ModuleMessage.ModuleMessageType.GreenSuccess);
                        BindCurrencyGrid();
                    }
                    else
                    {
                        ShowMessage($"{message}",
                            ModuleMessage.ModuleMessageType.RedError);
                    }
                }
                catch (Exception exception)
                {
                    ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage("Không tìm thấy file upload", ModuleMessage.ModuleMessageType.RedError);
            }
        }
        protected void ApprovalCurrency(object sender, EventArgs e)
        {
            string message;
            if (CurrencyRateBusiness.ApprovalChangeCurrency(out message))
            {
                ShowMessage($"{message}",
                    ModuleMessage.ModuleMessageType.GreenSuccess);
                CacheBase.Reload<CurrencyRateData>();
                CacheBase.Reload<ExchangeRateGridData>();
                BindCurrencyGrid();
            }
            else
            {
                ShowMessage($"{message}",
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }
        protected void UploadExchange(object sender, EventArgs e)
        {
            if (fupFileExchange.HasFile)
            {
                try
                {
                    List<ExchangeRateData> listResult = FunctionBase.ImportExcel<ExchangeRateData>(fupFileExchange?.FileContent);
                    string message;
                    if (ExchangeRateBusiness.InsertAndReview(listResult, UserInfo.UserID, out message))
                    {
                        ShowMessage($"{message}",
                            ModuleMessage.ModuleMessageType.GreenSuccess);
                        BindExchangeGrid();
                    }
                    else
                    {
                        ShowMessage($"{message}",
                            ModuleMessage.ModuleMessageType.RedError);
                    }
                }
                catch (Exception exception)
                {
                    ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage("Không tìm thấy file upload", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void BindCurrencyGrid()
        {
            DataTable dt = CurrencyRateBusiness.GetDataUpload();
            CurrencyGrid.Visible = dt?.Rows?.Count > 0;
            CurrencyReviewPannel.Visible = dt?.Rows?.Count > 0;
            CurrencyGrid.DataSource = dt;
            CurrencyGrid.DataBind();
        }
        protected void CurrencyGridReview(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrencyGrid.DataSource = CurrencyRateBusiness.GetDataUpload();
        }
        private void BindExchangeGrid()
        {
            DataTable dt = ExchangeRateBusiness.GetDataUpload();
            ExchangeGrid.Visible = dt?.Rows?.Count > 0;
            ExchangeUploadPannel.Visible = dt?.Rows?.Count > 0;
            ExchangeGrid.DataSource = dt;
            ExchangeGrid.DataBind();
        }
        protected void ExchangeRateGridReview(object sender, GridNeedDataSourceEventArgs e)
        {
            ExchangeGrid.DataSource = ExchangeRateBusiness.GetDataUpload();
        }

        protected void ApprovalExchangeRate(object sender, EventArgs e)
        {
            string message;
            if (ExchangeRateBusiness.ApprovalChangeCurrency(out message))
            {
                ShowMessage($"{message}",
                    ModuleMessage.ModuleMessageType.GreenSuccess);
                CacheBase.Reload<ExchangeRateGridData>();
                BindExchangeGrid();
            }
            else
            {
                ShowMessage($"{message}",
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void ExportCurrency(object sender, EventArgs e)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(CurrencyRateTable.CurrencyCode);
            dtResult.Columns.Add(CurrencyRateTable.Rate);
            dtResult.Columns.Add(CurrencyRateTable.MasterRate);
            dtResult.Columns.Add(CurrencyRateTable.MarginMinProfit);
            dtResult.Columns.Add(CurrencyRateTable.MarginLimit);
            dtResult.Columns.Add(CurrencyRateTable.IsDisable);
            List<CurrencyRateData> list = CacheBase.Receive<CurrencyRateData>();
            if (list.Count > 0)
            {
                foreach (CurrencyRateData item in list)
                {
                    DataRow dr = dtResult.NewRow();
                    dr[CurrencyRateTable.CurrencyCode] = item.CurrencyCode;
                    dr[CurrencyRateTable.Rate] = item.Rate;
                    dr[CurrencyRateTable.MasterRate] = item.MasterRate ?? "0";
                    dr[CurrencyRateTable.MarginMinProfit] = item.MarginMinProfit ?? "0";
                    dr[CurrencyRateTable.MarginLimit] = item.MarginLimit ?? "0";
                    dr[CurrencyRateTable.IsDisable] = FunctionBase.ConvertToBool(item.IsDisable) == false ? "0" : "1";
                    dtResult.Rows.Add(dr);
                    dtResult.AcceptChanges();
                }
                ExportToExcel(dtResult, "FX_CurrencyRate_" + DateTime.Now.ToString(PatternEnum.DateTime));
            }
            else
            {
                ShowMessage("Không có dữ liệu để xuất file", ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

        protected void ExportExchange(object sender, EventArgs e)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(ExchangeRateTable.CurrencyCode);
            dtResult.Columns.Add(ExchangeRateTable.TransactionTypeID);
            dtResult.Columns.Add(ExchangeRateTable.Rate);
            dtResult.Columns.Add(ExchangeRateTable.DealTime);
            dtResult.Columns.Add(ExchangeRateTable.IsDisable);
            List<CurrencyRateData> listCurrency = CacheBase.Receive<CurrencyRateData>();
            List<TransactionTypeData> listTransactionType = CacheBase.Receive<TransactionTypeData>();
            if (listCurrency.Count > 0 && listTransactionType.Count > 0)
            {
                foreach (CurrencyRateData currency in listCurrency)
                {
                    
                    foreach (TransactionTypeData tran in listTransactionType)
                    {
                        DataRow dr = dtResult.NewRow();
                        dr[ExchangeRateTable.CurrencyCode] = currency.CurrencyCode;
                        dr[ExchangeRateTable.TransactionTypeID] = tran.ID;
                        ExchangeRateData exchangeRate = GetExchangerateData(currency.CurrencyCode, tran.ID);
                        dr[ExchangeRateTable.Rate] = exchangeRate?.Rate ?? "0";
                        dr[ExchangeRateTable.DealTime] = exchangeRate?.DealTime ?? "0";
                        dr[ExchangeRateTable.IsDisable] = FunctionBase.ConvertToBool(exchangeRate?.IsDisable) == false ? "0" : "1";
                        dtResult.Rows.Add(dr);
                        dtResult.AcceptChanges();
                    }
                    
                    
                }
                ExportToExcel(dtResult, "FX_ExchangeRate_" + DateTime.Now.ToString(PatternEnum.DateTime));
            }
            else
            {
                ShowMessage("Không có dữ liệu để xuất file", ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }
    }

}