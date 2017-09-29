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
            btnExport.Visible = IsAdministrator();
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

        private void BindData()
        {
            BindBranchData(ddlBranch, new RadComboBoxItem("Tất cả", "-2"));
        }

        private void BindGrid()
        {
            gridData.DataSource = GetData();
            gridData.DataBind();
        }

        protected void ProcessOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = GetData();
        }

        private DataTable GetData()
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserName, new SQLParameterData(hidUserName.Value) },
                { UserTable.BranchID, new SQLParameterData(hidBranchID.Value, SqlDbType.Int) },
                { UserTable.Authorised, new SQLParameterData(hidAuthorised.Value, SqlDbType.Int) },
                { UserTable.UserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) }
            };
            return UserBusiness.SearchUser(dictionary);
        }

        private void SetPermission()
        {
            btnAdd.Visible = IsAdministrator();
            btnExport.Visible = false;
        }
    }
}