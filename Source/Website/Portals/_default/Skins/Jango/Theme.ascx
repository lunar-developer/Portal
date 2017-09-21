<%@ Control AutoEventWireup="false" CodeFile="Theme.ascx.cs" Explicit="True" Language="C#" Inherits="Portals._default.Skins.Jango.Theme" %>

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

<div class="c-layout-page" <%#SetHeaderMargin(true) %>>

<section class="c-layout-revo-slider c-layout-revo-slider-8"
         dir="ltr">
    <div class="tp-banner-container tp-fullscreen tp-fullscreen-mobile">
        <div class="rev_slider tp-banner"
             data-version="5.0">
            <ul>
                <li data-masterspeed="1000"
                    data-slotamount="1"
                    data-style="dark"
                    data-transition="fade">
                    <img alt=""
                         data-bgfit="cover"
                         data-bgposition="center center"
                         data-bgrepeat="no-repeat"
                         src="<%#SkinImageFolder %>bg-01.jpg">
                    <div class="customin customout tp-caption"
                         data-easing="Back.easeOut"
                         data-elementdelay="0.1"
                         data-endelementdelay="0.1"
                         data-endspeed="600"
                         data-hoffset=""
                         data-speed="500"
                         data-splitin="none"
                         data-splitout="none"
                         data-start="1000"
                         data-transform_in="x:0;y:0;z:0;rotationX:0.5;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:500;e:Back.easeInOut;"
                         data-transform_out="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:600;e:Back.easeInOut;"
                         data-voffset="-30"
                         data-x="center"
                         data-y="center">
                        <h3 class="c-block c-font-60 c-font-bold c-font-center c-font-uppercase c-font-white c-main-title">
                            Ưu đãi lớn
                            <br> dành cho đại lý
                        </h3>
                    </div>
                    <div class="randomrotateout tp-caption"
                         data-hoffset=""
                         data-speed="300"
                         data-start="1500"
                         data-transform_in="x:0;y:0;z:0;rotationX:0.5;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:300;e:Back.easeInOut;"
                         data-transform_out="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:400;e:Back.easeInOut;"
                         data-voffset="200"
                         data-x="center"
                         data-y="center">
                    </div>
                    <div class="customin customout tp-caption"
                         data-basealign="slide"
                         data-easing="Back.easeOut"
                         data-elementdelay="0.1"
                         data-endelementdelay="0.1"
                         data-endspeed="600"
                         data-hoffset="0"
                         data-speed="500"
                         data-splitin="none"
                         data-splitout="none"
                         data-start="1800"
                         data-transform_in="x:0;y:0;z:0;rotationX:0.5;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:500;e:Back.easeInOut;"
                         data-transform_out="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:600;e:Back.easeInOut;"
                         data-voffset="0"
                         data-width="full"
                         data-x="center"
                         data-y="bottom"
                         style="width: 100%;">
                        <div class="c-action-bar">
                            <div class="container">
                                <div class="c-content ">
                                    <h3 class="c-font-30 c-font-sbold c-font-white"> Ưu đãi dành cho đại lý phân phối xe ô tô </h3>
                                    <p class="c-font-20 c-font-thin c-font-white c-opacity-08">
                                        Lãi suất ưu đãi chỉ từ <b>7%</b> năm dành cho <b>Đại lý phân phối xe ô tô</b>
                                    </p>
                                </div>
                                <div class="c-btn-container">
                                    <a class="btn btn-lg c-action-btn c-btn-bold c-btn-border-2x c-btn-square c-btn-uppercase c-btn-white"
                                       href="<%#DotNetNuke.Common.Globals.NavigateURL(88) %>">
                                        View
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                <li data-masterspeed="1000"
                    data-slotamount="1"
                    data-style="dark"
                    data-transition="fade">
                    <img alt=""
                         data-bgfit="cover"
                         data-bgposition="center center"
                         data-bgrepeat="no-repeat"
                         src="<%#SkinImageFolder %>bg-02.jpg">
                    <div class="customin customout tp-caption"
                         data-easing="Back.easeOut"
                         data-elementdelay="0.1"
                         data-endelementdelay="0.1"
                         data-endspeed="600"
                         data-hoffset=""
                         data-speed="500"
                         data-splitin="none"
                         data-splitout="none"
                         data-start="1000"
                         data-transform_in="x:0;y:0;z:0;rotationX:0.5;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:500;e:Back.easeInOut;"
                         data-transform_out="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:600;e:Back.easeInOut;"
                         data-voffset="-30"
                         data-x="center"
                         data-y="center">
                        <h3 class="c-font-60 c-font-bold c-font-center c-font-uppercase c-font-white c-main-title">
                            Passionate About
                            <br> Your Success
                        </h3>
                    </div>
                    <div class="randomrotateout tp-caption"
                         data-hoffset=""
                         data-speed="300"
                         data-start="1800"
                         data-transform_in="x:0;y:0;z:0;rotationX:0.5;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:300;e:Back.easeInOut;"
                         data-transform_out="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;s:400;e:Back.easeInOut;"
                         data-voffset="500"
                         data-x="center"
                         data-y="center">
                        <a class="btn btn-lg c-action-btn-2 c-btn-bold c-btn-border-2x c-btn-square c-btn-uppercase c-btn-white"
                           href="#Contact">
                            Contact
                        </a>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</section>

