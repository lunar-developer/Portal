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
using Website.Library.Global;

namespace DesktopModules.Modules.Forex
{
    public partial class ExchangeRate : ForexModulesBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            GridBind();
        }

        private void GridBind()
        {
            CurrentRateGrid.Visible = true;
            CurrentRateGrid.LocalResourceFile = LocalResourceFile;
            CurrentRateGrid.DataSource = GetExchangeRate4Grid();
            CurrentRateGrid.DataBind();
        }
        protected void CurrentRateGridOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentRateGrid.DataSource = GetExchangeRate4Grid();
        }
        private static DataTable ExchangeRateTemplate4Grid
        {
            get
            {
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("#");
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.CurrencyCode);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.BigFigure);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.BuyRateFT);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.SellRateFT);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.BuyRateCash);
                dtResult.Columns.Add(ExchangeRateGridFieldEnum.SellRateCash);
                return dtResult;
            }
        }
        private static DataTable GetExchangeRate4Grid()
        {
            DataTable dtResult = ExchangeRateTemplate4Grid;
            int count = 0;
            foreach (ExchangeRateGridData rate in CacheBase.Receive<ExchangeRateGridData>())
            {
                count++;
                CurrencyRateData currencyRateData = CacheBase.Receive<CurrencyRateData>(rate.CurrencyCode);
                DataRow dr = dtResult.NewRow();
                dr["#"] = count;
                dr[ExchangeRateGridFieldEnum.CurrencyCode] = rate.CurrencyCode;
                dr[ExchangeRateGridFieldEnum.BigFigure] = string.IsNullOrWhiteSpace(currencyRateData?.Rate) ? string.Empty :
                                                            FunctionBase.FormatCurrency(currencyRateData.Rate);
                dr[ExchangeRateGridFieldEnum.BuyRateFT] = ExchangeRateFormat(rate.BuyRateFT, rate.BuyRateFTStatus, rate.IsDisableBuyFT);
                dr[ExchangeRateGridFieldEnum.SellRateFT] = ExchangeRateFormat(rate.SellRateFT, rate.SellRateFTStatus, rate.IsDisableSellFT);
                dr[ExchangeRateGridFieldEnum.BuyRateCash] = ExchangeRateFormat(rate.BuyRateCash, rate.BuyRateCashStatus, rate.IsDisableBuyCash);
                dr[ExchangeRateGridFieldEnum.SellRateCash] = ExchangeRateFormat(rate.SellRateCash, rate.SellRateCashStatus, rate.IsDisableSellCash);
                dtResult.Rows.Add(dr);
                dtResult.AcceptChanges();
            }

            return dtResult;
        }
        private void TransactionCreation(string currencyCode,string transactiontypeID)
        {
            if (!string.IsNullOrWhiteSpace(currencyCode)) currencyCode = currencyCode.Replace("/", "_");
            string script = EditUrl(ConfigurationEnum.TransactionCreationControlKey,
                600, 600, false, true, null, 
                TransactionTable.CurrencyCode, currencyCode, TransactionTable.TransactionTypeID, transactiontypeID);
            RegisterScript(script);
        }
        private void ExchangeRateCreation(string currencyCode)
        {
            if (!string.IsNullOrWhiteSpace(currencyCode)) currencyCode = currencyCode.Replace("/", "_");
            string script = EditUrl(ConfigurationEnum.ExchangeRateCreationControlKey,
                600, 600, true, true, "BtnReloadExchangeRate",
                TransactionTable.CurrencyCode, currencyCode);
            RegisterScript(script);
        }
        protected void GridOnItemCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item == null)
            {
                return;
            }
            string currencyCode = item.GetDataKeyValue("CurrencyCode").ToString();
            switch (e.CommandName)
            {
                case CommandTypeEnum.TransactionCreation:
                    if (IsBRMaker || IsBRManager)
                    {
                        string transactionTypeID = e.CommandArgument?.ToString();
                        TransactionCreation(currencyCode, transactionTypeID);
                    }
                    break;
                case CommandTypeEnum.ChangeCurrencyCode:
                    if (IsHODealer || IsHOManager)
                    {
                        ExchangeRateCreation(currencyCode);
                    }
                    break;
                default:
                    ShowMessage("Thao tác không hợp lệ",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                    break;
            }


        }

        protected void ReloadExchangeRate(object sender, EventArgs e)
        {
            GridBind();
        }
    }
    
}