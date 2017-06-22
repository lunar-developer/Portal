using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Enum;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;

namespace DesktopModules.Modules.UserManagement
{
    public partial class UserRequestInquiry : UserManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        private void BindData()
        {
            BindBranchData(ddlBranch, UserInfo.UserID.ToString(), false, true, new List<string> { "-1" });

            // Request Type
            ListItem item = new ListItem("Tất cả", "0");
            ddlRequestTypeID.Items.Add(item);
            foreach (FieldInfo fieldInfo in typeof(RequestTypeEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RequestTypeEnum.GetDescription(value);
                ddlRequestTypeID.Items.Add(new ListItem(text, value));
            }

            // Request Status
            item = new ListItem("Tất cả", "-1");
            ddlRequestStatus.Items.Add(item);
            foreach (FieldInfo fieldInfo in typeof(RequestStatusEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RequestStatusEnum.GetDescription(value);
                ddlRequestStatus.Items.Add(new ListItem(text, value));
            }
        }

        protected string GetEditUrl()
        {
            return UserRequestUrl;
        }

        protected string GetEditUrl(string requestID)
        {
            return $"{UserRequestUrl}/{UserRequestTable.UserRequestID}/{requestID}";
        }

        protected void SearchRequest(object sender, EventArgs e)
        {
            hidUserName.Value = txtUserName.Text.Trim();
            hidBranchID.Value = ddlBranch.SelectedValue;
            hidRequestTypeID.Value = ddlRequestTypeID.SelectedValue;
            hidRequestStatus.Value = ddlRequestStatus.SelectedValue;
            gridData.Visible = true;
            BindGrid();
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
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
                { UserRequestTable.BranchID, new SQLParameterData(hidBranchID.Value, SqlDbType.Int) },
                { UserRequestTable.RequestTypeID, new SQLParameterData(hidRequestTypeID.Value, SqlDbType.Int) },
                { UserRequestTable.RequestStatus, new SQLParameterData(hidRequestStatus.Value, SqlDbType.Int) },
                { UserTable.UserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { "IsAdministrator", new SQLParameterData(IsAdministrator() ? 1 : 0, SqlDbType.Bit) }
            };
            return UserRequestBusiness.SearchRequest(dictionary);
        }
    }
}