<div class="c-bg-white c-content-box c-size-md"
     id="Solutions">
    <div class="container-fluid">
        <div class="c-content-feature-10">

            <div class="c-content-title-1">
                <h3 class="c-center c-font-bold c-font-uppercase">Solutions</h3>
                <div class="c-line-center c-theme-bg"></div>
                <p class="c-font-center">
                    With the proliferation of Internet, mobile devices and social media, businesses and consumers are finding out new ways which will allow them to use such latest technologies and devices to speed up collections, cut costs, grow revenue and mitigate risks. If you wish to be ahead of the game, iRESLab can be your partner to offer you services in the areas of payment gateways, mobile payment strategies, use of smartphones and tablets as payment acceptance devices, Point-of-Sale (POS) terminals with contactless readers, NFC payment and acceptance, Smartcard application development and design and prepaid and loyalty app design.
                </p>
            </div>

            <ul class="c-list">
                <li>
                    <div class="animate c-card c-font-right fadeInLeft wow">
                        <i class="c-border c-border-opacity c-float-right c-font-27 c-theme-font fa fa-credit-card"></i>
                        <div class="c-content c-content-right">
                            <h3 class="c-font-bold c-font-uppercase">
                                Payment Gateway
                            </h3>
                            <p>
                                In today’s time and age when a big chunk of purchases are happening online, you need to have a tight integration of Payment Gateway in your business. The need of Payment Gateway is very personalized based on how your business accepts and processes varied number of electronic transactions such as credit cards, checks, digital cash, etc.
                            </p>
                        </div>
                    </div>
                    <div class="c-bg-opacity-2 c-border-bottom"></div>
                </li>
                <div class="c-bg-opacity-2 c-border-middle"></div>
                <li>
                    <div class="animate c-card fadeInRight wow">
                        <i class="c-border c-border-opacity c-float-left c-font-27 c-theme-font fa fa-google-wallet"></i>
                        <div class="c-content c-content-left">
                            <h3 class="c-font-bold c-font-uppercase">
                                Smart Wallet
                            </h3>
                            <p>
                                The emerging concept of ‘Smart Wallet’ maintained on the smartphones and tablet devices offers interesting opportunities to retailers to significantly drive revenue. Understanding how best to apply card (loyalty, gift, credit, etc.) promotions to drive “e-card” use in a secure and easy-to-use manner is an acknowledged revenue driver for merchants and retailers.
                            </p>
                        </div>
                    </div>
                    <div class="c-bg-opacity-2 c-border-bottom"></div>
                </li>
            </ul>
            <ul class="c-list">
                <li>
                    <div class="animate c-card c-font-right fadeInLeft wow">
                        <i class="c-border c-border-opacity c-float-right c-font-27 c-theme-font fa fa-gift"></i>
                        <div class="c-content c-content-right">
                            <h3 class="c-font-bold c-font-uppercase">
                                Prepaid and Loyalty
                            </h3>
                            <p>
                                Prepaid and loyalty are specialized and customized mobile transaction applications. All the nuances of Loyalty Programs such as – pre-transaction rewords which are offered based on previous usage, post-transaction rewords offered based on retailer’s objective for its subscriber, initial value assignment to prepaid cards and replenishing the card value as it expires – require a specialized application and business expertise.
                            </p>
                        </div>
                    </div>
                    <div class="c-bg-opacity-2 c-border-bottom"></div>
                </li>
                <div class="c-bg-opacity-2 c-border-middle"></div>
                <li class="c-border-grey-1-5">
                    <div class="animate c-card fadeInRight wow">
                        <i class="c-border c-border-opacity c-float-left c-font-27 c-theme-font fa fa-lock"></i>
                        <div class="c-content c-content-left">
                            <h3 class="c-font-bold c-font-uppercase">
                                Security
                            </h3>
                            <p>
                                Security is at the forefront of any payment related solution – and we understand it. iRESlab has specialized expertise in two important areas like PCI and EMV.
                            </p>
                        </div>
                    </div>
                    <div class="c-bg-opacity-2 c-border-bottom"></div>
                </li>
            </ul>
            <ul class="c-list">
                <li>
                    <div class="animate c-card c-font-right fadeInLeft wow">
                        <i class="c-border c-border-opacity c-float-right c-font-27 c-theme-font fa fa-wifi"></i>
                        <div class="c-content c-content-right">
                            <h3 class="c-font-bold c-font-uppercase">
                                NFC and HCE
                            </h3>
                            <p>
                                Cutting-edge technologies like Near-field communication (NFC) and Host-based card emulation (HCE) drive mobile payment applications. NFC is the set of standards for a mobile device communicating with another device in close proximity to it (usually within centimeters).
                            </p>
                        </div>
                    </div>
                    <div class="c-bg-opacity-2 c-border-bottom"></div>
                </li>
                <div class="c-bg-opacity-2 c-border-middle"></div>
                <li class="c-border-grey-1-5">
                    <div class="animate c-card fadeInRight wow">
                        <i class="c-border c-border-opacity c-float-left c-font-27 c-theme-font fa fa-mobile"></i>
                        <div class="c-content c-content-left">
                            <h3 class="c-font-bold c-font-uppercase">
                                Mobile Commerce
                            </h3>
                            <p>
                                If you wish you establish Mobile Commerce as part of your business strategy, you need to think of the complete ecosystem – the baseline hardware, system protocols to the end-user application and everything in between.
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