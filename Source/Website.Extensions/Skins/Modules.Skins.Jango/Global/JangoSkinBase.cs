using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Social.Notifications;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;
using BaseFolderEnum = Website.Library.Enum.FolderEnum;
using FolderEnum = Modules.Skins.Jango.Enum.FolderEnum;

namespace Modules.Skins.Jango.Global
{
    public class JangoSkinBase : SkinBase
    {
        /*
         * Properties
         */
        private static string ImageFolder;

        protected static string SkinImageFolder
            => ImageFolder ?? (ImageFolder = FunctionBase.GetAbsoluteUrl(FolderEnum.JangoImageFolder));

        protected static readonly string SkinScriptBundle = FolderEnum.JangoScriptBundle;
        protected static readonly string SkinStyleBundle = FolderEnum.JangoStyleBundle;
        private static readonly List<BundleData> Bundles = new List<BundleData>();

        public static List<BundleData> SkinBundles
        {
            get
            {
                List<BundleData> bundles = Bundles.ToList();
                Bundles.Clear();

                return bundles;
            }
        }


        /*
         * Constructor
         */

        static JangoSkinBase()
        {
            // Jango base style - will be merged with Base style
            BundleData jangoBaseStyle = new BundleData(BaseFolderEnum.BaseStyleBundle, false);
            jangoBaseStyle.Include(BaseFolderEnum.BaseStyleSkinFolder, new List<string>
            {
                "plugins.css",
                "components.css",
                "default.css",
                "dnn-custom.css",
                "site.css"
            });
            Bundles.Add(jangoBaseStyle);

            // Jango style
            BundleData jangoSkinStyle = new BundleData(FolderEnum.JangoStyleBundle, false);
            jangoSkinStyle.Include(FolderEnum.JangoStyleFolder, new List<string>
            {
                "animate.min.css",
                "layers.css",
                "navigation.css",
                "settings.css",
                "skin.css"
            });
            Bundles.Add(jangoSkinStyle);

            // Jango base script - will be merged with Base script
            BundleData jangoBaseScript = new BundleData(BaseFolderEnum.BaseScriptBundle);
            jangoBaseScript.Include(BaseFolderEnum.BaseScriptSkinFolder, new List<string>
            {
                "components.js",
                "app.js",
                "site.js"
            });
            Bundles.Add(jangoBaseScript);

            // Jango script
            BundleData jangoSkinScript = new BundleData(FolderEnum.JangoScriptBundle);
            jangoSkinScript.Include(FolderEnum.JangoScriptFolder, new List<string>
            {
                "wow.min.js",
                "reveal-animate.js",
                "jquery.themepunch.tools.min.js",
                "jquery.themepunch.revolution.min.js",
                "revolution.extension.slideanims.min.js",
                "revolution.extension.layeranimation.min.js",
                "revolution.extension.navigation.min.js",
                "revolution.extension.video.min.js",
                "skin.js"
            });
            Bundles.Add(jangoSkinScript);
        }


        /*
         * Functions
         */

