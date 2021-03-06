﻿using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Global;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace DesktopModules.Modules.UserManagement
{
    public partial class BranchConfirmation : UserManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            LoadData();
        }

        private void LoadData()
        {
            lblUserName.Text = UserInfo.Username;
            lblDisplayName.Text = UserInfo.DisplayName;
            BindData();
        }

        private void BindData()
        {
            BindAllBranchData(ddlBranch);
        }

        protected void ProcessOnBranchChanged(object sender, EventArgs eventArgs)
        {
            ResetData();
            UserData userManager = BranchBusiness.GetUserManager(ddlBranch.SelectedValue);
            if (userManager == null)
            {
                return;
            }

            lblManagerName.Text = userManager.DisplayName;
            lblManagerEmail.Text = userManager.UserName;
            lblManagerMobile.Text = userManager.Mobile;
            lblManagerPhoneExtension.Text = userManager.PhoneExtension;
        }

        protected void Confirm(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { BranchTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.DateTimeModify,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };
            bool result = UserBusiness.ConfirmBranch(parameterDictionary);
            if (result == false)
            {
                ShowMessage("Cập nhật chi nhánh KHÔNG thành công.", ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {
                string url = TabController.Instance.GetTab(PortalSettings.HomeTabId, PortalId).FullUrl;
                Response.Redirect(url);
            }
        }

        private void ResetData()
        {
            lblManagerName.Text = string.Empty;
            lblManagerEmail.Text = string.Empty;
            lblManagerMobile.Text = string.Empty;
            lblManagerPhoneExtension.Text = string.Empty;
        }
    }
}