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
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Telerik.Web.UI;
using Website.Library.Database;
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
        protected void ProcessOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = GetLogData();
        }
        #endregion

        #region COMBOBOX EVENTS
        protected void ProcessOnBranchChanged(object sender, EventArgs e)
        {
            txtLineManager.Text = BranchBusiness.GetManagerName(ddlBranch.SelectedValue);
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
                return;
            }

            // Show result
            if (isInsertMode)
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
            string listRoles = Request["Roles"];
            string message;
            if (IsInvalidUserRoles(listRoles.Split(','), out message))
            {
                ShowAlertDialog(message);
                return;
            }

            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserID, new SQLParameterData(hidUserID.Value, SqlDbType.Int) },
                { "Roles", new SQLParameterData(listRoles) },
                { UserTable.Remark, new SQLParameterData(txtRoleRemark.Text.Trim(), SqlDbType.NVarChar) },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID.ToString(), SqlDbType.Int) },
                {
                    BaseTable.DateTimeModify,
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

        protected void UpdateAccountStatus(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }

            string action = button.CommandName;
            string authorised = button.CommandArgument;
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { BaseTable.UserID, new SQLParameterData(hidUserID.Value, SqlDbType.Int) },
                { BaseTable.UserName, new SQLParameterData(txtUserName.Text.Trim()) },
                { UserTable.Authorised, new SQLParameterData(authorised, SqlDbType.Int) },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { BaseTable.DateTimeModify, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) },
                { BaseTable.Remark, new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) }
            };

            bool result = UserBusiness.UpdateAccountStatus(parameterDictionary);
            if (result)
            {
                ShowMessage($"<b>{action}</b> tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
            else
            {
                ShowMessage($"<b>{action}</b> tài khoản thất bại.", ModuleMessage.ModuleMessageType.RedError);
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
            bool isAdministrator = IsAdministrator();
            bool isRoleEdit = isAdministrator || isOwner;
            bool isAccountNormal = hidIsAccountLDAP.Value == "0";
            string accountStatus = hidAuthorise.Value;
            bool isEnable = accountStatus == UserAuthoriseEnum.Enabled;

            txtUserName.Enabled = isEditMode == false;
            ddlBranch.Enabled = isEditMode == false || IsSuperAdministrator();
            DivProfileRemark.Visible = isAdministrator && isOwner == false;
            btnSaveProfile.Text = isEditMode ? "Cập Nhật" : "Đồng Ý";
            btnSaveProfile.Visible = isRoleEdit && isEnable;

            btnEnable.Visible = isEditMode && isAdministrator && accountStatus == UserAuthoriseEnum.Disabled;
            btnDisable.Visible = isEditMode && isAdministrator && accountStatus == UserAuthoriseEnum.Enabled;
            btnUnlock.Visible = isEditMode && isAdministrator && accountStatus == UserAuthoriseEnum.Locked;

            DivTabRole.Visible = DivTabRoleContent.Visible = isEditMode;
            DivRoleTemplate.Visible = isAdministrator;
            DivRoleRemark.Visible = isAdministrator;
            btnUpdateRole.Visible = isAdministrator && isEnable;
            btnRequest.Visible = isRoleEdit && isEnable;

            DivTabPassword.Visible = DivTabPasswordContent.Visible = isEditMode && isAccountNormal && isEnable;
            DivOldPassword.Visible = isOwner;
            btnUpdatePassword.Visible = isRoleEdit && isAccountNormal && isEnable;

            DivTabHistory.Visible = DivTabHistoryContent.Visible = isEditMode;
        }

        private void LoadRoles()
        {
            // QUERY BRANCH PERMISSION & ROLE TEMPLATE
            DataSet dsResult = BranchBusiness.GetBranchPermissionAndTemplate(ddlBranch.SelectedValue);
            DataTable dtRoleGroups = dsResult.Tables[0];
            DataTable dtRoleTemplates = dsResult.Tables[1];


            // BIND ROLE GROUPS & ROLES
            BranchData branchData = CacheBase.Receive<BranchData>(ddlBranch.SelectedValue);
            bool isHeadOffice = FunctionBase.ConvertToBool(branchData.IsHeadOffice);
            bool isEnable = IsAdministrator();
            StringBuilder html = new StringBuilder();
            UserInfo user = UserController.Instance.GetUserById(PortalId, int.Parse(hidUserID.Value));
            List<string> listUserRoles = user.Roles.ToList();
            ListUserRoles = new List<int>();

            // Role Groups base on Branch Permission
            foreach (DataRow row in dtRoleGroups.Rows)
            {
                string roleGroupID = row[RoleGroupTable.RoleGroupID].ToString();
                string roleGroupName = row[RoleGroupTable.RoleGroupName].ToString();
                html.Append(RenderRole(int.Parse(roleGroupID), roleGroupName, isHeadOffice, isEnable, listUserRoles));
            }

            // Role Group System
            html.Append(RenderRole(-1, "System - Hệ Thống", isHeadOffice, IsSuperAdministrator(), listUserRoles));

            // Role Group Other
            if (listUserRoles.Count > 0)
            {
                html.Append(RenderOtherRoles(-2, "Other - Khác", isHeadOffice, isEnable, listUserRoles));
            }
            hidListUserRoles.Value = string.Join(",", ListUserRoles);
            DivRoles.InnerHtml = html.ToString();


            // BIND ROLE TEMPLATE
            LoadRoleTemplate(dtRoleTemplates);
        }

        private void LoadRoleTemplate(DataTable dtRoleTemplates)
        {
            ClearSelection(ddlTemplate);
            ddlTemplate.Items.Clear();
            foreach (DataRow row in dtRoleTemplates.Rows)
            {
                string text = row[RoleTemplateTable.TemplateName].ToString();
                string value = row["Roles"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(text, value);
                ddlTemplate.Items.Add(item);
            }
        }

        private string RenderRole(
            int roleGroupID,
            string roleGroupName,
            bool isHeadOffice,
            bool isEnable,
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
                    ? FunctionBase.IconSuccess
                    : string.Empty;
                if (isHasRole)
                {
                    listUserRoles.Remove(role.RoleName);
                    ListUserRoles.Add(role.RoleID);
                }


                bool isAllowEdit = IsRoleInScope(role.RoleID.ToString(), isHeadOffice);
                string checkBoxControl = "&nbsp;";
                if (isEnable && isAllowEdit)
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
                else if (isHasRole && roleGroupID == -1) // Role Group System
                {
                    checkBoxControl = $@"
                        <input  type='hidden'
                                name='Roles'
                                value='{role.RoleID}'/>";
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
            return string.Format(RoleHtmlTemplate, roleGroupName, checkBoxGroup, content);
        }

        private string RenderOtherRoles(
            int roleGroupID,
            string roleGroupName,
            bool isHeadOffice,
            bool isEnable,
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
                string grantImage = FunctionBase.IconSuccess;

                // Check Edit Permission
                bool isAllowEdit = IsRoleInScope(role.RoleID.ToString(), isHeadOffice);
                string checkBoxControl = "&nbsp;";
                if (isEnable && isAllowEdit)
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
            return string.Format(RoleHtmlTemplate, roleGroupName, checkBoxGroup, content);
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

        private void BindGrid(DataTable dtUserLog)
        {
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
            hidAuthorise.Value = "1";
            hidListUserRoles.Value = string.Empty;

            txtRemark.Text = string.Empty;
            txtRoleRemark.Text = string.Empty;
            txtOldPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }

        private void SetData(DataRow data)
        {
            bool isAccountLDAP = FunctionBase.ConvertToBool(data[UserTable.IsAccountLDAP].ToString());
            hidIsAccountLDAP.Value = isAccountLDAP ? "1" : "0";
            hidUserID.Value = data[UserTable.UserID].ToString();
            hidAuthorise.Value = data[UserTable.Authorised].ToString();

            txtUserName.Text = data[UserTable.UserName].ToString().Trim();
            txtUserID.Text = hidUserID.Value;
            txtDisplayName.Text = data[UserTable.DisplayName].ToString().Trim();
            ddlGender.SelectedValue = data[UserTable.Gender].ToString();
            txtMobile.Text = data[UserTable.Mobile].ToString().Trim();
            txtPhoneExtension.Text = data[UserTable.PhoneExtension].ToString().Trim();
            txtStaffID.Text = data[UserTable.StaffID].ToString().Trim();
            ddlTitle.SelectedValue = data[UserTable.Title].ToString();
            ddlBranch.SelectedValue = data[UserTable.BranchID].ToString();
            txtLineManager.Text = BranchBusiness.GetManagerName(ddlBranch.SelectedValue);
            txtAuthorised.Text = FormatAuthorise(hidAuthorise.Value);
            DateTime date = DateTime.Parse(data[UserTable.LastLoginDate].ToString());
            txtLastLoginDate.Text = date.ToString(PatternEnum.DateTimeDisplay);
        }

        private Dictionary<string, SQLParameterData> GetData()
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { UserTable.UserName, new SQLParameterData(txtUserName.Text.Trim()) },
                { UserTable.UserID, new SQLParameterData(hidUserID.Value, SqlDbType.Int) },
                { UserTable.DisplayName, new SQLParameterData(txtDisplayName.Text.Trim(), SqlDbType.NVarChar) },
                { UserTable.Gender, new SQLParameterData(ddlGender.SelectedValue) },
                { UserTable.Mobile, new SQLParameterData(txtMobile.Text.Trim()) },
                { UserTable.PhoneExtension, new SQLParameterData(txtPhoneExtension.Text.Trim()) },
                { UserTable.StaffID, new SQLParameterData(txtStaffID.Text.Trim()) },
                { UserTable.Title, new SQLParameterData(ddlTitle.SelectedValue, SqlDbType.NVarChar) },
                { UserTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { UserTable.Remark, new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID.ToString(), SqlDbType.Int) },
                {
                    BaseTable.DateTimeModify,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };
            return dictionary;
        }

        protected void CreateNewRequest(object sender, EventArgs e)
        {
            string url = $"{UserRequestUrl}/{UserTable.UserID}/{hidUserID.Value}";
            string script = GetWindowOpenScript(url, null);
            RegisterScript(script);
        }

        protected string RenderLogDetail(string requestID)
        {
            int userRequestID;
            if (int.TryParse(requestID, out userRequestID) == false || userRequestID <= 0)
            {
                return string.Empty;
            }

            string url = $"{UserRequestUrl}/{UserRequestTable.UserRequestID}/{requestID}";
            return $@"
                <a href='{url}' target ='_blank'>
                    <i class='fa fa-eye'></i>
                </a>";
        }
    }
}