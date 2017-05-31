using System.Web.UI.WebControls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Website.Library.Global;

namespace Modules.UserManagement.Global
{
    public class UserManagementModuleBase : DesktopModuleBase
    {
        private static string DetailUrl;
        private static readonly string LDAPEmail = FunctionBase.GetConfiguration(ConfigEnum.LDAPEmail);


        protected void BindBranchData(
            DropDownList dropDownList,
            bool isUseBranchCode = false,
            bool isUseOptionAll = false,
            params ListItem[] additionalItems)
        {
            BindBranchData(dropDownList, UserInfo.UserID.ToString(), isUseBranchCode, isUseOptionAll, additionalItems);
        }

        public static void BindBranchData(
            DropDownList dropDownList,
            string userID,
            bool isUseBranchCode = false,
            bool isUseOptionAll = false,
            params ListItem[] additionalItems)
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
                dropDownList.Items.Add(CreateListItem(branch, isUseBranchCode));
            }
        }

        public static void BindAllBranchData(
            DropDownList dropDownList,
            bool isUseBranchCode = false,
            bool isUseOptionAll = false,
            params ListItem[] additionalItems)
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


        


        
        protected string GetEditUrl()
        {
            return DetailUrl ??
                (DetailUrl = FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("UM_EditUrl")) ?? string.Empty);
        }


        protected bool IsAdministrator()
        {
            return IsInRole(RoleEnum.UserManagementAdministrator);
        }

        protected bool IsOwner(string userID)
        {
            return userID == UserInfo.UserID.ToString();
        }

        public static string FormatBranchCode(string value)
        {
            BranchData branch = CacheBase.Find<BranchData>(BranchTable.BranchCode, value.Trim());
            return branch == null ? value : $"{branch.BranchCode} - {branch.BranchName}";
        }

        public static string FormatBranchID(string value)
        {
            BranchData branch = CacheBase.Receive<BranchData>(value);
            return branch == null ? value : $"{branch.BranchCode} - {branch.BranchName}";
        }

        public static bool IsLDAPEmail(string value)
        {
            return value.ToLower().EndsWith(LDAPEmail);
        }
    }
}