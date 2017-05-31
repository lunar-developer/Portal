<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ApplicationInquiry.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.ApplicationInquiry" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Import Namespace="Modules.UserManagement.Global" %>
<%@ Import Namespace="Website.Library.Global" %>
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
                    <dnn:Label ID="lblFromDate"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                       ID="dpFromDate"
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
                    <dnn:Label ID="lblCustomerInfo"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtCustomerInfo"
                                 runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblFilterFlag"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList CssClass="form-control c-theme"
                                      ID="ddlFilterFlag"
                                      runat="server">
                        <asp:ListItem Value="0">Tất cả</asp:ListItem>
                        <asp:ListItem Value="1">Hồ sơ của tôi</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSearch"
                                OnClick="Search"
                                runat="server"
                                Text="Tìm Kiếm" />
                    <asp:Button CssClass="btn btn-default"
                                ID="btnExport"
                                OnClick="Export"
                                runat="server"
                                Text="Tải xuống" />
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="False"
                                 CssClass="dnnGrid"
                                 EnableViewState="true"
                                 ID="gridData"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server"
                                 Visible="false">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridTemplateColumn HeaderText="ApplicFormID">
                                    <HeaderStyle Width="200" />
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(ApplicationFormTable.UniqueID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditUser"
                                                        runat="server">
                                            <%#Eval(ApplicationFormTable.UniqueID).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="BranchCode">
                                    <HeaderStyle Width="300" />
                                    <ItemTemplate>
                                        <%#UserManagementModuleBase.FormatBranchCode(Eval(ApplicationFormTable.BranchCode).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="CustomerID"
                                                        HeaderText="CustomerID">
                                    <HeaderStyle Width="150" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="CustomerName"
                                                        HeaderText="CustomerName">
                                    <HeaderStyle Width="200" />
                                </dnn:DnnGridBoundColumn>

                                <dnn:DnnGridTemplateColumn HeaderText="CreateUserName">
                                    <HeaderStyle Width="300" />
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatUserName(Eval("CreateUserName").ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidFromDate"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidCustomerInfo"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidBranchCode"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidFilterFlag"
                         runat="server"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>