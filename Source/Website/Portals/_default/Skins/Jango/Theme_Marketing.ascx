<%@ Control AutoEventWireup="false" CodeFile="Theme.ascx.cs" Explicit="True" Language="C#" Inherits="Portals._default.Skins.Jango.Theme" %>

<%@ Register Src="~/DesktopModules/DDRMenu/Menu.ascx" TagName="Menu" TagPrefix="dnn" %>
<%@ Register Src="~/Admin/Skins/Copyright.ascx" TagName="CopyRight" TagPrefix="dnn" %>

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

<div class="c-layout-page" <%#SetHeaderMargin(true) %>>

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
        <div class="container">
            <div class="c-content-feature-10">

                <div class="c-content-title-1">
                    <h3 class="c-center c-font-bold c-font-uppercase">Quán Quân huy động 2017</h3>
                    <div class="c-line-center c-theme-bg"></div>
                    <p class="c-font-left">
                        "Quán Quân huy động 2017" - Sân chơi thi đua nội bộ <b>lớn nhất</b>, <b>lần đầu tiên</b> được tổ chức tại Vietbank dành cho tất cả CBNV (kể cả CBNV Hội Sở) với tổng giá trị giải thưởng lên đến <b>2 Tỷ đồng</b>.
                    </p>
                    <p class="c-font-left">
                        Bí quyết để chiến thắng của chính là sự kiên trì. Tích tiểu thành đại từ những món gửi nhỏ, có thể bạn không chiến thắng giải tuần, nhưng giải vinh quang nhất là <b>Quán Quân Huy động - 100 triệu đồng</b> vẫn chờ đợi bạn nếu:
                    </p>
                    <ul class="c-font-center"
                        style="list-style-type: disc; margin: 0 auto; width: 500px; font-size: 17px; font-weight: 400; color: #5c6873;">
                        <li style="text-align: left; list-style: disc;">Thuyết phục khách hàng gửi kỳ hạn dài</li>
                        <li style="text-align: left; list-style: disc;">Tăng cường tiếp thị chéo “Khách hàng gửi khách hàng”</li>
                        <li style="text-align: left; list-style: disc;">Đừng quên các món gửi CASA được tính điểm gấp đôi</li>

                    </ul>
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
                                <ul style="list-style-type: none; font-size: 17px; font-weight: 400; color: #5c6873;">
                                    <li style="text-align: right;">Giải thưởng tuần: 1 chiếc iPhone 7/tuần</li>
                                    <li style="text-align: right;">Giải thưởng năm: 100 triệu đồng </li>
                                    <li style="text-align: right;">Bạn sẽ là người chiến thắng và chủ nhân sở hữu những phần thưởng giá trị trên?</li>
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
                                        KẾT QUẢ XẾP HẠNG
                                    </a>
                                </h3>
                                <ul style="list-style-type: none; font-size: 17px; font-weight: 400; color: #5c6873;">
                                    <li style="text-align: left;">Bạn đang ở vị trí thứ mấy trong Bảng xếp hạng?</li>
                                    <li style="text-align: left;">TOP cá nhân đang dẫn đầu giải tuần và giải năm Quán Quân Huy Động 2017?</li>
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
                                    Giải đáp mọi thắc mắc liên quan đến đến chương trình “Quán Quân Huy Động”.
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

</body>