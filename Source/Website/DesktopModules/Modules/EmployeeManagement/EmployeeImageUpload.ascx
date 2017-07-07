<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeImageUpload.ascx.cs" Inherits="DesktopModules.Modules.EmployeeManagement.EmployeeImageUpload" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<style>
    .RadComboBox_Default
    {
        width: 100%;
    }

    .rcbInput
    {
        padding: 6px;
        width: 100%;
    }
</style>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnOK" />
    </Triggers>

    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                             runat="server" />
        </div>
        <div class="form-horizontal">
            <div class="col-sm-6">
                <div class="form-group">
                    <h3>CẬP NHẬT HÌNH ẢNH</h3>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label>Email</label>
                    </div>
                    <div class="col-sm-9">
                        <telerik:RadComboBox AutoPostBack="True"
                                             EnableLoadOnDemand="False"
                                             EnableVirtualScrolling="false"
                                             Filter="Contains"
                                             Height="200"
                                             ID="cbxEmail"
                                             ItemRequestTimeout="1000"
                                             ItemsPerRequest="10"
                                             MinFilterLength="3"
                                             OnSelectedIndexChanged="OnEmailChanged"
                                             runat="server"
                                             ShowMoreResultsBox="true"
                                             ShowWhileLoading="True"
                                             Width="100%">
                            <ExpandAnimation Duration="500"
                                             Type="InCubic">
                            </ExpandAnimation>
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label>Upload</label>
                    </div>
                    <div class="col-sm-9">
                        <asp:FileUpload CssClass="form-control c-theme"
                                        ID="fupFile"
                                        runat="server" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-10 text-center">
                        <asp:Button CssClass="btn btn-primary"
                                    ID="btnOK"
                                    OnClick="UpdateImage"
                                    runat="server"
                                    Text="Đồng ý" />
                    </div>
                </div>
            </div>
            <div class="col-sm-1"></div>
            <div class="col-sm-5">
                <div class="form-group">
                    <h3>THÔNG TIN NHÂN VIÊN</h3>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label class="dnnLabel">Họ và Tên</label>
                    </div>
                    <div class="col-sm-9 control-value">
                        <asp:Label ID="lblFullName"
                                   runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label class="dnnLabel">Phòng ban</label>
                    </div>
                    <div class="col-sm-9 control-value">
                        <asp:Label CssClass="text-justify"
                                   ID="lblBranch"
                                   runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label class="dnnLabel">Email</label>
                    </div>
                    <div class="col-sm-9 control-value">
                        <asp:Label ID="lblEmail"
                                   runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label class="dnnLabel">Điện thoại</label>
                    </div>
                    <div class="col-sm-9 control-value">
                        <asp:Label ID="lblMobile"
                                   runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 control-label">
                        <label class="dnnLabel">Extension</label>
                    </div>
                    <div class="col-sm-9 control-value">
                        <asp:Label ID="lblPhoneExtension"
                                   runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function checkFileExtension()
    {
        return validateFileExtension(getControl("fupFile"), ["jpg", "jpeg", "png"]);
    }
</script>