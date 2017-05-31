<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionBranchReceiveApplic.ascx.cs" Inherits="DesktopModules.Modules.Applic.SectionBranchReceiveApplication" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<asp:UpdatePanel ID="SectionAssessmentPanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:DoubleLabel ID="lblCardAppSourceCode" CssClass="c-font-bold" 
                                     SubCssClass="sub-label" 
                                     runat="server" 
                                     IsRequire="False" />
            </div>
            <div class="col-sm-2 control-value">
                <asp:DropDownList CssClass = "form-control c-theme"
                                  ID = "ctCardAppSourceCode"
                                  runat = "server">
                </asp:DropDownList>
            </div>
            <div class="col-sm-8"></div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:DoubleLabel ID="lblSaleMethod" CssClass="c-font-bold" 
                                     SubCssClass="sub-label" 
                                     runat="server" 
                                     IsRequire="False" />
            </div>
            <div class="col-sm-2 control-value">
                <asp:DropDownList CssClass = "form-control c-theme"
                                  ID = "ctSaleMethod"
                                  runat = "server">
                </asp:DropDownList>
            </div>
            <div class="col-sm-8"></div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblSaleProgram" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctSaleProgram"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblApplicationProcessingBranch" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctApplicationProcessingBranch"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblBranchCode" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctBranchCode"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblChecker" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctChecker"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblSalesOfficer" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctSalesOfficer"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblSaleReference" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctSaleReference"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblSalesSupportName" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctSalesSupportName"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-1"></div>
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblSaleMan" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-7 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctSaleMan"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-1"></div>
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblAcctNum" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-7 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctAcctNum"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-1"></div>
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblMobile" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-7 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctMobile"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-1"></div>
                    <div class="col-sm-4 control-label">
                        <control:DoubleLabel ID="lblEmail" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-7 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctEmail"
                                     runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

