using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
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
                string profileUrl =
                    TabController.Instance.GetTabByName("User Information", PortalSettings.PortalId).FullUrl
                    + $"/UserID/{UserController.Instance.GetCurrentUserInfo().UserID}";
                string logoffUrl = $"{FunctionBase.GetConfiguration(ConfigEnum.SiteUrl)}logoff";

                return $@"
                    <li class=' c-menu-type-classic'>
                        <a class='c-link dropdown-toggle' href='javascript:;'>
                            <i class=""fa fa-user""></i>&nbsp;My Account<span class='c-arrow c-toggler'></span>
                        </a>
                        <ul class='c-menu-type-classic c-pull-left dropdown-menu'>
                            <li>
                                <a href='{profileUrl}'>
				                    Account Information
			                    </a>
                            </li>
                            <li>
                                <a href='{logoffUrl}'>
				                    Sign Out
			                    </a>
                            </li>
                        </ul>
                    </li>";
            }

            // Unknow User => Show login button
            string loginUrl = Globals.LoginURL(HttpUtility.UrlEncode(Globals.NavigateURL()), true);
            string clickEvent = string.Empty;
            if (PortalSettings.EnablePopUps && PortalSettings.RegisterTabId == Null.NullInteger)
            {
                clickEvent = $"return {UrlUtils.PopUpUrl(loginUrl, this, PortalSettings, true, false, 350, 600)}";
            }

            return $@"
                <li>
                    <a href=""{loginUrl}"" onclick=""{
                    clickEvent
                }"" class=""btn btn-no-focus btn-sm c-btn c-btn-border-1x c-btn-border-opacity-04 c-btn-dark c-btn-sbold c-btn-square c-btn-uppercase"">
                        <i class=""fa fa-user""></i>Sign In
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
            breadcrumbs.Append("<div class = 'container'>");
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
    }
}