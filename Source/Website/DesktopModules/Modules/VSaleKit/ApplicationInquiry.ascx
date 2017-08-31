<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ApplicationInquiry.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.ApplicationInquiry" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Import Namespace="Modules.VSaleKit.Enum" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblFromDate"
                        runat="server" />
                </div>
                <div class="col-sm-2">
                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                        ID="calFromDate"
                        runat="server" />
                </div>
                <div class="col-sm-1 control-label">
                    <dnn:Label ID="lblToDate"
                        runat="server" />
                </div>
                <div class="col-sm-2">
                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                        ID="calToDate"
                        runat="server" />
                </div>
                <div class="col-sm-1 control-label">
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
                    <dnn:Label ID="lblCustomerInfo"
                        runat="server" />
                </div>
                <div class="col-sm-5">
                    <asp:TextBox CssClass="form-control c-theme"
                        ID="txtCustomerInfo"
                        runat="server" />
                </div>
                <div class="col-sm-1 control-label">
                    <dnn:Label ID="lblFilterFlag"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete ID="ddlFilterFlag" runat="server">
                        <Items>
                            <control:ComboBoxItem Value="0" Text="Tất cả" />
                            <control:ComboBoxItem Value="1" Text="Hồ sơ của tôi" />
                            <control:ComboBoxItem Value="2" Text="Hồ sơ chờ xử lý" />
                            <control:ComboBoxItem Value="3" Text="Hồ sơ đã xử lý" />
                        </Items>
                    </control:AutoComplete>
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
                        ID="btnAdd"
                        OnClick="AddApplication"
                        runat="server"
                        Text="Thêm mới" />
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid
                        AutoGenerateColumns="False"
                        ID="gridData"
                        OnNeedDataSource="OnNeedDataSource"
                        runat="server"
                        Visible="false">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="ApplicFormID">
                                    <HeaderStyle Width="120" />
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(ApplicationFormTable.UniqueID).ToString() %>"
                                            CssClass="c-edit-link c-theme-color"
                                            OnClick="ViewApplication"
                                            runat="server">
                                            <%#Eval(ApplicationFormTable.UniqueID).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="BranchCode" DataField="BranchName">
                                    <HeaderStyle Width="300" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CustomerID"
                                    HeaderText="CustomerID">
                                    <HeaderStyle Width="130" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CustomerName"
                                    HeaderText="CustomerName">
                                    <HeaderStyle Width="200" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="CreateUserName">
                                    <HeaderStyle Width="200" />
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatUserName(Eval("CreateUserName").ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="150" />
                                    <ItemTemplate>
                                        <%#ApplicStatusEnum.GetDescription(Eval("Status").ToString().Trim()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidFromDate"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidToDate"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidCustomerInfo"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidBranchID"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidFilterFlag"
            runat="server"
            Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>
