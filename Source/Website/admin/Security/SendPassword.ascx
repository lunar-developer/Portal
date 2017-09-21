<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Control Language="C#" Inherits="DotNetNuke.Modules.Admin.Security.SendPassword" AutoEventWireup="false" CodeFile="SendPassword.ascx.cs" %>
<div class="dnnForm dnnSendPassword dnnClear">

    <asp:Panel ID="pnlRecover" runat="server">

        <div id="divHelp" runat="server" class="dnnFormMessage dnnFormInfo">
            <asp:Label ID="lblHelp" runat="Server" />
        </div>

        <div id="divPassword" runat="server" class="dnnSendPasswordContent form-horizontal">
            <div class="form-group" id="divUsername" runat="server">
                <dnn:Label ID="plUsername" ControlName="txtUsername" runat="server" />
                <asp:TextBox ID="txtUsername" CssClass="dnnFormRequired form-control c-theme" runat="server" />
            </div>
            <div class="form-group" id="divEmail" runat="server">
                <dnn:Label ID="plEmail" ControlName="txtEmail" runat="server" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control c-theme" />
            </div>
            <div id="divCaptcha" runat="server" class="form-group">
                <dnn:Label ID="plCaptcha" ControlName="ctlCaptcha" runat="server" />
                <dnn:CaptchaControl ID="ctlCaptcha" CaptchaWidth="130" CaptchaHeight="40" runat="server"
                    TextBoxStyle-CssClass="form-control c-theme"
                    ErrorStyle-CssClass="dnnFormMessage dnnFormError" />
            </div>
        </div>
    </asp:Panel>

    <ul class="dnnActions dnnClear">
        <li id="liSend" runat="server">
            <asp:LinkButton ID="cmdSendPassword" CssClass="btn btn-primary" runat="server" /></li>
        <li id="liCancel" runat="server">
            <asp:LinkButton ID="cancelButton" runat="server" CssClass="btn btn-default" resourcekey="cmdCancel" CausesValidation="false" /></li>
    </ul>

</div>
