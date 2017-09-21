using System;
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
            BindBranchData(ddlBranch);
        }

        protected void Search(object sender, EventArgs e)
        {
            hidBranchID.Value = ddlBranch.SelectedValue;
            gridData.Visible = true;
            BindGrid();
        }

        private void BindGrid()
        {
            gridData.DataSource = RoleTemplateBusiness.GetRoleTemplate(hidBranchID.Value);
            gridData.DataBind();
        }

        protected void ProcessOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = RoleTemplateBusiness.GetRoleTemplate(hidBranchID.Value);
        }

        protected string FormatState(string value)
        {
            return FunctionBase.FormatState(value, false);
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