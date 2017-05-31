<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ResultUploader.ascx.cs" Inherits="DesktopModules.Modules.MarketingCampaign.ResultUploader" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>


<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnUpload" />
    </Triggers>
    <ContentTemplate>
        <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class = "c-font-bold c-font-uppercase">Cập nhật kết quả</h3>
            <div class = "c-bg-blue c-line-left"></div>
        </div>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class = "form-group">
                <div class = "col-sm-2"></div>
                <div class = "col-sm-3 control-label">
                    <dnn:Label ID = "lblFileBrowser"
                               runat = "server" />
                </div>
                <div class = "col-sm-4">
                    <asp:FileUpload CssClass = "form-control c-theme"
                                    ID = "fupFile"
                                    runat = "server" />
                </div>
                <div class = "col-sm-3"></div>
            </div>
            <div class = "form-group">
                <div class = "col-sm-5"></div>
                <div class = "col-sm-7">
                    <asp:Button CssClass = "btn btn-primary"
                                ID = "btnUpload"
                                OnClick = "Upload"
                                OnClientClick = "return checkExtension();"
                                runat = "server"
                                Text = "Upload" />
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type = "text/javascript">
    function checkExtension() {
        var value = getControl("fupFile").value;
        var part = value.split(".");
        var extension = part[part.length - 1].toLowerCase();
        if (extension !== "csv") {
            alertMessage("Hệ thống chỉ hỗ trợ file CSV.", undefined, undefined, hideLoading);
            return false;
        }
        return true;
    }
</script>