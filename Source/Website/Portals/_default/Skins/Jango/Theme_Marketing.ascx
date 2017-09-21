<%@ Control AutoEventWireup="false" CodeFile="Theme_Marketing.ascx.cs" Explicit="True" Language="C#" Inherits="Portals._default.Skins.Jango.ThemeMarketing" %>

<%@ Register Src="~/DesktopModules/DDRMenu/Menu.ascx" TagName="Menu" TagPrefix="dnn" %>
<%@ Register Src="~/Admin/Skins/Copyright.ascx" TagName="CopyRight" TagPrefix="dnn" %>
<%@ Register Src="~/admin/Skins/Toast.ascx" TagPrefix="dnn" TagName="Toast" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<%@ Import Namespace="Website.Library.Enum" %>


<%# Styles.Render(FolderEnum.BaseStyleBundle) %>
<%# Styles.Render(SkinStyleBundle) %>
<link href="<%#BaseImageFolder %>favicon.ico"
      rel="shortcut icon" />
<%# Scripts.Render(FolderEnum.BaseScriptBundle) %>
<%# Scripts.Render(SkinScriptBundle) %>

<body class="c-layout-header-fixed c-layout-header-fullscreen c-layout-header-mobile-fixed">

<header class="c-layout-header c-layout-header-4 c-layout-header-default-mobile"
        data-minimize-offset="80" <%#SetHeaderMargin() %>>
    <div class="c-navbar">
        <div class="container-fluid">
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
                    <button class="c-search-toggler" type="button">
                        <i class="fa fa-search"></i>
                    </button>
                </div>
                
                <div class="c-quick-search">
                    <control:AutoComplete 
                        CssClass="form-control quickSearch-AutoComlete"
                        ClientIDMode="Static"
                        ID ="GlobalSearchMenu"
                        EmptyMessage="Nhập để tìm kiếm"
                        OnClientSelectedIndexChanged="ProcessOnGlobalSearchMenuChange"
                        runat = "server">
                    </control:AutoComplete>
                    <span class="c-theme-link" id="GlobalButtonQuickSearch"><i class="fa fa-times" aria-hidden="true"></i></span>
                </div>

                <nav class="c-fonts-bold c-fonts-uppercase c-mega-menu c-mega-menu-dark c-mega-menu-dark-mobile c-pull-right">
                    <ul class="c-theme-nav nav navbar-nav">
                        <li class="c-search-toggler-wrapper">
                            <a href="javascript:;" class="c-btn-icon c-search-toggler dropdown-toggle"><i class="fa fa-search"></i></a>
                        </li>
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

    <section class=""
             dir="ltr"
             style="margin-top: 100px;">
        <div style="width: 100%;">
            <img alt=""
                 src="<%#SkinImageFolder %>bg-03.jpg"
                 style="width: 100%;">
        </div>
    </section>

    <div class="c-bg-white c-content-box c-size-md"
         id="Solutions">
        <div class="container-fluid">
            <div class="c-content-feature-10">

                <div class="c-content-title-1">
                    <h3 class="c-center c-font-bold c-font-uppercase">CHƯƠNG TRÌNH KHUYẾN MÃI MÙA HÈ RỰC NẮNG</h3>
                    <div class="c-line-center c-theme-bg"></div>
                </div>

                <ul class="c-list">
                    <li>
                        <div class="animate c-card c-font-right fadeInLeft wow">
                            <i class="c-float-right c-font-27 c-theme-font fa">
                                <img alt=""
                                     src="<%#SkinImageFolder %>iconRole.png"
                                     style="width: 30px;">
                            </i>
                            <div class="c-content c-content-right">
                                <h3 class="c-font-bold c-font-uppercase">
                                    <a class="c-theme-font"
                                       href="<%#DotNetNuke.Common.Globals.NavigateURL(136) %>">
                                        THỂ LỆ CHƯƠNG TRÌNH
                                    </a>
                                </h3>
                                <ul style="list-style-type: none; font-size: 17px; color: #5c6873;">
                                    <li style="text-align: right;font-weight: 600;">Chương trình ưu đãi hấp dẫn:</li>
                                    <li style="text-align: right;font-weight: 400;">100% nhận ngay quà tặng</li>
                                    <li style="text-align: right;font-weight: 400;">Cơ hội nhận chuyến du lịch Hawaii 6 ngày 5 đêm dành cho 4 người & Sổ tiết kiệm 100 triệu đồng.</li>
                                </ul>
                            </div>
                        </div>
                        <div class="c-bg-opacity-2 c-border-bottom"></div>
                    </li>
                    <div class="c-bg-opacity-2 c-border-middle"></div>
                    <li>
                        <div class="animate c-card fadeInRight wow">
                            <i class="c-float-left c-font-27 c-theme-font fa">
                                <img alt=""
                                     src="<%#SkinImageFolder %>iconResult.png"
                                     style="width: 38px;">
                            </i>
                            <div class="c-content c-content-left">
                                <h3 class="c-font-bold c-font-uppercase">
                                    <a class="c-theme-font"
                                       href="<%#DotNetNuke.Common.Globals.NavigateURL(95) %>">
                                        KẾT QUẢ CHƯƠNG TRÌNH ĐỊNH HƯỚNG
                                    </a>
                                </h3>
                                <ul style="list-style-type: none; font-size: 17px; font-weight: 400; color: #5c6873;">
                                    <li style="text-align: left;">Tiến độ thực hiện của Trung tâm kinh doanh?</li>
                                    <li style="text-align: left;">TOP Trung tâm kinh doanh đang dẫn đầu bảng xếp hạng?</li>
                                </ul>
                            </div>
                        </div>
                        <div class="c-bg-opacity-2 c-border-bottom"></div>
                    </li>
                </ul>
                <ul class="c-list">
                    <li>
                        <div class="animate c-card c-font-right fadeInLeft wow">
                            <i class="c-float-right c-font-27 c-theme-font fa">
                                <img alt=""
                                     src="<%#SkinImageFolder %>iconQandA.png"
                                     style="width: 45px;">
                            </i>
                            <div class="c-content c-content-right">
                                <h3 class="c-font-bold c-font-uppercase">
                                    <a class="c-theme-font"
                                       href="<%#DotNetNuke.Common.Globals.NavigateURL(135) %>">
                                        Q&A
                                    </a>
                                </h3>
                                <p style="font-size: 17px; font-weight: 400; color: #5c6873;">
                                    Giải đáp các thắc mắc liên quan đến Chương trình khuyến mại Mùa Hè Rực Nắng.
                                </p>
                            </div>
                        </div>
                        <div class="c-bg-opacity-2 c-border-bottom"></div>
                    </li>
                    <div class="c-bg-opacity-2 c-border-middle"></div>
                    <li class="c-border-grey-1-5">
                        <div class="animate c-card fadeInRight wow">
                            <i class="c-float-left c-font-27 c-theme-font fa">
                                <img alt=""
                                     src="<%#SkinImageFolder %>iconNews.png"
                                     style="width: 38px;">
                            </i>
                            <div class="c-content c-content-left">
                                <h3 class="c-font-bold c-font-uppercase">
                                    <a class="c-theme-font"
                                       href="<%#DotNetNuke.Common.Globals.NavigateURL(88) %>">
                                        TIN TỨC
                                    </a>
                                </h3>
                                <p style="font-size: 17px; font-weight: 400; color: #5c6873;">
                                    Các tin tức “nóng hổi” nhất sẽ thường xuyên được cập nhật tại đây.
                                </p>
                            </div>
                        </div>
                        <div class="c-bg-opacity-2 c-border-bottom"></div>
                    </li>
                </ul>

            </div>
        </div>
    </div>

    <div id="ContentPane"
         runat="server">
    </div>
