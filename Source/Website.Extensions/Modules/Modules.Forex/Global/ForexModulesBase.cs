using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Social.Notifications;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using OfficeOpenXml;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
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

        protected static string GetDisplayNameByID(string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                return string.Empty;
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
            ExchangeRateGridData exchangeGridData = CacheBase.Receive<ExchangeRateGridData>(currencyCode);
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
            if (flag.Equals(ExchangeRateStatusEnum.PriceUp)) return FunctionBase.FormatCurrency(rate).PadRight(50, ' ') + PriceUp;
            if (flag.Equals(ExchangeRateStatusEnum.PriceDown)) return FunctionBase.FormatCurrency(rate).PadRight(50, ' ') + PriceDown;
            return rate;
        }

        protected static string GetAskFigure(string bigfigure, string rate, string flag, string isDisable)
        {
            if (double.TryParse(bigfigure, out double big) == false)
            {
                return ExchangeRateFormat("0", flag, "1");
            }
            if (double.TryParse(rate, out double ask) == false)
            {
                return ExchangeRateFormat(bigfigure, flag, isDisable);
            }
            return ExchangeRateFormat($"{big + ask}", flag, isDisable);
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
        protected static void BindCurrency(RadComboBox dropDownList, string currencyCode = null, 
            bool isChange = true, bool isManagementPage = false)
        {
            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
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
        protected static void BindTransactionType(RadComboBox dropDownList, string transactionTypeID = null, 
            bool isChange = true, bool isManagementPage = false)
        {
            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
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
        protected static void BindCustomerType(RadComboBox dropDownList, string customerTypeID = null,
            bool isChange = true,bool isManagementPage = false)
        {

            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
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
        protected static void BindWorkflowStatus(RadComboBox dropDownList, string workflowStatusID = null, 
            bool isChange = true, bool isManagementPage = false)
        {

            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
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
            foreach (RequestTypeData cacheData in CacheBase.Receive<RequestTypeData>())
            {
                dropDownList.Items.Add(CreateItem(cacheData.Title, cacheData.ID, cacheData.IsDisable));
            }
        }
        protected static void BindReason(RadComboBox dropDownList,string customerTypeID, string reasonCode = null, 
            bool isChange = true, bool isManagementPage = false)
        {
            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
            if (!string.IsNullOrWhiteSpace(customerTypeID))
            {
                foreach (ReasonMappingCustomerTypeData mappingItem in CacheBase.Receive<ReasonMappingCustomerTypeData>())
                {
                    if (mappingItem.CustomerTypeID.Equals(customerTypeID))
                    {
                        ReasonData reason = CacheBase.Receive<ReasonData>(mappingItem.ReasonTypeID);
                        if (reason != null)
                        {
                            dropDownList.Items.Add(CreateItem(reason.Title, reason.ID, reason.IsDisable,
                                !string.IsNullOrWhiteSpace(reasonCode) && reason.ID.Equals(reasonCode)));
                        }
                        
                    }
                }

            }
            
            
            dropDownList.Enabled = isChange;
            if (!isChange)
            {
                dropDownList.TabIndex = -1;
            }
        }
        protected static void BindBranch(RadComboBox dropDownList, string branchID = null, 
            bool isChange = true, bool isManagementPage = false)
        {
            if (isManagementPage)
            {
                BindItems(dropDownList, SetOneItemSelectBox(SelectAllItem));
            }
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
        private static RadComboBoxItem DefaultComBoxControlItem => new RadComboBoxItem { Text = "Chưa chọn", Value = "" };
        private static RadComboBoxItem SelectAllItem => new RadComboBoxItem { Text = "Tất cả", Value = "" };

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
        protected static void SetTextControl(TextBox textBox, string value = null, bool isChange = true, bool isEnable = true, bool isSystem = false)
        {
            textBox.Text = value ?? string.Empty;
            textBox.ReadOnly = !isChange;
            textBox.Enabled = isEnable;
            string readOnlyCssClass = $"{(isChange == false || isEnable == false ? " exchange-label" : String.Empty)}";
            if (!string.IsNullOrWhiteSpace(readOnlyCssClass))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    readOnlyCssClass += "-nonevalue";
                }
                else
                {
                    if (isSystem)
                    {
                        readOnlyCssClass += "-system";
                    }
                }
            }
            textBox.CssClass = $"form-control c-theme{readOnlyCssClass}";
            if (!isChange)
            {
                textBox.TabIndex = -1;
            }
        }

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
            if (string.IsNullOrWhiteSpace(transactionDate)) return DateTime.Now.Date;
            DateTime tranDate;
            if (DateTime.TryParse(transactionDate, out tranDate))
            {
                return tranDate;
            }
            return DateTime.Now.Date;
        }


        #endregion

        protected void Finish(string transactionID, int currentStatusID,
            ModuleMessage.ModuleMessageType messageType = ModuleMessage.ModuleMessageType.GreenSuccess,
            string message = null, string currencyCode = null, string quantityAmount = null, 
            bool isRedirectPage = false,string reloadUrl = null)
        {
            string popupParam = Request.QueryString["popup"];
            bool isPopup = !string.IsNullOrWhiteSpace(popupParam) && popupParam.ToLower().Equals("true");
            if (!string.IsNullOrWhiteSpace(message))
            {
                Session[SessionEnum.IsNotificationMessage] = "1";
                Session[SessionEnum.MessageType] = messageType;
                Session[SessionEnum.Message] = message;
            }
            if (isRedirectPage)
            {
                Session[SessionEnum.IsRedirect] = "1";
            }
            if (isPopup)
            {
                RegisterScript(GetCloseScript());
            }
            else
            {
                if (currentStatusID == WorkflowStatusEnum.Open)
                {
                    Session[SessionEnum.IsNotificationMessage] = "1";
                    Session[SessionEnum.TransactionID] = transactionID;
                    Session[SessionEnum.QuantityAmount] = quantityAmount;
                    Session[SessionEnum.CurrencyCode] = currencyCode;
                }
                if (string.IsNullOrWhiteSpace(reloadUrl))
                {
                    if (IsHODealer || IsHOManager || IsHOAdmin)
                    {
                        reloadUrl = $"{BidManagementUrl}/{TransactionTable.ID}/{transactionID}";
                    }
                    else
                    {
                        reloadUrl = $"{TransactionManagementUrl}/{TransactionTable.ID}/{transactionID}";
                    }
                }
                string script = GetWindowOpenScript(reloadUrl, null, false);
                RegisterScript(script);
            }
        }
        #region Session

        protected bool IsRedirectAndShowNotification => IsRedirectPage && IsNotificationMessage &&
            Session[SessionEnum.TransactionID] != null && Session[SessionEnum.Message] != null;

        protected void ResetRedirectAndShowNotification()
        {
            RemoveNotificationMessage();
            ResetSessionTransactionID();
            ResetSessionMessage();
            RemoveRedirectPage();
        }
        protected void ResetSessionMessage()
        {
            if (Session[SessionEnum.MessageType] != null)
            {
                Session.Remove(SessionEnum.MessageType);
            }
            if (Session[SessionEnum.Message] != null)
            {
                Session.Remove(SessionEnum.Message);
            }
        }
        protected ModuleMessage.ModuleMessageType GetMessageType()
        {
            ModuleMessage.ModuleMessageType type = ModuleMessage.ModuleMessageType.GreenSuccess;
            if (Session[SessionEnum.MessageType] != null)
            {
                type =(ModuleMessage.ModuleMessageType) Session[SessionEnum.MessageType];
                Session.Remove(SessionEnum.MessageType);
            }
            return type;
        }
        protected void ResetSessionTransactionID()
        {
            if (Session[SessionEnum.TransactionID] != null)
            {
                Session.Remove(SessionEnum.TransactionID);
            }
        }
        protected string GetSessionTransactionID
        {
            get
            {
                if (Session[SessionEnum.TransactionID] != null)
                {
                    return Session[SessionEnum.TransactionID].ToString();
                }
                return string.Empty;
            }
        }
        protected bool IsRedirectPage => Session[SessionEnum.IsRedirect] != null &&
            Session[SessionEnum.IsRedirect].ToString() == "1";
        protected bool IsNotificationMessage => Session[SessionEnum.IsNotificationMessage] != null &&
            Session[SessionEnum.IsNotificationMessage].ToString() == "1";
        protected void RemoveRedirectPage()
        {
            if (Session[SessionEnum.IsRedirect] != null)
            {
                Session.Remove(SessionEnum.IsRedirect);
            }
        }
        protected void RemoveNotificationMessage()
        {
            if (Session[SessionEnum.IsNotificationMessage] != null)
            {
                Session.Remove(SessionEnum.IsNotificationMessage);
            }
        }

        protected bool IsSessionCallPopup => Session[SessionEnum.IsCallPopup] != null &&
            Session[SessionEnum.IsCallPopup].ToString() == "1";

        protected void SetSessionCallPopup()
        {
            Session[SessionEnum.IsCallPopup] = "1";
        }

        protected void ResetSessionCallPopup()
        {
            if (Session[SessionEnum.IsCallPopup] != null)
            {
                Session.Remove(SessionEnum.IsCallPopup);
            }
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
        private static List<string> GetRoleList(string roleNameList)
        {
            if (string.IsNullOrWhiteSpace(roleNameList)) return null;
            if (!roleNameList.Contains(";")) return new List<string> { roleNameList };
            string[] roleArr = roleNameList.Split(';');
            List<string> list = new List<string>();
            foreach (string roleName in roleArr)
            {
                list.Add(roleName);
            }
            return list;
        }
        protected bool CheckRole(string roleNameList)
        {
            List<string> list = GetRoleList(roleNameList);
            if (list == null || list.Count == 0) return false;
            foreach (string roleName in list)
            {
                if (IsInRole(roleName)) return true;
            }
            return false;
        }

        protected bool IsAcceptRole(int workflowStatusID)
        {
            foreach (ActionData item in CacheBase.Receive<ActionData>())
            {
                if (item.ActionCode.Equals(ActionCodeEnum.Submit) &&
                    item.WorkflowStatusID.Equals(workflowStatusID.ToString()))
                {
                    if (CheckRole(item.RoleName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected bool IsRejectRole(int workflowStatusID)
        {
            foreach (ActionData item in CacheBase.Receive<ActionData>())
            {
                if (item.ActionCode.Equals(ActionCodeEnum.Cancel) &&
                    item.WorkflowStatusID.Equals(workflowStatusID.ToString()))
                {
                    if (CheckRole(item.RoleName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected bool IsBRViewer => IsInRole(RoleEnum.BRViewer);
        protected bool IsBRMaker => IsInRole(RoleEnum.BRMaker);
        protected bool IsBRManager => IsInRole(RoleEnum.BRManager);
        protected bool IsHOViewer => IsInRole(RoleEnum.HOViewer);
        protected bool IsHODealer => IsInRole(RoleEnum.HODealer);
        protected bool IsHOManager => IsInRole(RoleEnum.HOManager);
        protected bool IsHOAdmin => IsInRole(RoleEnum.HOAdmin);
        #endregion
        #region Dialogbox Confirm
        protected static string TransactionManagementUrl=> 
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigurationEnum.TransactionManagementUrl));
        protected static string BidManagementUrl =>
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigurationEnum.BidManagementUrl));
        protected static string TransactionCreationUrl =>
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigurationEnum.TransactionCreationUrl));
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
        #region SendNotification
        private static void SendNotificationToRole(string subject, string body, UserInfo fromUser, List<string> roles)
        {
            Notification notification = new Notification
            {
                Subject = subject,
                Body = body,
                From = fromUser.Email,
                SenderUserID = fromUser.UserID,
                NotificationTypeID = 1
            };
            IList<RoleInfo> roleInfos = new List<RoleInfo>();
            foreach (string role in roles)
            {
                RoleInfo roleInfo = RoleController.Instance.GetRoleByName(PortalSettings.Current.PortalId, role);
                if (roleInfo != null)
                {
                    roleInfos.Add(roleInfo);
                }
            }

            if (roleInfos.Count > 0)
            {
                NotificationsController.Instance.SendNotification(
                    notification, PortalSettings.Current.PortalId, roleInfos, null);
            }
        }

        private void SendNotificationRoleInBranch(string subject, string body, string branchID, string roleName)
        {
            IList<UserInfo> userInfos = new List<UserInfo>();
            List<UserData> userInBranch = UserBusiness.GetUsersInBranch(branchID);
            foreach (UserData user in userInBranch)
            {
                if (int.TryParse(user.UserID, out int userID))
                {
                    UserInfo userInfo = UserController.Instance.GetUserById(PortalSettings.Current.PortalId, userID);
                    if (userInfo != null && userInfo.IsInRole(roleName))
                    {
                        userInfos.Add(userInfo);
                    }
                    
                }
                
            }
            if (userInfos.Count > 0)
            {
                Notification notification = new Notification
                {
                    Subject = subject,
                    Body = body,
                    From = UserInfo.Email,
                    SenderUserID = UserInfo.UserID,
                    NotificationTypeID = 1
                };
                NotificationsController.Instance.SendNotification(
                    notification, PortalSettings.Current.PortalId, null, userInfos);
            }
            
        }
        private void SendNotificationToUser(string subject, string body, string userID)
        {
            if (int.TryParse(userID, out int id))
            {
                UserInfo toUser = UserController.Instance.GetUserById(PortalSettings.Current.PortalId, id);
                if (toUser != null)
                {
                    FunctionBase.SendNotification(subject, body, UserInfo, toUser);
                }

            }

        }

        private static readonly string NotificationBody = @"
                <span class="""">
                    <a class=""btn btn-icon btn-icon-color-default"" href=""{0}"">
                        {1} <i class=""fa fa-external-link"" aria-hidden=""true""></i>
                    </a>{2} 
                </span>";

        private static string GetTargetStatus(string currentStatus)
        {
            foreach (WorkflowStatusData workflow in CacheBase.Receive<WorkflowStatusData>())
            {
                if (currentStatus.Equals(workflow.Status))
                {
                    return workflow.TargetStatus;
                }
            }
            return string.Empty;
        }

        private static List<string> GetTargetRoleListByTargetStatus(string currentStatus,string targetStaus, bool isHO)
        {
            int targetStausID = ParseWorkflowStatus(targetStaus);
            int currentStatusID = ParseWorkflowStatus(currentStatus);
            switch (targetStausID)
            {
                case WorkflowStatusEnum.Reject:
                    if (currentStatusID == WorkflowStatusEnum.BRRquestException)
                    {
                        return GetRoleList(RoleEnum.BRMaker);
                    }
                    else if (currentStatusID == WorkflowStatusEnum.HORequestEditException ||
                        currentStatusID == WorkflowStatusEnum.HORequestCancelException)
                    {
                        return GetRoleList(RoleEnum.HODealer);
                    }
                    else
                    {
                        return GetRoleList(isHO ? $"{RoleEnum.BRMaker};{RoleEnum.BRManager}" : RoleEnum.HODealer);
                    }
                case WorkflowStatusEnum.HOFinishTransaction:
                case WorkflowStatusEnum.HOFinishEdit:
                case WorkflowStatusEnum.HOFinishCancel:
                    return GetRoleList(RoleEnum.BRMaker); 
            }
            foreach (ActionData item in CacheBase.Receive<ActionData>())
            {
                if (item.WorkflowStatusID.Equals(targetStaus))
                {
                    return GetRoleList(item.RoleName);
                }
            }
            return null;
        }

        private static List<string> GetCurrentRoleListByCurrentStatus(string currentStatus)
        {
            foreach (ActionData item in CacheBase.Receive<ActionData>())
            {
                if (item.WorkflowStatusID.Equals(currentStatus))
                {
                    return GetRoleList(item.RoleName);
                }
            }
            return null;
        }
        private string GetNotificationSubject(string targetStatusID, string currentSubject)
        {
            
            if (string.IsNullOrWhiteSpace(currentSubject))
            {
                foreach (WorkflowStatusData item in CacheBase.Receive<WorkflowStatusData>())
                {
                    if (item.Status.Equals(targetStatusID))
                    {
                        string subject = item.Title?.Replace("HO","").Replace("BR","");
                        if (IsBRMaker || IsBRManager)
                        {
                            return $"{UserBranchName}- {subject}";
                        }
                        return subject;
                    }
                }
            }
            if (IsBRMaker || IsBRManager)
            {
                return $"{UserBranchName}- Gửi yêu cầu";
            }
            return "Phản hồi yêu cầu";
        }
        protected static string GetNotificationBodyByParam(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            string body = string.Empty;
            if (parameterDictionary.ContainsKey(TransactionTable.QuantityTransactionAmount) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.QuantityTransactionAmount]?.ParameterValue.ToString()))
            {
                body +=
                    $"Số lượng {parameterDictionary[TransactionTable.QuantityTransactionAmount]?.ParameterValue}";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.CapitalAmount) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.CapitalAmount]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Giá chào {parameterDictionary[TransactionTable.CapitalAmount]?.ParameterValue}";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.DealTime) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.DealTime]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Thời gian {parameterDictionary[TransactionTable.DealTime]?.ParameterValue} (giây)";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.DepositAmount) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.DepositAmount]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Số tiền ký quỹ {parameterDictionary[TransactionTable.DepositAmount]?.ParameterValue}";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.CustomerInvoiceAmount) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.CustomerInvoiceAmount]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Giá khách hàng {parameterDictionary[TransactionTable.CustomerInvoiceAmount]?.ParameterValue}";
            }
            if (parameterDictionary.ContainsKey("ActionTransaction") &&
                !string.IsNullOrWhiteSpace(parameterDictionary["ActionTransaction"]?.ParameterValue.ToString()))
            {
                body +=
                    $"| {parameterDictionary["ActionTransaction"]?.ParameterValue} ";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.BrokerageAmount) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.BrokerageAmount]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Giá môi giới {parameterDictionary[TransactionTable.BrokerageAmount]?.ParameterValue}";
            }
            if (parameterDictionary.ContainsKey(TransactionTable.Remark) &&
                !string.IsNullOrWhiteSpace(parameterDictionary[TransactionTable.Remark]?.ParameterValue.ToString()))
            {
                body +=
                    $"| Nội dung '{parameterDictionary[TransactionTable.Remark]?.ParameterValue}'";
            }
            return body;
        }

        protected void SendNotificationMessage(string subject, string body, string transactionID, string currentStatus,
            string branchID = null, string markerID = null, string dealerID = null, string targetStaus = null)
        {
            targetStaus = string.IsNullOrWhiteSpace(targetStaus) ? GetTargetStatus(currentStatus) : targetStaus;
            List<string> targerRoleList = GetTargetRoleListByTargetStatus(currentStatus, targetStaus, (IsHODealer || IsHOManager || IsHOAdmin));
            subject = GetNotificationSubject(targetStaus, subject);
            body = $"{string.Format(NotificationBody, $"{TransactionCreationUrl}/{TransactionTable.ID}/{transactionID}", subject, body)}";
            #region Send By Target Role
            if (targerRoleList != null && targerRoleList.Count > 0)
            {
                
                foreach (string item in targerRoleList)
                {

                    if (!targetStaus.Equals(WorkflowStatusEnum.HOReceiveRequest.ToString()) &&
                        !targetStaus.Equals(WorkflowStatusEnum.BRReceive.ToString()))
                    {
                        switch (item)
                        {
                            case RoleEnum.HOAdmin:
                            case RoleEnum.HOManager:
                                SendNotificationToRole(subject, body, UserInfo, new List<string> { item });
                                break;
                            case RoleEnum.BRManager:
                                SendNotificationRoleInBranch(subject, body, branchID, RoleEnum.BRManager);
                                break;
                            case RoleEnum.HODealer:
                                if (string.IsNullOrWhiteSpace(dealerID))
                                {
                                    SendNotificationToRole(subject, body, UserInfo, new List<string> { item });
                                }
                                else
                                {
                                    SendNotificationToUser(subject, body, dealerID);
                                }
                                break;
                            case RoleEnum.BRMaker:
                                if (string.IsNullOrWhiteSpace(markerID))
                                {
                                    SendNotificationRoleInBranch(subject, body, branchID, RoleEnum.BRMaker);
                                }
                                else
                                {
                                    SendNotificationToUser(subject, body, markerID);
                                }
                                break;

                        }
                    }

                }//end switch
                
            }
            #endregion
            #region Send Notification if proccess by other user
            List<string> currentRoleList = GetCurrentRoleListByCurrentStatus(currentStatus);
            if (currentRoleList != null && currentRoleList.Count > 0)
            {
                foreach (string currentRole in currentRoleList)
                {
                    if (currentRole.Equals(RoleEnum.BRMaker) &&
                        !string.IsNullOrWhiteSpace(markerID) &&
                        !markerID.Equals(UserInfo.UserID.ToString()))
                    {
                        SendNotificationToUser($"{subject} - Được xử lí bởi Maker: {UserInfo.DisplayName}", body, markerID);
                    }
                    else if (currentRole.Equals(RoleEnum.HODealer) &&
                        !string.IsNullOrWhiteSpace(dealerID) &&
                        !dealerID.Equals(UserInfo.UserID.ToString()))
                    {
                        SendNotificationToUser($"{subject} - Được xử lí bởi Dealer: {UserInfo.DisplayName}", body, dealerID);
                    }
                }
            }
            #endregion
        }

        #endregion
    }
}
