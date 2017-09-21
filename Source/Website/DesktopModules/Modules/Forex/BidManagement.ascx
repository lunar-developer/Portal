﻿<%@ Control Language="C#" AutoEventWireup="false" CodeFile="BidManagement.ascx.cs" Inherits="DesktopModules.Modules.Forex.BidManagement" %>
<%@ Import Namespace="Modules.Forex.Enum" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnExportReport" />
    </Triggers>
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "row">
                <div class="col-sm-12">
                    <asp:PlaceHolder ID = "phMessage"
                                     runat = "server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5">
                    <control:Grid  ID="CurrentRate" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                   OnNeedDataSource="CurrentRateOnNeedDataSource" 
                                   Width="100%">
                        <MasterTableView>
                            <Columns>
                                <control:GridBoundColumn DataField="#" HeaderText="#"
                                                         Visible="True" AllowFiltering="False">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridBoundColumn DataField="CurrencyCode"
                                                         Visible="True" AllowFiltering="False"
                                                         SortExpression='<%=ExchangeRateGridFieldEnum.CurrencyCode%>' 
                                                         HeaderText="ExchangeCode">
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridBoundColumn DataField="BigFigure"
                                                         Visible="True" AllowFiltering="False"
                                                         SortExpression='<%=ExchangeRateGridFieldEnum.BigFigure%>' 
                                                         HeaderText="BigFigure">
                                    <HeaderStyle Width="25%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridBoundColumn DataField="FundTransferGroup" Visible="True" AllowFiltering="False"
                                                         SortExpression='<%=ExchangeRateGridFieldEnum.FundTransferGroup%>'  
                                                         HeaderText="FundTransferGroup">
                                    <HeaderStyle Width="25%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridBoundColumn DataField="CashGroup" Visible="True" AllowFiltering="False"
                                                         SortExpression='<%=ExchangeRateGridFieldEnum.CashGroup%>' 
                                                         HeaderText="CashGroup">
                                    <HeaderStyle Width="25%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
                <div class="col-sm-7">
                    <control:Grid  ID="TransactionDailyResportGrid" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                   OnNeedDataSource="TransactionDailyResportGridOnNeedDataSource" 
                                   Width="100%">
                        <MasterTableView>
                            <Columns>
                                <control:GridBoundColumn DataField="NUM" HeaderText="#"
                                                         Visible="True" AllowFiltering="False">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridBoundColumn DataField="CurrencyCode"
                                                         Visible="True" AllowFiltering="False"
                                                         SortExpression=""
                                                         HeaderText="CurrencyCode">
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridTemplateColumn DataField="BUY"
                                                            Visible="True" AllowFiltering="False"
                                                            SortExpression="BUY" 
                                                            HeaderText="BUY">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatCurrency(Eval("BUY").ToString()) %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="AVGBUY"
                                                            Visible="True" AllowFiltering="False"
                                                            SortExpression="AVGBUY" 
                                                            HeaderText="AVGBUY">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatCurrency(Eval("AVGBUY").ToString()) %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="SELL"
                                                            Visible="True" AllowFiltering="False"
                                                            SortExpression="SELL" 
                                                            HeaderText="SELL">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatCurrency(Eval("SELL").ToString()) %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="AVGSELL"
                                                            Visible="True" AllowFiltering="False"
                                                            SortExpression="AVGSELL" 
                                                            HeaderText="AVGSELL">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatCurrency(Eval("AVGSELL").ToString()) %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        <div class="form-group" style="text-align: center;">
            <div class="col-md-12">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnExportReport" runat="server" OnClick="ExportReport"
                            Text="Xuất báo cáo" />
                <asp:Button CssClass="btn btn-success"
                            ID="btnInbox" runat="server" OnClick="ReloadInbox"
                            Text="Tải lại danh sách" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-12">
                <div class="dnnPanels" clientidmode="Static" id="Div2" runat="server">
                    <h2 class="dnnFormSectionHead">
                        <span tabindex="0"></span>
                        <a href="#">Danh sách chờ chào giá</a>
                    </h2>
                    <fieldset>
                        <control:Grid  ID="BidExchangeGird" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                       OnNeedDataSource="BidExchangeGirdGridOnNeedDataSource" 
                                       OnItemCommand="GridOnItemCommand"
                                       OnItemDataBound="GridOnDataBound"
                                       Width="100%">
                            <MasterTableView DataKeyNames="ID">
                                <Columns>
                                    <control:GridBoundColumn DataField="NUM" HeaderText="#" UniqueName="NumRecord"
                                                             Visible="True" AllowFiltering="False">
                                        <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="ID" HeaderText="ID" 
                                                             Visible="False" AllowFiltering="False">
                                        <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="TransactionStatusID" HeaderText="TransactionStatusID" 
                                                             Visible="False" AllowFiltering="False">
                                        <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="CreationDateTime" HeaderText="CreationDateTime" 
                                                             Visible="False" AllowFiltering="False">
                                        <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
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
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Middle" />
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
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridTemplateColumn>
                                    <control:GridTemplateColumn DataField="TransactionDate"
                                                             Visible="True" AllowFiltering="False"
                                                             SortExpression="TransactionDate" 
                                                             HeaderText="TransactionDate">
                                        <ItemTemplate>
                                            <%#FunctionBase.FormatDate(Eval("TransactionDate").ToString()) %>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridTemplateColumn>
                                    <control:GridBoundColumn DataField="BranchName" UniqueName="Branch"
                                                             Visible="True" AllowFiltering="False"
                                                             SortExpression="BranchName" 
                                                             HeaderText="BranchName">
                                        <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="TransactionStatus"
                                                             Visible="True" AllowFiltering="False"
                                                             SortExpression="TransactionStatus" 
                                                             HeaderText="TransactionStatus">
                                        <HeaderStyle Width="14%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                    </control:GridBoundColumn>
                                    <control:GridTemplateColumn DataField="Action" UniqueName="ActionColumn"
                                                                Visible="True" AllowFiltering="False"
                                                                SortExpression="Action" 
                                                                HeaderText="Action">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-sm-4">
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
                                        <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
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
    <asp:HiddenField runat="server" ID="ReloadTime" Value="0"/>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" src="/DesktopModules/Modules/Forex/Asset/script/forex.js"></script>
<script type ="text/javascript">
    

    addPageLoaded(function () {
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });
        var reloadTime = GetControlNumber("ReloadTime");
        if (reloadTime > 0)
        {
            window.setTimeout(function () { BtnReloadBidManagementGridView() }, reloadTime);
        }
        
    }, true);
</script>