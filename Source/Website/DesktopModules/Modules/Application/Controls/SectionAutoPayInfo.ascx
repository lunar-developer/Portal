<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionAutoPayInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionAutoDebitInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Payment Method"
                Text="Hình thức thanh toán" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlIsBasicCard"
                ClientIDMode="Static"
                runat="server"
                OnClientSelectedIndexChanged="processOnCardIndicatorChange">
                <Items>
                    <control:ComboBoxItem Value="1" Text="Chính"/>
                    <control:ComboBoxItem Value="0" Text="Phụ"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
</div>













<asp:UpdatePanel ID="SectionAssessmentPanel"
    runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblPaymentMothod" CssClass="c-font-bold notice-label"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass="form-control c-theme"
                            ID="ctPaymentMothod"
                            runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblPaymentBy" CssClass="c-font-bold notice-label"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass="form-control c-theme"
                            ID="ctPaymentBy"
                            runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCifNumberOfAccout" CssClass="c-font-bold"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <div class="input-group control-value">
                            <asp:TextBox CssClass="c-theme form-control"
                                ID="ctCifNumberOfAccout"
                                runat="server" />
                            <span class="input-group-btn">
                                <asp:Button CssClass="c-theme btn btn-primary btn-inputGroup"
                                    ID="btnCifNumberOfAccout"
                                    runat="server"
                                    Text="Tìm" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblAccountName" CssClass="c-font-bold"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="ctAccountName"
                            runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblPaymentAccountNumber" CssClass="c-font-bold"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="True" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass="form-control c-theme"
                            ID="ctPaymentAccountNumber"
                            runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblBranchOfPaymentAccount" CssClass="c-font-bold"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="ctBranchOfPaymentAccount"
                            runat="server" ReadOnly="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblProportionPMDebitAmount" CssClass="c-font-bold"
                            SubCssClass="sub-label"
                            runat="server"
                            IsRequire="True" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass="form-control c-theme"
                            ID="ctProportionPMDebitAmount"
                            runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

