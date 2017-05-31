<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DisbursementInquiry.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementInquiry" %>
<%@ Import Namespace="Modules.Disbursement.Database" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Global" %>
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

<div class="dnnPanels">
    <h2 class="dnnFormSectionHead">
        <a href="#">THÔNG TIN TÌM KIẾM</a>
    </h2>
    <fieldset>
        <div class="form-group">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label HelpText="CMND hoặc Mã số thuế"
                                   runat="server"
                                   Text="Mã khách hàng" />
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="tbCustomerID"
                                     placeholder="CMND hoặc Mã số thuế"
                                     runat="server">
                        </asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label HelpText="Tên khách hàng hoặc Tên công ty"
                                   runat="server"
                                   Text="Tên khách hàng" />
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="tbCustomerName"
                                     placeholder="Tên khách hàng hoặc Tên công ty"
                                     runat="server">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label runat="server"
                                   Text="Ngày xử lý" />
                    </div>
                    <div class="col-sm-8">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calProcessDate"
                                           runat="server"
                                           Width="120px" />
                    </div>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label runat="server"
                                   Text="Chi nhánh" />
                    </div>
                    <div class="col-sm-8">
                        <control:Combobox CssClass="form-control c-theme"
                                          ID="ddlBranch"
                                          placeholder="Chi nhánh tạo yêu cầu"
                                          runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label runat="server"
                                   Text="Trạng thái" />
                    </div>
                    <div class="col-sm-8">
                        <control:Combobox CssClass="form-control c-theme"
                                          ID="ddlStatus"
                                          placeholder="Trạng thái của yêu cầu"
                                          runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label runat="server"
                                   Text="Loại tiền tệ" />
                    </div>
                    <div class="col-sm-8">
                        <control:Combobox CssClass="form-control c-theme"
                                          ID="ddlCurrencyCode"
                                          runat="server" />
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>
            <div class="col-sm-2"></div>
            <div class="col-sm-10">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnSearch"
                            OnClick="Search"
                            runat="server"
                            Text="Tìm kiếm" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnAdd"
                            OnClick="Create"
                            runat="server"
                            Text="Thêm mới" />
                <asp:Button CssClass="btn btn-default"
                            ID="btnExport"
                            OnClick="Export"
                            runat="server"
                            Text="Export" />
            </div>
        </div>
    </fieldset>
</div>

<div class="c-margin-t-10 dnnPanels"
     id="DivProcessInfo"
     runat="server">
    <h2 class="dnnFormSectionHead">
        <a href="#">THÔNG TIN XỬ LÝ</a>
    </h2>
    <fieldset>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label HelpText="Tối đa 1000 ký tự"
                           runat="server"
                           Text="Ghi chú" />
            </div>
            <div class="col-sm-10">
                <asp:TextBox CssClass="c-theme form-control"
                             Height="80"
                             ID="tbRemark"
                             placeholder="Ghi chú xử lý"
                             runat="server"
                             TextMode="MultiLine" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <%--<label class="c-font-bold c-margin-t-10"
                       for="<%= chkSelectAll.ClientID %>">
                    Select All Pages
                </label>&nbsp;
                <asp:CheckBox ID="chkSelectAll"
                              runat="server" />--%>
            </div>
            <div class="col-sm-4">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnPreapprove"
                            OnClick="Submit"
                            runat="server"
                            Text="Duyệt" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnApprove"
                            OnClick="Submit"
                            runat="server"
                            Text="Duyệt" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnCancel"
                            OnClick="Submit"
                            runat="server"
                            Text="Duyệt" />
            </div>
        </div>
    </fieldset>
</div>


<div class="c-margin-t-20 form-group table-responsive">
    <control:Grid AllowMultiRowSelection="True"
                  AllowPaging="true"
                  AutoGenerateColumns="False"
                  CssClass="dnnGrid"
                  EnableViewState="true"
                  ID="gridData"
                  OnPageIndexChanged="OnPageIndexChanging"
                  OnPageSizeChanged="OnPageSizeChanging"
                  runat="server"
                  Visible="false">
        <ClientSettings>
            <Selecting AllowRowSelect="True"
                       EnableDragToSelectRows="True">
            </Selecting>
        </ClientSettings>
        <MasterTableView DataKeyNames="DisbursementID"
                         TableLayout="Fixed">
            <Columns>
                <telerik:GridClientSelectColumn HeaderText="#">
                    <HeaderStyle Width="40px" />
                </telerik:GridClientSelectColumn>
                <telerik:GridTemplateColumn HeaderText="Chi nhánh">
                    <HeaderStyle Width="180px" />
                    <ItemTemplate>
                        <%#UserManagementModuleBase.FormatBranchID(Eval(DisbursementTable.BranchID).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Mã khách hàng">
                    <HeaderStyle Width="140px" />
                    <ItemTemplate>
                        <asp:LinkButton CommandArgument="<%#Eval(DisbursementTable.DisbursementID).ToString() %>"
                                        CssClass="c-edit-link c-theme-color"
                                        OnClick="Edit"
                                        runat="server">
                            <%#Eval(DisbursementTable.CustomerID).ToString() %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="CustomerName"
                                         HeaderText="Tên khách hàng">
                    <HeaderStyle Width="200px" />
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="CurrencyCode"
                                            HeaderText="Tiền tệ">
                    <HeaderStyle Width="80px" />
                    <ItemTemplate>
                        <%#FormatCurrency(Eval(DisbursementTable.CurrencyCode).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="Amount"
                                            HeaderText="Số tiền">
                    <HeaderStyle Width="120px" />
                    <ItemTemplate>
                        <%#FunctionBase.FormatDecimal(Eval(DisbursementTable.Amount).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DisbursementMethod"
                                         HeaderText="Hình thức">
                    <HeaderStyle Width="100px" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DisbursementPurpose"
                                         HeaderText="Mục đích">
                    <HeaderStyle Width="300px" />
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="DisbursementDate"
                                            HeaderText="Ngày mong đợi">
                    <HeaderStyle Width="150px" />
                    <ItemTemplate>
                        <%#FunctionBase.FormatDate(Eval(DisbursementTable.DisbursementDate).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="DisbursementStatus"
                                            HeaderText="Trạng thái">
                    <HeaderStyle Width="250px" />
                    <ItemTemplate>
                        <%#GetStatusDescription(Eval(DisbursementTable.DisbursementStatus).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </control:Grid>
</div>
</div>

<asp:HiddenField ID="hidCustomerID"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidCustomerName"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidBranchID"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidStatus"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidProcessDate"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidCurrencyCode"
                 runat="server"
                 Visible="False" />
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
        {
            $(".dnnPanels").dnnPanels({
                defaultState: "open"
            });
        },
        true);
</script>