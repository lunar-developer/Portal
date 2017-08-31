<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DisbursementCompare.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementCompare" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" TagName="Label" Src="~/controls/Label.ascx" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.Disbursement.Database" %>
<%@ Import Namespace="Modules.UserManagement.Global" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
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
        <asp:PostBackTrigger ControlID="btnUpload" />
        <asp:PostBackTrigger ControlID="btnOnlyPortalDownload" />
        <asp:PostBackTrigger ControlID="btnDownloadMatched" />
        <asp:PostBackTrigger ControlID="btnOnlyCorebankDownload" />
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                        <control:Label
                            HelpText="Ngày giải ngân trên hệ thống"
                            runat="server"
                            Text="Ngày giải ngân" />
                </div>
                <div class="col-sm-1">
                        <dnn:DnnDatePicker 
                            Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                            EnableTyping="False"
                            ID="calProcessDate"
                            runat="server"
                            Width="120px" />
                </div>
                
                <div class="col-sm-2 control-label">
                        <control:Label
                            HelpText="Kết quả export từ corebank"
                            runat="server"
                            Text="File kết quả" />
                </div>
                <div class="col-sm-3">
                    <asp:FileUpload  
                        ID="inputFile"
                        runat="server"
                        CssClass="form-control c-theme"
                        Font-Bold="false"
                        Font-Size="Medium"/>
                </div>
                <div class="col-sm-3">
                    <asp:Button 
                        ID="btnUpload"
                        CssClass="btn btn-primary"
                        runat="server" 
                        Text="import"
                        OnClick="Upload"
                        OnClientClick = "return checkExtension();"/>
                </div>
            </div>
            <div id="PanelApplication">
                <div class="form-group">
                    <ul class="dnnAdminTabNav dnnClear">
                        <li>
                            <a href="#tabPortal">Chỉ có trên Portal</a>
                        </li>
                        <li>
                            <a href="#tabMatched">Match</a>
                        </li>
                        <li>
                            <a href="#tabCorebank">Chỉ có trên Corebank</a>
                        </li>
                    </ul>
                </div>

                <div class="dnnClear" id="tabPortal">
                    <div class="form-group">
                        <div class="col-sm-7">

                        </div>
                        <div class="col-sm-5">
                            <asp:Button 
                                ID="btnOnlyPortalAppy"
                                Visible="false"
                                CssClass="btn btn-primary"
                                runat="server" 
                                Text="Apply"
                                OnClick="btnOnlyPortalAppy_Click"/>
                            <asp:Button 
                                Visible="false"
                                ID="btnOnlyPortalDownload"
                                CssClass="btn btn-primary"
                                runat="server" 
                                Text="Download Only Portal"
                                OnClick="btnOnlyPortalDownload_Click"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <control:Grid
                                ID="portalGrid" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowFilteringByColumn="True"
                                OnNeedDataSource="Grid_OnNeedDataSource" 
                                Width="100%">
                                <ClientSettings AllowColumnsReorder = "false" ReorderColumnsOnClient="false">
                                    <Selecting AllowRowSelect="False"
                                                EnableDragToSelectRows="False">
                                    </Selecting>
                                </ClientSettings>
                                    <MasterTableView DataKeyNames="DisbursementID"
                                                    TableLayout="Fixed">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Chi nhánh">
                                            <HeaderStyle Width="25%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                            <ItemTemplate>
                                                <%#UserManagementModuleBase.FormatBranchID(Eval(DisbursementTable.BranchID).ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CustomerName" HeaderText="Tên khách hàng">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="PsnNbr" HeaderText="Mã KHCN">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="OrgNbr" HeaderText="Mã KHDN">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="Số tiền Đăng kí" >
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Right"/>
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatDecimal(Eval("PtAmount").ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Loại tiền Đăng kí">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <%#"704".Equals(Eval("PtCurrencyCode").ToString()) ? "VND": "USD"%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="IsFired" HeaderText="Áp phạt">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                        </div>
                    </div>
                </div>
                <div class="dnnClear" id="tabMatched">
                    <div class="form-group">
                        <div class="col-sm-8">

                        </div>
                        <div class="col-sm-4">
                            <asp:Button 
                                ID="btnApply"
                                Visible="false"
                                CssClass="btn btn-primary"
                                runat="server" 
                                Text="Apply"
                                OnClick="btnApply_Click"/>
                            <asp:Button 
                                Visible="false"
                                ID="btnDownLoadMatched"
                                CssClass="btn btn-primary"
                                runat="server" 
                                Text="Download Match"
                                OnClick="btnDownLoadMatched_Click"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <control:Grid
                                ID="Grid" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowFilteringByColumn="True"
                                OnNeedDataSource="Grid_OnNeedDataSource" 
                                Width="100%">
                                <ClientSettings AllowColumnsReorder = "false" ReorderColumnsOnClient="false">
                                    <Selecting AllowRowSelect="False"
                                                EnableDragToSelectRows="False">
                                    </Selecting>
                                </ClientSettings>
                                    <MasterTableView DataKeyNames="DisbursementID"
                                                    TableLayout="Fixed">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Chi nhánh">
                                            <HeaderStyle Width="25%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                            <ItemTemplate>
                                                <%#UserManagementModuleBase.FormatBranchID(Eval(DisbursementTable.BranchID).ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CustomerName" HeaderText="Tên khách hàng">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="PsnNbr" HeaderText="Mã KHCN">
                                            <HeaderStyle Width="9%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="OrgNbr" HeaderText="Mã KHDN">
                                            <HeaderStyle Width="9%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="Số tiền Giải ngân">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Right"/>
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatDecimal(Eval("CbAmount").ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Loại tiền Giải ngân">
                                            <HeaderStyle Width="8%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <%#"704".Equals(Eval("CbCurrencyCode").ToString()) ? "VND": "USD"%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                
                                        <telerik:GridTemplateColumn HeaderText="Số tiền Đăng kí">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Right"/>
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatDecimal(Eval("PtAmount").ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Loại tiền Đăng kí">
                                            <HeaderStyle Width="8%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <%#"704".Equals(Eval("PtCurrencyCode").ToString()) ? "VND": "USD"%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="IsFired" HeaderText="Áp phạt">
                                            <HeaderStyle Width="6%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                        </div>
                    </div>
                </div>
                <div class="dnnClear" id="tabCorebank">
                     <div class="form-group">
                        <div class="col-sm-9">

                        </div>
                        <div class="col-sm-3">
                            <asp:Button 
                                Visible="false"
                                ID="btnOnlyCorebankDownload"
                                CssClass="btn btn-primary"
                                runat="server" 
                                Text="Download Only Corebank"
                                OnClick="btnOnlyCorebankDownload_Click"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <control:Grid
                                ID="unmatchedGrid" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowFilteringByColumn="True"
                                OnNeedDataSource="Grid_OnNeedDataSource" 
                                Width="100%">
                                <ClientSettings AllowColumnsReorder = "false" ReorderColumnsOnClient="false">
                                    <Selecting AllowRowSelect="False"
                                                EnableDragToSelectRows="False">
                                    </Selecting>
                                </ClientSettings>
                                    <MasterTableView TableLayout="Fixed">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Chi nhánh">
                                            <HeaderStyle Width="30%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                            <ItemTemplate>
                                                <%#UserManagementModuleBase.FormatBranchID(Eval(DisbursementTable.BranchID).ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CustomerName" HeaderText="Tên khách hàng">
                                            <HeaderStyle Width="20%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Left"/>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PsnNbr" HeaderText="Mã KHCN">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="OrgNbr" HeaderText="Mã KHDN">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="Số tiền Giải ngân">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Right"/>
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatDecimal(Eval(DisbursementTable.Amount).ToString()) %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Loại tiền Giải ngân">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                            <ItemTemplate>
                                                <%#"704".Equals(Eval(DisbursementTable.CurrencyCode).ToString()) ? "VND": "USD"%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<style>
    .btn {
        margin-top: 0;
    }
</style>

<script type="text/javascript">
    addPageLoaded(function () {
        confirmMessage('#' + getClientID("btnApply"), "Bạn có chắc muốn Apply kết quả giải ngân không?", undefined, undefined, undefined, undefined);
        $("#PanelApplication").dnnTabs();
    }, true);

    function checkExtension() {
        var value = getControl("inputFile").value;
        var part = value.split(".");
        var extension = part[part.length - 1].toLowerCase();

        if (value == "") {
            alertMessage("Chưa chọn file tải lên.", undefined, undefined, hideLoading);
            return false;
        }

        if (extension === "xlsx" || extension === "xls") {
            return true;
        }
        alertMessage("Hệ thống chỉ hỗ trợ file XLSX hoặc XLS.", undefined, undefined, hideLoading);
        return false;
    }
</script>
