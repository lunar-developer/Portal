<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionCollateralInfo.ascx.cs" Inherits="DesktopModules.Modules.Applic.SectionCollateralInfo" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<asp:UpdatePanel ID="SectionCollectoralPanel"
                 runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCollectoralID2" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <div class="input-group control-value">
                            <asp:TextBox CssClass="c-theme form-control"
                                         ID="ctCollectoralID2"
                                         runat="server" />
                            <span class="input-group-btn">
                                <asp:Button CssClass="c-theme btn btn-primary btn-inputGroup"
                                            ID="btnCollectoralID2"
                                            runat="server"
                                            Text="Tìm" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCollectoralValue" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="c-theme form-control"
                                     ID="ctCollectoralValue"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCrdLmt2CollectoralValue" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="c-theme form-control"
                                     ID="ctCrdLmt2CollectoralValue"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCollectoralPurpose" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctCollectoralPurpose"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblInterestBasic" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctInterestBasic"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblCollectoralDesc" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctCollectoralDesc" Rows="2" TextMode="MultiLine"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

