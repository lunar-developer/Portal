<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ExchangeRateCreation.ascx.cs" Inherits="DesktopModules.Modules.Forex.ExchangeRateCreation" %>
<%@ Register Src="~/controls/DoubleLabel.ascx" TagName="DoubleLabel" TagPrefix="control" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnUploadCurrency" />
        <asp:PostBackTrigger ControlID = "btnUploadExchange" />
        <asp:PostBackTrigger ControlID = "btnExportCurrency" />
        <asp:PostBackTrigger ControlID = "btnExportExchange" />
    </Triggers>
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class="dnnTabs"
                 id="DivExchangeRate"
                 runat="server">
                <div class="form-group">
                    <ul class="dnnAdminTabNav dnnClear">
                        <li>
                            <a href="#TabUpdateExchange">Cập nhật giá</a>
                        </li>
                        <li>
                            <a href="#TabImportCurrency">Import giá ngoại tệ</a>
                        </li>
                        <li>
                            <a href="#TabImportExchange">Import giá mua bán</a>
                        </li>
                    </ul>
                </div>
                <div class="dnnClear"
                    id="TabUpdateExchange">
                        <div class="dnnPanels" clientidmode="Static" id="CurrencyInfo" runat="server">
                            <h2 class="dnnFormSectionHead">
                                <span tabindex="0"></span>
                                <a href="#">Thông tin Giá</a>
                            </h2>
                            <fieldset>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="col-md-4 control-label">
                                                <control:DoubleLabel ID="lblExchangeCode"
                                                               CssClass="c-font-bold notice-label" runat="server" />
                                            </div>
                                            <div class="col-md-8">
                                                <control:AutoComplete 
                                                                  ID = "ctExchangeCode" AutoPostBack="True"
                                                                  runat = "server" OnSelectedIndexChanged="ExChangeCodeChange">
                                                </control:AutoComplete>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4 control-label">
                                                <control:DoubleLabel ID="lblBigFigure" CssClass="c-font-bold notice-label" 
                                                                     SubCssClass="sub-label" 
                                                                     runat="server" 
                                                                     IsRequire="False" />
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                                             ID="txtBigFigure" ReadOnly="True" TabIndex="-1"
                                                             runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="col-md-4 control-label">
                                                <control:DoubleLabel ID="lblMasterRate" CssClass="c-font-bold notice-label" 
                                                                     SubCssClass="sub-label" 
                                                                     runat="server" 
                                                                     IsRequire="False" />
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                                             ID="txtMasterRate" ReadOnly="True" TabIndex="-1"
                                                             runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4 control-label">
                                                <control:DoubleLabel ID="lblMargin" CssClass="c-font-bold notice-label" 
                                                                     SubCssClass="sub-label" 
                                                                     runat="server" 
                                                                     IsRequire="False" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                                             ID="txtMargin" ReadOnly="True" TabIndex="-1"
                                                             runat="server" />
                                            </div>
                                            <div class="col-md-4 control-label">
                                                <control:DoubleLabel ID="lblLimit" CssClass="c-font-bold notice-label" 
                                                                     SubCssClass="sub-label" 
                                                                     runat="server" 
                                                                     IsRequire="False" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                                             ID="txtLimit" ReadOnly="True" TabIndex="-1"
                                                             runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="dnnPanels" clientidmode="Static" id="RateFundTransfer" runat="server">
                                    <h2 class="dnnFormSectionHead">
                                        <span tabindex="0"></span>
                                        <a href="#">Chuyển khoản  (Bid ask Pips)</a>
                                    </h2>
                                    <fieldset>
                                        <div class="form-group">
                                            <div class="col-sm-1 c-checkbox text-center has-info" runat="server" ID="chkBuyRateFT"></div>
                                            <div class="col-sm-4 control-label">
                                                <control:DoubleLabel ID="lblBuyRateFT" CssClass="c-font-bold notice-label" runat="server" />
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtBuyRateFT"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                    <div class="col-sm-3 control-label time-label">
                                                        <control:DoubleLabel ID="lblDealTimeBuyFT" runat="Server" CssClass="c-font-bold notice-label" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtDealTimeBuyFT"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-1 c-checkbox text-center has-info" runat="server" ID="chkSellRateFT"></div>
                                            <div class="col-sm-4 control-label">
                                                <control:DoubleLabel ID="lblSellRateFT" CssClass="c-font-bold notice-label" runat="server" />
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtSellRateFT"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                    <div class="col-sm-3 control-label time-label">
                                                        <control:DoubleLabel ID="lblDealTimeSellFT" runat="Server" CssClass="c-font-bold notice-label" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtDealTimeSellFT"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                    
                                    </fieldset>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="dnnPanels" clientidmode="Static" id="RateCash" runat="server">
                                    <h2 class="dnnFormSectionHead">
                                        <span tabindex="0"></span>
                                        <a href="#">Tiền mặt  (Bid ask Pips)</a>
                                    </h2>
                                    <fieldset>
                                        <div class="form-group">
                                            <div class="col-sm-1 c-checkbox text-center has-info" runat="server" ID="chkBuyRateCash"></div>
                                            <div class="col-sm-4 control-label">
                                                <control:DoubleLabel ID="lblBuyRateCash" CssClass="c-font-bold notice-label" runat="server" />
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtBuyRateCash"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                    <div class="col-sm-3 control-label time-label">
                                                        <control:DoubleLabel ID="lblDealTimeBuyCash" runat="Server" CssClass="c-font-bold notice-label" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtDealTimeBuyCash"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-1 c-checkbox text-center has-info" runat="server" ID="chkSellRateCash"></div>
                                            <div class="col-sm-4 control-label">
                                                <control:DoubleLabel ID="lblSellRateCash" CssClass="c-font-bold notice-label" runat="server" />
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtSellRateCash"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                    <div class="col-sm-3 control-label time-label">
                                                        <control:DoubleLabel ID="lblDealTimeSellCash" runat="Server" CssClass="c-font-bold notice-label" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox CssClass="form-control c-theme"
                                                                     ID="txtDealTimeSellCash"
                                                                     runat="server" ReadOnly="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                    
                                    </fieldset>
                                </div>
                            </div>
                        </div>
            
            
                        <div class="form-group">
                            <div class="col-sm-2 control-label">
                                <label>Ghi chú</label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox
                                    CssClass="form-control c-theme"
                                    Height="100" Enabled="False"
                                    ID="txtRemark"
                                    runat="server"
                                    TextMode="MultiLine" />
                            </div>
                        </div>
                        <br/>
                        <div class="form-group">
                            <div class="col-sm-2"></div>
                            <div class="col-sm-10">
                                <asp:Button CssClass="btn btn-primary" OnClientClick="return ValidateExchangeCreationForm();"
                                            ID="btnSubmit" OnClick="UpdateExchangeRate"
                                            runat="server"
                                            Text="Cập Nhật" />
                            </div>
                        </div>
                </div>
                <div class="dnnClear"
                     id="TabImportCurrency">
                    <div class = "form-group">
                        <div class = "col-sm-3 control-label">
                            <dnn:Label ID ="lblFileBrowserCurrency"
                                       runat = "server" />
                        </div>
                        <div class = "col-sm-4">
                            <asp:FileUpload CssClass = "form-control c-theme"
                                            ID = "fupFileCurrency"
                                            runat = "server" />
                        </div>
                        <div class = "col-sm-5"></div>
                    </div>
                    <div class = "form-group">
                        <div class = "col-sm-3"></div>
                        <div class = "col-sm-9">
                            <asp:Button CssClass = "btn btn-primary"
                                        ID = "btnUploadCurrency"
                                        OnClick="UploadCurrency"
                                        OnClientClick = 'return checkExtension("fupFileCurrency");'
                                        runat = "server"
                                        Text = "Upload" />
                            <a class="btn btn-default"
                               href="<%= LinkTemplateCurrency %>">
                                Download Template
                            </a>
                            <asp:Button CssClass = "btn btn-success"
                                        ID = "btnExportCurrency" OnClick="ExportCurrency"
                                        runat = "server"
                                        Text = "Xuất dữ liệu" />
                        </div>
                    </div>
                    <div class="dnnPanels" runat="server" ID="CurrencyReviewPannel" Visible="False">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#">Thông tin Upload</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:Button CssClass = "btn btn-primary"
                                                ID = "btnApprovalCurrency"
                                                OnClick="ApprovalCurrency"
                                                runat = "server"
                                                Text = "Duyệt" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <control:Grid  ID="CurrencyGrid" runat="server" AutoGenerateColumns="True" AllowFilteringByColumn="False"
                                                   OnNeedDataSource="CurrencyGridReview"
                                                   Width="100%">
                                    </control:Grid>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    
                    <div class="form-group">
                        <div class="col-sm-12 dnnPanels">
                            <h2 class="dnnFormSectionHead">
                                <a href="#">Hướng dẫn sử dụng</a>
                            </h2>
                            <fieldset>
                                <ul class="c-content-list-1 c-separator-dot c-theme c-margin-t-0">
                                    <li class="text-justify">
                                        Vui lòng sử dụng file theo <a class="c-theme-color c-edit-link" href="<%=LinkTemplateCurrency %>">Template</a> để thực hiện cập nhật kết quả.
                                    </li>
                                    <li class="text-justify">
                                        Dữ liệu trên file template phải là dữ liệu thuần giá trị (đã được xử lí, không được chứa công thức excel).
                                    </li>
                                    <li class="text-justify">
                                        <b>
                                            <i>Các trường dữ liệu trên template đã được ghi chú dạng comment, sau đây là các lưu ý thêm:</i>
                                        </b>
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        CurrencyCode: Cặp tỷ giá.
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        Rate: BigFigure
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        MarginMinProfit: Biên độ lời tối thiếu.
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        <b>Giá trị bỏ trống hoặc bằng 0 nghĩa là chương trình sẽ không xét điều kiện cho loại ngoại tệ này</b>.
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        MasterRate: Center Rate tỷ giá trung tâm
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        MarginLimit: Biên độ NHNN qui định
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        <b>MasterRate = 0 hoặc MarginLimit = 0. Chương trình sẽ không xét giao dịch có vượt qui định NHNN hay không.</b>
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        IsDisable: Trạng thái hoạt động của dữ liệu.
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        0: Còn hoạt động.
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        1: Ngưng hoạt động.
                                    </li>
                                    <li class="text-justify">
                                        <i>
                                            <b>Lưu ý: </b>Cần cập nhật giá ngoại tệ trước, giá mua bán sau.
                                        </i>
                                    </li>
                                </ul>
                            </fieldset>
                        </div>
                    </div>
                 </div>
                <div class="dnnClear"
                     id="TabImportExchange">
                    <div class = "form-group">
                        <div class = "col-sm-3 control-label">
                            <dnn:Label ID ="lblFileBrowserExchange"
                                       runat = "server" />
                        </div>
                        <div class = "col-sm-4">
                            <asp:FileUpload CssClass = "form-control c-theme"
                                            ID = "fupFileExchange"
                                            runat = "server" />
                        </div>
                        <div class = "col-sm-5"></div>
                    </div>
                    <div class = "form-group">
                        <div class = "col-sm-3"></div>
                        <div class = "col-sm-9">
                            <asp:Button CssClass = "btn btn-primary"
                                        ID = "btnUploadExchange"
                                        OnClick="UploadExchange"
                                        OnClientClick = 'return checkExtension("fupFileExchange");'
                                        runat = "server"
                                        Text = "Upload" />
                            <a class="btn btn-default"
                               href="<%= LinkTemplateExchange %>">
                                Download Template
                            </a>
                            <asp:Button CssClass = "btn btn-success"
                                        ID = "btnExportExchange" OnClick="ExportExchange"
                                        runat = "server"
                                        Text = "Xuất dữ liệu" />
                        </div>
                    </div>
                    <div class="dnnPanels" runat="server" ID="ExchangeUploadPannel" Visible="False">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#">Thông tin Upload</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:Button CssClass = "btn btn-primary"
                                                ID = "btnApprovalExchangeRate"
                                                OnClick="ApprovalExchangeRate"
                                                runat = "server"
                                                Text = "Duyệt" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <control:Grid  ID="ExchangeGrid" runat="server" AutoGenerateColumns="True" AllowFilteringByColumn="False"
                                                   OnNeedDataSource="ExchangeRateGridReview"
                                                   Width="100%">
                                    </control:Grid>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-12 dnnPanels">
                            <h2 class="dnnFormSectionHead">
                                <a href="#">Hướng dẫn sử dụng</a>
                            </h2>
                            <fieldset>
                                <ul class="c-content-list-1 c-separator-dot c-theme c-margin-t-0">
                                    <li class="text-justify">
                                        Vui lòng sử dụng file theo <a class="c-theme-color c-edit-link" href="<%=LinkTemplateExchange %>">Template</a> để thực hiện cập nhật kết quả.
                                    </li>
                                    <li class="text-justify">
                                        Dữ liệu trên file template phải là dữ liệu thuần giá trị (đã được xử lí, không được chứa công thức excel).
                                    </li>
                                    <li class="text-justify">
                                        <b>
                                            <i>Các trường dữ liệu trên template đã được ghi chú dạng comment, sau đây là các lưu ý thêm:</i>
                                        </b>
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        CurrencyCode: Cặp tỷ giá.
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        TransactionTypeID: Loại giao dịch
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        1: Ngân hàng mua (Chuyển khoản)
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        2: Ngân hàng bán (Chuyển khoản)
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        3: Ngân hàng mua (Tiền mặt)
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        4: Ngân hàng bán (Tiền mặt)
                                    </li>
                                    
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        Rate: Ask Figure giá giao dịch.
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        DealTime: Thời gian cho phép BR Maker thực hiện. Đơn vị giây.
                                    </li>
                                    <li class="c-margin-l-20 text-justify c-bg-before-green">
                                        IsDisable: Trạng thái hoạt động của dữ liệu.
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        0: Còn hoạt động.
                                    </li>
                                    <li class="c-margin-l-40 text-justify c-bg-before-green">
                                        1: Ngưng hoạt động.
                                    </li>
                                    <li class="text-justify">
                                        <i>
                                            <b>Lưu ý (1): </b>Từng cặp ngoại tệ phải đảm bảo có 4 dòng dữ liệu (VD: USD/VND phải có mua/bán CK, mua/bán tiền mặt). Chiều giao dịch nào không tồn tại có thể điền giá trị bất kỳ, và cột <b>IsDisable=1</b>.
                                        </i>
                                    </li>
                                    <li class="text-justify">
                                        <i>
                                            <b>Lưu ý (2): </b>Cần cập nhật giá ngoại tệ trước, giá mua bán sau.
                                        </i>
                                    </li>
                                </ul>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" src="/DesktopModules/Modules/Forex/Asset/script/forex.js"></script>
<script type="text/javascript">
    addPageLoaded(function()
    {
        $(".dnnTabs").dnnTabs();
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
    }, true);
    function checkExtension(inputfile) {
        var value = getControl(inputfile).value;
        var part = value.split(".");
        var extension = part[part.length - 1].toLowerCase();
        if (extension === "csv" || extension === "xls" || extension === "xlsx") {
            return true;
        }
        else {
            alertMessage("Hệ thống chỉ hỗ trợ file CSV và Excel (xls, xlsx)", undefined, undefined, hideLoading);
            return false;
        }
    }
</script>