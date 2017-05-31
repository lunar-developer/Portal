<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionOtherService.ascx.cs" Inherits="DesktopModules.Modules.Applic.SectionOtherService" %>
<%@ Import Namespace="Modules.Applic.Enum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:UpdatePanel ID="SectionOtherServicePanel"
                 runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblDebitAccount" CssClass="c-font-bold" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <div class="row">
                            <div class="col-sm-4"></div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="ctlDebitAccountType" CssClass="c-theme" RepeatDirection="Horizontal" runat="server">
                                    <asp:ListItem Selected="True" Text="&nbsp;VND&nbsp;&nbsp;" Value="VND" />
                                    <asp:ListItem Text="&nbsp;USD" Value="USD" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 control-label">
                        <dnn:Label ID="lblDebitCardType" CssClass="c-font-bold" runat="server" />
                    </div>
                    <div class="col-sm-9">
                        <div class="form-group">
                            <div class="col-sm-5 control-label">
                                <asp:CheckBox runat="server" CssClass="c-theme" ID="ctIsLocalDebitCrd" Text="&nbsp;Nội địa" />
                            </div>
                            <div class="col-sm-7 control-value">
                                <asp:DropDownList CssClass = "form-control c-theme"
                                                  ID = "ctLocalCard"
                                                  runat = "server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-5 control-label">
                                <dnn:Label ID="lblAccountNumber4LocalCard" CssClass="" runat="server" />
                            </div>
                            <div class="col-sm-7 control-value">
                                <asp:DropDownList runat="server" ID="ctLocalDebitAccountNumber" CssClass = "form-control c-theme">
                                    <asp:ListItem Value="-1">Vui lòng chọn số tài khoản</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-5 control-label">
                                <asp:CheckBox runat="server" CssClass="c-theme" ID="ctIsInterDebitCrd" Text="&nbsp;Quốc tế" />
                            </div>
                            <div class="col-sm-7 control-value">
                                <asp:DropDownList CssClass = "form-control c-theme"
                                                  ID = "ctInterCard"
                                                  runat = "server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-5 control-label">
                                <dnn:Label ID="lblAccountNumber4InterCard" CssClass="" runat="server" />
                            </div>
                            <div class="col-sm-7 control-value">
                                <asp:DropDownList runat="server" ID="ctInternationalDebitAccountNumber" CssClass = "form-control c-theme">
                                    <asp:ListItem Value="-1">Vui lòng chọn số tài khoản</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div> 
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lbleBankingService" CssClass="c-theme c-font-bold" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:CheckBoxList runat="server" ID="cteBankingService" CssClass="c-theme">
                            <asp:ListItem Value="SmsBanking" Text="&nbsp;SMS Banking&nbsp;"></asp:ListItem>
                            <asp:ListItem Value="MobileBanking" Text="&nbsp;Mobile Banking&nbsp;"></asp:ListItem>
                            <asp:ListItem Value="InternetBanking" Text="&nbsp;Internet Banking&nbsp;"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblMobileNumber" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctEBankMobileNumber"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lbleBankingUsername" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="cteBankingUsername"
                                     runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
