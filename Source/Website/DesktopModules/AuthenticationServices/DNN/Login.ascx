<%@ Control Language="C#" Inherits="DotNetNuke.Modules.Admin.Authentication.DNN.Login" AutoEventWireup="false" CodeFile="Login.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<asp:UpdatePanel runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="cmdLogin" />
    </Triggers>
    <ContentTemplate>
        <div class="dnnLoginService form-horizontal">
            <div class="form-group">
                <div class="col-sm-5 control-label">
                    <asp:Label AssociatedControlID="txtUsername"
                        ID="plUsername"
                        runat="server" />
                </div>
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control c-theme"
                        ID="txtUsername"
                        placeholder="Email"
                        runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-5 control-label">
                    <asp:Label AssociatedControlID="txtPassword"
                        ID="plPassword"
                        Text="Mật khẩu"
                        runat="server"
                        ViewStateMode="Disabled" />
                </div>
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control c-theme"
                        ID="txtPassword"
                        placeholder="Mật khẩu"
                        runat="server"
                        TextMode="Password" />
                </div>
            </div>
            <div class="form-group"
                id="divCaptcha1"
                runat="server"
                visible="false">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:Label AssociatedControlID="ctlCaptcha"
                        ID="plCaptcha"
                        resourcekey="Captcha"
                        runat="server" />
                </div>
            </div>
            <div class="form-group"
                id="divCaptcha2"
                runat="server"
                visible="false">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <dnn:CaptchaControl CaptchaHeight="40"
                        CaptchaWidth="130"
                        ErrorStyle-CssClass="dnnFormMessage dnnFormError dnnCaptcha"
                        ID="ctlCaptcha"
                        runat="server"
                        TextBoxStyle-CssClass="form-control c-theme"
                        ViewStateMode="Disabled" />
                </div>
            </div>
            <div class="form-group invisible">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:Label AssociatedControlID="cmdLogin"
                        ID="lblLoginRememberMe"
                        runat="server" />
                    <span class="dnnLoginRememberMe">
                        <asp:CheckBox ID="chkCookie"
                            resourcekey="Remember"
                            runat="server" />
                    </span>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:Label AssociatedControlID="cmdLogin"
                        ID="lblLogin"
                        runat="server"
                        ViewStateMode="Disabled"
                        Visible="False" />
                    <asp:LinkButton CssClass="btn btn-primary"
                        ID="cmdLogin"
                        option-loading="false"
                        runat="server"
                        Text="Đăng Nhập" />
                    <asp:HyperLink CausesValidation="false"
                        CssClass="btn btn-default"
                        ID="cancelLink"
                        Text="Hủy"
                        runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <div class="dnnLoginActions">
                        <ul class="dnnActions dnnClear">
                            <li id="liRegister"
                                runat="server">
                                <asp:HyperLink CssClass="btn btn-default"
                                    ID="registerLink"
                                    resourcekey="cmdRegister"
                                    runat="server"
                                    ViewStateMode="Disabled" />
                            </li>
                            <li id="liPassword"
                                runat="server">
                                <asp:HyperLink CssClass="btn btn-success"
                                    ID="passwordLink"
                                    Text="Quên Mật Khẩu"
                                    runat="server"
                                    ViewStateMode="Disabled" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($)
    {
        $(document).ready(function ()
        {
            $('.dnnLoginService').on("keydown",
                function (e)
                {
                    if (e.keyCode === 13)
                    {
                        processOnLoginClick();
                        e.preventDefault();
                        return false;
                    }
                    return true;
                });
            getJQueryControl("cmdLogin").click(processOnLoginClick);
        });
    }(jQuery, window.Sys));


    function processOnLoginClick()
    {
        if (validateInputArray(["txtUsername", "txtPassword"]) === false)
        {
            return false;
        }

        var $loginButton = getJQueryControl("cmdLogin");
        if ($loginButton.hasClass("dnnDisabledAction"))
        {
            return false;
        }

        $loginButton.addClass("dnnDisabledAction");
        eval($loginButton.attr("href"));
        return true;
    }
</script>
