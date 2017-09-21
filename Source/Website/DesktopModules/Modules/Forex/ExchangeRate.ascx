<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ExchangeRate.ascx.cs" Inherits="DesktopModules.Modules.Forex.ExchangeRate" %>
<%@ Import Namespace="Modules.Forex.Enum" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<link rel="stylesheet" type="text/css" href="/DesktopModules/Modules/Forex/Asset/css/formStyle.css">
<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid  ID="CurrentRateGrid" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                                   OnNeedDataSource="CurrentRateGridOnNeedDataSource" 
                                   OnItemCommand="GridOnItemCommand"
                                   Width="100%">
                        <MasterTableView DataKeyNames="CurrencyCode">
                            <ColumnGroups>
                                <control:GridColumnGroup Name="FundTransferGroup" 
                                                         HeaderText="FundTransferGroup">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </control:GridColumnGroup>
                                <control:GridColumnGroup Name="CashGroup" 
                                                         HeaderText="CashGroup">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </control:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <control:GridBoundColumn DataField="#" HeaderText="#"
                                                         Visible="True" AllowFiltering="False">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridTemplateColumn DataField="CurrencyCode"
                                                         Visible="True" AllowFiltering="False"
                                                         SortExpression="CurrencyCode"
                                                         HeaderText="ExchangeCode">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="btn btn-icon btn-transaction-creation" runat="server" 
                                                        ToolTip="Thay đổi giá mua bán tham khảo" 
                                                        CommandName="ChangeCurrencyCode">
                                            <%#Eval("CurrencyCode").ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridBoundColumn DataField="BigFigure"
                                                         Visible="True" AllowFiltering="False"
                                                         SortExpression="BigFigure"
                                                         HeaderText="BigFigure">
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridBoundColumn>
                                <control:GridTemplateColumn DataField="BuyRateFT" Visible="True" AllowFiltering="False"
                                                         SortExpression="BuyRateFT" 
                                                         ColumnGroupName="FundTransferGroup"
                                                         HeaderText="BuyRateFT">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="btn btn-icon btn-transaction-creation" runat="server" 
                                                        ToolTip="Mua chuyển khoản" 
                                                        CommandName="TransactionCreation"
                                                        CommandArgument='<%#TransactionTypeEnum.BuyByFundTranfer%>'>
                                            <%#Eval("BuyRateFT").ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="SellRateFT" Visible="True" AllowFiltering="False"
                                                         SortExpression="SellRateFT" 
                                                         ColumnGroupName="FundTransferGroup"
                                                         HeaderText="SellRateFT">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="btn btn-icon btn-transaction-creation" runat="server" 
                                                        ToolTip="Bán chuyển khoản" 
                                                        CommandName="TransactionCreation"
                                                        CommandArgument='<%#TransactionTypeEnum.SellByFundTranfer%>'>
                                            <%#Eval("SellRateFT").ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="BuyRateCash" Visible="True" AllowFiltering="False"
                                                         SortExpression="BuyRateCash"
                                                         ColumnGroupName="CashGroup"
                                                         HeaderText="BuyRateCash">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="btn btn-icon btn-transaction-creation" runat="server" 
                                                        ToolTip="Mua tiền mặt" 
                                                        CommandName="TransactionCreation"
                                                        CommandArgument='<%#TransactionTypeEnum.BuyByCash%>'>
                                            <%#Eval("BuyRateCash").ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                                <control:GridTemplateColumn DataField="SellRateCash" Visible="True" AllowFiltering="False"
                                                         SortExpression="SellRateCash" 
                                                         ColumnGroupName="CashGroup"
                                                         HeaderText="SellRateCash">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="btn btn-icon btn-transaction-creation" runat="server" 
                                                        ToolTip="Bán tiền mặt" 
                                                        CommandName="TransactionCreation"
                                                        CommandArgument='<%#TransactionTypeEnum.SellByCash%>'>
                                            <%#Eval("SellRateCash").ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </control:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        <asp:Button CssClass="btn btn-white invisible" runat="server" 
                    ID="btnReloadExchangeRate" OnClick="ReloadExchangeRate"
                    Text="Tải lại dữ liệu" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" src="/DesktopModules/Modules/Forex/Asset/script/forex.js"></script>
