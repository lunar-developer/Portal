using System;
using System.Collections.Generic;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class RoleTemplateInquiry : UserManagementModuleBase
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
            List<string> listExclude = new List<string> { "-1" };
            BindBranchData(ddlBranch, listExclude);
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }

        protected void Search(object sender, EventArgs e)
        {
            hidBranchID.Value = ddlBranch.SelectedValue;
            gridData.Visible = true;
            BindGrid();
        }

        private void BindGrid(int pageIndex = 0)
        {
            gridData.DataSource = RoleTemplateBusiness.GetRoleTemplate(hidBranchID.Value);
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataBind();
        }

        protected string FormatState(string value)
        {
            return FunctionBase.ConvertToBool(value) ? "Có" : "Không";
        }

        protected string GetUrl()
        {
            return EditUrl("Edit");
        }

        protected string GetEditLink(string templateID, string templateName)
        {
            return $@"<a  href='{EditUrl(RoleTemplateTable.TemplateID, templateID, "Edit")}'
                          target='_blank'
                          class='c-edit-link c-theme-color'>
                          {templateName}
                      </a>";
        }
    }
}