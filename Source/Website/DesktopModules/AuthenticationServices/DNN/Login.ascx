<%@ Control Language="C#" Inherits="DotNetNuke.Modules.Admin.Authentication.DNN.Login" AutoEventWireup="false" CodeFile="Login.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>

<asp:UpdatePanel runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="cmdLogin" />
    </Triggers>
    <ContentTemplate>
        <div class="dnnLoginService form-horizontal">
            <div class="form-group">
                <div class="col-sm-5 control-label">
                    <asp:label AssociatedControlID="txtUsername"
                               id="plUsername"
                               runat="server" />
                </div>
                <div class="col-sm-7">
                    <asp:textbox CssClass="form-control c-theme"
                                 id="txtUsername"
                                 placeholder="Email Address"
                                 runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-5 control-label">
                    <asp:label AssociatedControlID="txtPassword"
                               id="plPassword"
                               resourcekey="Password"
                               runat="server"
                               ViewStateMode="Disabled" />
                </div>
                <div class="col-sm-7">
                    <asp:textbox CssClass="form-control c-theme"
                                 id="txtPassword"
                                 placeholder="Password"
                                 runat="server"
                                 textmode="Password" />
                </div>
            </div>
            <div class="form-group"
                 id="divCaptcha1"
                 runat="server"
                 visible="false">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:label AssociatedControlID="ctlCaptcha"
                               id="plCaptcha"
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
                    <dnn:captchacontrol captchaheight="40"
                                        captchawidth="130"
                                        errorstyle-cssclass="dnnFormMessage dnnFormError dnnCaptcha"
                                        id="ctlCaptcha"
                                        runat="server"
                                        TextBoxStyle-CssClass="form-control c-theme"
                                        ViewStateMode="Disabled" />
                </div>
            </div>
            <div class="form-group invisible">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:label AssociatedControlID="cmdLogin"
                               id="lblLoginRememberMe"
                               runat="server" />
                    <span class="dnnLoginRememberMe">
                        <asp:checkbox id="chkCookie"
                                      resourcekey="Remember"
                                      runat="server" />
                    </span>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:label AssociatedControlID="cmdLogin"
                               id="lblLogin"
                               runat="server"
                               ViewStateMode="Disabled"
                               Visible="False" />
                    <asp:LinkButton cssclass="btn btn-primary"
                                    id="cmdLogin"
                                    option-loading="false"
                                    resourcekey="cmdLogin"
                                    runat="server"
                                    text="Login" />
                    <asp:HyperLink CausesValidation="false"
                                   CssClass="btn btn-default"
                                   id="cancelLink"
                                   resourcekey="cmdCancel"
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
                                               resourcekey="cmdPassword"
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
    (function($)
    {
        $(document).ready(function()
        {
            $('.dnnLoginService').on("keydown",
                function(e)
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