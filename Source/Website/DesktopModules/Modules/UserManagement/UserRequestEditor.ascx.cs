using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class UserRequestEditor : UserManagementModuleBase
    {
        private List<int> ListUserRoleID = new List<int>();
        private List<string> ListRequestRoleID = new List<string>();


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
            // Request Type
            foreach (FieldInfo fieldInfo in typeof(RequestTypeEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty;
                string text = RequestTypeEnum.GetDescription(value);
                ddlRequestType.Items.Add(new RadComboBoxItem(text, value));
            }

            // Set Default State
            btnApprove.CommandArgument = RequestStatusEnum.Approve;
            btnDecline.CommandArgument = RequestStatusEnum.Decline;
            btnCancel.CommandArgument = RequestStatusEnum.Cancel;
            RegisterConfirmDialog(btnApprove, $"Bạn có chắc muốn <b>{btnApprove.Text}</b> yêu cầu?");
            RegisterConfirmDialog(btnDecline, $"Bạn có chắc muốn <b>{btnDecline.Text}</b> yêu cầu?");
            RegisterConfirmDialog(btnCancel, $"Bạn có chắc muốn <b>{btnCancel.Text}</b> yêu cầu?");

            // Info
            string requestID = Request[UserRequestTable.UserRequestID];
            if (string.IsNullOrWhiteSpace(requestID))
            {
                CreateNewRequest();
                SetPermission();
            }
            else
            {
                LoadRequestData(requestID);
            }
        }

        private void CreateNewRequest()
        {
            ResetData();
            LoadBranch();

            string userID = Request[UserRequestTable.UserID];
            if (IsAdministrator() == false)
            {
                ddlBranch.Enabled = false;
                ddlUser.Enabled = false;
                userID = UserInfo.UserID.ToString();
            }

            // Bind User Info
            if (string.IsNullOrWhiteSpace(userID))
            {
                return;
            }
            UserData userCache = CacheBase.Receive<UserData>(userID);
            if (userCache == null)
            {
                ShowMessage("Không tìm thấy thông tin của User.");
                return;
            }

            ddlBranch.SelectedValue = userCache.BranchID;
            LoadUsers();
            ddlUser.SelectedValue = userCache.UserID;
            lblBranchManager.Text = BranchBusiness.GetManagerName(ddlBranch.SelectedValue);
        }

        private void LoadBranch(string branchID = null)
        {
            ClearSelection(ddlBranch);
            ddlBranch.Items.Clear();

            if (string.IsNullOrWhiteSpace(branchID))
            {
                BindBranchData(ddlBranch);
            }
            else
            {
                BranchData branch = CacheBase.Receive<BranchData>(branchID);
                if (branch == null)
                {
                    return;
                }

                string text = BranchBusiness.GetBranchName(branchID);
                RadComboBoxItem item = new RadComboBoxItem(text, branchID);
                ddlBranch.Items.Add(item);
            }
        }

        private void LoadUsers(string userID = null)
        {
            ClearSelection(ddlUser);
            ddlUser.Items.Clear();
            
            if (string.IsNullOrWhiteSpace(userID))
            {
                if (ddlBranch.SelectedValue == string.Empty)
                {
                    return;
                }

                foreach (UserData userData in UserBusiness.GetUsersInBranch(int.Parse(ddlBranch.SelectedValue)))
                {
                    string text = $"{userData.DisplayName} ({userData.UserName})";
                    ddlUser.Items.Add(new RadComboBoxItem(text, userData.UserID));
                }
            }
            else
            {
                UserData userData = CacheBase.Receive<UserData>(userID);
                if (userData == null)
                {
                    return;
                }
                string text = $"{userData.DisplayName} ({userData.UserName})";
                ddlUser.Items.Add(new RadComboBoxItem(text, userData.UserID));
            }
        }

        protected void ProcessOnBranchChange(object sender, EventArgs e)
        {
            DivRoles.Visible = false;
            lblBranchManager.Text = BranchBusiness.GetManagerName(ddlBranch.SelectedValue);
            LoadUsers();

            if (ddlRequestType.SelectedValue == RequestTypeEnum.UpdateBranch)
            {
                LoadOtherBranch();
            }
        }

        protected void ProcessOnRequestChange(object sender, EventArgs e)
        {
            switch (ddlRequestType.SelectedValue)
            {
                case RequestTypeEnum.UpdateBranch:
                    DivRoles.Visible = false;
                    LoadOtherBranch();
                    break;

                default: // Update Permission
                    DivNewBranch.Visible = false;
                    if (ddlUser.SelectedValue != "")
                    {
                        LoadRoles();
                    }
                    break;
            }
        }

        protected void ProcessOnUserChange(object sender, EventArgs e)
        {
            if (ddlRequestType.SelectedValue == RequestTypeEnum.UpdateRoles)
            {
                LoadRoles();
            }
        }


        private void LoadOtherBranch()
        {
            DivNewBranch.Visible = true;
            ClearSelection(ddlNewBranch);
            ddlNewBranch.Items.Clear();
            BindAllBranchData(ddlNewBranch, false, new List<string> { ddlBranch.SelectedValue });
        }

        private void LoadRoles()
        {
            DivRoles.Visible = true;


            // QUERY BRANCH PERMISSION & ROLE TEMPLATE
            DataSet dsResult = BranchBusiness.GetBranchPermissionAndTemplate(ddlBranch.SelectedValue);
            DataTable dtRoleGroups = dsResult.Tables[0];
            DataTable dtRoleTemplates = dsResult.Tables[1];


            // BIND ROLE GROUPS & ROLES
            bool isProcess = hidRequestStatus.Value != RequestStatusEnum.New;
            bool isEnable = (IsAdministrator() || IsOwner(ddlUser.SelectedValue)) && isProcess == false;
            StringBuilder html = new StringBuilder();
            UserInfo user = UserController.Instance.GetUserById(PortalId, int.Parse(ddlUser.SelectedValue));
            List<string> listUserRoles = user.Roles.ToList();
            ListUserRoleID = new List<int>();

            // Role Groups base on Branch Permission
            foreach (DataRow row in dtRoleGroups.Rows)
            {
                string roleGroupID = row[RoleGroupTable.RoleGroupID].ToString();
                string roleGroupName = row[RoleGroupTable.RoleGroupName].ToString();
                html.Append(RenderRole(int.Parse(roleGroupID), roleGroupName, isEnable, listUserRoles, isProcess));
            }

            // Role Group System
            html.Append(RenderRole(-1, "System - Hệ Thống", false, listUserRoles, isProcess));

            // Role Group Other
            if (listUserRoles.Count > 0)
            {
                html.Append(RenderOtherRoles(-2, "Other - Khác", isEnable, listUserRoles, isProcess));
            }
            hidListUserRoles.Value = hidUserRequestID.Value == "0"
                ? string.Join(",", ListUserRoleID)
                : string.Join(",", ListRequestRoleID);
            DivRoleContent.InnerHtml = html.ToString();


            // BIND ROLE TEMPLATE
            if (isEnable)
            {
                DivRoleTemplate.Visible = true;
                LoadRoleTemplate(dtRoleTemplates);
            }
        }

        private string RenderRole(int roleGroupID, string roleGroupName, bool isEnable,
            ICollection<string> listUserRoles, bool isProcess)
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
            else if (isProcess)
            {
                checkBoxGroup = "Yêu Cầu";
            }

            int i = -1;
            RoleController roleController = new RoleController();
            StringBuilder content = new StringBuilder();
            foreach (RoleInfo role in roleController.GetRolesByGroup(0, roleGroupID))
            {
                i++;
                string elementID = $"Role{role.RoleID}";
                string cssRow = i % 2 == 0 ? "even-row" : "odd-row";
                bool isHasRole = listUserRoles.Contains(role.RoleName);
                string grantImage = isHasRole
                    ? $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />"
                    : string.Empty;
                if (isHasRole)
                {
                    listUserRoles.Remove(role.RoleName);
                    ListUserRoleID.Add(role.RoleID);
                }


                bool isChecked = hidUserRequestID.Value == "0"
                    ? isHasRole
                    : ListRequestRoleID.Contains(role.RoleID.ToString());
                string checkBoxControl = "&nbsp;";
                if (isEnable)
                {
                    checkBoxControl = $@"
                        <div class='c-checkbox text-center has-info'>
                            <input  name='Roles'
                                    type='checkbox'
                                    {(isChecked ? "checked='checked'" : string.Empty)}
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
                else if (isProcess)
                {
                    if (roleGroupID != -1 && ListRequestRoleID.Contains(role.RoleID.ToString()))
                    {
                        checkBoxControl = $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />";
                    }
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

        private string RenderOtherRoles(int roleGroupID, string roleGroupName, bool isEnable,
            IEnumerable<string> listUserRoles, bool isProcess)
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
            else if (isProcess)
            {
                checkBoxGroup = "Yêu Cầu";
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
                bool isChecked = hidUserRequestID.Value == "0" || ListRequestRoleID.Contains(role.RoleID.ToString());

                string checkBoxControl = "&nbsp;";
                if (isEnable)
                {
                    checkBoxControl = $@"
                        <div class='c-checkbox text-center has-info'>
                            <input  name='Roles'
                                    type='checkbox'
                                    {(isChecked ? "checked='checked'" : string.Empty)}
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
                else if (isProcess)
                {
                    if (ListRequestRoleID.Contains(role.RoleID.ToString()))
                    {
                        checkBoxControl = $"<img src='{FunctionBase.GetAbsoluteUrl("/images/grant.gif")}' />";
                    }
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

        private void LoadRoleTemplate(DataTable dtRoleTemplates)
        {
            ddlTemplate.Items.Clear();
            foreach (DataRow row in dtRoleTemplates.Rows)
            {
                string text = row[RoleTemplateTable.TemplateName].ToString();
                string value = row["Roles"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(text, value)
                {
                    Enabled = FunctionBase.ConvertToBool(row[BaseTable.IsDisable].ToString())
                };
                ddlTemplate.Items.Add(item);
            }
        }

        protected void UpdateRequest(object sender, EventArgs e)
        {
            // Validate
            string roles = Request["Roles"];
            string newBranchID = ddlNewBranch.SelectedValue;
            if (ddlRequestType.SelectedValue == RequestTypeEnum.UpdateRoles)
            {
                if (string.IsNullOrWhiteSpace(roles))
                {
                    ShowAlertDialog("Vui lòng chọn ít nhất một Quyền");
                    return;
                }
                if (roles == hidListUserRoles.Value)
                {
                    ShowAlertDialog("Thông tin phân quyền không có sự thay đổi.");
                    return;
                }
                newBranchID = "0";
            }

            // Process
            if (hidUserRequestID.Value == "0")
            {
                InsertRequest(roles, newBranchID);
            }
            else
            {
                UpdateRequest(roles, newBranchID);
            }
        }

        protected void ProcessRequest(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            string action = button.Text;
            string status = button.CommandArgument;
            ProcessRequest(action, status);
        }

        private void InsertRequest(string roles, string newBranchID)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { UserRequestTable.UserID, new SQLParameterData(ddlUser.SelectedValue, SqlDbType.Int) },
                { UserRequestTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { UserRequestTable.NewBranchID, new SQLParameterData(newBranchID, SqlDbType.Int) },
                {
                    UserRequestTable.RequestTypeID,
                    new SQLParameterData(ddlRequestType.SelectedValue, SqlDbType.TinyInt)
                },
                { UserRequestTable.RequestPermission, new SQLParameterData(roles) },
                {
                    UserRequestTable.RequestReason,
                    new SQLParameterData(txtRequestReason.Text.Trim(), SqlDbType.NVarChar)
                },
                { BaseTable.UserIDCreate, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.DateTimeCreate,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };

            int result = UserRequestBusiness.CreateRequest(parameterDictionary);
            if (result < 0)
            {
                result *= -1;
                string url = $"{UserRequestUrl}/{UserRequestTable.UserRequestID}/{result}";
                string message = $@"
                    <div id='message'>
                        Bạn đang có một yêu cầu đang chờ xử lý. 
                        Vui lòng chờ hoặc huỷ yêu cầu cũ trước khi tạo một yêu cầu mới.
                        Click vào <b><a class='c-theme-color c-edit-link' href='{
                        url
                    }'>đây</a></b> để xem chi tiết yêu cầu đang chờ xử lý.
                    </div>";
                ShowMessage(message);
            }
            else if (result == 0)
            {
                ShowMessage("Cập nhật yêu cầu thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {
                Session[UserRequestTable.UserRequestID] = result.ToString();
                string url = $"{UserRequestUrl}/{UserRequestTable.UserRequestID}/{result}";
                RegisterScript(GetWindowOpenScript(url, null, false));
            }
        }

        private void UpdateRequest(string roles, string newBranchID)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { UserRequestTable.UserRequestID, new SQLParameterData(hidUserRequestID.Value, SqlDbType.Int) },
                { UserRequestTable.NewBranchID, new SQLParameterData(newBranchID, SqlDbType.Int) },
                { UserRequestTable.RequestPermission, new SQLParameterData(roles) },
                {
                    UserRequestTable.RequestReason,
                    new SQLParameterData(txtRequestReason.Text.Trim(), SqlDbType.NVarChar)
                },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.DateTimeModify,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };

            int result = UserRequestBusiness.UpdateRequest(parameterDictionary);
            if (result < 0)
            {
                ShowMessage("Cập nhật yêu cầu thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
            else if (result == 0)
            {
                ShowMessage(
                    "Thông tin của yêu cầu này vừa bị thay đổi, vui lòng reload(F5) để xem trạng thái mới nhất.");
            }
            else
            {
                ShowMessage("Cập nhật yêu cầu thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadRequestData(hidUserRequestID.Value);
            }
        }

        private void ProcessRequest(string action, string newStatus)
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { UserRequestTable.UserRequestID, new SQLParameterData(hidUserRequestID.Value, SqlDbType.Int) },
                { "CurrentStatus", new SQLParameterData(hidRequestStatus.Value, SqlDbType.Int) },
                { UserRequestTable.RequestStatus, new SQLParameterData(newStatus, SqlDbType.Int) },
                {
                    UserRequestTable.Remark,
                    new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar)
                },
                { UserRequestTable.UserIDProcess, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    UserRequestTable.DateTimeProcess,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                }
            };

            int result = UserRequestBusiness.ProcessRequest(
                ddlUser.SelectedValue, ddlRequestType.SelectedValue, parameterDictionary);
            if (result < 0)
            {
                ShowMessage($"{action} yêu cầu thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
            else if (result == 0)
            {
                ShowMessage(
                    "Thông tin của yêu cầu này vừa bị thay đổi, vui lòng reload(F5) để xem trạng thái mới nhất.");
            }
            else
            {
                ShowMessage($"{action} yêu cầu thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadRequestData(hidUserRequestID.Value);
            }
        }

        private void LoadRequestData(string requestID)
        {
            if (Session[UserRequestTable.UserRequestID] + string.Empty == requestID)
            {
                Session.Remove(UserRequestTable.UserRequestID);
                ShowMessage("Cập nhật yêu cầu thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }

            ResetData();
            UserRequestData userRequest = UserRequestBusiness.LoadRequest(requestID);
            if (userRequest == null)
            {
                ShowMessage("Không tìm thấy thông tin bạn đang yêu cầu.");
                DivForm.Visible = false;
                return;
            }
            if (userRequest.UserID != UserInfo.UserID.ToString()
                && UserBusiness.IsUserOfBranch(UserInfo.UserID.ToString(), int.Parse(userRequest.BranchID)) == false)
            {
                ShowMessage("Bạn không có quyền xem thông tin này.");
                DivForm.Visible = false;
                return;
            }

            SetData(userRequest);
            SetViewState();
            SetPermission();
        }

        private void ResetData()
        {
            ddlBranch.SelectedIndex = -1;
            lblBranchManager.Text = string.Empty;
            ddlRequestType.SelectedIndex = -1;
            lblRequestStatus.Text = string.Empty;
            ddlUser.SelectedIndex = -1;
            ddlNewBranch.SelectedIndex = -1;
            lblUserIDCreate.Text = FunctionBase.FormatUserID(UserInfo.UserID.ToString());
            lblDateTimeCreate.Text = string.Empty;
            txtRequestReason.Text = string.Empty;
            lblUserIDProcess.Text = lblDateTimeProcess.Text = string.Empty;
            txtRequestReason.Text = string.Empty;
            txtRemark.Text = string.Empty;

            hidUserRequestID.Value = "0";
            hidRequestStatus.Value = "0";

            DivRoles.Visible = DivNewBranch.Visible = DivProcess.Visible = false;
            DivRoleTemplate.Visible = false;
        }

        private void SetData(UserRequestData userRequest)
        {
            hidUserRequestID.Value = userRequest.UserRequestID;
            hidRequestStatus.Value = userRequest.RequestStatus;
            lblRequestStatus.Text = RequestStatusEnum.GetDescription(userRequest.RequestStatus);

            LoadBranch(userRequest.BranchID);
            ddlBranch.SelectedValue = userRequest.BranchID;
            lblBranchManager.Text = BranchBusiness.GetManagerName(ddlBranch.SelectedValue);
            ddlRequestType.SelectedValue = userRequest.RequestTypeID;

            LoadUsers(userRequest.UserID);
            ddlUser.SelectedValue = userRequest.UserID;
            ListRequestRoleID = userRequest.RequestPermission.Split(',').ToList();
            switch (ddlRequestType.SelectedValue)
            {
                case RequestTypeEnum.UpdateRoles:
                    LoadRoles();
                    break;

                default:
                    LoadOtherBranch();
                    break;
            }
            ddlNewBranch.SelectedValue = userRequest.NewBranchID;

            lblUserIDCreate.Text = FunctionBase.FormatUserID(userRequest.UserIDCreate);
            lblDateTimeCreate.Text = FunctionBase.FormatDate(userRequest.DateTimeCreate);
            txtRequestReason.Text = userRequest.RequestReason;
            lblUserIDProcess.Text = FunctionBase.FormatUserID(userRequest.UserIDProcess);
            lblDateTimeProcess.Text = FunctionBase.FormatDate(userRequest.DateTimeProcess);
            txtRemark.Text = userRequest.Remark;
        }

        private void SetViewState()
        {
            DivRoles.Visible = ddlRequestType.SelectedValue == RequestTypeEnum.UpdateRoles;
            DivNewBranch.Visible = ddlRequestType.SelectedValue == RequestTypeEnum.UpdateBranch;
            ddlNewBranch.Enabled = hidRequestStatus.Value == RequestStatusEnum.New;

            ddlBranch.Enabled = false;
            ddlRequestType.Enabled = false;
            ddlUser.Enabled = false;
            txtRequestReason.Enabled = false;

            DivProcess.Visible = true;
            if (hidRequestStatus.Value != RequestStatusEnum.New)
            {
                txtRemark.Enabled = false;
            }
        }

        private void SetPermission()
        {
            int requestID;
            bool isEditMode = int.TryParse(hidUserRequestID.Value, out requestID) && requestID > 0;
            bool isOwner = IsOwner(ddlUser.SelectedValue);
            bool isAdministrator = IsAdministrator();
            string requestStatus = hidRequestStatus.Value;

            btnSave.Visible = (isAdministrator || isOwner) && requestStatus == RequestStatusEnum.New;
            btnApprove.Visible = isEditMode && isAdministrator && requestStatus == RequestStatusEnum.New;
            btnDecline.Visible = isEditMode && isAdministrator && requestStatus == RequestStatusEnum.New;
            btnCancel.Visible = isEditMode && isOwner && requestStatus == RequestStatusEnum.New;
        }
    }
}