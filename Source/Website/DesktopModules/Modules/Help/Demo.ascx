<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Demo.ascx.cs" Inherits="DesktopModules.Modules.Help.Demo" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register Src="~/controls/DoubleLabel.ascx" TagName="DoubleLabel" TagPrefix="control" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate>
        <div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class="c-font-bold c-font-uppercase">Demo</h3>
            <div class="c-bg-blue c-line-left"></div>
        </div>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="dnnTabs">
                <div class="form-group">
                    <ul class="dnnAdminTabNav dnnClear">
                        <li>
                            <a href="#TabControl">Form Control</a>
                        </li>
                        <li>
                            <a href="#TabAlert">Message & Dialog</a>
                        </li>
                    </ul>
                </div>
                
                <div class="dnnClear"
                    id="TabControl">
                    <div class="dnnPanels">
                        <h2 class="dnnFormSectionHead">
                            <a href="#">Form</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-sm-2 control-label">
                                    <control:DoubleLabel ID="lblUserName" IsRequire="True" Text="aaa"
                                        runat="server" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-control c-theme"
                                        ID="tbUserName"
                                        placeholder="Username"
                                        runat="server">
                                    </asp:TextBox>
                                </div>
                                <div class="col-sm-2 control-label">
                                    <dnn:Label ID="lblDisplayName"
                                        runat="server" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-control c-theme"
                                        ID="tbDisplayName"
                                        placeholder="Diplay Name"
                                        runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2 control-label">
                                    <dnn:Label ID="lblGender"
                                        runat="server" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList CssClass="form-control c-theme"
                                        ID="ddlGender"
                                        runat="server">
                                        <asp:ListItem Text="Male"
                                            Value="M" />
                                        <asp:ListItem Text="Female"
                                            Value="F" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2 control-label">
                                    <dnn:Label ID="lblBirthday"
                                        runat="server" />
                                </div>
                                <div class="col-sm-4">
                                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                        ID="dpBirthday"
                                        runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2 control-label">
                                    <dnn:Label ID="lblBiography"
                                        runat="server" />
                                </div>

                                <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-control c-theme"
                                        ID="tbBiography"
                                        runat="server"
                                        TextMode="MultiLine" />
                                </div>
                                <div class="col-sm-2 control-label">
                                    <dnn:Label ID="lblState"
                                        runat="server" />
                                </div>
                                <div class="col-sm-4">
                                    <div class="c-checkbox has-info">
                                        <asp:CheckBox runat="server" ID="chkState" />
                                        <label for="<%=chkState.ClientID %>">
                                            <span class="inc"></span>
                                            <span class="check"></span>
                                            <span class="box"></span>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="dnnPanels">
                        <h2 class="dnnFormSectionHead">
                            <a href="#">Buttons</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <asp:Button CssClass="btn btn-primary"
                                        OnClick="Search"
                                        runat="server"
                                        Text="Search" />
                                    <asp:Button CssClass="btn btn-default"
                                        OnClick="Edit"
                                        runat="server"
                                        Text="Edit Url" />
                                    <asp:Button CssClass="btn btn-default"
                                        ID="btnExport"
                                        OnClick="Export"
                                        runat="server"
                                        Text="Export" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>

                <div class="dnnClear"
                    id="TabAlert">
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Button CssClass="btn btn-primary"
                                OnClick="ShowSuccess"
                                runat="server"
                                Text="Success" />
                            <asp:Button CssClass="btn btn-primary"
                                OnClick="ShowInfo"
                                runat="server"
                                Text="Info" />
                            <asp:Button CssClass="btn btn-default"
                                OnClick="ShowWarning"
                                runat="server"
                                Text="Warning" />
                            <asp:Button CssClass="btn btn-default"
                                OnClick="ShowError"
                                runat="server"
                                Text="Error" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnConfirm"
                                OnClick="Confirm"
                                runat="server"
                                Text="Confirm" />
                            <asp:Button CssClass="btn btn-default"
                                OnClick="Alert"
                                runat="server"
                                Text="Alert" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function () {
        $(".dnnTabs").dnnTabs({
            selected: 0
        });
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
    },
        true);
</script>
