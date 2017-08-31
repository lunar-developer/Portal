using System;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class UserAssignment : ApplicationModuleBase
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
            BindAssignmentPhaseData(ddlPhase, GetEmptyItem());
        }
        protected void ProcessOnPhaseChange(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlPhase.SelectedValue))
            {
                gridData.Visible = false;
                btnRefresh.Visible = btnAdd.Visible = false;
                return;
            }

            BindGrid();
            btnRefresh.Visible = btnAdd.Visible = true;
            gridData.Visible = true;
            gridData.DataBind();
        }

        private void BindGrid()
        {
            gridData.DataSource = CacheBase.Filter<UserPhaseData>(UserPhaseTable.PhaseID, ddlPhase.SelectedValue);
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = CacheBase.Filter<UserPhaseData>(UserPhaseTable.PhaseID, ddlPhase.SelectedValue);
        }


        protected void Refresh(object sender, EventArgs e)
        {
            BindGrid();
            gridData.DataBind();
        }

        protected void AddUser(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlPhase.SelectedValue))
            {
                return;
            }

            string script = EditUrl("Edit", 1024, 768, true, true, "refresh",
                UserPhaseTable.PhaseID, ddlPhase.SelectedValue);
            RegisterScript(script);
        }

        protected void EditUser(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (string.IsNullOrWhiteSpace(ddlPhase.SelectedValue) || button == null)
            {
                return;
            }

            string script = EditUrl("Edit", 1024, 768, true, true, "refresh",
                UserPhaseTable.PhaseID, ddlPhase.SelectedValue,
                UserPhaseTable.UserID, button.CommandArgument);
            RegisterScript(script);
        }
    }
}