<%@ Control Language="C#" AutoEventWireup="false" CodeFile="TransactionCreation.ascx.cs" Inherits="DesktopModules.Modules.Forex.TransactionCreation" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="control" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "row">
                <div class="col-sm-12">
                    <asp:PlaceHolder ID = "phMessage"
                                     runat = "server" />
                </div>
            </div>
            
        <div class="row" id="TransactionInfo" Visible="False" runat="server">
            <div class="col-md-6">
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblBranchName" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox CssClass="form-control c-theme exchange-label" runat="server"
                                     ReadOnly="True" ID="txtBranchName" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblMarker" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox CssClass="form-control c-theme exchange-label" runat="server"
                                     ReadOnly="True"      ID="txtMarker" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblTransactionType" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete  OnSelectedIndexChanged="TransactionTypeChange"
                                              ID = "ctTransactionType" OnClientSelectedIndexChanged="processOnSelectPostBack"
                                              runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblExchangeCode" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <control:AutoComplete  OnSelectedIndexChanged="ExChangeCodeChange"
                                              ID = "ctExchangeCode" OnClientSelectedIndexChanged="processOnSelectPostBack"
                                              runat = "server">
                        </control:AutoComplete>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblTransactionDate" runat="server" />
                    </div>
                    <div class="col-md-8">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False" 
                                           ID="calTransactionDate" 
                                           runat="server"
                                           Width="160px" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group" runat="server" ID="ReferenceRateInfoFrame">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblBigFigure"
                                       runat="server" 
                                       IsRequire="False" />
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                     ID="txtBigFigure" ReadOnly="True"
                                     runat="server" />
                    </div>
                    <div class="col-md-3 control-label">
                        <control:Label ID="lblReferenceRate" 
                                       runat="server" 
                                       IsRequire="False" />
                    </div>
                    <div class="col-md-2">
                        <span class="form-control c-theme exchange-label" runat="server" ID="txtReferenceRate"></span>
                    </div>
                </div>
                <div class="form-group" runat="server" ID="MasterRateInfoFrame" Visible="False">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblMasterRate"
                                       runat="server" 
                                       IsRequire="False" />
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                     ID="txtMasterRate" ReadOnly="True"
                                     runat="server" />
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                     ID="txtLimit" ReadOnly="True"
                                     runat="server" />
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                     ID="txtLimitRate" ReadOnly="True"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group" runat="server" ID="MarginInfoFrame">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblMargin"
                                       runat="server" 
                                       IsRequire="False" />
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                     ID="txtMargin" ReadOnly="True"
                                     runat="server" />
                    </div>
                    <div class="col-md-4"></div>
                                
                </div>
                            
                <div class="form-group" runat="server" ID="QuantityFrame" Visible="False">
                    <div class="col-md-4 control-label">
                        <control:Label ID="lblQuantityTransactionAmount" runat="server" />
                    </div>
                    <div class="col-md-4 control-value">
                        <asp:TextBox CssClass="form-control c-theme" MaxLength="12"
                                     ID="txtQuantityTransactionAmount"
                                     runat="server" ReadOnly="False" />
                    </div>
                    <div class="col-md-4"></div>
                </div>
                            
            </div>
        </div>
        <div class="row">
                <div class="col-md-6">
                    <div class="dnnPanels" clientidmode="Static" id="CustomerInfo" Visible="False" runat="server">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#">Thông tin khách hàng</a>
                        </h2>
                        <fieldset>
                            
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblCustomerIDNo" runat="server" />
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtCustomerIDNo"
                                                 runat="server" ReadOnly="False" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Button
                                        CssClass="btn btn-primary c-margin-0" 
                                        TabIndex="-1" Visible="False" Enabled="False"
                                        ID="ctrlQueryCustomer"
                                        OnClientClick="return onBeforeQueryCustomer();"
                                        runat="server"
                                        Text="Tìm" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblCustomerFullname" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtCustomerFullname"
                                                 runat="server" ReadOnly="False" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblCustomerType" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <control:AutoComplete
                                                  ID = "ctCustomerType"
                                                  runat = "server">
                                    </control:AutoComplete>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblReasonTransaction" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <control:AutoComplete
                                                  ID = "ctReasonTransaction"
                                                  runat = "server">
                                    </control:AutoComplete>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblRemark" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox
                                        CssClass="form-control c-theme"
                                        Height="100" ReadOnly="False"
                                        ID="txtRemark"
                                        runat="server"
                                        TextMode="MultiLine" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="dnnPanels" clientidmode="Static" id="PannelBid" Visible="False" runat="server">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#">Hội sở chào giá</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblCapitalAmount" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtCapitalAmount"
                                                 runat="server" ReadOnly="False" />
                                </div>
                            </div>
                            <div class="form-group" runat="server" ID="ctrlDepositAmount" Visible="False">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblDepositAmount" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtDepositAmount" Enabled="False"
                                                 runat="server" ReadOnly="True" />
                                </div>
                            </div>
                            <div class="form-group" runat="server" ID="ctrlBrokerage" Visible="False">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblBrokerage" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtBrokerage"
                                                 runat="server" ReadOnly="False" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblRemainTime" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme"
                                                 ID="txtRemainTime"
                                                 runat="server" ReadOnly="False" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="dnnPanels" clientidmode="Static" id="PannelCustomerInvoiceAmount" Visible="False" runat="server">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#">Giá khách hàng</a>
                        </h2>
                        <fieldset>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <control:Label ID="lblCustomerInvoiceAmount" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="form-control c-theme" 
                                                 ID="txtCustomerInvoiceAmount"
                                                 runat="server" ReadOnly="False" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary" 
                                ID="btnSubmit" OnClick="SubmitForm" Enabled="False" Visible="False"
                                OnClientClick="return BtnSubmition();" runat="server"
                                Text="Yêu cầu giá" />
                    <asp:Button CssClass="btn btn-success" Visible="False" Enabled="False"
                                ID="btnUpdateCustomerInfo" OnClick="UpdateCustomerInfo"
                                OnClientClick="return UpdateCustomerInfo();" runat="server"
                                Text="Cập nhật thông tin" />
                    <asp:Button CssClass="btn btn-danger" Visible="False" Enabled="False"
                                ID="btnCancel" OnClick="RejectTransaction"
                                runat="server"
                                Text="Từ chối giá" />
                    <asp:Button CssClass="invisible" Enabled="False"
                                ID="btnRequestAgain" OnClick="UpdateTimeout"
                                runat="server" 
                                Text="Yêu cầu lại giá" />
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="HiddenAcceptToChangeStatusArr" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenWorkflowStatus" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenTransactionID" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenCurrencyCode" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenRequestTypeID" Value="1"/>
        <asp:HiddenField runat="server" ID="HiddenMasterRate" Value="0"/>
        <asp:HiddenField runat="server" ID="HiddenLimit" Value="0"/>        
        <asp:HiddenField runat="server" ID="HiddenTransactionTypeID" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenCapitalAmount" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenInvoiceAmount" Value=""/>    
        <asp:HiddenField runat="server" ID="HiddenCurrentQuantityAmount" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenMaxDealerApprovalEdit" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenMaxDealerApprovalCancel" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenMaxRequestEditPercent" Value=""/>
        <asp:HiddenField runat="server" ID="HiddenMaxRequestEditAmount" Value=""/>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" src="/DesktopModules/Modules/Forex/Asset/script/forex.js"></script>
