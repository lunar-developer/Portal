<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserInquiry.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserInquiry" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Modules.UserManagement.Business" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblUserName"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                        ID="txtUserName"
                        runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblBranch"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete
                        ID="ddlBranch"
                        runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblAuthorised"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete
                        ID="ddlAuthorised"
                        runat="server">
                        <Items>
                            <control:ComboBoxItem Value="-1" Text="TẤT CẢ"/>
                            <control:ComboBoxItem Value="1" Text="ĐANG HOẠT ĐỘNG"/>
                            <control:ComboBoxItem Value="2" Text="ĐANG TẠM KHOÁ"/>
                            <control:ComboBoxItem Value="0" Text="ĐÃ KHOÁ"/>
                        </Items>
                    </control:AutoComplete>
                </div>
                <div class="col-sm-2 control-label"></div>
                <div class="col-sm-4"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                        ID="btnSearch"
                        OnClick="SearchUser"
                        runat="server"
                        Text="Tìm Kiếm" />
                    <asp:Button CssClass="btn btn-success"
                        ID="btnAdd"
                        OnClick="AddUser"
                        runat="server"
                        Text="Thêm mới" />
                    <asp:Button CssClass="btn btn-default"
                        ID="btnExport"
                        OnClick="ExportUser"
                        runat="server"
                        Text="Tải xuống" />
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-12 table-responsive">
                    <control:Grid 
                        AutoGenerateColumns="false"
                        CssClass="dnnGrid"
                        EnableViewState="true"
                        ID="gridData"
                        OnNeedDataSource="ProcessOnNeedDataSource"
                        runat="server"
                        Visible="false">
                        <MasterTableView TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Email" SortExpression="UserName">
                                    <HeaderStyle Width="30%" />
                                    <ItemTemplate>
                                        <a class="c-edit-link c-theme-color"
                                            href="<%#GetEditUrl(Eval(UserTable.UserID).ToString()) %>"
                                            target="_blank">
                                            <%#Eval(UserTable.UserName).ToString() %>
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="DisplayName"
                                    HeaderText="DisplayName">
                                    <HeaderStyle Width="30%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Mobile"
                                    HeaderText="Mobile">
                                    <HeaderStyle Width="20%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PhoneExtension"
                                    HeaderText="PhoneExtension">
                                    <HeaderStyle Width="20%" />
                                </dnn:DnnGridBoundColumn>
                                <telerik:GridTemplateColumn DataField="BranchID" SortExpression="BranchID"
                                    HeaderText="Chi Nhánh">
                                    <HeaderStyle Width="30%" />
                                    <ItemTemplate>
                                        <%#BranchBusiness.GetBranchName(Eval(BranchTable.BranchID).ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="Authorised" 
                                    HeaderText="Authorised">
                                    <HeaderStyle Width="20%" />
                                </dnn:DnnGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidUserName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidBranchID"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidAuthorised"
            runat="server"
            Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>