        protected string RenderAccountMenu()
        {
            if (Request.IsAuthenticated)
            {
                int portalID = PortalSettings.PortalId;
                UserInfo userInfo = UserController.Instance.GetCurrentUserInfo();
                List<string> listUrl = new List<string>();
                int unreadNotifications = NotificationsController.Instance.CountNotifications(userInfo.UserID, portalID);
                string iconNotification = unreadNotifications > 0
                    ? @"<i class=""fa fa-exclamation icon-danger notification-icon""></i>"
                    : string.Empty;


                // INBOX URL
                string inboxUrl =
                    $"{FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("UM_InboxUrl"))}";
                listUrl.Add($@"
                    <li>
                        <a href=""{inboxUrl}"">
				            Hộp thư Cá Nhân
                            <span class=""badge c-bg-red-2 c-margin-l-5 c-margin-t--5"">{unreadNotifications}</span>
			            </a>
                    </li>");


                // PROFILE URL
                string profileUrl =
                    $"{FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("UM_EditUrl"))}/UserID/{userInfo.UserID}";
                listUrl.Add($@"
                    <li>
                        <a href=""{profileUrl}"">
				            Thông tin Tài khoản
			            </a>
                    </li>");

                
                // REQUEST URL
                string requestUrl = FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("UM_ManageRequestUrl"));
                listUrl.Add($@"
                    <li>
                        <a href=""{requestUrl}"">
				            Thông tin Phiếu yêu cầu
			            </a>
                    </li>");


                // CONTACT URL
                string ldapEmail = FunctionBase.GetConfiguration("UM_LDAPEmail");
                if (userInfo.Username.ToLower().EndsWith(ldapEmail))
                {
                    string contactInfoUrl = FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("EM_UpdateContactInfoUrl"));
                    listUrl.Add($@"
                        <li>
                            <a href=""{contactInfoUrl}"">
				                Thông tin Liên lạc
			                </a>
                        </li>");
                }


                // LOG OFF URL
                string logoffUrl = $"{FunctionBase.GetConfiguration(ConfigEnum.SiteUrl)}logoff";
                listUrl.Add($@"
                    <li>
                        <a id=""GlobalSignOutLink"" href=""{logoffUrl}"">
				            <i class=""fa fa-sign-out""></i> Thoát
			            </a>
                    </li>");


                return $@"
                    <li class=""c-menu-type-classic"">
                        <a class=""c-link dropdown-toggle"" href=""javascript:;"">
                            <i class=""fa fa-user""></i>
                            &nbsp;{userInfo.DisplayName}{iconNotification}
                            <span class=""c-arrow c-toggler""></span>
                        </a>
                        <ul class=""c-menu-type-classic c-pull-right dropdown-menu"">
                            {string.Join("", listUrl)}
                        </ul>
                    </li>";
            }

            // Unknow User => Show login button
            string loginUrl = Globals.LoginURL(HttpUtility.UrlEncode(Globals.NavigateURL()), true);
            string clickEvent = string.Empty;
            if (PortalSettings.EnablePopUps && PortalSettings.RegisterTabId == Null.NullInteger)
            {
                clickEvent = $@"return {UrlUtils.PopUpUrl(loginUrl, this, PortalSettings, true, false, 400, 600)}";
            }

            return $@"
                <li>
                    <a href=""{loginUrl}"" onclick=""{
                    clickEvent
                }"" class=""btn btn-no-focus btn-sm c-btn c-btn-border-1x c-btn-border-opacity-04 c-btn-dark c-btn-sbold c-btn-square c-btn-uppercase"">
                        <i class=""fa fa-user""></i>Đăng Nhập
                    </a>
                </li>";
        }

        protected string RenderBreadcrumbs()
        {
            // Get breadcrumbs title
            string title = PortalSettings.ActiveTab.Title;
            if (string.IsNullOrWhiteSpace(title))
            {
                title = PortalSettings.ActiveTab.LocalizedTabName;
            }

            StringBuilder breadcrumbs = new StringBuilder();
            breadcrumbs.Append("<div class = 'c-fonts-bold c-fonts-uppercase c-layout-breadcrumbs-1'>");
            breadcrumbs.Append("<div class = 'container-fluid'>");
            breadcrumbs.Append("<div class = 'c-page-title c-pull-left'>");
            breadcrumbs.Append($"<h3 class = 'c-font-sbold c-font-uppercase'>{title}</h3>");
            breadcrumbs.Append("</div>");
            breadcrumbs.Append("<ul class = 'c-page-breadcrumbs c-pull-right c-theme-nav'>");
            for (var i = 0; i < PortalSettings.ActiveTab.BreadCrumbs.Count; ++i)
            {
                // Only add separators if we're past the root level
                if (i > 0)
                {
                    breadcrumbs.Append("<li>/</li>");
                }

                // Grab the current tab
                TabInfo tabInfo = (TabInfo) PortalSettings.ActiveTab.BreadCrumbs[i];
                string tabUrl = tabInfo.FullUrl;
                string tabName = tabInfo.LocalizedTabName;

                // Determine if we should use the tab's title instead of tab name
                if (string.IsNullOrEmpty(tabInfo.Title) == false)
                {
                    tabName = tabInfo.Title;
                }
                breadcrumbs.Append("<li>");
                breadcrumbs.Append($"<a href = '{(tabInfo.DisableLink ? "javascript:;" : tabUrl)}'>{tabName}</a>");
                breadcrumbs.Append("</li>");
            }
            breadcrumbs.Append("</ul>");
            breadcrumbs.Append("</div>");
            breadcrumbs.Append("</div>");
            return breadcrumbs.ToString();
        }


        /*
         * On Load Events
         */
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindSearchMenu();
        }


        /*
         * Render Search Menu
         */
        protected void BindSearchMenu()
        {
            RadComboBox comboBox = FindControl("GlobalSearchMenu") as RadComboBox;
            if (comboBox == null)
            {
                return;
            }

            UserInfo userInfo = UserController.Instance.GetCurrentUserInfo();
            List<TabInfo> listTabs = userInfo == null || userInfo.UserID == -1
                ? GetPublicTabs()
                : GetUserTabs(userInfo);
            BindMenuData(comboBox, listTabs);
        }

        private void BindMenuData(RadComboBox comboBox, IEnumerable<TabInfo> listTabs)
        {
            foreach (TabInfo tabInfo in listTabs)
            {
                comboBox.Items.Add(CreateItem(tabInfo));
            }
        }

        private RadComboBoxItem CreateItem(TabInfo tabInfo)
        {
            RadComboBoxItem item = new RadComboBoxItem(tabInfo.IndentedTabName, tabInfo.FullUrl)
            {
                Enabled = tabInfo.DisableLink == false
            };
            if (tabInfo.TabID == PortalSettings.ActiveTab.TabID)
            {
                item.Selected = true;
            }
            return item;
        }

        private static bool IsInvalidTab(TabInfo tabInfo)
        {
            return 
                tabInfo.HasBeenPublished == false ||
                tabInfo.IsVisible == false ||
                tabInfo.IsSuperTab ||
                tabInfo.IsSystem ||
                tabInfo.IsDeleted ||
                string.IsNullOrWhiteSpace(tabInfo.FullUrl);
        }

        private List<TabInfo> GetPublicTabs()
        {
            List<TabInfo> listTabs;
            const string cacheKey = "Global_Tabs_Public";
            if (HttpContext.Current.Cache[cacheKey] == null)
            {
                listTabs = new List<TabInfo>();
                TabCollection tabCollection = TabController.Instance.GetTabsByPortal(PortalSettings.PortalId);
                foreach (TabInfo tabInfo in tabCollection.Values.OrderBy(item => item.TabPath))
                {
                    if (IsInvalidTab(tabInfo))
                    {
                        continue;
                    }
                    foreach (PermissionInfoBase tabPermissionItem in tabInfo.TabPermissions)
                    {
                        if (tabPermissionItem.PermissionKey.ToUpper().Equals(PermissionEnum.View)
                            && tabPermissionItem.RoleName.Equals("All Users"))
                        {
                            listTabs.Add(tabInfo);
                            break;
                        }
                    }
                }

                // Insert to Cache
                int.TryParse(FunctionBase.GetConfiguration("Site_GlobalMenu_CacheTimeout", "60"), out int minutes);
                HttpContext.Current.Cache.Insert(
                    cacheKey,
                    listTabs,
                    null,
                    DateTime.UtcNow.AddMinutes(minutes),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable,
                    null);
            }
            else
            {
                listTabs = HttpContext.Current.Cache[cacheKey] as List<TabInfo>;
            }
            
            return listTabs;
        }

        private List<TabInfo> GetUserTabs(UserInfo userInfo)
        {
            List<TabInfo> listTabs;
            string cacheKey = $"Global_Tabs_{userInfo.UserID}";
            TabCollection tabCollection = TabController.Instance.GetTabsByPortal(PortalSettings.PortalId);
            List<TabInfo> listPortalTabs = tabCollection.Values.OrderBy(item => item.TabPath).ToList();

            if (HttpContext.Current.Cache[cacheKey] == null)
            {
                #region Super User
                if (userInfo.IsSuperUser)
                {
                    listTabs = listPortalTabs;
                }
                #endregion

                #region Normal User
                else
                {
                    listTabs = new List<TabInfo>();
                    foreach (TabInfo tabInfo in listPortalTabs)
                    {
                        if (IsInvalidTab(tabInfo))
                        {
                            continue;
                        }

                        foreach (PermissionInfoBase tabPermissionItem in tabInfo.TabPermissions)
                        {
                            if (tabPermissionItem.PermissionKey.ToUpper().Equals(PermissionEnum.View) == false)
                            {
                                continue;
                            }

                            if (tabPermissionItem.RoleName.Equals("All Users")
                                || tabPermissionItem.UserID != -1 && tabPermissionItem.UserID == userInfo.UserID
                                || userInfo.Roles.Any(userRole => tabPermissionItem.RoleName.Equals(userRole)))
                            {
                                listTabs.Add(tabInfo);
                                break;
                            }
                        }
                    }
                }
                #endregion

                // Insert to Cache
                int.TryParse(FunctionBase.GetConfiguration("Site_GlobalUserMenu_CacheTimeout", "10"), out int minutes);
                TimeSpan timeout = new TimeSpan(0, 0, minutes, 0, 0);
                HttpContext.Current.Cache.Insert(
                    cacheKey,
                    listTabs,
                    null,
                    Cache.NoAbsoluteExpiration,  
                    timeout,
                    CacheItemPriority.NotRemovable,
                    null);
            }
            else
            {
                listTabs = HttpContext.Current.Cache[cacheKey] as List<TabInfo>;
            }
           
            return listTabs;
        }
    }
}