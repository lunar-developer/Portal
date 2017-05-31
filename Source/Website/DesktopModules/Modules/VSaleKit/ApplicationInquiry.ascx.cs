using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class ApplicationInquiry : VSaleKitModuleBase
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
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
            }
            finally
            {
                SetPermission();
            }
        }

        protected void Search(object sender, EventArgs e)
        {
            hidFromDate.Value = dpFromDate.SelectedDate?.ToString(PatternEnum.Date);
            hidCustomerInfo.Value = txtCustomerInfo.Text.Trim();
            hidBranchCode.Value = ddlBranch.SelectedValue;
            hidFilterFlag.Value = ddlFilterFlag.SelectedValue;
            BindGrid();
        }

        protected void EditUser(object sender, EventArgs e)
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

        protected void Export(object sender, EventArgs e)
        {
            ExportToExcel(GetData());
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
            BindBranchData(ddlBranch);
            ddlBranch.SelectedIndex = 0;

            dpFromDate.SelectedDate = DateTime.Now;
            dpFromDate.MinDate = DateTime.Now.AddDays(-30);
            dpFromDate.MaxDate = DateTime.Now;
        }

        private void BindGrid(int pageIndex = 0)
        {
            gridData.Visible = true;
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataSource = GetData();
            gridData.DataBind();
        }

        private DataTable GetData()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { BaseTable.UserName, UserInfo.Username },
                { BaseTable.FromDate, hidFromDate.Value },
                { "CustomerInfo", hidCustomerInfo.Value },
                { ApplicationFormTable.BranchCode, hidBranchCode.Value },
                { "FilterFlag", hidFilterFlag.Value }
            };
            return ApplicationFormBusiness.SearchApplication(dictionary);
        }

        private void SetPermission()
        {
            btnExport.Visible = UserInfo.IsSuperUser;
        }
    }
}