<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationForm.ascx.cs" Inherits="DesktopModules.Modules.Application.ApplicationForm" %>

<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Src="./Controls/SectionApplicationInfo.ascx" TagPrefix="app" TagName="SectionApplicationInfo" %>
<%@ Register Src="./Controls/SectionCustomerInfo.ascx" TagPrefix="app" TagName="SectionCustomerInfo" %>
<%@ Register Src="./Controls/SectionContactInfo.ascx" TagPrefix="app" TagName="SectionContactInfo" %>
<%@ Register Src="./Controls/SectionAutoPayInfo.ascx" TagPrefix="app" TagName="SectionAutoPayInfo" %>
<%@ Register Src="./Controls/SectionFinanceInfo.ascx" TagPrefix="app" TagName="SectionFinanceInfo" %>
<%@ Register Src="./Controls/SectionReferenceInfo.ascx" TagPrefix="app" TagName="SectionReferenceInfo" %>
<%@ Register Src="./Controls/SectionSaleInfo.ascx" TagPrefix="app" TagName="SectionSaleInfo" %>
<%@ Register Src="./Controls/SectionCollateralInfo.ascx" TagPrefix="app" TagName="SectionCollateralInfo" %>
<%@ Register Src="./Controls/SectionPolicyInfo.ascx" TagPrefix="app" TagName="SectionPolicyInfo" %>
<%@ Register Src="./Controls/SectionCardInfo.ascx" TagPrefix="app" TagName="SectionCardInfo" %>
<%@ Register Src="./Controls/SectionAssessmentInfo.ascx" TagPrefix="app" TagName="SectionAssessmentInfo" %>
<%@ Register Src="./Controls/SectionHistoryInfo.ascx" TagPrefix="app" TagName="SectionHistoryInfo" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<dnn:DnnJsInclude FilePath="~/DesktopModules/Modules/Application/Assets/application.js"
    ForceBundle="True"
    ForceVersion="True"
    runat="server" />
<dnn:DnnCssInclude FilePath="~/DesktopModules/Modules/Application/Assets/application.css"
    ForceBundle="True"
    ForceVersion="True"
    runat="server" />


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-horizontal">

            <div class="form-group">
                <asp:PlaceHolder ID="phMessage" runat="server" />
            </div>

            <div class="dnnPanels" clientidmode="Static" id="ApplicationInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN HỒ SƠ</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionApplicationInfo
                                ClientIDMode="Static"
                                ID="SectionApplicationInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" id="CustomerInfo">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN CHỦ THẺ CHÍNH</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionCustomerInfo
                                ClientIDMode="Static"
                                ID="SectionCustomerInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" id="ContactInfo">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN LIÊN LẠC</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionContactInfo
                                ClientIDMode="Static"
                                ID="SectionContactInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="AutoPayInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">TRÍCH NỢ TỰ ĐỘNG</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionAutoPayInfo
                                ClientIDMode="Static"
                                ID="SectionAutoPayInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="FinanceInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN NGHỀ NGHIỆP & TÀI CHÍNH</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionFinanceInfo
                                ClientIDMode="Static"
                                ID="SectionFinanceInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="ReferenceInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN THAM CHIẾU</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionReferenceInfo
                                ClientIDMode="Static"
                                ID="SectionReferenceInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="SaleInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN ĐƠN VỊ TIẾP NHẬN</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionSaleInfo
                                ClientIDMode="Static"
                                ID="SectionSaleInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="CollateralInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN TÀI SẢN ĐẢM BẢO</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionCollateralInfo
                                ClientIDMode="Static"
                                ID="SectionCollateralInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="PolicyInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN CHÍNH SÁCH CẤP TÍN DỤNG</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionPolicyInfo
                                ClientIDMode="Static"
                                ID="SectionPolicyInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="CardInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN THẺ & TBGD</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionCardInfo
                                ClientIDMode="Static"
                                ID="SectionCardInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="AssessmentInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN THẨM ĐỊNH</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionAssessmentInfo
                                ClientIDMode="Static"
                                ID="SectionAssessmentInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="HistoryInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">LỊCH SỬ THAO TÁC</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionHistoryInfo
                                ClientIDMode="Static"
                                ID="SectionHistoryInfo"
                                runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>

            <div class="c-margin-t-30 dnnPanels" clientidmode="Static" id="ProcessInfo" runat="server">
                <h2 class="dnnFormSectionHead">
                    <span tabindex="0"></span>
                    <a href="#">THÔNG TIN XỬ LÝ</a>
                </h2>
                <fieldset>
                    <div class="col-sm-12">
                        <div class="form-group">
                            <div class="col-sm-2 control-label">
                                <label>Thao tác</label>
                            </div>
                            <div class="col-sm-4">
                                <control:AutoComplete ID="ctrlRoute" runat="server" ClientIDMode="Static" />
                            </div>
                            <div class="col-sm-6"></div>
                        </div>
                        <div id="DivProcessUser" runat="server" class="form-group">
                            <div class="col-sm-2 control-label">
                                <label>User xử lý</label>
                            </div>
                            <div class="col-sm-4">
                                <control:AutoComplete ID="ctrlUser" runat="server" ClientIDMode="Static" />
                            </div>
                            <div class="col-sm-6"></div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-2 control-label">
                                <label>Ghi chú</label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox
                                    CssClass="form-control c-theme"
                                    Height="100"
                                    ID="ctrlRemark"
                                    runat="server"
                                    TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>


            <div class="menu-bar">
                <div class="container menu-bar-container">
                    <asp:Button
                        CssClass="btn btn-primary"
                        ID="btnInsert"
                        OnClick="InsertApplication"
                        OnClientClick="return validateData(this)"
                        runat="server"
                        Text="Lưu" />
                    <asp:Button
                        CssClass="btn btn-primary"
                        ID="btnUpdate"
                        OnClick="UpdateApplication"
                        OnClientClick="return validateData(this)"
                        runat="server"
                        Text="Cập Nhật" />
                    <asp:Button
                        CssClass="btn btn-primary"
                        ID="btnProcess"
                        ClientIDMode="Static"
                        OnClick="ProcessApplication"
                        runat="server"
                        Text="Xử Lý" />
                </div>
            </div>


            <asp:HiddenField
                ID="ctrlApplicationID"
                runat="server"
                Visible="False" />
            <asp:HiddenField
                ID="ctrlProcessID"
                runat="server"
                Visible="False" />
            <asp:HiddenField
                ID="ctrlPhaseID"
                runat="server"
                Visible="False" />
            <asp:HiddenField
                ID="ctrlApplicationStatus"
                runat="server"
                Visible="False" />
            <asp:HiddenField
                ID="ctrlCurrentUserID"
                runat="server"
                Visible="False" />
            <asp:HiddenField
                ID="ctrlPreviousUserID"
                runat="server"
                Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function ()
    {
        fixClientIDModeStatic();
        initializePage();
    }, true);
</script>
