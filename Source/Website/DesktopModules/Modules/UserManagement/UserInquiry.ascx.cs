using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.Web.UI;
using Website.Library.DataTransfer;

namespace DesktopModules.Modules.UserManagement
{
    public partial class UserInquiry : UserManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    return;
                }

                AutoWire();
                BindData();
            }
            finally
            {
                SetPermission();
            }
        }

        protected void SearchUser(object sender, EventArgs e)
        {
            hidUserName.Value = txtUserName.Text.Trim();
            hidBranchID.Value = ddlBranch.SelectedValue;
            hidAuthorised.Value = ddlAuthorised.SelectedValue;
            gridData.Visible = true;
            BindGrid();
        }

        protected void AddUser(object sender, EventArgs e)
        {
            string script = GetWindowOpenScript(UserDetailUrl, new Dictionary<string, string>());
            RegisterScript(script);
        }

        protected string GetEditUrl(string userID)
        {
            return $"{UserDetailUrl}/{UserTable.UserID}/{userID}";
        }

        protected void ExportUser(object sender, EventArgs e)
        {
            ExportToExcel(GetData(), "Users");
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }

        private void BindData()
        {
            BindBranchData(ddlBranch, UserInfo.UserID.ToString(), false, true);
        }

        private void BindGrid(int pageIndex = 0)
        {
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataSource = GetData();
            gridData.DataBind();
        }

        private DataTable GetData()
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserName, new SQLParameterData(hidUserName.Value, SqlDbType.VarChar) },
                { UserTable.BranchID, new SQLParameterData(hidBranchID.Value, SqlDbType.Int) },
                { UserTable.Authorised, new SQLParameterData(hidAuthorised.Value, SqlDbType.Int) },
                { UserTable.UserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) }
            };
            return UserBusiness.SearchUser(dictionary);
        }

        private void SetPermission()
        {
            btnAdd.Visible = btnExport.Visible = IsAdministrator();
        }
    }
}