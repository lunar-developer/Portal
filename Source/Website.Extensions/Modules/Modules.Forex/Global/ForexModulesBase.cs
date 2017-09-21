using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Forex.Business;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Modules.UserManagement.DataTransfer;
using OfficeOpenXml;
using Telerik.Web.UI;
using Website.Library.Global;
using RoleEnum = Modules.Forex.Enum.RoleEnum;

namespace Modules.Forex.Global
{
    public class ForexModulesBase: DesktopModuleBase
    {
        protected static int ParseWorkflowStatus(string workflowStatus)
        {
            if (string.IsNullOrWhiteSpace(workflowStatus) || !int.TryParse(workflowStatus, out int status))
            {
                return 0;
            }
            return status;
        }

        protected BranchData CurrentUserBranchData => CacheBase.Receive<BranchData>(CurrentUserData?.BranchID);
        protected string CurrentUserName => CurrentUserData.DisplayName;

        protected string GetCreationUserByID(string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                return CurrentUserName;
            }
            return CacheBase.Receive<UserData>(userID)?.DisplayName ?? string.Empty;
        }

        protected string GetBranchName(string branchID)
        {
            if (string.IsNullOrWhiteSpace(branchID))
            {
                return UserBranchName;
            }
            BranchData branch = CacheBase.Receive<BranchData>(branchID);
            if (branch != null)
            {
                return $"{branch.BranchCode}-{branch.BranchName}";
            }
            return "N/A";
        }
        protected string UserBranchName => $"{CurrentUserBranchData.BranchCode}-{CurrentUserBranchData.BranchName}";

        protected static ExchangeRateData GetExchangerateData(string currencyCode, string transactionTypeID)
        {
            ExchangeRateGridData exchangeGridData = GetExchangeRateGridData(currencyCode);
            ExchangeRateData exchangeRate = new ExchangeRateData { CurrencyCode = currencyCode, TransactionTypeID = transactionTypeID };
            int value = Int32.Parse(transactionTypeID);
            switch (value)
            {
                case TransactionTypeEnum.BuyByFundTranfer:
                    exchangeRate.Rate = exchangeGridData.BuyRateFT;
                    exchangeRate.DealTime = exchangeGridData.DealTimeBuyFT;
                    exchangeRate.IsDisable = exchangeGridData.IsDisableBuyFT;
                    exchangeRate.RateStatus = exchangeGridData.BuyRateFTStatus;
                    break;
                case TransactionTypeEnum.SellByFundTranfer:
                    exchangeRate.Rate = exchangeGridData.SellRateFT;
                    exchangeRate.DealTime = exchangeGridData.DealTimeSellFT;
                    exchangeRate.IsDisable = exchangeGridData.IsDisableSellFT;
                    exchangeRate.RateStatus = exchangeGridData.SellRateFTStatus;
                    break;
                case TransactionTypeEnum.BuyByCash:
                    exchangeRate.Rate = exchangeGridData.BuyRateCash;
                    exchangeRate.DealTime = exchangeGridData.DealTimeBuyCash;
                    exchangeRate.IsDisable = exchangeGridData.IsDisableBuyCash;
                    exchangeRate.RateStatus = exchangeGridData.BuyRateCashStatus;
                    break;
                case TransactionTypeEnum.SellByCash:
                    exchangeRate.Rate = exchangeGridData.SellRateCash;
                    exchangeRate.DealTime = exchangeGridData.DealTimeSellCash;
                    exchangeRate.IsDisable = exchangeGridData.IsDisableSellCash;
                    exchangeRate.RateStatus = exchangeGridData.SellRateCashStatus;
                    break;
            }
            return exchangeRate;
        }

        protected UserData CurrentUserData => CacheBase.Receive<UserData>(UserInfo.UserID.ToString());

        protected static string ExchangeRateFormat(string rate, string flag, string isDisable = "0")
        {
            if (string.IsNullOrWhiteSpace(rate) || 
                string.IsNullOrWhiteSpace(flag) ||
                string.IsNullOrWhiteSpace(isDisable) ||
                isDisable == "1")
                return "---".PadRight(50, ' ') + PriceCancel;
            if (flag.Equals(ExchangeRateStatusEnum.PriceUp)) return rate.PadRight(50, ' ') + PriceUp;
            if (flag.Equals(ExchangeRateStatusEnum.PriceDown)) return rate.PadRight(50, ' ') + PriceDown;
            return rate;
        }

        protected static CurrencyRateData GetCurrencyData(string currencyCode)
        {
            return CacheBase.Receive<CurrencyRateData>(currencyCode);
        }

