using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class UserDetail : UserManagementModuleBase
    {
        private List<int> ListUserRoles;


        #region PAGE EVENTS
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            // Initialize Page
            AutoWire();
            BindData();

            // Add or Edit User
            string userID = Request.QueryString[UserTable.UserID];
            if (string.IsNullOrWhiteSpace(userID))
            {
                CreateNewUser();
            }
            else
            {
                LoadUser(userID);
            }
        }
        #endregion

        #region GRID EVENTS
        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(null, e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid(null);
        }
        #endregion

        #region COMBOBOX EVENTS
        protected void ProcessOnBranchChanged(object sender, EventArgs e)
        {
            if (DivRoles.Visible == false)
            {
                return;
            }

            LoadRoles();
        }
        #endregion

        #region BUTTON EVENTS
        protected void SaveProfile(object sender, EventArgs e)
        {
            bool isInsertMode = int.Parse(hidUserID.Value) == 0;
            if (isInsertMode)
            {
                string email = txtUserName.Text.Trim().ToLower();
                if (FunctionBase.IsEmail(email) == false)
                {
                    ShowMessage("Thông tin <b>Email</b> không hợp lệ.");
                    txtUserName.Focus();
                    return;
                }
                if (email.EndsWith(LDAPEmail))
                {
                    ShowMessage(
                        @"Nếu bạn là <b>nhân viên của VietBank</b> vui lòng liên hệ phòng nhân sự để được tạo tài khoản.
                        Chức năng Thêm mới chỉ áp dụng cho <b>Cộng tác viên</b>.");
                    txtUserName.Focus();
                    return;
                }
            }

            // Process Insert|Update
            string message;
            Dictionary<string, SQLParameterData> dictionary = GetData();
            int userID = isInsertMode
                ? UserBusiness.CreateUser(dictionary, out message)
                : UserBusiness.UpdateProfile(dictionary, out message);

            if (userID <= 0)
            {
                ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
            }
            else if (isInsertMode)
            {
                Session[UserTable.Authorised] = "1";
                string url = $"{UserDetailUrl}/{UserTable.UserID}/{userID}";
                RegisterScript(GetWindowOpenScript(url, null, false));
            }
            else
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
        }

        protected void UpdateRole(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserID, new SQLParameterData(hidUserID.Value, SqlDbType.Int) },
                { "Roles", new SQLParameterData(Request.Params["Roles"], SqlDbType.VarChar) },
                { UserTable.Remark, new SQLParameterData(txtRoleRemark.Text.Trim(), SqlDbType.NVarChar) },
                { UserTable.ModifyUserID, new SQLParameterData(UserInfo.UserID.ToString(), SqlDbType.Int) },
                {
                    UserTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };

            if (UserBusiness.UpdateRole(dictionary, txtUserName.Text.Trim()))
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
            else
            {
                ShowMessage("Cập nhật thông tin Tài khoản thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void UpdatePassword(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();
            string message;
            if (newPassword != txtConfirmPassword.Text.Trim())
            {
                message = GetSharedResource("PasswordMismatch");
                ShowMessage(message);
                return;
            }

            // Process update password
            string oldPassword = txtOldPassword.Text.Trim();
            UserInfo user = UserController.GetUserById(PortalId, int.Parse(hidUserID.Value));
            PasswordUpdateStatus status;
            if (UserBusiness.UpdatePassword(user, oldPassword, newPassword, UserInfo.UserID, out status))
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
            else
            {
                message = GetSharedResource(status.ToString());
                ShowMessage(message);
            }
        }
        #endregion

        private void CreateNewUser()
        {
            if (IsAdministrator() == false)
            {
                ShowMessage("Bạn không có quyền thực hiện chức năng này.");
                DivUserInformation.Visible = false;
                return;
            }

            hidUserID.Value = "0";
            SetPermission();
            ShowMessage("<b>Nhắc nhở</b>: Chức năng <b>Thêm mới Tài khoản</b> chỉ áp dụng cho <b>Cộng tác viên</b>.");
        }

        private void LoadUser(string userID)
        {
            DataSet dsResult = UserBusiness.LoadUser(userID, UserInfo.UserID.ToString());
            if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
            {
                ShowMessage("Không tìm thấy thông tin Tài khoản hoặc Bạn không có quyền xem thông tin này.");
                DivUserInformation.Visible = false;
                return;
            }
            if (Session[UserTable.Authorised] != null && Session[UserTable.Authorised].ToString() == "1")
            {
                Session.Remove(UserTable.Authorised);
                ShowMessage("Lưu thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }

            ResetData();

            // Load User Information
            SetData(dsResult.Tables[0].Rows[0]);
            SetPermission();
            LoadRoles();
            LoadHistory(dsResult.Tables[1]);
        }

        private void SetPermission()
        {
            int userID;
            bool isEditMode = int.TryParse(hidUserID.Value, out userID) && userID > 0;
            bool isOwner = isEditMode && IsOwner(hidUserID.Value);
            bool isRoleEdit = IsAdministrator() || isOwner;
            bool isAccountNormal = hidIsAccountLDAP.Value == "0";

            txtUserName.Enabled = isEditMode == false;
            ddlBranch.Enabled = isEditMode == false || IsSuperAdministrator();
            DivProfileRemark.Visible = IsAdministrator() && isOwner == false;
            btnSaveProfile.Text = isEditMode ? "Cập Nhật" : "Đồng Ý";
            btnSaveProfile.Visible = isRoleEdit;

            DivTabRole.Visible = DivTabRoleContent.Visible = isEditMode;
            DivRoleTemplate.Visible = IsSuperAdministrator();
            DivRoleRemark.Visible = IsSuperAdministrator();
            btnUpdateRole.Visible = IsSuperAdministrator();

            DivTabPassword.Visible = DivTabPasswordContent.Visible = isEditMode && isAccountNormal;
            DivOldPassword.Visible = isOwner;
            btnUpdatePassword.Visible = isRoleEdit && isAccountNormal;

            DivTabHistory.Visible = DivTabHistoryContent.Visible = isEditMode;
        }

        private void LoadRoles()
        {
            // QUERY BRANCH PERMISSION & ROLE TEMPLATE
            DataSet dsResult = BranchBusiness.GetBranchPermissionAndTemplate(ddlBranch.SelectedValue);
            DataTable dtRoleGroups = dsResult.Tables[0];
            DataTable dtRoleTemplates = dsResult.Tables[1];


            // BIND ROLE GROUPS & ROLES
            bool isEnable = IsSuperAdministrator();
            StringBuilder html = new StringBuilder();
            UserInfo user = UserController.Instance.GetUserById(PortalId, int.Parse(hidUserID.Value));
            List<string> listUserRoles = user.Roles.ToList();
            ListUserRoles = new List<int>();

            // Role Groups base on Branch Permission
            foreach (DataRow row in dtRoleGroups.Rows)
            {
                string roleGroupID = row[RoleGroupTable.RoleGroupID].ToString();
                string roleGroupName = row[RoleGroupTable.RoleGroupName].ToString();
                html.Append(RenderRole(int.Parse(roleGroupID), roleGroupName, isEnable, listUserRoles));
            }

            // Role Group System
            html.Append(RenderRole(-1, "System - Hệ Thống", false, listUserRoles));

            // Role Group Other
            if (listUserRoles.Count > 0)
            {
                html.Append(RenderOtherRoles(-2, "Other - Khác", isEnable, listUserRoles));
            }
            hidListUserRoles.Value = string.Join(",", ListUserRoles);
            DivRoles.InnerHtml = html.ToString();


            // BIND ROLE TEMPLATE
            LoadRoleTemplate(dtRoleTemplates);
        }

        private void LoadRoleTemplate(DataTable dtRoleTemplates)
        {
            ddlTemplate.Items.Clear();
            foreach (DataRow row in dtRoleTemplates.Rows)
            {
                string text = row[RoleTemplateTable.TemplateName].ToString();
                string value = row["Roles"].ToString();
                ddlTemplate.Items.Add(new ListItem(text, value));
            }
        }

        private string RenderRole(int roleGroupID, string roleGroupName, bool isEnable,
            ICollection<string> listUserRoles)
        {
            string checkBoxGroup = "&nbsp;";
            if (isEnable)
            {
                checkBoxGroup = $@"
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
            }

            int i = -1;
            RoleController roleController = new RoleController();
            StringBuilder content = new StringBuilder();
            foreach (RoleInfo role in roleController.GetRolesByGroup(0, roleGroupID))
            {
                i++;
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                bool isHasRole = IsInRole(role.RoleName, int.Parse(hidUserID.Value));
                string grantImage = isHasRole
                    ? $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />"
                    : string.Empty;
                if (isHasRole)
                {
                    listUserRoles.Remove(role.RoleName);
                    ListUserRoles.Add(role.RoleID);
                }

                string checkBoxControl = "&nbsp;";
                if (isEnable)
                {
                    checkBoxControl = $@"
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
                }
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
            return string.Format(RoleHtml, roleGroupName, checkBoxGroup, content);
        }

        private string RenderOtherRoles(int roleGroupID, string roleGroupName, bool isEnable,
            IEnumerable<string> listUserRoles)
        {
            string checkBoxGroup = "&nbsp;";
            if (isEnable)
            {
                checkBoxGroup = $@"
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
            }


            int i = -1;
            RoleController roleController = new RoleController();
            StringBuilder content = new StringBuilder();
            foreach (string roleName in listUserRoles)
            {
                i++;
                RoleInfo role = roleController.GetRoleByName(PortalId, roleName);
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                string grantImage = $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />";

                string checkBoxControl = "&nbsp;";
                if (isEnable)
                {
                    checkBoxControl = $@"
                        <div class='c-checkbox text-center has-info'>
                            <input  name='Roles'
                                    type='checkbox'
                                    checked='checked'
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
                }
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
            return string.Format(RoleHtml, roleGroupName, checkBoxGroup, content);
        }

        private void LoadHistory(DataTable dtUserLog)
        {
            gridData.Visible = true;
            BindGrid(dtUserLog);
        }

        private void BindData()
        {
            BindBranchData(ddlBranch);
        }

        private void BindGrid(DataTable dtUserLog, int pageIndex = 0)
        {
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataSource = dtUserLog ?? GetLogData();
            gridData.DataBind();
        }

        private DataTable GetLogData()
        {
            return UserBusiness.GetUserLog(hidUserID.Value);
        }

        private void ResetData()
        {
            txtUserName.Text = string.Empty;
            txtUserID.Text = string.Empty;
            txtDisplayName.Text = string.Empty;
            ddlGender.SelectedIndex = -1;
            txtMobile.Text = string.Empty;
            txtPhoneExtension.Text = string.Empty;
            txtStaffID.Text = string.Empty;
            ddlTitle.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            txtLineManager.Text = string.Empty;
            txtAuthorised.Text = string.Empty;
            txtLastLoginDate.Text = string.Empty;

            hidUserID.Value = "0";
            hidIsAccountLDAP.Value = "0";
            hidListUserRoles.Value = string.Empty;

            txtRemark.Text = string.Empty;
            txtRoleRemark.Text = string.Empty;
            txtOldPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }

        private void SetData(DataRow data)
        {
            txtUserName.Text = data[UserTable.UserName].ToString().Trim();
            txtUserID.Text = data[UserTable.UserID].ToString();
            txtDisplayName.Text = data[UserTable.DisplayName].ToString().Trim();
            ddlGender.SelectedValue = data[UserTable.Gender].ToString();
            txtMobile.Text = data[UserTable.Mobile].ToString().Trim();
            txtPhoneExtension.Text = data[UserTable.PhoneExtension].ToString().Trim();
            txtStaffID.Text = data[UserTable.StaffID].ToString().Trim();
            ddlTitle.SelectedValue = data[UserTable.Title].ToString();
            ddlBranch.SelectedValue = data[UserTable.BranchID].ToString();
            txtLineManager.Text = data[UserTable.LineManager].ToString();
            txtAuthorised.Text = data[UserTable.Authorised].ToString();
            DateTime date = DateTime.Parse(data[UserTable.LastLoginDate].ToString());
            txtLastLoginDate.Text = date.ToString(PatternEnum.DateTimeDisplay);
            hidIsAccountLDAP.Value = bool.Parse(data[UserTable.IsAccountLDAP].ToString()) ? "1" : "0";
            hidUserID.Value = txtUserID.Text;
        }

        private Dictionary<string, SQLParameterData> GetData()
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserName, new SQLParameterData(txtUserName.Text.Trim(), SqlDbType.VarChar) },
                { UserTable.UserID, new SQLParameterData(hidUserID.Value, SqlDbType.Int) },
                { UserTable.DisplayName, new SQLParameterData(txtDisplayName.Text.Trim(), SqlDbType.NVarChar) },
                { UserTable.Gender, new SQLParameterData(ddlGender.SelectedValue, SqlDbType.VarChar) },
                { UserTable.Mobile, new SQLParameterData(txtMobile.Text.Trim(), SqlDbType.VarChar) },
                { UserTable.PhoneExtension, new SQLParameterData(txtPhoneExtension.Text.Trim(), SqlDbType.VarChar) },
                { UserTable.StaffID, new SQLParameterData(txtStaffID.Text.Trim(), SqlDbType.VarChar) },
                { UserTable.Title, new SQLParameterData(ddlTitle.SelectedValue, SqlDbType.NVarChar) },
                { UserTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { UserTable.Remark, new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) },
                { UserTable.ModifyUserID, new SQLParameterData(UserInfo.UserID.ToString(), SqlDbType.Int) },
                {
                    UserTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };
            return dictionary;
        }
    }
}