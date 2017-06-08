<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserInquiry.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserInquiry" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Modules.UserManagement.Business" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


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
                    <control:Combobox CssClass="form-control c-theme"
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
                    <asp:DropDownList CssClass="form-control c-theme"
                                      ID="ddlAuthorised"
                                      runat="server">
                        <asp:ListItem Value="-1">TẤT CẢ</asp:ListItem>
                        <asp:ListItem Value="1">ĐANG HOẠT ĐỘNG</asp:ListItem>
                        <asp:ListItem Value="2">ĐANG TẠM KHOÁ</asp:ListItem>
                        <asp:ListItem Value="0">ĐÃ KHOÁ</asp:ListItem>
                    </asp:DropDownList>
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
                    <asp:Button CssClass="btn btn-primary"
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
                    <control:Grid AllowPaging="true"
                                  AutoGenerateColumns="false"
                                  CssClass="dnnGrid"
                                  EnableViewState="true"
                                  ID="gridData"
                                  OnPageIndexChanged="OnPageIndexChanging"
                                  OnPageSizeChanged="OnPageSizeChanging"
                                  runat="server"
                                  Visible="false">
                        <MasterTableView TableLayout="Fixed">
                            <Columns>
                                <dnn:DnnGridTemplateColumn HeaderText="UserName">
                                    <HeaderStyle Width="30%" />
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserTable.UserID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditUser"
                                                        runat="server">
                                            <%#Eval(UserTable.UserName).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
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
                                <dnn:DnnGridTemplateColumn DataField="BranchID"
                                                        HeaderText="BranchID">
                                    <HeaderStyle Width="30%" />
                                    <ItemTemplate>
                                        <%#BranchBusiness.GetBranchName(Eval(BranchTable.BranchID).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
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