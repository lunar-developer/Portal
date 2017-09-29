<%@ Control Language="C#" AutoEventWireup="false" CodeFile="TransactionManagement.ascx.cs" Inherits="DesktopModules.Modules.Forex.TransactionManagement" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="control" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="dnn" TagName="Toast" Src="~/admin/Skins/Toast.ascx" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnExportExcel" />
    </Triggers>
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "row">
                <div class="col-sm-12"><asp:PlaceHolder ID = "phMessage"
                                                        runat = "server" /></div>
            </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblBranchName" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Trung tâm kinh doanh"
                            ID = "ctBranch" 
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblCustomerType" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Loại khách hàng"
                            ID = "ctCustomerType"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="CustomerTypeChange"
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                            
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblTransactionDate" runat="server" />
                    </div>
                    <div class="col-md-4">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False" 
                                           ID="calTransactionFromDate" 
                                           runat="server"
                                           Width="160px" />
                    </div>
                    <div class="col-md-4">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calTransactionToDate" 
                                           runat="server"
                                           Width="160px" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblCreateDate" runat="server" />
                    </div>
                    <div class="col-md-4">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calCreateFromDate" 
                                           runat="server"
                                           Width="160px" />
                    </div>
                    <div class="col-md-4">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calCreateToDate" 
                                           runat="server"
                                           Width="160px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblTransactionType" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Loại giao dịch"
                            ID = "ctTransactionType"
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblReasonTransaction" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Mục địch mua bán"
                            ID = "ctReasonTransaction"
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblExchangeCode" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Cặp ngoại tệ"
                            ID = "ctExchangeCode"
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblTransactionStatus" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete EmptyMessage="Trạng thái giao dịch"
                            ID = "ctTransactionStatus"
                            runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group" style="text-align: center;">
            <div class="col-md-12">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnFind" runat="server" OnClick="FindData"
                            Text="Tìm kiếm" />
                <asp:Button CssClass="btn btn-success"
                            ID="btnInbox" runat="server" OnClick="FindProcessData"
                            Text="Danh Sách chờ xử lí" />
                <asp:Button CssClass="btn btn-default" runat="server"
                            ID="btnExportExcel" OnClick="ExportReport"
                            Text="Xuất báo cáo" />
                <asp:Button CssClass="btn btn-white invisible" runat="server" 
                            ID="btnReload" OnClick="ReloadGirdData"
                            Text="Tải lại dữ liệu" />
            </div>
        </div>
            
            <div class="form-group">
                <div class="col-sm-12">
                    <div class="dnnPanels" clientidmode="Static" id="Div2" runat="server">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#" runat="server" ID="GridTitle">Danh sách chờ xử lí</a>
                        </h2>
                        <fieldset>
                            <control:Grid  ID="GirdView" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                           AllowPaging="True"
                                           OnNeedDataSource="GridOnNeedDataSource" 
                                           OnItemCommand="GridOnItemCommand"
                                           OnItemDataBound="GridOnDataBound"
                                           OnColumnCreated="GirdOnColumnCreated"
                                           Width="100%">
                                <MasterTableView DataKeyNames="ID">
                                    <Columns>
                                        <control:GridTemplateColumn DataField="NUM" HeaderText="#" UniqueName="NumRecord"
                                                                 Visible="True" AllowFiltering="False">
                                            <ItemTemplate>
                                                <%#IsNewItemFormat(Eval("NUM").ToString(),Eval("ID").ToString(),Eval("TransactionStatusID").ToString()) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                        <control:GridBoundColumn DataField="ID" HeaderText="ID" UniqueName="TransactionID"
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="MarkerUserID" HeaderText="ID" UniqueName="MarkerUserID"
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="DealerUserID" HeaderText="DealerUserID" UniqueName="DealerUserID"
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="BranchID" HeaderText="BranchID" UniqueName="BranchID"
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="TransactionStatusID" UniqueName="StatusID"
                                                                 HeaderText="TransactionStatusID" 
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="CreationDateTime" HeaderText="CreationDateTime" 
                                                                 Visible="False" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="BranchName" UniqueName="Branch"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="BranchName" 
                                                                 HeaderText="BranchName">
                                            <HeaderStyle Width="12%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="CustomerFullName" UniqueName="CustomerName"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="CustomerFullName" 
                                                                 HeaderText="CustomerFullName">
                                            <HeaderStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="CurrencyCode"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression=""
                                                                 HeaderText="CurrencyCode">
                                            <HeaderStyle Width="9%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="TransactionType"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="TransactionType" 
                                                                 HeaderText="TransactionType">
                                            <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridTemplateColumn DataField="QuantityTransactionAmount"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="QuantityTransactionAmount" 
                                                                 HeaderText="QuantityTransactionAmount">
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatCurrency(Eval("QuantityTransactionAmount").ToString()) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="9%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                        <control:GridTemplateColumn DataField="CapitalAmount"
                                                                    Visible="True" AllowFiltering="False"
                                                                    SortExpression="CapitalAmount" 
                                                                    HeaderText="CapitalAmount">
                                            <ItemTemplate>
                                                <%#string.IsNullOrWhiteSpace(Eval("CapitalAmount")?.ToString()) ? string.Empty:
                                                        FunctionBase.FormatCurrency(Eval("CapitalAmount").ToString()) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="9%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                        <control:GridTemplateColumn DataField="CustomerInvoiceAmount" UniqueName="InvoiceAmount"
                                                                    Visible="False" AllowFiltering="False"
                                                                    SortExpression="CustomerInvoiceAmount" 
                                                                    HeaderText="CustomerInvoiceAmount">
                                            <ItemTemplate>
                                                <%#string.IsNullOrWhiteSpace(Eval("CustomerInvoiceAmount")?.ToString()) ? string.Empty:
                                                       FunctionBase.FormatCurrency(Eval("CustomerInvoiceAmount").ToString()) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="12%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                        <control:GridBoundColumn DataField="TransactionStatus"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="TransactionStatus" 
                                                                 HeaderText="TransactionStatus">
                                            <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridTemplateColumn DataField="Action" UniqueName="ActionColumn"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="Action" 
                                                                 HeaderText="Action">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton CssClass="btn btn-icon btn-icon-color-default" runat="server" 
                                                            ToolTip="Xem lịch sử" 
                                                            Visible='<%#IsCommandVisible("History",Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            CommandName="History"
                                                            CommandArgument='<%#Eval("TransactionStatusID").ToString() %>'>
                                                            <i class="fa fa-history" aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton CssClass="btn btn-icon btn-icon-color-success" 
                                                            ToolTip="Xem lại hoặc điều chỉnh thông tin"
                                                            Visible='<%#IsCommandVisible("ViewEdit",Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            CommandName="ViewEdit" 
                                                            CommandArgument='<%#Eval("TransactionStatusID").ToString() %>'
                                                            runat="server">
                                                            <i class="fa fa-eye" aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton CssClass='<%#GetAcceptClientClass(Eval("NUM").ToString()) %>' runat="server" 
                                                            ToolTip='<%#AcceptTooltipButton(Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            Visible='<%#IsCommandVisible("Accept",Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            CommandName="Accept" 
                                                            CommandArgument='<%#Eval("TransactionStatusID").ToString() %>'>
                                                            <i class='<%#AcceptImageButton(Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>' aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton CssClass='<%#GetRejectClientClass(Eval("NUM").ToString()) %>'
                                                            ToolTip='<%#RejectTooltipButton(Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            Visible='<%#IsCommandVisible("Reject",Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>'
                                                            CommandName="Reject" 
                                                            CommandArgument='<%#Eval("TransactionStatusID").ToString() %>' 
                                                            runat="server">
                                                            <i class='<%#RejectImageButton(Eval("TransactionStatusID").ToString(),Eval("CreationDateTime").ToString()) %>' aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="18%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    <asp:HiddenField runat="server" ID="IsFindDataHidden" Value="0"/>
    <asp:HiddenField runat="server" ID="ReloadTime" Value="0"/>
    <asp:HiddenField runat="server" ID="TransactionIDLastest" Value="0"/>
    </ContentTemplate>
</asp:UpdatePanel>
<dnn:Toast ID="Toast"
           runat="server" />
<script type="text/javascript" src="/DesktopModules/Modules/Forex/Asset/script/forex.js"></script>
<script type ="text/javascript">
    

    addPageLoaded(function()
    {
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
        //var currentUrl = window.location.href.split("?")[0];
        //if (typeof currentUrl !== "undefined" && currentUrl !== null)
        //{
        //    window.history.pushState("object or string", "Title", currentUrl);
        //}
        
        var isFind = GetControlNumber("IsFindDataHidden");
        var reloadTime = GetControlNumber("ReloadTime");
        if (isFind === 0 && reloadTime > 0)
        {
            window.setTimeout(function () { BtnReloadTransactionManagementGridView() }, reloadTime);
        }
        
    }, true);
    
</script>