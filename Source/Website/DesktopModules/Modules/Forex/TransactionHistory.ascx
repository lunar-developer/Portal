<%@ Control Language="C#" AutoEventWireup="false" CodeFile="TransactionHistory.ascx.cs" Inherits="DesktopModules.Modules.Forex.TransactionHistory" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="control" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnExportExcel" />
    </Triggers>
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
        <div class="dnnPanels" clientidmode="Static" id="TransactionInfo" runat="server">
            <h2 class="dnnFormSectionHead">
                <span tabindex="0"></span>
                <a href="#">Thông tin giao dịch</a>
            </h2>
            <fieldset>
                <div class="row">
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
                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                             ID="txtTransactionType" ReadOnly="True"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 control-label">
                                <control:Label ID="lblExchangeCode" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                             ID="txtExchangeCode" ReadOnly="True"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 control-label">
                                <control:Label ID="lblTransactionDate" runat="server" />
                            </div>
                            <div class="col-md-5">
                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                             ID="txtTransactionDate" ReadOnly="True"
                                             runat="server" />
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
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
                        <div class="form-group" runat="server">
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
                        <div class="form-group">
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
                        <div class="form-group">
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
            </fieldset>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="dnnPanels" clientidmode="Static" id="CustomerInfo" runat="server">
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
                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                             ID="txtCustomerType" ReadOnly="True"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 control-label">
                                <control:Label ID="lblReasonTransaction" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control c-theme exchange-label-nonevalue"
                                             ID="txtReasonTransaction" ReadOnly="True"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 control-label">
                                <control:Label ID="lblRemark" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox
                                    CssClass="form-control c-theme"
                                    Height="60" ReadOnly="False"
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
                        <div class="form-group" runat="server" ID="ctrlDepositAmount">
                            <div class="col-md-4 control-label">
                                <control:Label ID="lblDepositAmount" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox CssClass="form-control c-theme"
                                             ID="txtDepositAmount" Enabled="False"
                                             runat="server" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" ID="ctrlBrokerage">
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
                <div class="col-sm-12">
                    <div class="dnnPanels" clientidmode="Static" id="Div2" runat="server">
                        <h2 class="dnnFormSectionHead">
                            <span tabindex="0"></span>
                            <a href="#" runat="server" ID="GridTitle">Lịch sử thao tác</a>
                        </h2>
                        <fieldset>
                            <control:Grid  ID="GirdView" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                           OnNeedDataSource="GridOnNeedDataSource" 
                                           Width="100%">
                                <MasterTableView>
                                    <Columns>
                                        <control:GridBoundColumn DataField="NUM" HeaderText="#"
                                                                 Visible="True" AllowFiltering="False">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="ModifiedUser"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression=""
                                                                 HeaderText="ModifiedUser">
                                            <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridTemplateColumn DataField="ModifiedDateTime"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="ModifiedDateTime" 
                                                                 HeaderText="ModifiedDateTime">
                                            <ItemTemplate>
                                                <%#FunctionBase.FormatDate(Eval("ModifiedDateTime").ToString()) %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="17%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridTemplateColumn>
                                        <control:GridBoundColumn DataField="Remark"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="Remark" 
                                                                 HeaderText="Remark">
                                            <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                        <control:GridBoundColumn DataField="Action"
                                                                 Visible="True" AllowFiltering="False"
                                                                 SortExpression="Action" 
                                                                 HeaderText="Action">
                                            
                                            <HeaderStyle Width="45%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </control:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                            <div class="form-group" style="text-align: center;">
                                <div class="col-md-12">
                                    <asp:Button CssClass="btn btn-primary" runat="server"
                                                ID="btnExportExcel" OnClick="ExportReport"
                                                Text="Xuất lịch sử giao dịch" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    <asp:HiddenField runat="server" ID="HiddenTransactionID" Value=""/>
    </ContentTemplate>
</asp:UpdatePanel>

