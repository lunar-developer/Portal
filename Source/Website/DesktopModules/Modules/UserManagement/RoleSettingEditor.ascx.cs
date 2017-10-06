using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class RoleSettingEditor : DesktopModuleBase
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
            // Load Role Scope
            foreach (FieldInfo fieldInfo in typeof(RoleScopeEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RoleScopeEnum.GetScopeName(value);
                ddlRoleScope.Items.Add(new RadComboBoxItem(text, value));
            }

            // Load Role Groups
            ArrayList listRoleGroup = RoleController.GetRoleGroups(0);
            foreach (RoleGroupInfo roleGroup in listRoleGroup)
            {
                RadComboBoxItem item = new RadComboBoxItem(
                    GetDisplayText(roleGroup.RoleGroupName, roleGroup.Description),
                    roleGroup.RoleGroupID.ToString());
                ddlRoleGroup.Items.Add(item);
            }

            // Set Default Role Group
            string roleGroupID = Request[RoleExtensionTable.RoleGroupID];
            if (string.IsNullOrWhiteSpace(roleGroupID))
            {
                return;
            }
            ddlRoleGroup.SelectedValue = roleGroupID;
            ProcessOnSelectRoleGroup(null, null);

            // Set Default Role
            string roleID = Request[BaseTable.RoleID];
            if (string.IsNullOrWhiteSpace(roleID))
            {
                return;
            }
            ddlRole.SelectedValue = roleID;
            ProcessOnSelectRole(null, null);
        }

        protected void ProcessOnSelectRoleGroup(object sender, EventArgs e)
        {
            int roleGroupID;
            if (int.TryParse(ddlRoleGroup.SelectedValue, out roleGroupID) == false)
            {
                return;
            }

            ResetData();
            HideAllButton();
            ClearSelection(ddlRole);
            ddlRole.Items.Clear();

            RoleController controller = new RoleController();
            foreach (RoleInfo role in controller.GetRolesByGroup(0, roleGroupID))
            {
                RadComboBoxItem item = new RadComboBoxItem(
                    GetDisplayText(role.RoleName, role.Description),
                    role.RoleID.ToString());
                ddlRole.Items.Add(item);
            }
        }

        protected void ProcessOnSelectRole(object sender, EventArgs e)
        {
            ResetData();
            LoadData(ddlRole.SelectedValue);
        }

        private static string GetDisplayText(string name, string description)
        {
            return $"{name} - {description}";
        }

        private void ResetData()
        {
            txtRoleName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlRoleScope.SelectedIndex = 0;
            chkIsDisable.Checked = false;
            ListAllRole.Items.Clear();
            ListExcludeRole.Items.Clear();
        }

        private void LoadData(string roleID)
        {
            int id;
            if (int.TryParse(roleID, out id) == false)
            {
                return;
            }

            // Load Role & Setting
            RoleInfo roleInfo = RoleController.Instance.GetRoleById(0, id);
            if (roleInfo == null)
            {
                return;
            }
            RoleExtensionData roleData = CacheBase.Receive<RoleExtensionData>(roleID);
            List<string> listExcludeRoleID = new List<string>();
            txtRoleName.Text = roleInfo.RoleName;
            txtDescription.Text = roleInfo.Description;
            if (roleData != null)
            {
                ddlRoleScope.SelectedValue = roleData.RoleScope;
                chkIsDisable.Checked = FunctionBase.ConvertToBool(roleData.IsDisable);
                listExcludeRoleID = roleData.ListExcludeRoleID.Split(';').ToList();
            }

            // Bind List Exclude Roles
            RoleController controller = new RoleController();
            List<RoleInfo> listRoles = controller.GetRoles(0)
                .Where(item => item.RoleGroupID != -1 && item.RoleID != roleInfo.RoleID)
                .OrderBy(item => item.RoleName).ToList();
            foreach (RoleInfo role in listRoles)
            {
                RadListBoxItem item = new RadListBoxItem(
                    GetDisplayText(role.RoleName, role.Description),
                    role.RoleID.ToString());

                if (listExcludeRoleID.Contains(role.RoleID.ToString()))
                {
                    ListExcludeRole.Items.Add(item);
                }
                else
                {
                    ListAllRole.Items.Add(item);
                }
            }
            SetPermission(roleData != null);
        }


        protected void Save(object sender, EventArgs e)
        {
            string roleID = ddlRole.SelectedValue;
            string listExcludeRole = string.Join(";", ListExcludeRole.Items.Select(item => item.Value).ToList());
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
                {
                    { BaseTable.RoleID, new SQLParameterData(roleID, SqlDbType.Int)},
                    { RoleExtensionTable.RoleScope, new SQLParameterData(ddlRoleScope.SelectedValue)},
                    { RoleExtensionTable.ListExcludeRoleID, new SQLParameterData(listExcludeRole)},
                    { BaseTable.IsDisable, new SQLParameterData(chkIsDisable.Checked, SqlDbType.Bit)},
                    { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int)},
                    { BaseTable.DateTimeModify, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)}
                };
            bool result = RoleExtensionBusiness.UpdateRoleInfo(dictionary);
            if (result)
            {
                ShowMessage("Cập nhật cấu hình phân Quyền thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                SetPermission(true);
            }
            else
            {
                ShowMessage("Cập nhật cấu hình phân Quyền không thành công.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Delete(object sender, EventArgs e)
        {
            string roleID = ddlRole.SelectedValue;
            bool result = RoleExtensionBusiness.DeleteRoleInfo(roleID);
            if (result)
            {
                ShowMessage($"Xóa cấu hình phân Quyền <b>{txtRoleName.Text}</b> thành công.",
                    ModuleMessage.ModuleMessageType.GreenSuccess);
                ResetData();
                HideAllButton();
                ClearSelection(ddlRole);
            }
            else
            {
                ShowMessage($"Xóa cấu hình phân Quyền <b>{txtRoleName.Text}</b> thất bại.",
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }


        private void SetPermission(bool isExistSetting)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = isExistSetting;
        }

        private void HideAllButton()
        {
            btnUpdate.Visible = btnDelete.Visible = false;
        }
    }
}
