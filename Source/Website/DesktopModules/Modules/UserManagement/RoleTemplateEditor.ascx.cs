using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

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
            BindBranchData(ddlBranch);

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
            ClearSelection(ddlBranch);
            ddlIsDisable.SelectedIndex = 0;
            lblDateTimeModify.Text = string.Empty;
            lblUserIDModify.Text = string.Empty;
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
            btnDelete.Visible = IsAdministrator();
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
            lblDateTimeModify.Text = FunctionBase.FormatDate(row[BaseTable.DateTimeModify].ToString());
            lblUserIDModify.Text = FunctionBase.FormatUserID(row[BaseTable.UserIDModify].ToString());
        }

        private void RenderRoleGroup(DataTable roleGroupTable, ICollection<int> listRoleID)
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

        private static string RenderRole(string roleGroupID, string roleGroupName, ICollection<int> listRoleID)
        {
            string checkBoxGroup = $@"
                <div class='c-checkbox text-center has-info'>
                    <input  type='checkbox' 
                            id='RoleGroup{roleGroupID}'
                            autocomplete='off'
                            onclick='toggleGroup(this, {roleGroupID})' />
                    <label for='RoleGroup{roleGroupID}'>
                        <span class='inc'></span>
                        <span class='check'></span>
                        <span class='box'></span>
                    </label>
                </div>";


            int i = -1;
            RoleController roleController = new RoleController();
            StringBuilder content = new StringBuilder();
            foreach (RoleInfo role in roleController.GetRolesByGroup(0, int.Parse(roleGroupID)))
            {
                i++;
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                bool isHasRole = listRoleID.Contains(role.RoleID);
                string grantImage = isHasRole
                    ? $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />"
                    : string.Empty;

                string checkBoxControl = $@"
                    <div class='c-checkbox text-center has-info'>
                        <input  name='Roles'
                                type='checkbox'
                                {(isHasRole ? "checked='checked'" : string.Empty)}
                                value='{role.RoleID}'
                                class='c-check'
                                id='{elementID}'
                                group='{roleGroupID}'
                                autocomplete='off'>
                        <label for='{elementID}'>
                            <span class='inc'></span>
                            <span class='check'></span>
                            <span class='box'></span>
                        </label>
                    </div>";

                content.Append($@"
                    <tr class='{cssRow}'>
                        <td class='text-center'>
                            {checkBoxControl}
                        </td>
                        <td>
                            {grantImage}
                        </td>
                        <td>
                            <label for='{elementID}'>{role.RoleName}</label>
                        </td>
                        <td>
                            <label for='{elementID}'>{role.Description}</label>
                        </td>
                    </tr>");
            }
            return string.Format(RoleHtmlTemplate, roleGroupName, checkBoxGroup, content);
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
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.DateTimeModify,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
                { "ListRoleID", new SQLParameterData(Request["Roles"]) }
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
                string script = $"document.location.href = \"{PortalSettings.ActiveTab.FullUrl}\";";
                ShowAlertDialog("Xóa thông tin thành công.", null, false, script);
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
            if (roleGroupTable.Rows.Count == 0)
            {
                DivRoles.InnerHtml = "Không tìm thấy thông tin cấu hình phân quyền của chi nhánh.";
            }
            else
            {
                RenderRoleGroup(roleGroupTable, new List<int>());
            }
        }
    }
}