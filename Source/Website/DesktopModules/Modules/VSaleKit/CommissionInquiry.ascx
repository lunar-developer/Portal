<%@ Control Language="C#" AutoEventWireup="false" CodeFile="CommissionInquiry.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.CommissionInquiry" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <Triggers>
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label 
                        ID="lblBranch"
                        runat="server"/>
                </div>
                <div class="col-sm-4">
                    <control:Combobox 
                        CssClass="form-control c-theme"
                        ID="ddlBranch"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                        runat="server">
                    </control:Combobox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label 
                        ID="lblUsers"
                        runat="server"/>
                </div>
                <div class="col-sm-4">
                    <control:Combobox 
                        CssClass="form-control c-theme"
                        ID="ddlUsers"
                        Visible="true"
                        runat="server">
                    </control:Combobox>
                </div> 
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-4">
                    <asp:Button 
                        ID="btnSearch"
                        CssClass="btn btn-primary"
                        OnClick="btnSearch_Click"
                        runat="server" 
                        Visible="true"
                        Text="Tìm kiếm"/>
                    <asp:Button 
                        ID="btnAdd"
                        CssClass="btn btn-default"
                        runat="server"
                        OnClick="btnAdd_Click"
                        Text="Thêm"
                        Visible="false"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <dnn:DnnGrid 
                        ID ="gridAssignUser"                                     
                        AutoGenerateColumns="true"
                        EnableViewState="true"
                        CssClass="dnnGrid"
                        Width="100%"
                        runat="server"
                        Visible="false">
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
                <div class="col-sm-10"></div>
                <div class="col-sm-2">
                    <asp:Button 
                        ID="btnRemove"
                        CssClass="btn btn-default"
                        runat="server" 
                        Text="Xóa"
                        Width="110"
                        OnClick="btnRemove_Click"
                        Visible="false" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>