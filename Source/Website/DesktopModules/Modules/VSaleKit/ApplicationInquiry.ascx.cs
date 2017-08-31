using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using PermissionEnum = Modules.VSaleKit.Enum.PermissionEnum;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class ApplicationInquiry : VSaleKitModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            AutoWire();
            BindData();
        }

        private void BindData()
        {
            calFromDate.SelectedDate = DateTime.Now.AddDays(-7);
            calFromDate.MinDate = DateTime.Now.AddDays(-30);
            calFromDate.MaxDate = DateTime.Now;

            calToDate.SelectedDate = DateTime.Now;
            calToDate.MaxDate = DateTime.Now;

            BindBranchData(ddlBranch, new RadComboBoxItem("Tất cả", "-2"));
            ddlBranch.SelectedIndex = 0;

            btnAdd.Visible = IsHasPermission(PermissionEnum.Add);
        }

        protected void Search(object sender, EventArgs e)
        {
            hidFromDate.Value = calFromDate.SelectedDate?.ToString(PatternEnum.Date);
            hidToDate.Value = calToDate.SelectedDate?.ToString(PatternEnum.Date);
            hidCustomerInfo.Value = txtCustomerInfo.Text.Trim();
            hidBranchID.Value = ddlBranch.SelectedValue;
            hidFilterFlag.Value = ddlFilterFlag.SelectedValue;

            if (hidFilterFlag.Value == "0")
            {
                if (string.IsNullOrWhiteSpace(hidCustomerInfo.Value))
                {
                    if(string.IsNullOrWhiteSpace(hidFromDate.Value) || string.IsNullOrWhiteSpace(hidToDate.Value))
                    {
                        ShowAlertDialog("Vui lòng nhập thông tin khách hàng hoặc thời gian tìm kiếm");
                        return;
                    }
                }
            }
            BindGrid();
            gridData.Visible = true;
            gridData.LocalResourceFile = LocalResourceFile;
            gridData.DataBind();
        }

        private void BindGrid()
        {
             gridData.DataSource = GetData();
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = GetData();
        }

        protected void ViewApplication(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { ApplicationFormTable.UniqueID, button.CommandArgument }
            };
            string script = GetWindowOpenScript(ApplicationUrl, dictionary);
            RegisterScript(script);
        }

        private DataTable GetData()
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { BaseTable.UserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { BaseTable.FromDate, new SQLParameterData(hidFromDate.Value, SqlDbType.VarChar) },
                { BaseTable.ToDate, new SQLParameterData(hidToDate.Value, SqlDbType.VarChar) },
                { "CustomerInfo", new SQLParameterData(hidCustomerInfo.Value, SqlDbType.NVarChar) },
                { "BranchID", new SQLParameterData(hidBranchID.Value, SqlDbType.VarChar) },
                { "FilterFlag", new SQLParameterData(hidFilterFlag.Value, SqlDbType.Int) }
            };
            return ApplicationFormBusiness.SearchApplication(dictionary);
        }

        protected void AddApplication(object sender, EventArgs e)
        {
            string script = GetWindowOpenScript(ApplicationUrl, null);
            RegisterScript(script);
        }
    }
}