</div>

<footer class="c-layout-footer c-layout-footer-1"
        id="Contact">
    <%--<div class = "c-prefooter">
        <div class = "container">
            <div class = "row">
                <div class = "col-md-6">
                    <div class = "c-feedback margin-left-0">
                        <h3>VietBank Portal</h3>
                    </div>
                    <p class = "c-about">
                        For any inquiries, questions or commendations, fill out the contact form.
                    </p>
                    <div class = "c-links">
                        <ul class = "c-nav">
                            <li>
                                <a href = "<%#GetHomeUrl() %>">Home</a>
                            </li>
                        </ul>
                    </div>
                    <p class = "c-contact">
                        # Street
                        <br> District 3
                        <br> Ho Chi Minh City, Viet Nam
                    </p>
                </div>
                <div class = "col-md-6">
                    <div class = "c-feedback">
                        <h3>Contact Us</h3>
                        <form action = "#">
                            <div class = "c-form-wrap">
                                <div class = "c-form-wrap-group">
                                    <input class = "form-control"
                                           placeholder = "Your Name"
                                           type = "text">
                                    <input class = "form-control"
                                           placeholder = "Subject"
                                           type = "text">
                                </div>
                                <div class = "c-form-wrap-group">
                                    <input class = "form-control"
                                           placeholder = "Your Email"
                                           type = "text">
                                    <input class = "form-control"
                                           placeholder = "Contact Phone"
                                           type = "text">
                                </div>
                            </div>
                            <textarea class="form-control" name="message" placeholder="Write comment here ..." rows="8"></textarea>
                            <button class = "btn btn-block btn-lg c-btn-dark c-btn-sbold c-btn-square c-btn-uppercase"
                                    type = "button">
                                Send it
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
    <div class="c-postfooter">
        <div class="container-fluid">
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