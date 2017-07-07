<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationForm.ascx.cs" Inherits="DesktopModules.Modules.Application.ApplicationForm" %>

<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register src="./Controls/SectionApplicationInfo.ascx" TagPrefix="app" TagName="SectionApplicationInfo" %>
<%@ Register src="./Controls/SectionCustomerInfo.ascx" TagPrefix="app" TagName="SectionCustomerInfo" %>
<%@ Register src="./Controls/SectionContactInfo.ascx" TagPrefix="app" TagName="SectionContactInfo" %>
<%@ Register src="./Controls/SectionHistoryInfo.ascx" TagPrefix="app" TagName="SectionHistoryInfo" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<dnn:DnnJsInclude FilePath="~/DesktopModules/Modules/Application/Assets/application.js"
                  ForceBundle="True"
                  ForceVersion="True"
                  runat="server" />
<dnn:DnnCssInclude FilePath="~/DesktopModules/Modules/Application/Assets/application.css"
                   ForceBundle="True"
                   ForceVersion="True"
                   runat="server" />


<asp:UpdatePanel runat="server"
                 UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-horizontal">
            

            <div class="form-group">
                <asp:PlaceHolder ID="phMessage" runat="server" />
            </div>
            

            <div class="dnnPanels"
                 clientidmode="Static"
                 id="ApplicationInfo"
                 runat="server">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">THÔNG TIN HỒ SƠ</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server"
                                     UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionApplicationInfo ClientIDMode="Static"
                                                        ID="SectionApplicationInfo"
                                                        runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
            

            <div class="c-margin-t-30 dnnPanels"
                 id="CustomerInfo">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">THÔNG TIN CHỦ THẺ CHÍNH</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server"
                                     UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionCustomerInfo ClientIDMode="Static"
                                                     ID="SectionCustomerInfo"
                                                     runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
            

            <div class="c-margin-t-30 dnnPanels"
                 id="ContactInfo">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">THÔNG TIN LIÊN LẠC</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server"
                                     UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionContactInfo ClientIDMode="Static"
                                                    ID="SectionContactInfo"
                                                    runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
            
            
            <div class="dnnPanels"
                 clientidmode="Static"
                 id="AutoPayInfo"
                 runat="server">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">TRÍCH NỢ TỰ ĐỘNG</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server"
                                     UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionApplicationInfo ClientIDMode="Static"
                                                        ID="SectionApplicationInfo1"
                                                        runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>


            <div class="c-margin-t-30 dnnPanels"
                 clientidmode="Static"
                 id="HistoryInfo"
                 runat="server">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">LỊCH SỬ THAO TÁC</a>
                </h2>
                <fieldset>
                    <asp:UpdatePanel runat="server"
                                     UpdateMode="Conditional">
                        <ContentTemplate>
                            <app:SectionHistoryInfo ClientIDMode="Static"
                                                    ID="SectionHistoryInfo"
                                                    runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>


            <div class="c-margin-t-30 dnnPanels"
                 ClientIDMode="Static"
                 id="ProcessInfo"
                 runat="server">
                <h2 class="dnnFormSectionHead">
                    <a href="#" tabindex="0">THÔNG TIN XỬ LÝ</a>
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
                        <div class="form-group">
                            <div class="col-sm-2 control-label">
                                <label>Ghi chú</label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox CssClass="form-control c-theme"
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
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnInsert"
                                OnClick="InsertData"
                                OnClientClick="return validateData()"
                                runat="server"
                                Text="Lưu" />
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnUpdate"
                                OnClick="UpdateData"
                                OnClientClick="return validateData()"
                                runat="server"
                                Text="Cập Nhật" />
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnProcess"
                                OnClientClick="return onProcess(arguments[1])"
                                runat="server"
                                Text="Xử Lý" />
                </div>
            </div>


            <asp:HiddenField ID="ctrlApplicationID"
                             runat="server"
                             Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
    {
        fixClientIDModeStatic();
        initializePage();
    }, true);
</script>