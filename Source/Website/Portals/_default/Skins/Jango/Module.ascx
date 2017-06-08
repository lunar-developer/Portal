<%@ Control AutoEventWireup="false" CodeFile="Module.ascx.cs" Explicit="True" Language="C#" Inherits="Portals._default.Skins.Jango.Module" %>

<%@ Register Src="~/DesktopModules/DDRMenu/Menu.ascx" TagName="Menu" TagPrefix="dnn" %>
<%@ Register Src="~/Admin/Skins/Copyright.ascx" TagName="CopyRight" TagPrefix="dnn" %>
<%@ Register Src="~/admin/Skins/Toast.ascx" TagPrefix="dnn" TagName="Toast" %>

<%@ Import Namespace="Website.Library.Enum" %>


<%# Styles.Render(FolderEnum.BaseStyleBundle) %>
<link href="<%#BaseImageFolder %>favicon.ico"
      rel="shortcut icon" />
<%# Scripts.Render(FolderEnum.BaseScriptBundle) %>

<body class="c-layout-header-fixed c-layout-header-mobile-fixed">

    <header class="c-layout-header c-layout-header-4 c-layout-header-default-mobile"
            data-minimize-offset="80" <%#SetHeaderMargin() %>>
        <div class="c-navbar">
            <div class="container">
                <div class="clearfix c-navbar-wrapper">
                    <div class="c-brand c-pull-left">
                        <a class="c-logo"
                           href="<%#GetHomeUrl() %>">
                            <img alt="VietBank"
                                 class="c-desktop-logo"
                                 src="<%#BaseImageFolder %>logo.png" />
                            <img alt="VietBank"
                                 class="c-desktop-logo-inverse"
                                 src="<%#BaseImageFolder %>logo.png" />
                            <img alt="VietBank"
                                 class="c-mobile-logo"
                                 src="<%#BaseImageFolder %>logo.png" />
                        </a>
                        <button class="c-hor-nav-toggler"
                                data-target=".c-mega-menu"
                                type="button">
                            <span class="c-line"></span>
                            <span class="c-line"></span>
                            <span class="c-line"></span>
                        </button>
                    </div>

                    <nav class="c-fonts-bold c-fonts-uppercase c-mega-menu c-mega-menu-dark c-mega-menu-dark-mobile c-pull-right">
                        <ul class="c-theme-nav nav navbar-nav">
                            <dnn:Menu ID="Menu"
                                      MenuStyle="Menus/MainMenu"
                                      NodeSelector="*"
                                      runat="server">
                            </dnn:Menu>
                            <li>
                                <%#RenderAccountMenu() %>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
    </header>

    <div class="c-layout-page">

        <!-- Breadcrumbs -->
        <%#RenderBreadcrumbs() %>

        <!-- Modules -->
        <div id="ContentPane"
             runat="server">
        </div>
    </div>

    <footer class="c-layout-footer c-layout-footer-1"
            id="Contact">
        <div class="c-postfooter">
            <div class="container">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p class="c-copyright c-font-14">
                            <dnn:COPYRIGHT ID="dnnCopyright"
                                           runat="server" />
                        </p>
                    </div>
                    <div class="col-md-6 col-sm-6"></div>
                </div>
            </div>
        </div>
    </footer>

    <div class="c-layout-go2top">
        <i class="fa fa-angle-up"></i>
    </div>

    <dnn:Toast ID="Toast"
               runat="server" />
</body>