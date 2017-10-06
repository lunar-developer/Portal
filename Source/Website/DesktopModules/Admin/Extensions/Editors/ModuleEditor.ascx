<%@ Control Language="C#" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Admin.Extensions.ModuleEditor" CodeFile="ModuleEditor.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.Security.Permissions.Controls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<h2 class="dnnFormSectionHead" id="moduleSettingsHead" runat='server'><a href="" class="dnnLabelExpanded"><%=LocalizeString("ModuleSettings")%></a></h2>
<fieldset>
    <asp:Panel ID="helpPanel" runat="server" CssClass="dnnFormMessage dnnFormInfo">
        <asp:Label ID="lblHelp" runat="server" />
    </asp:Panel>
    <dnn:dnnformeditor id="desktopModuleForm" runat="Server" formmode="Short">
        <Items>
            <dnn:DnnFormLiteralItem ID="moduleName" runat="server" DataField = "ModuleName" />
            <dnn:DnnFormTextBoxItem ID="folderName" runat="server" DataField = "FolderName" />
            <dnn:DnnFormComboBoxItem ID="category" runat="server" DataField = "Category" ListTextField="Name" ListValueField="Name" />
            <dnn:DnnFormTextBoxItem ID="controllerClass" runat="server" DataField = "BusinessControllerClass" Columns="55" />
            <dnn:DnnFormTextBoxItem ID="dependencies" runat="server" DataField = "Dependencies" Columns="55"/>
            <dnn:DnnFormTextBoxItem ID="permissions" runat="server" DataField = "Permissions" Columns="55"/>
            <dnn:DnnFormLiteralItem ID="isPortable" runat="server" DataField="IsPortable" />
            <dnn:DnnFormLiteralItem ID="isSearchable" runat="server" DataField="IsSearchable" />
            <dnn:DnnFormLiteralItem ID="isUpgradable" runat="server" DataField="IsUpgradeable" />
            <dnn:DnnFormComboBoxItem ID="Shareable" runat="server" DataField="Shareable" ListTextField="Name" ListValueField="Name" />
            <dnn:DnnFormToggleButtonItem ID="IsPremiumm" runat="server" DataField="IsPremium" />
            <dnn:DnnFormTemplateItem ID="PremiumModules" runat="server">
                <ItemTemplate>
				    <dnn:Label ID="plPremium" runat="server" ControlName="ctlPortals" />
                    <dnn:DualListBox id="ctlPortals" runat="server" DataValueField="PortalID" DataTextField="PortalName" AddKey="AddPortal" RemoveKey="RemovePortal" AddAllKey="AddAllPortals" RemoveAllKey="RemoveAllPortals" AddImageURL="~/images/rt.gif" AddAllImageURL="~/images/ffwd.gif" RemoveImageURL="~/images/lt.gif" RemoveAllImageURL="~/images/frev.gif" ContainerStyle-HorizontalAlign="Center" >
                        <AvailableListBoxStyle Height="130px" Width="225px" />
                        <HeaderStyle />
                        <SelectedListBoxStyle Height="130px" Width="225px"  />
                    </dnn:DualListBox>                            
                </ItemTemplate>
            </dnn:DnnFormTemplateItem>
        </Items>
    </dnn:dnnformeditor>
    <asp:Panel ID="pnlPermissions" runat="server" Visible="false">
        <div>
            <dnn:desktopmodulepermissionsgrid id="dgPermissions" runat="server" />
        </div>
        <ul class="dnnActions dnnClear">
            <li>
                <asp:LinkButton ID="cmdUpdate" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdate" /></li>
        </ul>
    </asp:Panel>
</fieldset>
<asp:Panel ID="pnlDefinitions" runat="server" Visible="False">
    <h2 class="dnnFormSectionHead" id="dnnPanel-ExtensionPackageSettings"><a href="" class="dnnLabelExpanded"><%=LocalizeString("Definitions")%></a></h2>
    <fieldset>
        <div class="form-horizontal">
            <div id="definitionSelectRow" class="dnnFormItem" runat="server">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:label id="plSelectDefinition" controlname="cboDefinitions" runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <dnn:dnncombobox id="cboDefinitions" runat="server" datatextfield="DefinitionName" datavaluefield="ModuleDefId" autopostback="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"></div>
                    <div class="col-sm-8">
                        <div>
                            <asp:LinkButton ID="cmdAddDefinition" resourcekey="cmdAddDefinition" runat="server" CssClass="btn btn-primary" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlDefinition" runat="server" Visible="false">
            <dnn:dnnformeditor id="definitionsEditor" runat="Server" formmode="Short">
                <Items>
                    <dnn:DnnFormLiteralItem ID="definitionNameLiteral" runat="server" DataField = "DefinitionName" />
                    <dnn:DnnFormTextBoxItem ID="definitionName" runat="server" DataField="DefinitionName" Required="True" />
                    <dnn:DnnFormTextBoxItem ID="friendlyName" runat="server" DataField = "FriendlyName" Required="true" />
                    <dnn:DnnFormTextBoxItem ID="cacheTime" runat="server" DataField = "DefaultCacheTime" />
                </Items>
            </dnn:dnnformeditor>
            <asp:Panel ID="pnlControls" CssClass="dnnFormItem" runat="server" Visible="false">
                <dnn:label id="lblControls" runat="server" resourcekey="Controls" controlname="grdControls" />
                <asp:DataGrid ID="grdControls" runat="server" CellSpacing="0" AutoGenerateColumns="false" EnableViewState="true" GridLines="None" CssClass="dnnASPGrid">
                    <HeaderStyle CssClass="dnnGridHeader" />
                    <ItemStyle CssClass="dnnGridItem" />
                    <Columns>
                        <dnn:textcolumn datafield="ControlKey" headertext="Control" />
                        <dnn:textcolumn datafield="ControlTitle" headertext="Title" />
                        <dnn:textcolumn datafield="ControlSrc" headertext="Source" />
                        <dnn:imagecommandcolumn headerstyle-width="18px" commandname="Edit" iconkey="Edit" editmode="URL" keyfield="ModuleControlID" />
                        <dnn:imagecommandcolumn headerstyle-width="18px" commandname="Delete" iconkey="Delete" keyfield="ModuleControlID" />
                    </Columns>
                </asp:DataGrid>
                <asp:HyperLink ID="cmdAddControl" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdAddControl" />
            </asp:Panel>
            <div class="dnnFormItem">
                <asp:Label ID="lblDefinitionError" runat="server" CssClass="dnnFormMessage dnnFormError" Visible="false" ResourceKey="DuplicateName" />
            </div>
            <ul class="dnnActions dnnClear">
                <li>
                    <asp:LinkButton ID="cmdUpdateDefinition" runat="server" CssClass="dnnPrimaryAction" /></li>
                <li>
                    <asp:LinkButton ID="cmdCancelDefinition" resourcekey="cmdCancelDefinition" CssClass="dnnSecondaryAction" runat="server" Visible="False" CausesValidation="False" /></li>
                <li>
                    <asp:LinkButton ID="cmdDeleteDefinition" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdDeleteDefinition" CausesValidation="False" /></li>
            </ul>
        </asp:Panel>
    </fieldset>
</asp:Panel>