        protected static TransactionTypeData GetTransactionTypeData(string transactionTypeID)
        {
            return CacheBase.Receive<TransactionTypeData>(transactionTypeID);
        }

        protected static ExchangeRateGridData GetExchangeRateGridData(string currencyCode)
        {
            return CacheBase.Receive<ExchangeRateGridData>(currencyCode);
        }
        #region Select Box Control

        protected static RadComboBoxItem CreateItem(string text, string value, string isDisable = "0", bool isSelected = false)
        {
            RadComboBoxItem item = new RadComboBoxItem(text, value)
            {
                Enabled = FunctionBase.ConvertToBool(isDisable) == false,
                Selected = isSelected
            };
            return item;
        }
        private static void BindItems(RadComboBox dropDownList, RadComboBoxItem[] additionalItems)
        {
            dropDownList.ClearSelection();
            dropDownList.ClearCheckedItems();
            dropDownList.Items.Clear();
            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
        }

        private static RadComboBoxItem[] SetOneItemSelectBox(RadComboBoxItem item = null)
        {
            if (item == null) item = DefaultComBoxControlItem;
            RadComboBoxItem[] additionalItems = new RadComboBoxItem[1];
            additionalItems.SetValue(item, 0);
            return additionalItems;
        }
        protected static void BindCurrency(RadComboBox dropDownList, string currencyCode = null, bool isChange = true)
        {
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (CurrencyRateData cacheData in CacheBase.Receive<CurrencyRateData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.CurrencyCode, cacheData.CurrencyCode,
                    cacheData.IsDisable, !string.IsNullOrWhiteSpace(currencyCode) && cacheData.CurrencyCode.Equals(currencyCode)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindTransactionType(RadComboBox dropDownList, string transactionTypeID = null, bool isChange = true)
        {
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (TransactionTypeData cacheData in CacheBase.Receive<TransactionTypeData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable,
                    !string.IsNullOrWhiteSpace(transactionTypeID) && cacheData.ID.Equals(transactionTypeID)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindCustomerType(RadComboBox dropDownList, string customerTypeID = null, bool isChange = true)
        {
            
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (CustomerTypeData cacheData in CacheBase.Receive<CustomerTypeData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable,
                    !string.IsNullOrWhiteSpace(customerTypeID) && cacheData.ID.Equals(customerTypeID)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindWorkflowStatus(RadComboBox dropDownList, string workflowStatusID = null, bool isChange = true)
        {
            
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (WorkflowStatusData cacheData in CacheBase.Receive<WorkflowStatusData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable,
                    !string.IsNullOrWhiteSpace(workflowStatusID) && cacheData.ID.Equals(workflowStatusID)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindRequestType(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (RequestTypeData cacheData in CacheBase.Receive<RequestTypeData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable));
            }
        }
        protected static void BindReason(RadComboBox dropDownList, string reasonCode = null, bool isChange = true)
        {
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (ReasonData cacheData in CacheBase.Receive<ReasonData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable,
                    !string.IsNullOrWhiteSpace(reasonCode) && cacheData.ID.Equals(reasonCode)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindBranch(RadComboBox dropDownList, string branchID = null, bool isChange = true)
        {
            BindItems(dropDownList, SetOneItemSelectBox());
            foreach (BranchData cacheData in CacheBase.Receive<BranchData>())
            {
                dropDownList.Items.Add(CreateItem($"{cacheData.BranchCode}-{cacheData.BranchName}", cacheData.BranchID, cacheData.IsDisable,
                    !string.IsNullOrWhiteSpace(branchID) && cacheData.BranchID.Equals(branchID)));
            }
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static RadComboBoxItem DefaultComBoxControlItem => new RadComboBoxItem { Text = "Chưa chọn", Value = "" };


        #endregion



        #region SetControl
        protected static string GetReason(string reasonCode)
        {
            foreach (ReasonData cacheData in CacheBase.Receive<ReasonData>())
            {
                if (cacheData.ID.Equals(reasonCode))
                {
                    return cacheData.Title;
                }
            }
            return "N/A";
        }
        protected string CheckBoxControl(string elementID, bool isChecked, string value = null, bool isChange = true)
        {
            string checkBoxControl = "&nbsp;";
            if (String.IsNullOrWhiteSpace(value))
            {
                value = (isChecked ? "true" : "false");
            }
            checkBoxControl = $@"
                            <input  name='{elementID}'
                                    type='checkbox'
                                    {(isChecked ? "checked='checked'" : String.Empty)}
                                    value='{value}'
                                    {(isChange == false ? "disabled='disabled'" : String.Empty)}
                                    class='c-check'
                                    id='{elementID}'
                                    autocomplete='off'>
                            <label for='{elementID}'>
                                <span class='inc'></span>
                                <span class='check'></span>
                                <span class='box'></span>
                            </label>
                        ";
            return checkBoxControl;
        }
        protected static void SetTextControl(TextBox textBox, string value = null, bool isChange = true, bool isEnable = true)
        {
            textBox.Text = value ?? string.Empty;
            textBox.ReadOnly = !isChange;
            textBox.Enabled = isEnable;
            string readOnlyCssClass = $"{(isChange == false || isEnable == false ? " exchange-label" : String.Empty)}";
            if (!String.IsNullOrWhiteSpace(readOnlyCssClass))
            {
                readOnlyCssClass += $"{(String.IsNullOrWhiteSpace(value) ? "-nonevalue" : String.Empty)}";
            }
            textBox.CssClass = $"form-control c-theme{readOnlyCssClass}";
            if (!isChange)
            {
                textBox.TabIndex = -1;
            }
        }

        protected static readonly DateTime CurrentDate = DateTime.Now.Date;
        protected static void SetDatePicker(DnnDatePicker calDatePicker, DateTime value, bool isChange = true)
        {
            calDatePicker.SelectedDate = value;
            calDatePicker.Enabled = isChange;
            if (!isChange)
            {
                calDatePicker.TabIndex = -1;
            }
        }
        
        protected DateTime GetTransactionDate(string transactionDate)
        {
            if (string.IsNullOrWhiteSpace(transactionDate)) return CurrentDate;
            DateTime tranDate;
            if (DateTime.TryParse(transactionDate, out tranDate))
            {
                return tranDate;
            }
            return CurrentDate;
        }
        

        #endregion
        #region Page Image & Item


        protected static string PriceUp => @"<span class=""btn-icon-color-success""><i class=""fa fa-arrow-up"" aria-hidden=""true""></i></span>";
        protected static string PriceDown => @"<span class=""btn-icon-color-danger""><i class=""fa fa-arrow-down"" aria-hidden=""true""></i></span>";
        protected static string PriceCancel => @"<span class=""btn-icon-color-danger""><i class=""fa fa-ban"" aria-hidden=""true""></i></span>";
        protected static string IsCheckedImage => FunctionBase.GetAbsoluteUrl("/images/grant.gif");
        #endregion
        #region Permission
        protected bool IsAdministrator => IsInRole(RoleEnum.Administrator) || IsSuperAdministrator;
        
        protected bool IsSuperAdministrator => UserInfo.IsSuperUser;

        protected bool CheckRole(string roleNameList)
        {
            if (string.IsNullOrWhiteSpace(roleNameList)) return false;
            if (!roleNameList.Contains(";")) return IsInRole(roleNameList);
            string[] roleArr = roleNameList.Split(';');
            foreach (string roleName in roleArr)
            {
                if (IsInRole(roleName)) return true;
            }
            return false;
        }

        protected bool IsAcceptRole(int workflowStatusID)
        {
            ActionData action =
                ActionBusiness.GetItemtByActionCode(workflowStatusID, UserInfo.UserID, ActionCodeEnum.Submit);
            return !string.IsNullOrWhiteSpace(action?.IsRole) && FunctionBase.ConvertToBool(action.IsRole);
        }
        protected bool IsUpdateRole(int workflowStatusID)
        {
            ActionData action =
                ActionBusiness.GetItemtByActionCode(workflowStatusID, UserInfo.UserID, ActionCodeEnum.Update);
            return !string.IsNullOrWhiteSpace(action?.IsRole) && FunctionBase.ConvertToBool(action.IsRole);
        }
        protected bool IsRejectRole(int workflowStatusID)
        {
            ActionData action =
                ActionBusiness.GetItemtByActionCode(workflowStatusID, UserInfo.UserID, ActionCodeEnum.Cancel);
            return !string.IsNullOrWhiteSpace(action?.IsRole) && FunctionBase.ConvertToBool(action.IsRole);
        }
        protected bool IsBRViewer => IsInRole(RoleEnum.BRViewer);
        protected bool IsBRMaker => IsInRole(RoleEnum.BRMaker);
        protected bool IsBRManager => IsInRole(RoleEnum.BRManager);
        protected bool IsHOViewer => IsInRole(RoleEnum.HOViewer);
        protected bool IsHODealer => IsInRole(RoleEnum.HODealer);
        protected bool IsHOManager => IsInRole(RoleEnum.BRManager);
        #endregion
        #region Dialogbox Confirm
        

        protected static Dictionary<string, string> TargetStatusDictionary
        {
            get
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (WorkflowStatusData cacheData in CacheBase.Receive<WorkflowStatusData>())
                {
                    if (!string.IsNullOrWhiteSpace(cacheData.TargetStatus))
                    {
                        WorkflowStatusData targetItem = CacheBase.Receive<WorkflowStatusData>(cacheData.TargetStatus);
                        if (targetItem != null)
                        {
                            dictionary.Add(cacheData.Status,$"Bạn có muốn thực hiện thao tác <b>{targetItem.Title.Replace("BR", "").Replace("HO", "")}</b>");
                        }

                    }
                }

                return dictionary;
            }
        }
        protected static string TargetStatusJsonObject
        {
            get
            {

                List<string> list = new List<string>();
                foreach (var item in TargetStatusDictionary)
                {
                    list.Add($@"{{""key"": {item.Key},
                            ""val"": ""{item.Value}""}}");
                }

                return $"[{string.Join(",", list)}]";
            }
        }
        
        protected static double MaxAmountRequestChangePercent
        {
            get
            {
                if (double.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.MaxAmountChangePercent),
                    out double percent))
                {
                    return percent;
                }
                return -1;
            }
        }
        protected static double MaxAmountRequestChange
        {
            get
            {
                if (double.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.MaxAmountChange),
                    out double amount))
                {
                    return amount;
                }
                return -1;
            }
        }
        protected static double MaxAmountDealerApprovalEdit
        {
            get
            {
                if (double.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.DealerMaxEditLimit),
                    out double amount))
                {
                    return amount;
                }
                return -1;
            }
        }
        protected static double MaxAmountDealerApprovalCancel
        {
            get
            {
                if (double.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.DealerMaxCancelLimit),
                    out double amount))
                {
                    return amount;
                }
                return -1;
            }
        }

        protected static int DepositAmountPercent
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.DepositPercent), out int p))
                {
                    return p;
                }
                return -1;
            }
        }
        protected static int BeginWorkingMorning
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingBeginMorning), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static int EndWorkingMorning
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingEndMorning), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static int BeginWorkingAfternoon
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingBeginAfternoon), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static int EndWorkingAfternoon
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingEndAfternoon), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static int BeginWorkingSaturday
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingBeginSaturday), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static int EndWorkingSaturday
        {
            get
            {
                if (int.TryParse(FunctionBase.GetConfiguration(ConfigurationEnum.WorkingEndSaturday), out int time))
                {
                    return time;
                }
                return -1;
            }
        }
        protected static bool IsWorkTime
        {
            get
            {
                DateTime now = DateTime.Now;
                string time = $"{now.Hour.ToString().PadLeft(2, '0')}{now.Minute.ToString().PadLeft(2, '0')}";
                if (int.TryParse($"{time}", out int nowTime))
                {
                    if (now.DayOfWeek == DayOfWeek.Monday ||
                        now.DayOfWeek == DayOfWeek.Tuesday ||
                        now.DayOfWeek == DayOfWeek.Wednesday ||
                        now.DayOfWeek == DayOfWeek.Thursday ||
                        now.DayOfWeek == DayOfWeek.Friday)
                    {
                        if ((nowTime >= BeginWorkingMorning && nowTime <= EndWorkingMorning) ||
                            (nowTime >= BeginWorkingAfternoon && nowTime <= EndWorkingAfternoon))
                        {
                            return true;
                        }
                    }
                    if (now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (nowTime >= BeginWorkingSaturday && nowTime <= EndWorkingSaturday) return true;
                    }
                }
                
                
                return false;
            }
        }

        #endregion
        #region Import Excel
        protected static string GetExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;
            var part = fileName.Split('.');
            return part.Length == 0 ? string.Empty : part[part.Length - 1]?.ToLower();
        }
        protected static DataTable ToDataTable(ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();

            /*get Header*/
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            /*get Body*/
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
        protected static List<T> ImportExcel<T>(ExcelPackage package) where T : class
        {
            Type type = typeof(T);
            List<T> listResult = new List<T>();
            List<string> listFields = new List<string>();
            //
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            /*get Header*/
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                listFields.Add(firstRowCell?.Text?.Trim());
            }

            /*get Body*/
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                List<string> listData = new List<string>();
                foreach (var cell in row)
                {
                    listData.Add(cell.Text?.Trim());
                }
                // Read data
                T instance = (T)Activator.CreateInstance(type);
                for (int i = 0; i < listFields.Count; i++)
                {
                    string field = listFields[i];
                    if (string.IsNullOrWhiteSpace(field))
                    {
                        continue;
                    }
                    type.GetField(field)?.SetValue(instance, listData[i]);
                    type.GetProperty(field)?.SetValue(instance, listData[i]);
                }
                listResult.Add(instance);
            }
            return listResult;
        }
        #endregion
    }
}