<script type = "text/javascript">

    addPageLoaded(function ()
    {
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
        var workflowStatusId = GetControlNumber("HiddenWorkflowStatus");
        var transactionType = GetControlNumber("HiddenTransactionTypeID");
        var capitalAmount = GetControlMoney("HiddenCapitalAmount");
        var invoiceAmount = GetControlMoney("HiddenInvoiceAmount");
        var margin = GetControlNumber("txtMargin");
        if (workflowStatusId >= 1)
        {
            var opts = {
                message: "Bạn có chắc thực hiện thao tác này?",
                isUseRuntimeMessage: true,
                getRunTimeMessage: function () {
                    switch (workflowStatusId)
                    {
                        case 4:
                            var currentInvoiceAmount = GetControlMoney("txtCustomerInvoiceAmount");
                            if ((transactionType === 3 || transactionType === 1) &&
                                capitalAmount !== 0 & currentInvoiceAmount !== 0 &&
                                margin !== 0 &&
                                (currentInvoiceAmount > capitalAmount - margin)) {
                                return "Giá khách hàng (mua) <b>dưới biên độ lời tối thiểu</b>, bạn có muốn trình duyệt cấp quản lí ? ";
                            }
                            if ((transactionType === 2 || transactionType === 4) &&
                                capitalAmount !== 0 & currentInvoiceAmount !== 0 &&
                                margin !== 0 &&
                                (currentInvoiceAmount < capitalAmount + margin)) {
                                return "Giá khách hàng (bán) <b>dưới biên độ lời tối thiểu</b>, bạn có muốn trình duyệt cấp quản lí ? ";
                            }
                            return "Bạn có chắc muốn thực hiện thao tác cung cấp giá khách hàng?";
                        case 9:
                            var curentBrokerageAmount = GetControlMoney("txtBrokerage");
                            if (((transactionType === 3 || transactionType === 1) && (curentBrokerageAmount - invoiceAmount < 0)) ||
                                ((transactionType === 2 || transactionType === 4) && (invoiceAmount - curentBrokerageAmount < 0))) {
                                return "Lợi nhuận của đơn vị âm, bạn có muốn tiếp tục? ";
                            }
                            return "Bạn có chắc muốn thực hiện thao tác cung cấp giá môi giới?";
                        case 14:
                            var maxedit = GetControlMoney("HiddenMaxDealerApprovalEdit");
                            if (invoiceAmount > maxedit) {
                                return "Yêu cầu điều chỉnh vượt hạn mức duyệt, bạn có muốn trình cấp quản lí không?";
                            }
                            return "Bạn có chắc muốn thực hiện thao tác duyệt điều chỉnh?";
                        case 20:
                            var maxcancel = GetControlMoney("HiddenMaxDealerApprovalCancel");
                            if (invoiceAmount > maxcancel) {
                                return "Yêu cầu hủy vượt hạn mức duyệt, bạn có muốn trình cấp quản lí không?";
                            }
                            return "Bạn có chắc muốn thực hiện thao tác duyệt hủy?";
                        default:
                            return GetAcceptToChangeStatusMessage(workflowStatusId) + "?";
                    }
                },
                jquery: getControl("btnSubmit")
            };
            registerConfirm(opts);
            
        }
    }, true);
    var setTransactionTimeout = function()
    {
        getControl("btnRequestAgain").click();
    };
    var startTimer = function () {
        var workflowStatusId = GetControlNumber("HiddenWorkflowStatus");
        var txtRemainTime = GetControlNumber("txtRemainTime");
        var element = getControl("txtRemainTime");
        if ((workflowStatusId === 3 || workflowStatusId === 4 || workflowStatusId === 7) &&
            txtRemainTime > 0)
        {
            txtRemainTime = txtRemainTime - 1;
            element.value = txtRemainTime;
            //$(element).
            if (txtRemainTime === 0)
            {
                alertMessage("Hết thời gian chào giá, vui lòng thực hiện lại thao tác yêu cầu giá", undefined, undefined, function ()
                {
                    setTransactionTimeout();
                });
            }
            
        }
        window.setTimeout(startTimer, 1000);
    };

    startTimer();

</script>
    
