<%@ Control Language="C#" AutoEventWireup="false" CodeFile="AssignUser.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.AssignUser" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <Triggers>
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-12">
                    <dnn:DnnGrid 
                        ID ="gridUser"                                     
                        AutoGenerateColumns="true"
                        EnableViewState="true"
                        CssClass="dnnGrid"
                        Width="100%"
                        runat="server"
                        Visible="true">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridClientSelectColumn HeaderText="#" UniqueName="ClientSelectColumn">
                                    <ItemStyle HorizontalAlign="Center"/>
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                </dnn:DnnGridClientSelectColumn>
                            </Columns>
                        </MasterTableView>
                    </dnn:DnnGrid>
                </div> 
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <asp:Button 
                        ID="btnAdd"
                        CssClass="btn btn-primary"
                        OnClick="btnAdd_Click"
                        runat="server" 
                        Visible="true"
                        Text="Thêm"/>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>