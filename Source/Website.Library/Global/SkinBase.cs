using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Permissions;
using DotNetNuke.UI.Skins;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public class SkinBase : Skin
    {
        /*
         * Properties
         */
        protected static readonly string BaseImageFolder = FunctionBase.GetAbsoluteUrl(FolderEnum.BaseImageFolder);
        private const int DNNAdminMenuHeight = 54;


        /*
         * Functions
         */
        protected string GetHomeUrl()
        {
            return Globals.NavigateURL(PortalSettings.HomeTabId);
        }

        protected string SetHeaderMargin(bool isNegative = false)
        {
            // Avoid DNN Admin Menu overlap
            return IsLoadDNNAdminMenu()
                ? $"style = 'margin-top: {(isNegative ? "-" : string.Empty)}{DNNAdminMenuHeight}px !important;'"
                : string.Empty;
        }

        protected string SetBodyMargin()
        {
            // Avoid DNN Admin Menu overlap
            return IsLoadDNNAdminMenu()
                ? $"style = 'margin-top: {DNNAdminMenuHeight + 100}px !important;'"
                : string.Empty;
        }

        private static bool IsLoadDNNAdminMenu()
        {
            return FunctionBase.IsInRole(RoleEnum.Administrator) || CanEditPage() || CanEditModule();
        }

        private static bool CanEditPage()
        {
            return TabPermissionController.HasTabPermission(TabController.CurrentPage.TabPermissions, PermissionEnum.Edit);
        }

        private static bool CanEditModule()
        {
            return TabController.CurrentPage.Modules.Cast<ModuleInfo>().Any(ModulePermissionController.CanManageModule);
        }
    }
}