using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Website.Library.Global;

namespace Modules.UserManagement.Global
{
    public class UserManagementModuleBase : DesktopModuleBase
    {
        public static readonly string UserDetailUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration(ConfigEnum.EditUrl));
        public static readonly string LDAPEmail = FunctionBase.GetConfiguration(ConfigEnum.LDAPEmail);

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
            BindAllBranchData(dropDownList, false, false);
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode, bool isUseOptionAll)
        {
            BindAllBranchData(dropDownList, false, false, new List<string>());
        }

        public static void BindAllBranchData(DropDownList dropDownList, bool isUseBranchCode, bool isUseOptionAll,
            List<string> listExclude)
        {
            BindAllBranchData(dropDownList, false, false, new List<string>(), null);
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
        

        protected bool IsAdministrator()
        {
            return IsInRole(RoleEnum.Administrator) || IsSuperAdministrator();
        }

        protected bool IsSuperAdministrator()
        {
            return UserInfo.IsSuperUser;
        }

        protected bool IsOwner(string userID)
        {
            return userID == UserInfo.UserID.ToString();
        }

        public static string FormatBranchCode(string branchCode)
        {
            return BranchBusiness.GetBranchNameByBranchCode(branchCode);
        }

        public static string FormatBranchID(string branchID)
        {
            return BranchBusiness.GetBranchName(branchID);
        }

        public static bool IsLDAPEmail(string value)
        {
            return value.ToLower().EndsWith(LDAPEmail);
        }


        protected static string RoleHtml = @"
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
    }
}