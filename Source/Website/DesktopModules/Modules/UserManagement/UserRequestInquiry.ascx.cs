using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
            // Branch
            BindBranchData(ddlBranch, new RadComboBoxItem("Tất cả", "-2"));


            // Request Type
            RadComboBoxItem item = new RadComboBoxItem("Tất cả", "0");
            ddlRequestTypeID.Items.Add(item);
            foreach (FieldInfo fieldInfo in typeof(RequestTypeEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RequestTypeEnum.GetDescription(value);
                ddlRequestTypeID.Items.Add(new RadComboBoxItem(text, value));
            }


            // Request Status
            item = new RadComboBoxItem("Tất cả", "-1");
            ddlRequestStatus.Items.Add(item);
            foreach (FieldInfo fieldInfo in typeof(RequestStatusEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RequestStatusEnum.GetDescription(value);
                ddlRequestStatus.Items.Add(new RadComboBoxItem(text, value));
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