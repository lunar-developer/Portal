using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Telerik.Web.UI;
using Website.Library.Global;

namespace Modules.UserManagement.Global
{
    public class UserManagementModuleBase : DesktopModuleBase
    {
        public static readonly string UserDetailUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigEnum.EditUrl));
        public static readonly string UserRequestUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigEnum.EditRequestUrl));
        public static readonly string BranchConfirmationUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigEnum.BranchConfirmationUrl));
        public static readonly string ManageRequestUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigEnum.ManageRequestUrl));

        public static readonly string LDAPEmail = FunctionBase.GetConfiguration(ConfigEnum.LDAPEmail);


        #region Deprecated 
        protected void BindBranchData(DropDownList dropDownList)
        {
            BindBranchData(dropDownList, UserInfo.UserID.ToString());
        }

        public static void BindBranchData(DropDownList dropDownList, string userID)
        {
            BindBranchData(dropDownList, userID, false);
        }

        public static void BindBranchData(DropDownList dropDownList, string userID, bool isUseBranchCode)
        {
            BindBranchData(dropDownList, userID, isUseBranchCode, false);
        }

        public static void BindBranchData(DropDownList dropDownList, string userID, bool isUseBranchCode,
            bool isUseOptionAll)
        {
            BindBranchData(dropDownList, userID, isUseBranchCode, isUseOptionAll, new List<string>());
        }

        public static void BindBranchData(DropDownList dropDownList, string userID, bool isUseBranchCode,
            bool isUseOptionAll, List<string> listExclude)
        {
            BindBranchData(dropDownList, userID, isUseBranchCode, isUseOptionAll, listExclude, null);
        }

        public static void BindBranchData(DropDownList dropDownList, string userID, bool isUseBranchCode,
            bool isUseOptionAll, List<string> listExclude, params ListItem[] additionalItems)
        {
            if (isUseOptionAll)
            {
                dropDownList.Items.Add(CreateListAllItem());
            }
            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
            foreach (BranchData branch in UserBusiness.GetUserBranch(userID))
            {
                if (isUseBranchCode)
                {
                    if (listExclude.Contains(branch.BranchCode))
                    {
                        continue;
                    }
                }
                else
                {
                    if (listExclude.Contains(branch.BranchID))
                    {
                        continue;
                    }
                }

                dropDownList.Items.Add(CreateListItem(branch, isUseBranchCode));
            }
        }

        private static ListItem CreateListAllItem()
        {
            return new ListItem("Tất Cả", "-2");
        }

        private static ListItem CreateListItem(BranchData branch, bool isUseBranchCode)
        {
            string text = string.IsNullOrWhiteSpace(branch.BranchCode)
                ? branch.BranchName
                : $"{branch.BranchCode} - {branch.BranchName}";
            string value = isUseBranchCode ? branch.BranchCode : branch.BranchID;

            // Get Item Data
            ListItem item = new ListItem(text, value);
            if (FunctionBase.ConvertToBool(branch.IsDisable))
            {
                item.Attributes.Add("disabled", "disabled");
            }
            return item;
        }

        protected void BindBranchData(DropDownList dropDownList, List<string> listExclude)
        {
            BindBranchData(dropDownList, UserInfo.UserID.ToString(), false, false, listExclude);
        }

        protected void BindBranchData(DropDownList dropDownList, List<string> listExclude,
            params ListItem[] additionalItems)
        {
            BindBranchData(dropDownList, UserInfo.UserID.ToString(), listExclude, additionalItems);
        }

        public static void BindBranchData(DropDownList dropDownList, string userID, List<string> listExclude,
            params ListItem[] additionalItems)
        {
            BindBranchData(dropDownList, userID, false, false, listExclude, additionalItems);
        }

        public static void BindAllBranchData(DropDownList dropDownList)
        {
            BindAllBranchData(dropDownList, false);
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode)
        {
            BindAllBranchData(dropDownList, isUseBranchCode, false);
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode, bool isUseOptionAll)
        {
            BindAllBranchData(dropDownList, isUseBranchCode, isUseOptionAll, new List<string>());
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode, bool isUseOptionAll,
            List<string> listExclude)
        {
            BindAllBranchData(dropDownList, isUseBranchCode, isUseOptionAll, listExclude, null);
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode, bool isUseOptionAll,
            List<string> listExclude, params ListItem[] additionalItems)
        {
            if (isUseOptionAll)
            {
                dropDownList.Items.Add(CreateListAllItem());
            }
            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
            foreach (BranchData branch in CacheBase.Receive<BranchData>())
            {
                if (isUseBranchCode)
                {
                    if (listExclude.Contains(branch.BranchCode))
                    {
                        continue;
                    }
                }
                else
                {
                    if (listExclude.Contains(branch.BranchID))
                    {
                        continue;
                    }
                }

                dropDownList.Items.Add(CreateListItem(branch, isUseBranchCode));
            }
        }
        #endregion










        #region Bind Data

        protected static RadComboBoxItem CreateItem(string text, string value, string isDisable = "0")
        {
            RadComboBoxItem item = new RadComboBoxItem(text, value)
            {
                Enabled = FunctionBase.ConvertToBool(isDisable) == false
            };
            return item;
        }

        protected static void BindItems(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            dropDownList.ClearSelection();
            dropDownList.ClearCheckedItems();
            dropDownList.Items.Clear();

            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
        }


        public void BindBranchData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindBranchData(dropDownList, UserInfo.UserID.ToString(), false, additionalItems);
        }

        public static void BindBranchData
            (RadComboBox dropDownList,
            string userID,
            bool isUseBranchCode,
            params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (BranchData branch in UserBusiness.GetUserBranch(userID))
            {
                if (branch.BranchID == "-1")
                {
                    continue;
                }

                string text = $"{branch.BranchCode} - {branch.BranchName}";
                string value = isUseBranchCode ? branch.BranchCode : branch.BranchID;
                dropDownList.Items.Add(CreateItem(text, value, branch.IsDisable));
            }
        }


        public static void BindAllBranchData(RadComboBox dropDownList, bool isUseBranchCode = false, params RadComboBoxItem[] additionalItems)
        {
            BindAllBranchData(dropDownList, isUseBranchCode, null, additionalItems);
        }

        public static void BindAllBranchData(RadComboBox dropDownList, List<string> listExclude, params RadComboBoxItem[] additionalItems)
        {
            BindAllBranchData(dropDownList, false, listExclude, additionalItems);
        }

        public static void BindAllBranchData(
            RadComboBox dropDownList, bool isUseBranchCode, List<string> listExclude, params RadComboBoxItem[] additionalItems)
        {
            if (listExclude == null)
            {
                listExclude = new List<string>();
            }

            BindItems(dropDownList, additionalItems);
            foreach (BranchData branch in BranchBusiness.GetAllBranchInfo())
            {
                string value = isUseBranchCode ? branch.BranchCode : branch.BranchID;
                if (branch.BranchID == "-1" || listExclude.Contains(value))
                {
                    continue;
                }

                string text = $"{branch.BranchCode} - {branch.BranchName}";
                dropDownList.Items.Add(CreateItem(text, value, branch.IsDisable));
            }
        }

        #endregion


        public static List<string> GetUserPermission(int userID)
        {
            UserInfo userInfo = UserController.Instance.GetUserById(0, userID);
            if (userInfo == null || userInfo.UserID == -1)
            {
                return new List<string>();
            }
            IList<UserRoleInfo> listRoleInfo = RoleController.Instance.GetUserRoles(userInfo, true);
            return listRoleInfo.Select(item => item.RoleID.ToString()).ToList();
        }

        protected bool IsAdministrator()
        {
            return IsInRole(RoleEnum.Administrator);
        }

        protected bool IsAdministrator(string userID)
        {
            return IsInRole(RoleEnum.Administrator, int.Parse(userID));
        }

        protected bool IsSuperAdministrator()
        {
            return UserInfo.IsSuperUser;
        }

        protected bool IsOwner(string userID)
        {
            return userID == UserInfo.UserID.ToString();
        }

        protected static bool IsRoleInScope(string roleID, bool isHeadOffice)
        {
            RoleExtensionData roleData = CacheBase.Receive<RoleExtensionData>(roleID);
            if (roleData == null
                || FunctionBase.ConvertToBool(roleData.IsDisable))
            {
                return true;
            }

            switch (roleData.RoleScope)
            {
                case RoleScopeEnum.HeadOffice:
                    return isHeadOffice;

                case RoleScopeEnum.BranchOffice:
                    return isHeadOffice == false;

                default:
                    return true;
            }
        }

        protected static bool IsInvalidUserRoles(string[] listRoles, out string message)
        {
            message = string.Empty;
            foreach (string roleID in listRoles)
            {
                RoleExtensionData roleData = CacheBase.Receive<RoleExtensionData>(roleID);
                if (roleData == null
                    || FunctionBase.ConvertToBool(roleData.IsDisable))
                {
                    continue;
                }

                List<string> listExcludeRoleID = roleData.ListExcludeRoleID.Split(';').ToList();
                foreach (string remainRoleID in listRoles)
                {
                    if (roleID == remainRoleID
                        || listExcludeRoleID.Contains(remainRoleID) == false)
                    {
                        continue;
                    }

                    message = $@"
                        <p>
                            <b>{FunctionBase.GetRoleName(roleID)}</b> không được phép cấp cùng với:</br>
                            <b>{FunctionBase.GetRoleName(remainRoleID)}</b>.
                        </p>";
                    return true;
                }
            }

            return false;
        }

        public static string FormatBranchCode(string branchCode)
        {
            return BranchBusiness.GetBranchNameByBranchCode(branchCode);
        }

        public static string FormatBranchID(string branchID)
        {
            return BranchBusiness.GetBranchName(branchID);
        }

        public static string FormatAuthorise(string authorisedFlag)
        {
            switch (authorisedFlag)
            {
                case UserAuthoriseEnum.Disabled:
                    return "ĐÃ KHOÁ";

                case UserAuthoriseEnum.Enabled:
                    return "ĐANG HOẠT ĐỘNG";

                default:
                    return "ĐANG TẠM KHOÁ";
            }
        }

        public static bool IsLDAPEmail(string value)
        {
            return value.ToLower().EndsWith(LDAPEmail);
        }


        protected static string RoleHtmlTemplate = @"
            <div class='form-group'>
                <div class='col-sm-12'>
                    <h2 class='dnnFormSectionHead'>
                        <a href='#'>{0}</a>
                    </h2>
                    <fieldset>
                        <table class='table c-margin-t-10'>
                            <colgroup>
                                <col width='10%' />
                                <col width='10%' />
                                <col width='25%' />
                                <col width='55%' />
                            </colgroup>
                            <thead>
                                <tr>
                                    <th class='text-center'>
                                        {1}
                                    </th>
                                    <th>Hiện hữu</th>
                                    <th>Quyền</th>
                                    <th>Diễn Giải</th>
                                </tr>
                            </thead>
                            <tbody>
                                {2}
                            </tbody>
                        </table>
                    </fieldset>
                </div>
            </div>";

        protected static string RoleHistoryHtmlTemplate = @"
            <div class='form-group'>
                <div class='col-sm-12'>
                    <h2 class='dnnFormSectionHead'>
                        <a href='#'>{0}</a>
                    </h2>
                    <fieldset>
                        <table class='table c-margin-t-10'>
                            <colgroup>
                                <col width='10%' />
                                <col width='10%' />
                                <col width='10%' />
                                <col width='25%' />
                                <col width='45%' />
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>Trước đó</th>
                                    <th>Yêu cầu</th>
                                    <th>Hiện hữu</th>
                                    <th>Quyền</th>
                                    <th>Diễn Giải</th>
                                </tr>
                            </thead>
                            <tbody>
                                {1}
                            </tbody>
                        </table>
                    </fieldset>
                </div>
            </div>";
    }
}