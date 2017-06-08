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
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;
using ServiceStack;
using Telerik.Web.UI;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class UserDetail : UserManagementModuleBase
    {
        #region PAGE EVENTS

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            AutoWire();
            BindData();

            // Show success message when receive post authorised request
            string authorised = Request.Params[UserTable.Authorised];
            if (string.IsNullOrWhiteSpace(authorised) == false)
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }

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
                if (email.EndsWith(FunctionBase.GetConfiguration("UM_LDAPEmail")))
                {
                    ShowMessage(@"Nếu bạn là <b>nhân viên của VietBank</b> vui lòng liên hệ phòng nhân sự để được tạo tài khoản.
                        Chức năng Thêm mới chỉ áp dụng cho <b>Cộng tác viên</b>.");
                    txtUserName.Focus();
                    return;
                }
            }

            // Process Insert|Update
            string message;
            Dictionary<string, string> dictionary = GetData();
            int userID = isInsertMode
                ? UserBusiness.CreateUser(dictionary, out message)
                : UserBusiness.UpdateProfile(dictionary, out message);

            if (userID == 0)
            {
                ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
            }
            else if(isInsertMode)
            {
                dictionary = new Dictionary<string, string>
                {
                    { UserTable.Authorised, "1" }
                };
                string url = $"{GetEditUrl()}?{UserTable.UserID}={userID}";
                RegisterScript(GetAutoPostScript(url, dictionary, false));
            }
            else
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
        }

        protected void UpdateRole(object sender, EventArgs e)
        {
            Dictionary<string,string> dictionary = new Dictionary<string, string>
            {
                { UserTable.UserID, hidUserID.Value },
                { "Roles", Request.Params["Roles"] },
                { UserTable.Remark, txtRoleRemark.Text.Trim() },
                { UserTable.ModifyUserID, UserInfo.UserID.ToString() },
                { UserTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) }
            };

            string message;
            if (UserBusiness.UpdateRole(dictionary, txtUserName.Text.Trim(), out message))
            {
                ShowMessage("Cập nhật thông tin Tài khoản thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadUser(hidUserID.Value);
            }
            else
            {
                ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void UpdatePassword(object sender, EventArgs e)
        {
            string password = txtNewPassword.Text.Trim();
            string message;
            if (password != txtConfirmPassword.Text.Trim())
            {
                message = GetSharedResource("PasswordMismatch");
                ShowMessage(message);
                return;
            }

            // Process update password
            UserInfo user = UserController.GetUserById(PortalId, int.Parse(hidUserID.Value));
            PasswordUpdateStatus status;
            if (UserBusiness.UpdatePassword(user, password, UserInfo.UserID, out status))
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

            // Reset all fields
            ResetData();

            // Load User Information
            DataRow user = dsResult.Tables[0].Rows[0];
            txtUserName.Text = user[UserTable.UserName].ToString();
            txtUserID.Text = user[UserTable.UserID].ToString();
            txtDisplayName.Text = user[UserTable.DisplayName].ToString();
            ddlGender.SelectedValue = user[UserTable.Gender].ToString();
            txtMobile.Text = user[UserTable.Mobile].ToString();
            txtPhoneExtension.Text = user[UserTable.PhoneExtension].ToString();
            txtStaffID.Text = user[UserTable.StaffID].ToString();
            string branchValue = CacheBase.Receive<UserData>(userID)?.BranchID;
            if (!string.IsNullOrWhiteSpace(branchValue) && !string.IsNullOrWhiteSpace(user[UserTable.Title].ToString()))
            {
              //  int positionCode = int.Parse(user[UserTable.Title].ToString());
                int branchID = int.Parse(branchValue);
             //   txtTitle.Text = new BranchProvider().GetUserPosition(branchID, positionCode);
                txtTitle.Text = user[UserTable.Title].ToString();
            }
            else
            {
                txtTitle.Text = @"N/A";
            }
            ddlTitle.SelectedValue = user[UserTable.Title].ToString();
            ddlBranch.SelectedValue = user[UserTable.BranchID].ToString();
            txtLineManager.Text = user[UserTable.LineManager].ToString();
            txtAuthorised.Text = user[UserTable.Authorised].ToString();
            DateTime date = DateTime.Parse(user[UserTable.LastLoginDate].ToString());
            txtLastLoginDate.Text = date.ToString(PatternEnum.DateTimeDisplay);
            hidIsAccountLDAP.Value = bool.Parse(user[UserTable.IsAccountLDAP].ToString()) ? "1" : "0";
            hidUserID.Value = userID;
            SetPermission();

            LoadRoles();
            LoadHistory(dsResult.Tables[1]);
        }

        private void SetPermission()
        {
            bool isEditMode = int.Parse(hidUserID.Value) > 0;
            bool isOwner = isEditMode && IsOwner(hidUserID.Value);
            bool isRoleEdit = IsAdministrator() || isOwner;
            bool isAccountNormal = hidIsAccountLDAP.Value == "0";

            txtUserName.Enabled = ddlBranch.Enabled = isEditMode == false;
            DivProfileRemark.Visible = DivRoleRemark.Visible = IsAdministrator();

            btnSaveProfile.Text = isEditMode ? "Cập Nhật" : "Đồng Ý";
            btnSaveProfile.Visible = isRoleEdit;

            DivTabRole.Visible = DivTabRoleContent.Visible = isEditMode;
            btnUpdateRole.Visible = IsAdministrator();

            DivTabPassword.Visible = DivTabPasswordContent.Visible = isEditMode && isAccountNormal;
            btnUpdatePassword.Visible = isRoleEdit && isAccountNormal;

            DivTabHistory.Visible = DivTabHistoryContent.Visible = isEditMode;
        }

        private List<string> GetRoleList()
        {
            if (string.IsNullOrWhiteSpace(ddlBranch.SelectedValue) || ddlBranch.SelectedValue.Equals("-1")) return null;
            if (string.IsNullOrWhiteSpace(ddlTitle.SelectedValue) || ddlTitle.SelectedValue.Equals("-1")) return null;
            int branchID = int.Parse(ddlBranch.SelectedValue);
            int positionCode = int.Parse(ddlTitle.SelectedValue);
            List<BranchPositionRoleData> roleList = new BranchProvider().GetListBranchPositionRole(branchID, positionCode);
            return roleList.Select(item => item.RoleID).ToList();
        }
        
        private void LoadRoles(List<string> list = null)
        {
            bool isEnable = IsAdministrator();
            StringBuilder html = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(GetRoleBranch()) && !GetRoleBranch().Equals("-1"))
            {
                int branchIDSelected = int.Parse(GetRoleBranch());
                BranchProvider branchProvider = new BranchProvider();
                List<BranchRoleGroupData> branchRoleGroupList = new List<BranchRoleGroupData>();
                    
                    //branchProvider.GetListBranchRoleGroup(branchIDSelected);
                foreach (var branchRoleGroup in branchRoleGroupList)
                {
                    int branchID = int.Parse(branchRoleGroup.BranchID);
                    int roleGroupID = int.Parse(branchRoleGroup.RoleGroupID);
                    string roleGroupName = RoleController.GetRoleGroup(PortalId, branchID).RoleGroupName;
                    bool isEdit = !string.IsNullOrWhiteSpace(branchRoleGroup.IsReadOnly) ?
                                                !bool.Parse(branchRoleGroup.IsReadOnly) : isEnable;
                    html.Append(RenderRole(roleGroupID, roleGroupName, isEdit, list));
                }
            }
            
            html.Append(RenderRole(-1, "System", false));
            DivRoles.InnerHtml = html.ToString();

            
        }

        private string RenderRole(int roleGroupID, string roleGroupName, bool isEnable, List<string> list = null)
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
            if (isEnable)
            {
                html.Append("<div class='c-checkbox text-center has-info'>");
                html.Append(
                    $"<input type='checkbox' id='RoleGroup{roleGroupID}' autocomplete='off' onclick='toggleGroup(this, {roleGroupID})' />");
                html.Append($"<label for='RoleGroup{roleGroupID}'>");
                html.Append("<span class='inc'></span>");
                html.Append("<span class='check'></span>");
                html.Append("<span class='box'></span>");
                html.Append("</label>");
                html.Append("</div>");
            }
            else
            {
                html.Append("&nbsp;");
            }
            html.Append("</th>");
            html.Append("<th>Hiện hữu</th>");
            html.Append("<th>Quyền</th>");
            html.Append("<th>Diễn Giải</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            list = (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text.Equals(@"N/A")) && (list == null || list.IsNullOrEmpty())
                ? GetRoleList()
                : list;
            int i = -1;
            foreach (RoleInfo role in roleController.GetRolesByGroup(0, roleGroupID))
            {
                i++;
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                bool isHasRole = list == null || list.IsNullOrEmpty() ?
                    IsInRole(role.RoleName, int.Parse(hidUserID.Value)) : list.Contains(role.RoleID.ToString());
                string grantImage = IsInRole(role.RoleName, int.Parse(hidUserID.Value))
                    ? $"<img src='{Request.ApplicationPath}/images/grant.gif' />".Replace(@"//",@"/")
                    : string.Empty;

                html.Append($"<tr class='{cssRow}'>");
                html.Append("<td class='text-center'>");
                if (isEnable)
                {
                    html.Append("<div class='c-checkbox text-center has-info'>");
                    html.Append(
                        $"<input name='Roles' type='checkbox' {(isHasRole ? "checked='checked'" : string.Empty)} value='{role.RoleID}' class='c-check' id='{elementID}' group='{roleGroupID}' autocomplete='off'>");
                    html.Append($"<label for='{elementID}'>");
                    html.Append("<span class='inc'></span>");
                    html.Append("<span class='check'></span>");
                    html.Append("<span class='box'></span>");
                    html.Append("</label>");
                    html.Append("</div>");
                }
                else
                {
                    html.Append("&nbsp;");
                }
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

        private void LoadHistory(DataTable dtUserLog)
        {
            BindGrid(dtUserLog);
        }

        private void BindData()
        {
            BindBranchData(ddlBranch);
            if (IsLogInBranch())
            {
                ddlBranch.Enabled = false;
            }
            LoadPosition(GetRoleBranch());
        }
        private bool IsLogInBranch()
        {
            string userBranchID = CacheBase.Receive<UserData>(UserInfo.UserID.ToString())?.BranchID;
            return ddlBranch.Items.Count == 1 && ddlBranch.SelectedValue.Equals(userBranchID);
        }

        private string GetRoleBranch()
        {
            string branchValue = IsLogInBranch()
                ? CacheBase.Receive<UserData>(UserInfo.UserID.ToString())?.BranchID
                : ddlBranch.SelectedValue;
            if (!string.IsNullOrWhiteSpace(branchValue))
            {
                return branchValue;
            }
            return string.Empty;
        }
        protected void LoadRoleDefault(object sender, EventArgs e)
        {
            LoadRoles(GetRoleList());
        }

        private void LoadPosition(string branchID)
        {
            if (!string.IsNullOrWhiteSpace(branchID) && !branchID.Equals("-1"))
            {
                //BindBranchPositionData(ddlTitle, branchID);
            }
        }
        protected void ChangeBranchValue(object sender, EventArgs e)
        {
            LoadPosition(ddlBranch.SelectedValue);
        }
        private void BindGrid(DataTable dtUserLog, int pageIndex = 0)
        {
            gridData.Visible = true;
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
            txtDisplayName.Text = string.Empty;
            ddlGender.SelectedIndex = -1;
            txtMobile.Text = txtPhoneExtension.Text = string.Empty;
            ddlTitle.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            txtRemark.Text = txtRoleRemark.Text = string.Empty;
            hidIsAccountLDAP.Value = "0";

            txtNewPassword.Text = txtConfirmPassword.Text = string.Empty;
        }

        private Dictionary<string, string> GetData()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { UserTable.UserID, hidUserID.Value },
                { UserTable.UserName, txtUserName.Text.Trim() },
                { UserTable.DisplayName, txtDisplayName.Text.Trim() },
                { UserTable.Gender, ddlGender.SelectedValue },
                { UserTable.Mobile, txtMobile.Text.Trim() },
                { UserTable.PhoneExtension, txtPhoneExtension.Text.Trim() },
                { UserTable.StaffID, txtStaffID.Text.Trim() },
                { UserTable.Title, ddlTitle.SelectedValue },
                { UserTable.BranchID, ddlBranch.SelectedValue },
                { UserTable.Remark, txtRemark.Text.Trim() },
                { UserTable.ModifyUserID, UserInfo.UserID.ToString() },
                { UserTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) }
            };
            return dictionary;
        }
    }
}