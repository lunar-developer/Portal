using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace DesktopModules.Modules.UserManagement
{
    public partial class RoleTemplateEditor : UserManagementModuleBase
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
            ListItem item = new ListItem("Chưa chọn", "-2");
            item.Attributes.Add("disabled", "disabled");
            BindBranchData(ddlBranch, false, false, item);
            btnSave.Visible = IsAdministrator();
            btnDelete.Visible = false;
            RegisterConfirmDialog(btnDelete, "Bạn có chắc muốn xóa thông tin này?");

            string templateID = Request.QueryString[RoleTemplateTable.TemplateID];
            LoadData(templateID);
        }

        private void ResetData()
        {
            tbTemplateName.Text = string.Empty;
            tbRemark.Text = string.Empty;
            ddlBranch.SelectedIndex = 0;
            ddlIsDisable.SelectedIndex = 0;
            hidTemplateID.Value = "0";
        }

        private void LoadData(string templateID)
        {
            ResetData();
            if (string.IsNullOrWhiteSpace(templateID))
            {
                return;
            }

            DataSet dsResult = RoleTemplateBusiness.GetRoleTemplateDetail(templateID);
            if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
            {
                ShowMessage("Không tìm thấy thông tin bạn đang yêu cầu, vui lòng kiểm tra lại.");
                DivEditor.Visible = false;
                return;
            }
            string branchID = dsResult.Tables[0].Rows[0][BranchTable.BranchID].ToString();
            if (UserBusiness.IsUserOfBranch(UserInfo.UserID.ToString(), int.Parse(branchID)) == false)
            {
                ShowMessage("Bạn không có quyền xem thông tin này.");
                DivEditor.Visible = false;
                return;
            }

            ddlBranch.Enabled = false;
            btnDelete.Visible = true;
            hidTemplateID.Value = templateID;
            SetData(dsResult.Tables[0].Rows[0]);

            List<int> listRoleID = new List<int>();
            foreach (DataRow row in dsResult.Tables[1].Rows)
            {
                listRoleID.Add(int.Parse(row[BaseTable.RoleID].ToString()));
            }
            RenderRoleGroup(dsResult.Tables[2], listRoleID);
        }

        private void SetData(DataRow row)
        {
            ddlBranch.SelectedValue = row[BranchTable.BranchID].ToString();
            tbTemplateName.Text = row[RoleTemplateTable.TemplateName].ToString();
            tbRemark.Text = row[BaseTable.Remark].ToString();
            ddlIsDisable.SelectedValue = bool.Parse(row[BaseTable.IsDisable].ToString()) ? "1" : "0";
        }

        private void RenderRoleGroup(DataTable roleGroupTable, List<int> listRoleID)
        {
            StringBuilder html = new StringBuilder();
            foreach (DataRow row in roleGroupTable.Rows)
            {
                string roleGroupID = row[RoleGroupTable.RoleGroupID].ToString();
                string roleGroupName = row[RoleGroupTable.RoleGroupName].ToString();
                html.Append(RenderRole(roleGroupID, roleGroupName, listRoleID));
            }
            DivRoles.InnerHtml = html.ToString();
        }

        private string RenderRole(string roleGroupID, string roleGroupName, List<int> listRoleID)
        {
            RoleController roleController = new RoleController();
            StringBuilder html = new StringBuilder();
            html.Append("<div class='form-group'>");
            html.Append("<div class='col-sm-12'>");
            html.Append("<h2 class='dnnFormSectionHead'>");
            html.Append($"<a href='#'>{roleGroupName}</a>");
            html.Append("</h2>");
            html.Append("<fieldset>");
            html.Append("<table class='table c-margin-t-10'>");
            html.Append("<colgroup>");
            html.Append("<col width='10%' />");
            html.Append("<col width='10%' />");
            html.Append("<col width='25%' />");
            html.Append("<col width='55%' />");
            html.Append("</colgroup>");
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th class='text-center'>");
            html.Append("<div class='c-checkbox text-center has-info'>");
            html.Append(
                $"<input type='checkbox' id='RoleGroup{roleGroupID}' autocomplete='off' onclick='toggleGroup(this, {roleGroupID})' />");
            html.Append($"<label for='RoleGroup{roleGroupID}'>");
            html.Append("<span class='inc'></span>");
            html.Append("<span class='check'></span>");
            html.Append("<span class='box'></span>");
            html.Append("</label>");
            html.Append("</div>");
            html.Append("</th>");
            html.Append("<th>Hiện hữu</th>");
            html.Append("<th>Quyền</th>");
            html.Append("<th>Diễn Giải</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");

            int i = -1;
            foreach (RoleInfo role in roleController.GetRolesByGroup(0, int.Parse(roleGroupID)))
            {
                i++;
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                bool isHasRole = listRoleID.Contains(role.RoleID);
                string grantImage = isHasRole
                    ? $"<img src='{Request.ApplicationPath}/images/grant.gif' />".Replace(@"//", @"/")
                    : string.Empty;

                html.Append($"<tr class='{cssRow}'>");
                html.Append("<td class='text-center'>");
                html.Append("<div class='c-checkbox text-center has-info'>");
                html.Append(
                    $"<input name='Roles' type='checkbox' {(isHasRole ? "checked='checked'" : string.Empty)} value='{role.RoleID}' class='c-check' id='{elementID}' group='{roleGroupID}' autocomplete='off'>");
                html.Append($"<label for='{elementID}'>");
                html.Append("<span class='inc'></span>");
                html.Append("<span class='check'></span>");
                html.Append("<span class='box'></span>");
                html.Append("</label>");
                html.Append("</div>");
                html.Append("</td>");
                html.Append("<td>");
                html.Append(grantImage);
                html.Append("</td>");
                html.Append("<td>");
                html.Append($"<label for='{elementID}'>{role.RoleName}</label>");
                html.Append("</td>");
                html.Append("<td>");
                html.Append($"<label for='{elementID}'>{role.Description}</label>");
                html.Append("</td>");
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</fieldset>");
            html.Append("</div>");
            html.Append("</div>");
            return html.ToString();
        }

        protected void Update(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> parameterdiDictionary = new Dictionary<string, SQLParameterData>
            {
                { RoleTemplateTable.TemplateID, new SQLParameterData(hidTemplateID.Value, SqlDbType.Int) },
                {
                    RoleTemplateTable.TemplateName, new SQLParameterData(tbTemplateName.Text.Trim(), SqlDbType.NVarChar)
                },
                { BranchTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { BaseTable.Remark, new SQLParameterData(tbRemark.Text.Trim(), SqlDbType.NVarChar) },
                { BaseTable.IsDisable, new SQLParameterData(ddlIsDisable.SelectedValue == "1", SqlDbType.Bit) },
                { BaseTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
                { "ListRoleID", new SQLParameterData(Request["Roles"], SqlDbType.VarChar) }
            };
            int templateID = RoleTemplateBusiness.UpdateRoleTemplate(parameterdiDictionary);
            if (templateID > 0)
            {
                ShowMessage("Cập nhật thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadData(templateID.ToString());
            }
            else
            {
                ShowMessage("Cập nhật thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Delete(object sender, EventArgs e)
        {
            bool result = RoleTemplateBusiness.DeleteRoleTemplate(hidTemplateID.Value);
            if (result)
            {
                ShowMessage("Xóa thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                DivEditor.Visible = false;
            }
            else
            {
                ShowMessage("Xóa thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void ProcessOnChangeBranch(object sender, EventArgs e)
        {
            DataTable roleGroupTable = BranchBusiness.GetBranchPermission(ddlBranch.SelectedValue);
            RenderRoleGroup(roleGroupTable, new List<int>());
        }
    }
}