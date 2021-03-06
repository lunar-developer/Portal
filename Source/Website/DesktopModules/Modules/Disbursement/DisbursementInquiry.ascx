﻿<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DisbursementInquiry.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementInquiry" %>
<%@ Import Namespace="Modules.Disbursement.Database" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Global" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<style>
    .RadGrid tr.rgRow td, .RadGrid tr.rgRow td:hover
    {
        border: 0.8px dotted #cfd8dc;
    }
    .RadGrid tr.rgAltRow td,.RadGrid tr.rgAltRow td:hover
    {
        border: 0.8px dotted #cfd8dc;
    }
</style>
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
                                   Text="Ngày duyệt hồ sơ" />
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
                <asp:Button 
                            ID="btnUpload"
                            Visible="false"
                            CssClass="btn btn-default"
                            runat="server" 
                            Text="import kết quả GN"
                            OnClick="Upload"/>
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
                            Text="SME Duyệt" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnCancel"
                            OnClick="Submit"
                            runat="server"
                            Text="Duyệt Hủy" />
            </div>
        </div>
    </fieldset>
</div>


<div class="c-margin-t-20 form-group table-responsive">
    <control:Grid 
        ID="gridData" 
        runat="server"
        PageSize="10"
        AutoGenerateColumns="False" 
        AllowFilteringByColumn="True"
        OnNeedDataSource="Grid_OnNeedDataSource" 
        Width="100%"
        Visible="false">
        <ClientSettings>
            <Selecting AllowRowSelect="False"
                       EnableDragToSelectRows="False">
            </Selecting>
        </ClientSettings>
        <MasterTableView DataKeyNames="DisbursementID"
                         TableLayout="Fixed">
            <Columns>
                <%--<telerik:GridClientSelectColumn HeaderText="#">
                    <HeaderStyle Width="40px" />
                </telerik:GridClientSelectColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Chi nhánh">
                    <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="180px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#UserManagementModuleBase.FormatBranchID(Eval(DisbursementTable.BranchID).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                
                <telerik:GridBoundColumn DataField="CustomerName"
                                         HeaderText="Tên khách hàng">
                    <HeaderStyle Width="200px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="200px"  HorizontalAlign="Left"/>
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn HeaderText="Mã KHCN">
                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="140px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:LinkButton CommandArgument="<%#Eval(DisbursementTable.DisbursementID).ToString() %>"
                                        CssClass="c-edit-link c-theme-color"
                                        OnClick="Edit"
                                        runat="server">
                            <%#Eval(DisbursementTable.CustomerID).ToString() %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Mã KHDN">
                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="140px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:LinkButton CommandArgument="<%#Eval(DisbursementTable.DisbursementID).ToString() %>"
                                        CssClass="c-edit-link c-theme-color"
                                        OnClick="Edit"
                                        runat="server">
                            <%#Eval(DisbursementTable.OrganizationID).ToString() %>
                        </asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                
                <telerik:GridTemplateColumn DataField="Amount"
                                            HeaderText="Số tiền">
                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="120px"  HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <%#FunctionBase.FormatDecimal(Eval(DisbursementTable.Amount).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="CurrencyCode"
                                            HeaderText="Loại tiền">
                    <HeaderStyle Width="80px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="80px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <%#FormatCurrency(Eval(DisbursementTable.CurrencyCode).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="CurrencyCode"
                                            HeaderText="Lãi suất">
                    <HeaderStyle Width="80px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="80px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <%#decimal.Parse(Eval(DisbursementTable.InterestRate).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn  DataField="LoanExpire"
                                         HeaderText="Thời hạn vay">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#FunctionBase.FormatDate(Eval(DisbursementTable.LoanExpire).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DisbursementPurpose"
                                         HeaderText="Mục đích">
                    <HeaderStyle Width="300px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="300px"  HorizontalAlign="Left"/>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LoanMethod"
                                         HeaderText="Phương thức vay">
                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="100px"  HorizontalAlign="Left"/>
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="DisbursementDate"
                                            HeaderText="Ngày ĐK">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#FunctionBase.FormatDate(Eval(DisbursementTable.CreateDateTime).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="DisbursementDate"
                                            HeaderText="Ngày Giải Ngân">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#FunctionBase.FormatDate(Eval(DisbursementTable.DisbursementDate).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Hình thức GN">
                    <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="110px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#"CK".Equals(Eval(DisbursementTable.DisbursementMethod).ToString()) ? "Chuyển khoản":"Tiền mặt" %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="CustomerType"
                                            HeaderText="Loại KH">
                    <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="110px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <%#"E".Equals(Eval(DisbursementTable.CustomerType).ToString()) ? "Hiện hữu" : "Mới" %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="DisbursementStatus"
                                            HeaderText="Trạng thái">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#GetStatusDescription(Eval(DisbursementTable.DisbursementStatus).ToString()) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn  DataField="Note"
                                         HeaderText="Ghi chú">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Left"/>
                    <ItemTemplate>
                        <%#"0".Equals(Eval(DisbursementTable.Note).ToString()) ? "GN & TN trong ngày": ("2".Equals(Eval(DisbursementTable.Note).ToString())? "Khác": "GN trong hệ thống")  %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn  DataField="Note"
                                         HeaderText="Tuân thủ thông báo">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle  Width="150px"  HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <%#"Y".Equals(Eval(DisbursementTable.ViolateFlag).ToString()) ? "Vi phạm": "Tuân thủ"  %>
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
<style>
.table-hover tbody tr:hover td, .table-hover tbody tr:hover th {
    background-color: #f9acf0;
}
</style>
<script type="text/javascript">
    addPageLoaded(function()
    {
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
    }, true);
</script>