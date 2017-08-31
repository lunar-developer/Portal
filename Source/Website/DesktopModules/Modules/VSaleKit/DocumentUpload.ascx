<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DocumentUpload.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.DocumentUpload" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div id="DivForm" runat="server" visible="False">
                <div class="col-sm-5">
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Mã hồ sơ</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblUniqueID" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Chính sách</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblPolicy" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Loại chứng từ</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblDocumentType" runat="server" />
                        </div>
                    </div>
                    <div id="DivFile" runat="server" visible="False">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <label class="dnnLabel">Tên File</label>
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:Label ID="lblFileName" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12" style="overflow: hidden">
                                <img id="imgFileContent" runat="server" alt="" src="#"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-7">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <label class="dnnLabel">Files</label>
                        </div>
                        <div class="col-sm-10">
                            <control:FileUpload
                                ID="fuImage"
                                runat="server"
                                Width="100%"
                                OnClientFilesUploaded="setSupportExtension"
                                AllowedFileExtensions=".jpg,.jpeg,.gif,.png" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Button runat="server"
                                ID="btnUpload"
                                OnClientClick="return validateOnSubmit()"
                                OnClick="UploadDocument"
                                CssClass="btn btn-primary"
                                Text="OK" />
                        </div>
                    </div>
                </div>
            </div>




            <asp:HiddenField ID="hidUniqueID"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidPolicyID"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidDocumentCode"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidAction"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidCurrentFileNumber"
                runat="server"
                Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function ()
    {
        setTimeout(function ()
        {
            setSupportExtension();
        }, 300);
    }, true);

    function setSupportExtension()
    {
        $("input[type='file']").attr('accept', '.jpg,.png,.gif,.bmp');
    }


    function validateOnSubmit()
    {
        var control = $find(getClientID("fuImage"));
        if (control.getUploadedFiles().length === 0)
        {
            alertMessage("Vui lòng chọn <b>Files</b> cần upload trước.", null, null, hideLoading);
            return false;
        }
        return true;
    }
</script>
