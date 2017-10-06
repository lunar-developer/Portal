<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Control Language="C#" Inherits="DotNetNuke.Modules.Admin.Security.PasswordReset" AutoEventWireup="false" CodeFile="PasswordReset.ascx.cs" %>
<div class="dnnForm dnnPasswordReset dnnClear">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-sm-12 dnnFormMessage dnnFormInfo" id="DivPasswordInfo" runat="server"></div>
        </div>
        <div class="form-group dnnFormMessage dnnFormInfo" runat="server" visible="False" id="resetMessages">
            <span>
                <asp:Label ID="lblInfo" runat="Server" /></span>
            <span class="error">
                <asp:Label ID="lblHelp" runat="Server" /></span>
        </div>
        <asp:Panel ID="divPassword" runat="server" class="dnnPasswordResetContent" DefaultButton="cmdChangePassword">
            <div class="form-group">
                <div class="col-sm-12">
                    <asp:TextBox ID="txtUsername" runat="server" TextMode="SingleLine" CssClass="form-control c-theme" ReadOnly="True" />
                    <asp:RequiredFieldValidator ID="valUsername" CssClass="dnnFormMessage dnnFormError dnnRequired" runat="server" Display="Dynamic" ControlToValidate="txtUsername" />
                </div>
            </div>
            <div class="">
                <div class="">
                    <asp:Panel ID="passwordContainer" runat="server" CssClass="password-strength-container">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" size="12" MaxLength="39" AutoCompleteType="Disabled" CssClass="form-control c-theme password-strength" />
                    </asp:Panel>
                    <asp:RegularExpressionValidator ID="valPassword" CssClass="dnnFormMessage dnnFormError dnnRequired" runat="server" resourcekey="Password.Required" Display="Dynamic" ControlToValidate="txtPassword" ValidationExpression="[\w\W]{7,}" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <div class="password-strength-container">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="39" TextMode="Password" CssClass="form-control c-theme password-confirm" />
                        <asp:RequiredFieldValidator ID="valConfirmPassword" CssClass="dnnFormMessage dnnFormError dnnRequired" runat="server" resourcekey="Confirm.Required" Display="Dynamic" ControlToValidate="txtConfirmPassword" />
                    </div>
                </div>
            </div>
            <div id="divQA" runat="server" visible="false">
                <div class="dnnFormItem">
                    <asp:Label ID="lblQuestion" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <asp:TextBox ID="txtAnswer" runat="server" />
                    <asp:RequiredFieldValidator ID="valAnswer" CssClass="dnnFormMessage dnnFormError dnnRequired" runat="server" Display="Dynamic" resourcekey="Answer.Required" ControlToValidate="txtAnswer" />
                </div>
            </div>
            <ul class="dnnActions dnnClear">
                <li>
                    <asp:LinkButton ID="cmdChangePassword" CssClass="btn btn-primary" runat="server" resourcekey="cmdChangePassword" /></li>
                <li id="liLogin" runat="server">
                    <asp:HyperLink ID="hlCancel" CssClass="btn btn-default" runat="server" resourcekey="cmdCancel" /></li>
            </ul>
        </asp:Panel>
    </div>
</div>
