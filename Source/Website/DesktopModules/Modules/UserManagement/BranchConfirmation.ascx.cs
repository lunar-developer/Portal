using System;
using Modules.UserManagement.Business;
using Modules.UserManagement.Global;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
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
            ListItem item = new ListItem("Chưa chọn", string.Empty);
            item.Attributes.Add("disabled", "disabled");
            BindAllBranchData(ddlBranch, false, false, new List<string> { "-1" }, item);
        }

        protected void ProcessOnBranchChanged(object sender, EventArgs eventArgs)
        {
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
                { UserTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    UserTable.ModifyDateTime,
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
    }
}