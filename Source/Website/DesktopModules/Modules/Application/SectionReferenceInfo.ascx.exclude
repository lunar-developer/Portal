<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionReferenceInfo.ascx.cs" Inherits="DesktopModules.Modules.Applic.SectionReferenceInfo" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<asp:UpdatePanel ID="SectionAssessmentPanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:DoubleLabel ID="lblMaritalStatus" CssClass="c-font-bold" 
                                     SubCssClass="sub-label" 
                                     runat="server" 
                                     IsRequire="False" />
            </div>
            <div class="col-sm-2 control-value">
                <asp:DropDownList CssClass = "form-control c-theme"
                                  ID = "ctMaritalStatus"
                                  runat = "server">
                </asp:DropDownList>
            </div>
            <div class="col-sm-8"></div>
        </div>
        <div class="dnnTabs">
            <div class="form-group">
                <ul class="dnnAdminTabNav dnnClear">
                    <li id="LiTabSpouse">
                        <a href="#TabSpouse">Người hôn phối</a>
                    </li>
                    <li id="LiTabContract"
                        runat="server">
                        <a href="#TabContact">Người thân</a>
                    </li>
                </ul>
            </div>
            <div class="dnnClear" id="TabSpouse">
                <div class="form-group">
                    <div class="col-sm-2 control-label">
                        <control:DoubleLabel ID="lblContractTypeSpouse" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-2 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctContractTypeSpouse"
                                          runat = "server">
                            <asp:ListItem Value="1" Text="Vợ"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Chồng"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-8"></div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblContractNameSpouse" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctContractNameSpouse"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblIDDocNoSpouse" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctIDDocNoSpouse"
                                             runat="server"  />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblMobileSpouse" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctMobileSpouse"
                                             runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblEmployerNameSpouse" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctEmployerNameSpouse"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblRemarksSpouse" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctRemarksSpouse"
                                             runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="dnnClear" id="TabContact">
                <div class="form-group">
                    <div class="col-sm-2 control-label">
                        <control:DoubleLabel ID="lblContractTypeContact1" CssClass="c-font-bold" 
                                             SubCssClass="sub-label" 
                                             runat="server" 
                                             IsRequire="False" />
                    </div>
                    <div class="col-sm-2 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctContractTypeContact1"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-8"></div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblContractNameContact1" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctContractNameContact1"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblIDDocNoContact1" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctIDDocNoContact1"
                                             runat="server"  />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblMobileContact1" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctMobileContact1"
                                             runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblEmployerNameContact1" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctEmployerNameContact1"
                                             runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <control:DoubleLabel ID="lblRemarksContact1" CssClass="c-font-bold" 
                                                     SubCssClass="sub-label" 
                                                     runat="server" 
                                                     IsRequire="False" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:TextBox CssClass="c-theme form-control"
                                             ID="ctRemarksContact1"
                                             runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

